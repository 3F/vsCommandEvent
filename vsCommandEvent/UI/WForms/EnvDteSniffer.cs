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
using System.Windows.Forms;
using net.r_eg.vsCE.SBEScripts;
using net.r_eg.vsCE.UI.WForms.Controls;
using CEAfterEventHandler = EnvDTE._dispCommandEvents_AfterExecuteEventHandler;
using CEBeforeEventHandler = EnvDTE._dispCommandEvents_BeforeExecuteEventHandler;

namespace net.r_eg.vsCE.UI.WForms
{
    public partial class EnvDteSniffer: Form
    {
        /// <summary>
        /// Used loader
        /// </summary>
        protected IEnvironment env;

        /// <summary>
        /// Transport support
        /// </summary>
        protected ITransferCommand link;

        /// <summary>
        /// Provides command events for automation clients
        /// </summary>
        protected EnvDTE.CommandEvents cmdEvents;

        /// <summary>
        /// object synch.
        /// </summary>
        private Object _lock = new Object();


        public void attachCommandEvents(CEBeforeEventHandler before, CEAfterEventHandler after)
        {
            cmdEvents = env.Events.CommandEvents;
            lock(_lock) {
                cmdEvents.BeforeExecute -= before;
                cmdEvents.BeforeExecute += before;
                cmdEvents.AfterExecute  -= after;
                cmdEvents.AfterExecute  += after;
            }
        }

        public void detachCommandEvents(CEBeforeEventHandler before, CEAfterEventHandler after)
        {
            if(cmdEvents == null) {
                return;
            }
            lock(_lock) {
                cmdEvents.BeforeExecute -= before;
                cmdEvents.AfterExecute  -= after;
            }
        }

        public EnvDteSniffer(IEnvironment env, ITransferCommand link)
        {
            this.env    = env;
            this.link   = link;

            InitializeComponent();
            Icon = Resource.Package_32;
        }

        protected void commandEventBefore(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            flash(Lights.FlashType.Green);
            commandEvent(true, guid, id, customIn, customOut);
        }

        protected void commandEventAfter(string guid, int id, object customIn, object customOut)
        {
            commandEvent(false, guid, id, customIn, customOut);
            flash(Lights.FlashType.Yellow);
        }

        protected void commandEvent(bool pre, string guid, int id, object customIn, object customOut)
        {
            if(dgvCESniffer == null) {
                return;
            }
            string tFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern + " .fff";
            dgvCESniffer.Rows.Add(DateTime.Now.ToString(tFormat), pre, guid, id, Value.pack(customIn), Value.pack(customOut), Util.enumViewBy(guid, id));
        }

        protected void flash(Lights.FlashType type, int delay = 250)
        {
            (new System.Threading.Tasks.Task(() =>
            {
                System.Threading.Thread.Sleep(delay);
                if(lightsTraffic.IsDisposed) {
                    return;
                }

                lightsTraffic.BeginInvoke((MethodInvoker)delegate {
                    if(chkActivate.Checked) {
                        lightsTraffic.switchOn(type);
                    }
                });

            })).Start();
        }

        private void EnvDteSniffer_Load(object sender, EventArgs e)
        {
            lightsTraffic.switchOnRed();
        }

        private void chkActivate_CheckedChanged(object sender, EventArgs e)
        {
            if(chkActivate.Checked) {
                lightsTraffic.switchOnYellow();
                attachCommandEvents(commandEventBefore, commandEventAfter);
            }
            else {
                detachCommandEvents(commandEventBefore, commandEventAfter);
                lightsTraffic.switchOnRed();
            }
        }

        private void EnvDteSniffer_FormClosing(object sender, FormClosingEventArgs e)
        {
            detachCommandEvents(commandEventBefore, commandEventAfter);
        }

        private void buttonFlush_Click(object sender, EventArgs e)
        {
            dgvCESniffer.Rows.Clear();
        }

        private void btnAddToFilters_Click(object sender, EventArgs e)
        {
            if(dgvCESniffer.Rows.Count < 1 || dgvCESniffer.SelectedRows.Count < 1) {
                return;
            }

            foreach(DataGridViewRow rc in dgvCESniffer.SelectedRows)
            {
                link.command(
                    (string)rc.Cells[dgvCESnifferColumnGuid.Name].Value,
                    Convert.ToInt32(rc.Cells[dgvCESnifferColumnId.Name].Value),
                    rc.Cells[dgvCESnifferColumnCustomIn.Name].Value,
                    rc.Cells[dgvCESnifferColumnCustomOut.Name].Value,
                    (string)rc.Cells[dgvCESnifferColumnEnum.Name].Value
                );
            }
        }

        private void menuAddToFilters_Click(object sender, EventArgs e)
        {
            btnAddToFilters_Click(sender, e);
        }

        private void menuCopy_Click(object sender, EventArgs e)
        {
            if(dgvCESniffer.SelectedRows.Count < 1) {
                return;
            }
            Clipboard.SetDataObject(dgvCESniffer.GetClipboardContent());
        }

        private void menuRemove_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dgvCESniffer.SelectedRows) {
                if(!row.IsNewRow) {
                    dgvCESniffer.Rows.Remove(row);
                }
            }
        }

        private void menuFlush_Click(object sender, EventArgs e)
        {
            buttonFlush_Click(sender, e);
        }
    }
}
