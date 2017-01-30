using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopTracker
{
    public partial class frmMain : Form
    {
        // fields

        private AppFolderManager _appFolderManager;

        // constructors

        public frmMain()
        {
            this.InitializeComponent();

            this._appFolderManager = new AppFolderManager();
        }

        // methods

        private ctlCounter AddCounterControl()
        {
            var counter = new ctlCounter()
            {
                Top = 0,
                Left = 0,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            counter.CloseButton += this.Counter_CloseButton;

            this.flowLayoutPanel1.Controls.Add(counter);

            return counter;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.AddCounterControl();
        }

        private void Counter_CloseButton(object sender, EventArgs e)
        {
            if (MessageBox.Show("Emin misiniz?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            this.flowLayoutPanel1.Controls.Remove(sender as Control);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var items = this._appFolderManager.LoadItems();

            foreach (var item in items)
            {
                var control = this.AddCounterControl();
                control.Item = item;
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            var items = new List<CounterItem>();

            foreach (var control in this.flowLayoutPanel1.Controls)
            {
                var counterControl = control as ctlCounter;

                if (counterControl == null)
                {
                    continue;
                }

                items.Add(counterControl.Item);
            }

            this._appFolderManager.SaveItems(items);
        }
    }
}
