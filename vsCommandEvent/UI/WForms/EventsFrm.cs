/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using net.r_eg.SobaScript;
using net.r_eg.SobaScript.Mapper;
using net.r_eg.vsCE.Bridge;
using net.r_eg.vsCE.Configuration;
using net.r_eg.vsCE.Events;
using net.r_eg.vsCE.Events.CommandEvents;
using net.r_eg.vsCE.Events.Types;
using net.r_eg.vsCE.Extensions;
using net.r_eg.vsCE.UI.WForms.Components;
using net.r_eg.vsCE.UI.WForms.Controls;
using EOWP = net.r_eg.vsCE.Events.OWP;

namespace net.r_eg.vsCE.UI.WForms
{
    /// <summary>
    /// TODO: !Most important! This from vsSBE 'as is', need to refactor.
    /// </summary>
    internal partial class EventsFrm: Form, ITransfer
    {
        /// <summary>
        /// Operations with events etc.,
        /// </summary>
        protected Logic.Events logic;

        /// <summary>
        /// UI-helper for MSBuild Properties
        /// </summary>
        protected PropertiesFrm frmProperties;

        /// <summary>
        /// Testing tool - Evaluating Property
        /// </summary>
        protected PropertyCheckFrm frmPropertyCheck;

        /// <summary>
        /// UI-helper for DTE Commands
        /// </summary>
        protected DTECommandsFrm frmDTECommands;

        /// <summary>
        /// Testing tool - DTE Commands
        /// </summary>
        protected DTECheckFrm frmDTECheck;

        /// <summary>
        /// Testing tool - SBE-Scripts
        /// </summary>
        protected ScriptCheckFrm frmSBEScript;

        /// <summary>
        /// UI-helper - EnvDTE Sniffer
        /// </summary>
        protected EnvDteSniffer frmSniffer;

        /// <summary>
        /// For work with Components of SBE-Scripts core
        /// </summary>
        protected ComponentsFrm frmComponents;

        /// <summary>
        /// Wizard - Automatic Version Numbering
        /// </summary>
        protected Wizards.VersionFrm frmWizVersion;

        /// <summary>
        /// Mapper of available components.
        /// </summary>
        protected IInspector inspector;

        /// <summary>
        /// Binder of main events 
        /// </summary>
        private IEvLevel elvl;

        /// <summary>
        /// Flag of notification if it's required
        /// </summary>
        private bool requiresNotification;

        /// <summary>
        /// Is notified about changes
        /// </summary>
        private bool isNotified;

        /// <summary>
        /// TODO: replaces checkBoxStatus
        /// </summary>
        private bool enabledStatus = false;

        /// <summary>
        /// Application settings.
        /// </summary>
        internal IAppSettings App
        {
            get { return Settings._; }
        }

        /// <summary>
        /// Implements transport for MSBuild property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="project"></param>
        public void property(string name, string project = null)
        {
            textEditor.insertToSelection(logic.formatMSBuildProperty(name, project));
            Focus();
        }

        /// <summary>
        /// Implements transport for DTE command
        /// </summary>
        /// <param name="data"></param>
        public void command(string data)
        {
            if(radioModeOperation.Checked) {
                textEditor.insertToSelection(data + System.Environment.NewLine, false);
            }
            else {
                textEditor.insertToSelection(data);
            }
            Focus();
        }

        /// <summary>
        /// Implements transport for new action by event type.
        /// </summary>
        /// <param name="type">The type of event.</param>
        /// <param name="cfg">The event configuration for action.</param>
        public void action(SolutionEventType type, ISolutionEvent cfg)
        {
            ISolutionEvent evt = addAction(-1);

            if(evt == null || cfg == null) {
                Log.Debug("UI.action for `{0}` - cfg or evt is null /skip", type);
                return;
            }

            cfg.CloneByReflectionInto(evt, true);

            refreshActions(true);
            refreshSettings();
            notice(true);

            MessageBox.Show(String.Format("The new action `{0}`:\n`{1}` has been added.", evt.Name, evt.Caption), "New action");
        }

        /// <summary>
        /// Implements transport for EnvDTE command
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="id"></param>
        /// <param name="customIn"></param>
        /// <param name="customOut"></param>
        /// <param name="description"></param>
        public void command(string guid, int id, object customIn, object customOut, string description)
        {
            dgvCEFilters.Rows.Add(guid, id, customIn, customOut, description);
        }

        public EventsFrm(Bootloader loader)
        {
            InitializeComponent();

            elvl        = loader.EvLevel;
            inspector   = new Inspector(loader.Soba);
            logic       = new Logic.Events(loader);

            textEditor.codeCompletionInit(inspector, loader.Soba.EvMSBuild);

            Icon = Resource.Package_32;
            toolTip.SetToolTip(pictureBoxWarnWait, Resource.StringWarnForWaiting);

            Text = $"{loader.Env.SolutionFileName} - {Settings.APP_NAME}";

#if DEBUG
            Text += " [Debug version]";
            toolStripMenuVersion.Text = $"based on {Version.B_SHA1}";
#else
            toolStripMenuVersion.Text = $"v{Version.S_NUM}+{Version.B_SHA1}";
#endif

            btnApply.Location = new Point((statusStrip.Location.X - btnApply.Width) - 10, btnApply.Location.Y);

            //TODO: it was before with original dataGridView... need to check with DataGridViewExt and move it inside if still needed
            foreach(Control ctrl in Util.getControls(this, c => c.GetType() == typeof(DataGridViewExt))) {
                Util.fixDGVRowHeight((DataGridViewExt)ctrl); // solves problem with the height property
            }
        }

