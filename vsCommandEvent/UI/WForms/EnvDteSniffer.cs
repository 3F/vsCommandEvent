/*!
 * Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) vsCommandEvent contributors https://github.com/3F/vsCommandEvent/graphs/contributors
 * Licensed under the GNU LGPLv3.
 * See accompanying LICENSE file or visit https://github.com/3F/vsCommandEvent
*/

using System;
using System.Windows.Forms;
using net.r_eg.SobaScript;
using net.r_eg.vsCE.Extensions;
using net.r_eg.vsCE.UI.WForms.Controls;
using CEAfterEventHandler = net.r_eg.vsCE.Events.AggregatedEventsEnvDte.AfterExecuteEventHandler;
using CEBeforeEventHandler = net.r_eg.vsCE.Events.AggregatedEventsEnvDte.BeforeExecuteEventHandler;

namespace net.r_eg.vsCE.UI.WForms
{
    public partial class EnvDteSniffer: Form
    {
        /// <summary>
        /// Used environment
        /// </summary>
        protected IEnvironment env;

        /// <summary>
        /// Transport support
        /// </summary>
        protected ITransfer link;

        /// <summary>
        /// Size of buffer for existing records.
        /// </summary>
        protected int rcBuffer = 2048;

        private readonly object sync = new();

        public void attachCommandEvents(CEBeforeEventHandler before, CEAfterEventHandler after)
        {
            if(env?.AggregatedEvents == null) return;
            lock(sync)
            {
                detachCommandEvents(before, after);
                env.AggregatedEvents.BeforeExecute += before;
                env.AggregatedEvents.AfterExecute  += after;
            }
        }

        public void detachCommandEvents(CEBeforeEventHandler before, CEAfterEventHandler after)
        {
            if(env?.AggregatedEvents == null) return;
            lock(sync)
            {
                env.AggregatedEvents.BeforeExecute -= before;
                env.AggregatedEvents.AfterExecute  -= after;
            }
        }

        public EnvDteSniffer(IEnvironment env, ITransfer link)
        {
            this.env    = env;
            this.link   = link;

            InitializeComponent();
            Icon = Resource.Package_32;

            chkPin.Checked = true;
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

            if(dgvCESniffer.Rows.Count > rcBuffer) {
                dgvCESniffer.Rows.RemoveAt(0);
            }

            string tFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern + " .fff";
            dgvCESniffer.Rows.Add(DateTime.Now.ToString(tFormat), pre, guid, id, Value.Pack(customIn), Value.Pack(customOut), Util.enumViewBy(guid, id));
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

        private void btnRaise_Click(object sender, EventArgs e)
        {
            if(dgvCESniffer.Rows.Count < 1 || dgvCESniffer.SelectedRows.Count < 1) {
                return;
            }

            int id = -1;
            string guid = null;

            try
            {
                foreach(DataGridViewRow rc in dgvCESniffer.SelectedRows)
                {
                    object cId      = rc.Cells[dgvCESnifferColumnId.Name].Value;
                    object cGuid    = rc.Cells[dgvCESnifferColumnGuid.Name].Value;
                    object cIn      = Value.PackArgument(rc.Cells[dgvCESnifferColumnCustomIn.Name].Value);
                    object cOut     = Value.PackArgument(rc.Cells[dgvCESnifferColumnCustomOut.Name].Value);

                    object customIn     = cIn.IsNullOrEmptyString()? string.Empty : cIn;
                    object customOut    = cOut.IsNullOrEmptyString()? string.Empty : cOut;

                    id      = Convert.ToInt32(cId);
                    guid    = cGuid.ToString().Trim();

                    env.raise(guid, id, ref customIn, ref customOut);
                }
            }
            catch(Exception ex)
            {
                Log.Error($"Failed raise command '{guid}', '{id}': {ex.Message}");
            }
        }

        private void btnAddToFilters_Click(object sender, EventArgs e)
        {
            if(dgvCESniffer.Rows.Count < 1 || dgvCESniffer.SelectedRows.Count < 1) {
                return;
            }

            foreach(DataGridViewRow rc in dgvCESniffer.SelectedRows)
            {
                link.command
                (
                    Convert.ToBoolean(rc.Cells[dgvCESnifferColumnPre.Name].Value),
                    (string)rc.Cells[dgvCESnifferColumnGuid.Name].Value,
                    Convert.ToInt32(rc.Cells[dgvCESnifferColumnId.Name].Value),
                    rc.Cells[dgvCESnifferColumnCustomIn.Name].Value,
                    rc.Cells[dgvCESnifferColumnCustomOut.Name].Value,
                    (string)rc.Cells[dgvCESnifferColumnEnum.Name].Value
                );
            }
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

        private void buttonFlush_Click(object sender, EventArgs e) => dgvCESniffer.Rows.Clear();

        private void menuAddToFilters_Click(object sender, EventArgs e) => btnAddToFilters_Click(sender, e);

        private void menuRaise_Click(object sender, EventArgs e) => btnRaise_Click(sender, e);

        private void menuFlush_Click(object sender, EventArgs e) => buttonFlush_Click(sender, e);

        private void chkPin_CheckedChanged(object sender, EventArgs e) => TopMost = chkPin.Checked;
    }
}
