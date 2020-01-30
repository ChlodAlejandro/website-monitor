using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Website_Monitor
{
    public partial class ChangeDialog : Form
    {
        public ChangeDialog()
        {
            InitializeComponent();
            KeyDown += exit_KeyDown;
            colorSwitcheroo();
        }

        public async Task colorSwitcheroo()
        {
            while (true)
            {
                BackColor = Color.Red;
                message.ForeColor = Color.White;
                exit.ForeColor = Color.White;
                await Task.Delay(1000);
                BackColor = Color.White;
                message.ForeColor = Color.Red;
                exit.ForeColor = Color.Red;
                await Task.Delay(1000);
            }
        }

        private void exit_KeyDown(object sender, KeyEventArgs args)
        {
            exitDialog();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            exitDialog();
        }

        private void exitDialog()
        {
            Close();
        }
    }
}
