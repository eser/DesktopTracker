using DesktopTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DesktopTracker
{
    public partial class frmMain : Form
    {
        // fields

        private AppFolderManager _appFolderManager;
        private KeyboardHook _keyboardHook;
        private int _lastIndex;

        // constructors

        public frmMain()
        {
            this.InitializeComponent();

            this._lastIndex = 0;

            this._appFolderManager = new AppFolderManager();
            this._keyboardHook = new KeyboardHook();
            this._keyboardHook.KeyPressed += this._keyboardHook_KeyPressed;

            this.Text += string.Format(" {0}", Application.ProductVersion);
        }

        // methods

        private void RegisterHotKeys()
        {
            this._keyboardHook.RegisterHotKey(Helpers.ModifierKeys.Win, Keys.Delete);
        }

        private void UnregisterHotKeys()
        {
            this._keyboardHook.Dispose();
        }

        private ctlCounter AddCounterControl()
        {
            var counter = new ctlCounter()
            {
                Top = 0,
                Left = 0,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,

                Index = this._lastIndex++
            };
            counter.CloseButton += this.Counter_CloseButton;

            this.flowLayoutPanel1.Controls.Add(counter);

            return counter;
        }

        private void RemoveCounterControl(ctlCounter control)
        {
            this.flowLayoutPanel1.Controls.Remove(control);

            this.ReindexAll();
        }

        private void LoadItems()
        {
            var items = this._appFolderManager.LoadItems();

            foreach (var item in items)
            {
                var control = this.AddCounterControl();
                control.Item = item;
            }
        }

        private void SaveItems()
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

        private void ReindexAll()
        {
            this._lastIndex = 0;

            foreach (var control in this.flowLayoutPanel1.Controls)
            {
                var counterControl = control as ctlCounter;

                if (counterControl == null)
                {
                    continue;
                }

                counterControl.Index = this._lastIndex++;
            }
        }

        private void SetFocusControl(int index)
        {
            if (this.flowLayoutPanel1.Controls.Count <= index)
            {
                return;
            }

            var control = this.flowLayoutPanel1.Controls[index] as ctlCounter;

            // control.SetFocus();
            control.Toggle();
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

            this.RemoveCounterControl(sender as ctlCounter);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.LoadItems();
            this.RegisterHotKeys();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SaveItems();
            this.UnregisterHotKeys();
        }

        private void _keyboardHook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            this.Activate();
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.N)
                {
                    this.AddCounterControl();
                    e.Handled = true;

                    return;
                }

                for (var i = 0; i < 10; i++)
                {
                    if (e.KeyCode == Keys.D1 + i)
                    {
                        this.SetFocusControl(i);
                        e.Handled = true;

                        return;
                    }
                }
            }
        }
    }
}
