using System;
using System.Windows.Forms;

namespace DesktopTracker
{
    public partial class ctlCounter : UserControl
    {
        // fields

        private int _index;
        private CounterItem _item;
        private Timer _timer;

        // events

        public event EventHandler CloseButton;

        // constructors

        public ctlCounter()
        {
            this.InitializeComponent();

            this._timer = new Timer()
            {
                Interval = 1000,
                Enabled = false
            };
            this._timer.Tick += this._timer_Tick;

            this._item = new CounterItem()
            {
                Label = string.Empty,
                Seconds = 0
            };
        }

        // properties

        public int Index {
            get {
                return this._index;
            }
            set {
                this._index = value;
            }
        }

        public CounterItem Item {
            get {
                return this._item;
            }
            set {
                this._item = value;

                this.txtLabel.Text = this._item.Label;
                this.UpdateSecondLabel();
            }
        }

        // methods

        public void Start()
        {
            this._timer.Start();

            this.btnStart.Enabled = false;
            this.btnPause.Enabled = true;
        }

        public void Pause()
        {
            this._timer.Stop();

            this.btnStart.Enabled = true;
            this.btnPause.Enabled = false;
        }

        public void Toggle()
        {
            if (!this.btnStart.Enabled)
            {
                this.Start();
            }
            else
            {
                this.Pause();
            }
        }

        public void SetFocus()
        {
            // this.Focus();
            this.txtLabel.Focus();
        }

        private void UpdateSecondLabel()
        {
            this.lblCounter.Text = TimeSpan.FromSeconds(this._item.Seconds).ToString();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Start();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            this.Pause();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            this._item.Seconds += 1;

            this.UpdateSecondLabel();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.CloseButton?.Invoke(this, null);
        }

        private void txtLabel_TextChanged(object sender, EventArgs e)
        {
            this._item.Label = this.txtLabel.Text;
        }
    }
}