        /// <summary>
        /// Retrieve data from UI
        /// </summary>
        /// <param name="onlyInRAM"></param>
        protected void saveData(bool onlyInRAM = false)
        {
            if(logic.SBEItem == null)
            {
                if(!onlyInRAM) {
                    logic.saveData();
                }
                return;
            }

            try {
                saveData(logic.SBEItem);

                switch(logic.SBE.type) {
                    case SolutionEventType.Common:
                    case SolutionEventType.CommandEvent:
                    case SolutionEventType.OWP:
                    {
                        saveData((ISolutionEventOWP)logic.SBEItem);
                        saveData((ICommandEvent)logic.SBEItem);
                        break;
                    }
                }

                if(!onlyInRAM) {
                    logic.saveData();
                }
                requiresNotification = onlyInRAM && isNotified;
            }
            catch(Exception ex) {
                MessageBox.Show("Failed applying settings:\n" + ex.Message, "Configuration of event", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected void saveData(ISolutionEvent evt)
        {
            evt.Enabled                 = enabledStatus;
            evt.Name                    = (String.IsNullOrWhiteSpace(evt.Name))? logic.UniqueNameForAction : evt.Name;
            evt.SupportMSBuild          = checkBoxMSBuildSupport.Checked;
            evt.SupportSBEScripts       = checkBoxSBEScriptSupport.Checked;
            evt.Process.Waiting         = checkBoxWaitForExit.Checked;
            evt.Process.Hidden          = checkBoxProcessHide.Checked;
            evt.Process.TimeLimit       = (int)numericTimeLimit.Value;
            evt.Confirmation            = chkConfirmation.Checked;
            evt.BuildType               = (chkBuildContext.Checked)? logic.getBuildTypeBy(comboBoxBuildContext.SelectedIndex) : BuildType.Common;

            if(evt.Mode.Type == ModeType.CSharp && !radioModeCSharp.Checked) {
                logic.cacheToRemove(evt.Mode);
            }

            if(radioModeInterpreter.Checked)
            {
                evt.Mode = new ModeInterpreter() {
                    Command = textEditor.Text,
                    Handler = comboBoxInterpreter.Text,
                    Newline = comboBoxNewline.Text,
                    Wrapper = comboBoxWrapper.Text.Trim()
                };
            }
            else if(radioModeFiles.Checked)
            {
                evt.Mode = new ModeFile() {
                    Command = textEditor.Text
                };
            }
            else if(radioModeScript.Checked)
            {
                evt.Mode = new ModeScript() {
                    Command = textEditor.Text
                };
            }
            else if(radioModeTargets.Checked) {
                evt.Mode = new ModeTargets() {
                    Command = textEditor.Text
                };
            }
            else if(radioModeCSharp.Checked)
            {
                evt.Mode        = (IMode)pGridCompilerCfg.SelectedObject;
                IModeCSharp cfg = (IModeCSharp)evt.Mode;

                cfg.Command = textEditor.Text;
                if(!cfg.CachingBytecode) {
                    logic.cacheToRemove(evt.Mode);
                }
                else {
                    // probably a new data - reset for recalculation later.
                    logic.cacheReset(evt.Mode);
                }
            }
            else if(radioModeOperation.Checked)
            {
                evt.Mode = new ModeOperation() {
                    Command             = getCommandOperation(),
                    AbortOnFirstError   = checkBoxOperationsAbort.Checked
                };
            }
            else if(radioModeEnvCmd.Checked)
            {
                evt.Mode = new ModeEnvCommand() {
                    Command             = getCommandDte(),
                    AbortOnFirstError   = checkBoxCommandsAbort.Checked
                };
            }
        }

        protected void saveData(ISolutionEventOWP evt)
        {
            List<EOWP.IMatching> list = new List<EOWP.IMatching>();
            foreach(DataGridViewRow row in dgvOWP.Rows)
            {
                if(row.Cells[owpTerm.Name].Value == null || row.Cells[owpType.Name].Value == null) {
                    continue;
                }
                EOWP.IMatching m = new EOWP.Condition();
                m.Phrase    = (row.Cells[owpTerm.Name].Value == null)? "" : row.Cells[owpTerm.Name].Value.ToString();
                m.Type      = (EOWP.ComparisonType)Enum.Parse(typeof(EOWP.ComparisonType), row.Cells[owpType.Name].Value.ToString());
                m.PaneGuid  = (row.Cells[owpGuid.Name].Value == null)? "" : row.Cells[owpGuid.Name].Value.ToString();
                m.PaneName  = (row.Cells[owpItem.Name].Value == null)? "" : row.Cells[owpItem.Name].Value.ToString();
                list.Add(m);
            }
            evt.Match = list.ToArray();
        }

        protected void saveData(ICommandEvent evt)
        {
            List<Filter> list = new List<Filter>();
            foreach(DataGridViewRow row in dgvCEFilters.Rows)
            {
                if(row.IsNewRow) {
                    continue;
                }

                object customIn  = Value.PackArgument(row.Cells[dgvCEFiltersColumnCustomIn.Name].Value);
                object customOut = Value.PackArgument(row.Cells[dgvCEFiltersColumnCustomOut.Name].Value);
                object guid      = row.Cells[dgvCEFiltersColumnGuid.Name].Value;

                list.Add(new Filter()
                {
                    Guid        = (guid == null)? String.Empty : ((string)guid).Trim(),
                    CustomIn    = customIn.IsNullOrEmptyString() ? null : customIn,
                    CustomOut   = customOut.IsNullOrEmptyString() ? null : customOut,
                    Description = (string)row.Cells[dgvCEFiltersColumnDescription.Name].Value,
                    Id          = Convert.ToInt32(row.Cells[dgvCEFiltersColumnId.Name].Value),
                    Cancel      = Convert.ToBoolean(row.Cells[dgvCEFiltersColumnCancel.Name].Value),
                    Pre         = Convert.ToBoolean(row.Cells[dgvCEFiltersColumnPre.Name].Value),
                    Post        = Convert.ToBoolean(row.Cells[dgvCEFiltersColumnPost.Name].Value),
                });
            }
            evt.Filters = list.ToArray();
        }

        /// <summary>
        /// Rendering data into UI elements
        /// </summary>
        protected void renderData()
        {
            if(logic.SBEItem == null) {
                controlsLock(true);
                return;
            }
            controlsLock(false);

            renderData(logic.SBEItem);

            pictureBoxWarnWait.Visible = true;

            textEditor._.Select(0, 0);
            toolTip.SetToolTip(checkBoxWaitForExit, String.Empty);
            checkBoxWaitForExit.Cursor = Cursors.Default;

            switch(logic.SBE.type)
            {
                case SolutionEventType.Common:
                case SolutionEventType.CommandEvent:
                case SolutionEventType.OWP:
                {
                    renderData((ISolutionEventOWP)logic.SBEItem);
                    renderData((ICommandEvent)logic.SBEItem);
                    break;
                }
            }
        }

        protected void renderData(ISolutionEvent evt)
        {
            enabledStatus                       = evt.Enabled;
            checkBoxMSBuildSupport.Checked      = evt.SupportMSBuild;
            checkBoxSBEScriptSupport.Checked    = evt.SupportSBEScripts;
            checkBoxWaitForExit.Checked         = evt.Process.Waiting;
            numericTimeLimit.Value              = evt.Process.TimeLimit;
            checkBoxProcessHide.Checked         = evt.Process.Hidden;
            chkConfirmation.Checked             = evt.Confirmation;
            buildTypeSelect(evt.BuildType);

            if(evt.Mode == null) {
                Log.Warn("Mode is corrupt, reinitialized with default type - '{0}'", logic.DefaultMode.Type);
                evt.Mode = logic.DefaultMode;
            }
            pGridCompilerCfg.SelectedObject = (evt.Mode.Type == ModeType.CSharp)? (IModeCSharp)evt.Mode : new ModeCSharp();

            // update settings for editor
            if(!isChangingMode(evt.Mode.Type)) {
                textEditor.config(logic.getCommonCfg(evt.Mode.Type));
            }

            switch(evt.Mode.Type)
            {
                case ModeType.Interpreter:
                {
                    radioModeInterpreter.Checked    = true;
                    textEditor.Text                 = ((IModeInterpreter)evt.Mode).Command;
                    comboBoxInterpreter.Text        = ((IModeInterpreter)evt.Mode).Handler;
                    comboBoxNewline.Text            = ((IModeInterpreter)evt.Mode).Newline;
                    comboBoxWrapper.Text            = ((IModeInterpreter)evt.Mode).Wrapper;
                    return;
                }
                case ModeType.File:
                {
                    radioModeFiles.Checked = true;
                    textEditor.Text = ((IModeFile)evt.Mode).Command;
                    return;
                }
                case ModeType.Script: {
                    radioModeScript.Checked = true;
                    textEditor.Text = ((IModeScript)evt.Mode).Command;
                    return;
                }
                case ModeType.Targets: {
                    radioModeTargets.Checked = true;
                    textEditor.Text = ((IModeTargets)evt.Mode).Command;
                    return;
                }
                case ModeType.CSharp: {
                    radioModeCSharp.Checked = true;
                    textEditor.Text = ((IModeCSharp)evt.Mode).Command;
                    return;
                }
                case ModeType.Operation:
                {
                    radioModeOperation.Checked  = true;
                    IModeOperation mode         = (IModeOperation)evt.Mode;

                    setCommandOperation(mode.Command);
                    checkBoxOperationsAbort.Checked = mode.AbortOnFirstError;
                    return;
                }
                case ModeType.EnvCommand:
                {
                    radioModeEnvCmd.Checked = true;
                    IModeEnvCommand mode    = (IModeEnvCommand)evt.Mode;

                    setCommandDte(mode.Command);
                    checkBoxCommandsAbort.Checked = mode.AbortOnFirstError;
                    return;
                }
            }
        }

        protected void renderData(ISolutionEventOWP evt)
        {
            dgvOWP.Rows.Clear();
            if(evt.Match == null) {
                return;
            }
            foreach(EOWP.IMatching m in evt.Match) {
                dgvOWP.Rows.Add(m.PaneGuid, m.PaneName, m.Phrase, m.Type.ToString());
            }
        }

        protected void renderData(ICommandEvent evt)
        {
            dgvCEFilters.Rows.Clear();
            if(evt.Filters == null) {
                return;
            }
            foreach(IFilter f in evt.Filters) {
                dgvCEFilters.Rows.Add(f.Guid, f.Id, Value.Pack(f.CustomIn), Value.Pack(f.CustomOut), f.Description, f.Cancel, f.Pre, f.Post);
            }
        }

        protected void fillActionsList()
        {
            dgvActions.Rows.Clear();
            foreach(ISolutionEvent item in logic.SBE.evt) {
                dgvActions.Rows.Add(item.Enabled, item.Name, item.Caption);
            }
        }

        protected void setCommandOperation(Command[] commands)
        {
            dgvOperations.Rows.Clear();
            if(commands == null) {
                return;
            }
            foreach(Command c in commands) {
                dgvOperations.Rows.Add(c.name, c.args);
            }
        }

        protected Command[] getCommandOperation()
        {
            List<Command> list = new List<Command>();
            foreach(DataGridViewRow row in dgvOperations.Rows)
            {
                if(row.IsNewRow) {
                    continue;
                }
                
                list.Add(new Command()
                {
                    name = (string)row.Cells[dgvOpColumnName.Name].Value,
                    args = (string)row.Cells[dgvOpColumnArgs.Name].Value,
                });
            }
            return list.ToArray();
        }

        protected void setCommandDte(CommandDte[] commands)
        {
            dgvEnvCmd.Rows.Clear();
            if(commands == null) {
                return;
            }
            foreach(CommandDte c in commands) {
                dgvEnvCmd.Rows.Add(c.Guid, c.Id, Value.Pack(c.CustomIn), Value.Pack(c.CustomOut));
            }
        }

        protected CommandDte[] getCommandDte()
        {
            List<CommandDte> list = new List<CommandDte>();
            foreach(DataGridViewRow row in dgvEnvCmd.Rows)
            {
                if(row.IsNewRow) {
                    continue;
                }

                object customIn  = Value.PackArgument(row.Cells[dgvEnvCmdColumnCustomIn.Name].Value);
                object customOut = Value.PackArgument(row.Cells[dgvEnvCmdColumnCustomOut.Name].Value);
                object guid      = row.Cells[dgvEnvCmdColumnGuid.Name].Value;

                list.Add(new CommandDte()
                {
                    Guid        = (guid == null) ? String.Empty : ((string)guid).Trim(),
                    Id          = Convert.ToInt32(row.Cells[dgvEnvCmdColumnId.Name].Value),
                    CustomIn    = customIn.IsNullOrEmptyString() ? null : customIn,
                    CustomOut   = customOut.IsNullOrEmptyString() ? null : customOut,
                });
            }
            return list.ToArray();
        }

        protected void refreshSettings()
        {
            clearControls();
            renderData();
            updateColors();

            if(!requiresNotification) {
                notice(false);
            }
        }

        protected void refreshActions(bool rememberIndex = true)
        {
            int selectedRowIndex = (rememberIndex)? currentActionIndex() : 0;
            fillActionsList();
            selectAction(selectedRowIndex);
        }

        protected void selectAction(int index, bool refreshSettings = false)
        {
            if(dgvActions.Rows.Count < 1) {
                return;
            }

            index = Math.Max(0, Math.Min(index, dgvActions.Rows.Count - 1));
            dgvActions.ClearSelection();
            dgvActions.Rows[index].Selected = true;
            logic.setEventIndexes(index);

            if(refreshSettings) {
                this.refreshSettings();
            }
        }

        protected int currentActionIndex()
        {
            //return (dgvActions.CurrentRow == null)? 0 : dgvActions.CurrentRow.Index;
            return (dgvActions.SelectedRows.Count < 1) ? 0 : dgvActions.SelectedRows[0].Index;
        }

        protected void refreshSettingsWithIndex(int index)
        {
            logic.setEventIndexes(index);
            refreshSettings();
        }

        protected void addFirstAction()
        {
            if(dgvActions.Rows.Count > 0) {
                return;
            }

            ISolutionEvent evt  = logic.addEventItem(-1);
            evt.Enabled         = true;
            evt.Name            = "MyFirstAct1";
            evt.Caption         = "You can find our Sniffer and more tools in [#] Settings - Tools";

            dgvActions.Rows.Add(evt.Enabled, evt.Name, evt.Caption);
            selectAction(0, true);
        }

        protected ISolutionEvent addAction(int copyFrom = -1)
        {
            try {
                ISolutionEvent evt = logic.addEventItem(copyFrom);
                dgvActions.Rows.Add(evt.Enabled, evt.Name, evt.Caption);
                selectAction(dgvActions.Rows.Count - 1, true);
                return evt;
            }
            catch(Exception ex) {
                Log.Error("Failed to add event-item: '{0}'", ex.Message);
            }
            finally {
                notice(true);
            }
            return null;
        }

        protected void removeRow(DataGridViewExt dgv, DataGridViewButtonColumn btn, DataGridViewCellEventArgs idx)
        {
            if(idx.RowIndex == -1 || idx.ColumnIndex == -1) {
                return; //headers
            }

            if(idx.ColumnIndex == dgv.Columns.IndexOf(btn) && idx.RowIndex < dgv.Rows.Count - 1) {
                dgv.Rows.Remove(dgv.Rows[idx.RowIndex]);
                notice(true);
            }
        }

        protected void clearControls()
        {
            dgvOWP.Rows.Clear();
            dgvCEFilters.Rows.Clear();
            dgvOperations.Rows.Clear();
            dgvEnvCmd.Rows.Clear();
            textEditor.Text = String.Empty;
        }

        protected void controlsLock(bool disabled)
        {
            //foreach(Control c in Util.getControls(splitContainer.Panel2, c => true)) {
            //    c.Enabled = false;
            //}
            groupBoxPMode.Enabled
                = tabControlCfg.Enabled
                = tabControlCommands.Enabled
                = !disabled;

            if(disabled) {
                panelStatusSide.BackColor = Color.Gray;
                textEditor.setBackgroundFromString("#DDDDDD");
            }
            else {
                updateColors();
            }
        }

        protected void notice(bool isOn)
        {
            isNotified = isOn;
            if(isOn) {
                btnApply.FlatAppearance.BorderColor = Color.FromArgb(255, 0, 0);
                return;
            }
            btnApply.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
        }

        /// <summary>
        /// UI Selector of modes
        /// </summary>
        /// <param name="type"></param>
        protected void uiViewMode(ModeType type)
        {
            textEditor.config(logic.getCommonCfg(type));
            updateColors();
            tabPageCfgInterpreter.Enabled       = false;
            numericTimeLimit.Enabled            = false;
            tabPageCompilerCfg.Enabled          = false;
            textEditor.Enabled                  = true;
            checkBoxProcessHide.Enabled         = true;
            checkBoxOperationsAbort.Enabled     = false;
            checkBoxCommandsAbort.Enabled       = false;
            checkBoxSBEScriptSupport.Enabled    = true;
            updateTimeLimitField();
            updateCodeCompletionStatus();

            tabs(type);

            if(type == ModeType.Interpreter)
            {
                tabPageCommand.Text = "Command script for stream processor";
                tabPageCfgInterpreter.Enabled   = true;
                numericTimeLimit.Enabled        = true;
                return;
            }
            if(type == ModeType.File)
            {
                tabPageCommand.Text = "Files for execution (separated by enter key)";
                numericTimeLimit.Enabled = true;
                return;
            }
            if(type == ModeType.Script) {
                tabPageCommand.Text = "Script:";
                checkBoxSBEScriptSupport.Enabled = false;
                checkBoxSBEScriptSupport.Checked = true;
                checkBoxProcessHide.Enabled      = false;
                return;
            }
            if(type == ModeType.Targets) {
                tabPageCommand.Text = ".targets";
                if(textEditor.Text.Length < 1) {
                    textEditor.Text = Resource.StringDefaultValueForTargetsMode;
                }
                return;
            }
            if(type == ModeType.CSharp) {
                tabPageCommand.Text = "C# code";
                tabPageCompilerCfg.Enabled  = true;
                checkBoxProcessHide.Enabled = false;
                if(textEditor.Text.Length < 1) {
                    textEditor.Text = Resource.StringCSharpModeCodeByDefault;
                }
                return;
            }
            if(type == ModeType.Operation)
            {
                tabPageCommand.Text = "DTE execute (separated by enter key)";
                checkBoxOperationsAbort.Enabled = true;
                checkBoxProcessHide.Enabled     = false;
                return;
            }
            if(type == ModeType.EnvCommand)
            {
                checkBoxCommandsAbort.Enabled = true;
                checkBoxProcessHide.Enabled = false;
                return;
            }
        }

        protected void tabs(ModeType type)
        {
            hideTabPage(tabPageCommand, tabControlCfg);
            hideTabPage(tabPageEnvCmd, tabControlCfg);
            hideTabPage(tabPageOperations, tabControlCfg);

            switch(type) {
                case ModeType.EnvCommand: {
                    showTabPage(tabPageEnvCmd, 0, tabControlCfg);
                    return;
                }
                case ModeType.Operation: {
                    showTabPage(tabPageOperations, 0, tabControlCfg);
                    return;
                }
            }
            showTabPage(tabPageCommand, 0, tabControlCfg);
        }

        protected bool isChangingMode(ModeType current)
        {
            switch(current) {
                case ModeType.Interpreter: { return !radioModeInterpreter.Checked; }
                case ModeType.File: { return !radioModeFiles.Checked; }
                case ModeType.Script: { return !radioModeScript.Checked; }
                case ModeType.Targets: { return !radioModeTargets.Checked; }
                case ModeType.CSharp: { return !radioModeCSharp.Checked; }
                case ModeType.Operation: { return !radioModeOperation.Checked; }
            }
            return true;
        }

        protected void updateTimeLimitField()
        {
            if(checkBoxWaitForExit.Checked 
                && (radioModeFiles.Checked || radioModeInterpreter.Checked))
            {
                numericTimeLimit.Enabled = true;
                return;
            }
            numericTimeLimit.Enabled = false;
        }

        protected void updateCodeCompletionStatus()
        {
            textEditor.CodeCompletionEnabled = checkBoxSBEScriptSupport.Checked;
        }

        protected void buildTypeSelect(BuildType type)
        {
            chkBuildContext.Checked         = type != BuildType.Common;
            comboBoxBuildContext.Enabled    = chkBuildContext.Checked;

            int index = logic.getBuildTypeIndex(type);
            if(index >= 0) {
                comboBoxBuildContext.SelectedIndex = index;
            }
        }

        protected bool hideTabPage(TabPage tab, TabControl control)
        {
            if(!control.TabPages.Contains(tab)) {
                return false;
            }
            control.TabPages.Remove(tab);
            return true;
        }

        protected bool showTabPage(TabPage tab, int index, TabControl control)
        {
            if(control.TabPages.Contains(tab)) {
                return false;
            }
            control.TabPages.Insert(index, tab);
            control.SelectTab(index);
            return true;
        }

        protected void envVariablesUIHelper()
        {
            if(Util.focusForm(frmProperties)) {
                return;
            }
            frmProperties = new PropertiesFrm(logic.Env, this);
            frmProperties.Show();
        }

        protected void updateFormData()
        {
            logic.fillEvents();
            refreshActions(false);
            refreshSettings();
        }

        protected void updateColors()
        {
            panelStatusSide.BackColor = (enabledStatus)? Color.FromArgb(111, 145, 6) : Color.FromArgb(168, 47, 17);

            if(radioModeScript.Checked || radioModeTargets.Checked || radioModeCSharp.Checked) {
                textEditor.setBackgroundFromString("#FFFFFF");
                return;
            }

            if(enabledStatus) {
                textEditor.setBackgroundFromString("#F2FAF1");
            }
            else {
                textEditor.setBackgroundFromString("#F8F3F3");
            }
        }


        private void EventsFrm_Load(object sender, EventArgs e)
        {
            if(!App.IsCfgExists)
            {
                Log.Fatal("Configuration data is corrupt. User: {0} / Main: {1}", (App.UserConfig != null), (App.Config != null));
                MessageBox.Show("We can't continue. See details in log.", "Configuration data is corrupt");
                FormClosing -= EventsFrm_FormClosing;
                Close();
                return;
            }

            EventHandler call = (csender, ce) => { notice(true); };

            Util.noticeAboutChanges(typeof(CheckBox), this, call);
            Util.noticeAboutChanges(typeof(RadioButton), this, call);
            Util.noticeAboutChanges(typeof(TextBox), this, call);
            Util.noticeAboutChanges(typeof(RichTextBox), this, call);
            Util.noticeAboutChanges(typeof(ComboBox), this, call);
            Util.noticeAboutChanges(typeof(CheckedListBox), this, call);
            Util.noticeAboutChanges(typeof(DataGridViewExt), this, call);
            Util.noticeAboutChanges(typeof(PropertyGrid), this, call);
            textEditor._.TextChanged += call;

            elvl.OpenedSolution += onSolutionChanged;
            elvl.ClosedSolution += onSolutionChanged;

            try {
                Util.setDescriptionHeightFor(pGridCompilerCfg, 38);
                logic.fillBuildTypes(comboBoxBuildContext);

                updateFormData();
                logic.setEventIndexes(0);
            }
            catch(Exception ex) {
                Log.Error("Failed to load form: {0}", ex.Message);
            }

            //notice(false);
            addFirstAction();
        }

        private void textBoxCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Control && e.KeyCode == Keys.Space) {
                e.SuppressKeyPress = true;
                envVariablesUIHelper();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            dgvActions.EndEdit();
            dgvCEFilters.EndEdit();
            
            saveData();
            refreshActions();
            notice(false);
        }

        private void toolStripMenuReset_Click(object sender, EventArgs e)
        {
            logic.restoreData();
            logic.fillEvents();
            renderData();

            logic.setEventIndexes(0);
            refreshActions(false);
            refreshSettings();

            notice(false);
            requiresNotification = false;
        }

        private void radioModeFiles_CheckedChanged(object sender, EventArgs e)
        {
            if(!radioModeFiles.Checked) {
                return;
            }
            uiViewMode(ModeType.File);
            textEditor.colorize(TextEditor.ColorSchema.FilesMode);
            textEditor._.ShowLineNumbers = false;
        }

        private void radioModeOperation_CheckedChanged(object sender, EventArgs e)
        {
            if(!radioModeOperation.Checked) {
                return;
            }
            uiViewMode(ModeType.Operation);
            textEditor.colorize(TextEditor.ColorSchema.OperationMode);
            textEditor._.ShowLineNumbers = false;
        }

        private void radioModeInterpreter_CheckedChanged(object sender, EventArgs e)
        {
            if(!radioModeInterpreter.Checked) {
                return;
            }
            uiViewMode(ModeType.Interpreter);
            textEditor.colorize(TextEditor.ColorSchema.InterpreterMode);
            textEditor._.ShowLineNumbers = false;
        }

        private void radioModeScript_CheckedChanged(object sender, EventArgs e)
        {
            if(!radioModeScript.Checked) {
                return;
            }
            uiViewMode(ModeType.Script);
            textEditor.colorize(TextEditor.ColorSchema.SBEScripts);
            textEditor._.ShowLineNumbers        = true;
            textEditor.CodeCompletionEnabled    = true;
        }

        private void radioModeTargets_CheckedChanged(object sender, EventArgs e)
        {
            if(!radioModeTargets.Checked) {
                return;
            }
            uiViewMode(ModeType.Targets);
            textEditor.colorize(TextEditor.ColorSchema.MSBuildTargets);
            textEditor._.ShowLineNumbers = true;
        }

        private void radioModeCSharp_CheckedChanged(object sender, EventArgs e)
        {
            if(!radioModeCSharp.Checked) {
                return;
            }
            uiViewMode(ModeType.CSharp);
            textEditor.colorize(TextEditor.ColorSchema.CSharpLang);
            textEditor._.ShowLineNumbers = true;
        }

        private void radioModeEnvCmd_CheckedChanged(object sender, EventArgs e)
        {
            if(!radioModeEnvCmd.Checked) {
                return;
            }
            uiViewMode(ModeType.EnvCommand);
        }

        private void chkBuildContext_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxBuildContext.Enabled = chkBuildContext.Checked;
        }

        private void checkBoxSBEScriptSupport_CheckedChanged(object sender, EventArgs e)
        {
            updateCodeCompletionStatus();
        }

        private void checkBoxWaitForExit_CheckedChanged(object sender, EventArgs e)
        {
            updateTimeLimitField();
        }

        private void toolStripMenuHelp_ButtonClick(object sender, EventArgs e)
        {
            toolStripMenuHelp.ShowDropDown();
        }

        private void toolStripMenuSettings_ButtonClick(object sender, EventArgs e)
        {
            toolStripMenuSettings.ShowDropDown();
        }

        private void toolStripMenuBug_ButtonClick(object sender, EventArgs e)
        {
            toolStripMenuBug.ShowDropDown();
        }

        private void toolStripMenuGalleryPage_Click(object sender, EventArgs e)
        {
            Util.openUrl("https://visualstudiogallery.msdn.microsoft.com/ad9f19b2-04c0-46fe-9637-9a52ce4ca661/");
        }

        private void toolStripMenuChangelog_Click(object sender, EventArgs e)
        {
            Util.openUrl("https://vsce.r-eg.net/Changelist/#vsix");
        }

        private void toolStripMenuWiki_Click(object sender, EventArgs e)
        {
            Util.openUrl("https://vsce.r-eg.net/");
        }

        private void toolStripMenuIssue_Click(object sender, EventArgs e)
        {
            Util.openUrl("https://github.com/3F/vsCommandEvent/issues");
        }

        private void toolStripMenuForkGithub_Click(object sender, EventArgs e)
        {
            Util.openUrl("https://github.com/3F/vsCommandEvent");
        }

        private void toolStripMenuLicense_Click(object sender, EventArgs e)
        {
            Util.openUrl("https://github.com/3F/vsSolutionBuildEvent/blob/master/LICENSE");
        }

        private void menuGetVSSBE_Click(object sender, EventArgs e)
        {
            Util.openUrl("https://github.com/3F/vsSolutionBuildEvent");
        }

        private void toolStripMenuAbout_Click(object sender, EventArgs e)
        {
            (new AboutFrm()).Show();
        }

        private void toolStripMenuReport_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show("Seen error or have a question - Click 'Yes'", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(ret == DialogResult.Yes) {
                Util.openUrl("https://github.com/3F/vsCommandEvent/issues/new");
            }
        }

        private void toolStripMenuDebugMode_Click(object sender, EventArgs e)
        {
            App.UserConfig.Global.DebugMode = App.DebugMode = toolStripMenuDebugMode.Checked = !toolStripMenuDebugMode.Checked;
            logic.updateUserCfg();
        }

        private void menuLogIgnoreTrace_Click(object sender, EventArgs e)
        {
            App.UserConfig.Global.LogIgnoreLevels["TRACE"] = menuLogIgnoreTrace.Checked = !menuLogIgnoreTrace.Checked;
            logic.updateUserCfg();
        }

        private void menuLogIgnoreDebug_Click(object sender, EventArgs e)
        {
            App.UserConfig.Global.LogIgnoreLevels["DEBUG"] = menuLogIgnoreDebug.Checked = !menuLogIgnoreDebug.Checked;
            logic.updateUserCfg();
        }

        private void menuLogIgnoreInfo_Click(object sender, EventArgs e)
        {
            App.UserConfig.Global.LogIgnoreLevels["INFO"] = menuLogIgnoreInfo.Checked = !menuLogIgnoreInfo.Checked;
            logic.updateUserCfg();
        }

        private void menuLogIgnoreWarn_Click(object sender, EventArgs e)
        {
            App.UserConfig.Global.LogIgnoreLevels["WARN"] = menuLogIgnoreWarn.Checked = !menuLogIgnoreWarn.Checked;
            logic.updateUserCfg();
        }

        private void menuLogIgnoreError_Click(object sender, EventArgs e)
        {
            App.UserConfig.Global.LogIgnoreLevels["ERROR"] = menuLogIgnoreError.Checked = !menuLogIgnoreError.Checked;
            logic.updateUserCfg();
        }

        private void toolStripMenuPluginDir_Click(object sender, EventArgs e)
        {
            Util.openUrl(String.Format("\"{0}\"", App.LibPath));
        }

        private void menuCommonCfgDir_Click(object sender, EventArgs e)
        {
            Util.openUrl(String.Format("\"{0}\"", App.CommonPath));
        }

        private void toolStripMenuApply_Click(object sender, EventArgs e)
        {
            btnApply_Click(sender, e);
        }

        private void toolStripMenuMSBuildProp_Click(object sender, EventArgs e)
        {
            envVariablesUIHelper();
        }

        private void toolStripMenuDTECmdExec_Click(object sender, EventArgs e)
        {
            if(Util.focusForm(frmDTECheck)) {
                return;
            }
            frmDTECheck = new DTECheckFrm(logic.Env);
            frmDTECheck.Show();
        }

        private void toolStripMenuDTECmd_Click(object sender, EventArgs e)
        {
            if(Util.focusForm(frmDTECommands)) {
                return;
            }
            IEnumerable<EnvDTE.Command> commands = logic.Env.Commands.Cast<EnvDTE.Command>();
            frmDTECommands = new DTECommandsFrm(commands, this);
            frmDTECommands.Show();
        }

        private void toolStripMenuEvaluatingProperty_Click(object sender, EventArgs e)
        {
            if(Util.focusForm(frmPropertyCheck)) {
                return;
            }
            frmPropertyCheck = new PropertyCheckFrm(logic.Env);
            frmPropertyCheck.Show();
        }

        private void menuSBEScript_Click(object sender, EventArgs e)
        {
            if(Util.focusForm(frmSBEScript)) {
                return;
            }
            frmSBEScript = new ScriptCheckFrm(logic.Env);
            frmSBEScript.Show();
        }

        private void menuItemSniffer_Click(object sender, EventArgs e)
        {
            if(Util.focusForm(frmSniffer)) {
                return;
            }
            frmSniffer = new EnvDteSniffer(logic.Env, this);
            frmSniffer.Show();
        }

        private void menuComponents_Click(object sender, EventArgs e)
        {
            if(Util.focusForm(frmComponents)) {
                return;
            }
            frmComponents = new ComponentsFrm(logic.Loader, inspector);
            frmComponents.Show();
        }

        private void menuWizardVersion_Click(object sender, EventArgs e)
        {
            if(Util.focusForm(frmWizVersion)) {
                return;
            }
            frmWizVersion = new Wizards.VersionFrm(logic.Loader, this);
            frmWizVersion.Show();
        }

        private void EventsFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Util.closeTool(frmProperties);
            Util.closeTool(frmPropertyCheck);
            Util.closeTool(frmDTECommands);
            Util.closeTool(frmDTECheck);
            Util.closeTool(frmSBEScript);
            Util.closeTool(frmSniffer);
            Util.closeTool(frmComponents);
            Util.closeTool(frmWizVersion);
            logic.restoreData();

            elvl.OpenedSolution -= onSolutionChanged;
            elvl.ClosedSolution -= onSolutionChanged;
        }

