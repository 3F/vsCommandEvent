/*
 * Copyright (c) 2013-2015  Denis Kuzmin (reg) <entry.reg@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using net.r_eg.vsCE.SBEScripts;
using net.r_eg.vsCE.SBEScripts.Components;
using net.r_eg.vsCE.SBEScripts.Dom;
using DomIcon = net.r_eg.vsCE.SBEScripts.Dom.Icon;

namespace net.r_eg.vsCE.UI.WForms
{
    public partial class ComponentsFrm: Form
    {
        /// <summary>
        /// Used loader
        /// </summary>
        protected IBootloader bootloader;

        /// Mapper of the available components
        /// </summary>
        protected IInspector inspector;

        /// <summary>
        /// Information by existing components
        /// </summary>
        protected Dictionary<string, List<INodeInfo>> cInfo = new Dictionary<string, List<INodeInfo>>();

        /// <summary>
        /// Update list of components from configuration set
        /// </summary>
        /// <param name="components"></param>
        public void updateComponents(Configuration.Component[] components)
        {
            Settings.Cfg.Components = components;
            foreach(IComponent c in bootloader.Registered) {
                Configuration.Component found = components.Where(p => p.ClassName == c.GetType().Name).FirstOrDefault();
                if(found != null) {
                    c.Enabled = found.Enabled;
                }
            }
        }

        /// <summary>
        /// Information about component by class name.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public IEnumerable<INodeInfo> infoByComponent(string className)
        {
            foreach(INodeInfo info in cInfo[className]) {
                yield return info;
            }
        }

        /// <summary>
        /// Refresh list of components
        /// </summary>
        /// <param name="grid"></param>
        public void fillComponents(DataGridView grid)
        {
            grid.Rows.Clear();
            foreach(IComponent c in bootloader.Registered)
            {
                Type type = c.GetType();
                if(!Inspector.isComponent(type)) {
                    continue;
                }

                bool enabled        = c.Enabled;
                string className    = c.GetType().Name;

                Configuration.Component[] cfg = Settings.Cfg.Components;
                if(cfg != null && cfg.Length > 0) {
                    Configuration.Component v = cfg.Where(p => p.ClassName == className).FirstOrDefault();
                    if(v != null) {
                        enabled = v.Enabled;
                    }
                }

                cInfo[className] = new List<INodeInfo>();
                bool withoutAttr = true;

                foreach(Attribute attr in type.GetCustomAttributes(true))
                {
                    if(attr.GetType() == typeof(ComponentAttribute) || attr.GetType() == typeof(DefinitionAttribute)) {
                        withoutAttr = false;
                    }

                    if(attr.GetType() == typeof(ComponentAttribute) && ((ComponentAttribute)attr).Parent == null)
                    {
                        fillComponents((ComponentAttribute)attr, enabled, className, grid);
                    }
                    else if(attr.GetType() == typeof(DefinitionAttribute) && ((DefinitionAttribute)attr).Parent == null)
                    {
                        DefinitionAttribute def = (DefinitionAttribute)attr;
                        grid.Rows.Add(DomIcon.definition, enabled, def.Name, className, def.Description);
                    }
                    else if(((DefinitionAttribute)attr).Parent != null)
                    {
                        cInfo[className].Add(new NodeInfo((DefinitionAttribute)attr));
                    }
                    else if(((ComponentAttribute)attr).Parent != null)
                    {
                        cInfo[className].Add(new NodeInfo((ComponentAttribute)attr));
                    }
                }

                if(withoutAttr) {
                    grid.Rows.Add(DomIcon.package, enabled, String.Empty, className, String.Empty);
                }
                cInfo[className].AddRange(new List<INodeInfo>(domElemsBy(className)));
            }
            grid.Sort(grid.Columns[2], System.ComponentModel.ListSortDirection.Descending);
        }

        public ComponentsFrm(IBootloader bootloader, IInspector inspector)
        {
            this.bootloader = bootloader;
            this.inspector  = inspector;

            InitializeComponent();
            Icon = Resource.Package_32;

            Util.fixDGVRowHeight(dgvComponents);
        }

        protected void fillComponents(ComponentAttribute attr, bool enabled, string className, DataGridView grid)
        {
            grid.Rows.Add(DomIcon.package, enabled, attr.Name, className, attr.Description);

            if(attr.Aliases == null) {
                return;
            }
            foreach(string alias in attr.Aliases)
            {
                int idx = grid.Rows.Add(DomIcon.alias, enabled, alias, className, String.Format("Alias to '{0}' Component", attr.Name));

                grid.Rows[idx].ReadOnly = true;
                grid.Rows[idx].Cells[1] = new DataGridViewCheckBoxCell() { Style = { 
                                                                               ForeColor = System.Drawing.Color.Transparent, 
                                                                               SelectionForeColor = System.Drawing.Color.Transparent }};
            }
        }

        private void componentInfo(string name)
        {
            Util.openUrl(String.Format("http://vsce.r-eg.net/doc/Scripts/SBE-Scripts/Components/{0}/", name));
        }

        protected IEnumerable<INodeInfo> domElemsBy(string className)
        {
            if(inspector == null) {
                Log.Debug("domElemsBy: Inspector is null");
                yield break;
            }

            List<INodeInfo> ret = new List<INodeInfo>();
            foreach(IComponent c in bootloader.Registered)
            {
                if(c.GetType().Name != className) {
                    continue;
                }

                foreach(INodeInfo info in inspector.getBy(c.GetType())) {
                    ret.Add(info);
                    ret.AddRange(domElemsBy(info.Link));
                }
            }

            // TODO:
            foreach(INodeInfo info in ret.Distinct()) {
                yield return info;
            }
        }

        protected IEnumerable<INodeInfo> domElemsBy(NodeIdent ident)
        {
            foreach(INodeInfo info in inspector.getBy(ident))
            {
                if(!String.IsNullOrEmpty(info.Name)) {
                    yield return info;
                }

                foreach(INodeInfo child in domElemsBy(info.Link)) {
                    yield return child;
                }
            }
        }

        /// <summary>
        /// Update configuration for all components from present list
        /// </summary>
        protected void componentApply()
        {
            List<Configuration.Component> list = new List<Configuration.Component>();
            foreach(DataGridViewRow row in dgvComponents.Rows)
            {
                if(row.ReadOnly) {
                    continue;
                }
                list.Add(new Configuration.Component() { 
                    ClassName   = (row.Cells[dgvComponentsClass.Name].Value == null)? "" : row.Cells[dgvComponentsClass.Name].Value.ToString(),
                    Enabled     = Boolean.Parse(row.Cells[dgvComponentsEnabled.Name].Value.ToString()),
                });
            }
            updateComponents(list.ToArray());
        }

        private void ComponentsFrm_Load(object sender, System.EventArgs e)
        {
            fillComponents(dgvComponents);
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            dgvComponents.EndEdit();
            componentApply();
        }

        private void dgvComponents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0 || e.ColumnIndex == dgvComponents.Columns.IndexOf(dgvComponentsEnabled)) {
                return;
            }
            componentInfo(dgvComponents.Rows[e.RowIndex].Cells[dgvComponentsClass.Name].Value.ToString());
        }

        private void dgvComponents_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex == -1 || e.ColumnIndex != dgvComponents.Columns.IndexOf(dgvComponentsIcon)) {
                return;
            }
            string name = dgvComponents.Rows[e.RowIndex].Cells[dgvComponentsClass.Name].Value.ToString();

            foreach(DataGridViewRow row in dgvComponents.Rows) {
                if(row.Cells[dgvComponentsClass.Name].Value.ToString() == name) {
                    row.Cells[dgvComponentsEnabled.Name].Value = dgvComponents.Rows[e.RowIndex].Cells[dgvComponentsEnabled.Name].Value;
                }
            }
        }

        private void dgvComponents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dgvComponents.Columns.IndexOf(dgvComponentsIcon)) {
                dgvComponents.EndEdit();
            }
        }

        private void dgvComponents_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0) {
                return;
            }

            dgvComponentInfo.Rows.Clear();
            foreach(INodeInfo info in infoByComponent(dgvComponents.Rows[e.RowIndex].Cells[dgvComponentsClass.Name].Value.ToString()))
            {
                Bitmap bmap = DomIcon.definition;
                switch(info.Type) {
                    case InfoType.Property: {
                        bmap = DomIcon.property;
                        break;
                    }
                    case InfoType.Method: {
                        bmap = DomIcon.function;
                        break;
                    }
                    case InfoType.Definition: {
                        bmap = DomIcon.definition;
                        break;
                    }
                }
                dgvComponentInfo.Rows.Add(bmap, info.Displaying, (info.Signature == null)? "" : info.Signature.Replace("\n", "  \n"), info.Description);
            }
        }

        private void btnCompNew_Click(object sender, EventArgs e)
        {
            Util.openUrl("http://vssbe.r-eg.net/doc/Dev/New%20Component/");
        }

        private void menuItemCompNew_Click(object sender, EventArgs e)
        {
            btnCompNew_Click(sender, e);
        }

        private void menuItemCompDoc_Click(object sender, EventArgs e)
        {
            if(dgvComponents.SelectedRows.Count < 1) {
                return;
            }
            componentInfo(dgvComponents.SelectedRows[0].Cells[dgvComponentsClass.Name].Value.ToString());
        }
    }
}