        private void menuActionsAdd_Click(object sender, EventArgs e)
        {
            addAction();
        }

        private void menuActionsClone_Click(object sender, EventArgs e)
        {
            addAction((dgvActions.Rows.Count < 1)? -1 : currentActionIndex());
        }

        private void linkAddAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            addFirstAction();
        }

        private void menuActionsEditName_Click(object sender, EventArgs e)
        {
            if(dgvActions.Rows.Count < 1) {
                return;
            }
            dgvActions.CurrentCell = dgvActions.Rows[currentActionIndex()].Cells[dgvActionName.Name];
            dgvActions.BeginEdit(true);
        }

        private void menuActionsEditCaption_Click(object sender, EventArgs e)
        {
            if(dgvActions.Rows.Count < 1) {
                return;
            }
            dgvActions.CurrentCell = dgvActions.Rows[currentActionIndex()].Cells[dgvActionCaption.Name];
            dgvActions.BeginEdit(true);
        }

        private void menuActionsReset_Click(object sender, EventArgs e)
        {
            toolStripMenuReset_Click(sender, e);
        }

        private void menuActionsRemove_Click(object sender, EventArgs e)
        {
            if(dgvActions.Rows.Count < 1) {
                return;
            }
            int index = currentActionIndex();
            dgvActions.Rows.RemoveAt(index);
            logic.removeEventItem(index);
            refreshSettingsWithIndex(currentActionIndex());
            notice(true);
        }

        private void dgvActions_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if(e.ColumnIndex != dgvActions.Columns.IndexOf(dgvActionName)) {
                return;
            }

            string origin = logic.SBE.evt[e.RowIndex].Name;
            try
            {
                logic.SBE.evt[e.RowIndex].Name = null;
                e.Value = logic.genUniqueName(logic.validateName(e.Value.ToString()), logic.SBE.evt);
                logic.SBE.evt[e.RowIndex].Name = (string)e.Value;
            }
            catch(Exception ex) {
                Log.Debug("Name for action: failed parsing - '{0}'", ex.Message);
                e.Value = origin;
            }
            e.ParsingApplied = true;
        }

        private void dgvActions_MouseDown(object sender, MouseEventArgs e)
        {
            // MouseDown because the CellClick event may not be called for some rows
            // the RowEnter called is too late..
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                DataGridView.HitTestInfo inf = dgvActions.HitTest(e.X, e.Y);
                if(inf.RowIndex == -1) {
                    return;
                }
                saveData(true);

                if(inf.ColumnIndex != dgvActions.Columns.IndexOf(dgvActionEnabled)) {
                    refreshSettingsWithIndex(inf.RowIndex);
                    return;
                }
                logic.setEventIndexes(inf.RowIndex);
            }
        }

        private void dgvActions_MouseUp(object sender, MouseEventArgs e)
        {
            if(dgvActions.HitTest(e.X, e.Y).ColumnIndex == dgvActions.Columns.IndexOf(dgvActionEnabled)) {
                refreshSettings();
            }
        }

        private void dgvActions_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) {
                saveData(true);
                refreshSettingsWithIndex(currentActionIndex());
            }
        }

        private void dgvActions_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.F2: {
                    if(dgvActions.CurrentCell != null && dgvActions.CurrentCell.ColumnIndex == dgvActions.Columns.IndexOf(dgvActionEnabled)) {
                        menuActionsEditName_Click(this, e);
                        break;
                    }
                    return;
                }
                case Keys.Space: {
                    dgvActions.Rows[currentActionIndex()].Cells[dgvActionEnabled.Name].Value = !logic.SBEItem.Enabled;
                    break;
                }
                case Keys.Enter: {
                    e.SuppressKeyPress = true;
                    break;
                }
            }
            e.Handled = true;
        }

        private void dgvActions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dgvActions.Columns.IndexOf(dgvActionEnabled)) {
                dgvActions.EndEdit();
            }
        }

        private void dgvActions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0) {
                return;
            }
            bool enabled    = Boolean.Parse(dgvActions.Rows[e.RowIndex].Cells[dgvActionEnabled.Name].Value.ToString());
            object oname    = dgvActions.Rows[e.RowIndex].Cells[dgvActionName.Name].Value;
            string caption  = (string)dgvActions.Rows[e.RowIndex].Cells[dgvActionCaption.Name].Value;

            logic.updateInfo(e.RowIndex, (oname == null) ? "" : oname.ToString(), enabled, caption);
            refreshSettings();
            updateColors();
            requiresNotification = true;
        }

        private void dgvActions_DragDropSortedRow(object sender, DataGridViewExt.MovingRowArgs e)
        {
            logic.moveEventItem(e.Data.from, e.Data.to);
        }

        private void dgvActions_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            linkAddAction.Visible = false;
        }

        private void dgvActions_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if(dgvActions.Rows.Count < 1) {
                linkAddAction.Visible = true;
            }
        }

        private void menuTplTargetsDefault_Click(object sender, EventArgs e)
        {
            radioModeTargets.Checked = true;
            textEditor.Text = Resource.StringDefaultValueForTargetsMode;
        }

        private void menuTplCSharpDefault_Click(object sender, EventArgs e)
        {
            radioModeCSharp.Checked = true;
            textEditor.Text = Resource.StringCSharpModeCodeByDefault;
        }

        private void menuActionExec_Click(object sender, EventArgs e)
        {
            logic.execAction();
        }

        private void toolStripMenuVersion_MouseHover(object sender, EventArgs e)
        {
            toolStripMenuVersion.BorderSides = ToolStripStatusLabelBorderSides.Bottom;
            statusStrip.Cursor = Cursors.Hand;
        }

        private void toolStripMenuVersion_MouseLeave(object sender, EventArgs e)
        {
            toolStripMenuVersion.BorderSides = ToolStripStatusLabelBorderSides.None;
            statusStrip.Cursor = Cursors.Default;
        }

        private void toolStripMenuVersion_Click(object sender, EventArgs e)
        {
            toolStripMenuAbout_Click(sender, e);
        }

        private void menuDteClear_Click(object sender, EventArgs e)
        {
            dgvCEFilters.Rows.Clear();
        }

        private void menuDteReset_Click(object sender, EventArgs e)
        {
            toolStripMenuReset_Click(sender, e);
        }

        private void menuDteSniffer_Click(object sender, EventArgs e)
        {
            menuItemSniffer_Click(sender, e);
        }

        private void menuCfgUseSln_Click(object sender, EventArgs e)
        {
            menuCfgUseSln.Checked   = !menuCfgUseSln.Checked;
            var userCfg             = App.ConfigManager.getUserConfigFor(ContextType.Solution);

            userCfg.Data.Global.IgnoreConfiguration = !menuCfgUseSln.Checked;
            userCfg.save();
            saveData(true);

            App.ConfigManager.switchOn((menuCfgUseSln.Checked) ? ContextType.Solution : ContextType.Common);
            updateFormData();
        }

        private void menuCfgClone_Click(object sender, EventArgs e)
        {
            logic.cloneCfg((App.ConfigManager.Context == ContextType.Solution) ? ContextType.Common : ContextType.Solution);
            updateFormData();
            notice(true);
        }

        private void toolStripMenuSettings_DropDownOpening(object sender, EventArgs e)
        {
            menuCfgClone.Text = String.Format("Clone from {0} config", (App.ConfigManager.Context == ContextType.Solution) ? "Common" : "Solution");

            if(!logic.Env.IsOpenedSolution) {
                menuCfgUseSln.Enabled   = false;
                menuCfgClone.Enabled    = false;
                return;
            }
            menuCfgUseSln.Enabled   = true;
            menuCfgClone.Enabled    = true;

            if(!App.ConfigManager.IsExistCfg(ContextType.Solution)) {
                return;
            }

            if(App.ConfigManager.getConfigFor(ContextType.Solution).InRAM) {
                return;
            }
            menuCfgUseSln.Checked = !App.ConfigManager.getUserConfigFor(ContextType.Solution).Data.Global.IgnoreConfiguration;
        }

        private void toolStripMenuBug_DropDownOpening(object sender, EventArgs e)
        {
            toolStripMenuDebugMode.Checked = App.DebugMode;
            
            Func<string, bool> IsIgnoreLevel = (string level) =>
            {
                if(!App.UserConfig.Global.LogIgnoreLevels.ContainsKey(level)) {
                    return false;
                }
                return App.UserConfig.Global.LogIgnoreLevels[level];
            };
            
            menuLogIgnoreTrace.Checked  = IsIgnoreLevel("TRACE");
            menuLogIgnoreDebug.Checked  = IsIgnoreLevel("DEBUG");
            menuLogIgnoreInfo.Checked   = IsIgnoreLevel("INFO");
            menuLogIgnoreWarn.Checked   = IsIgnoreLevel("WARN");
            menuLogIgnoreError.Checked  = IsIgnoreLevel("ERROR");
        }

        private void onSolutionChanged(object sender, EventArgs e)
        {
            updateFormData();
        }
        
        private void dataGridViewOutput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            removeRow(dgvOWP, owpRemove, e);
        }

        private void dgvCEFilters_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            removeRow(dgvCEFilters, dgvCEFiltersColumnRemove, e);
        }

        private void dgvEnvCmd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            removeRow(dgvEnvCmd, dgvEnvCmdColumnRemove, e);
        }

        private void dgvOperations_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            removeRow(dgvOperations, dgvOpColumnRemove, e);
        }
    }
}