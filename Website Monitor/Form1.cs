using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Website_Monitor
{
    public partial class frmWebsiteMonitor : Form
    {
        public String siteURL;
        public int frequency;
        public int tests;
        public bool running;

        public frmWebsiteMonitor()
        {
            InitializeComponent();
        }

        private bool changed;

        private async void checkChanges()
        {
            string curDir = Directory.GetCurrentDirectory();
            while (running)
            {
                tests++;
                using (WebClient wc = new WebClient())
                {
                    if (File.Exists("refresh.html"))
                    {
                        status.Text = "Downloading new file...";
                        wc.DownloadFile(new Uri(siteURL), "refresh-temp.html");
                        status.Text = "Checking changes...";
                        if (File.ReadAllText(curDir + "\\refresh-temp.html") == File.ReadAllText(curDir + "\\refresh.html"))
                        {
                            status.Text = "No changes. Cycle " + tests;
                            if (File.Exists("refresh-temp.html")) File.Delete("refresh-temp.html");
                            changed = false;
                        } else
                        {
                            status.Text = "CHANGE DETECTED. " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                            ChangeDialog boomLOL = new ChangeDialog();
                            boomLOL.ShowDialog();
                            if (File.Exists("refresh.html")) File.Delete("refresh.html");
                            File.Copy("refresh-temp.html", "refresh.html");
                            if (File.Exists("refresh-temp.html")) File.Delete("refresh-temp.html");
                            browser.Url = new Uri(siteURL);
                            changed = true;
                        }
                    } else
                    {
                        wc.DownloadFile(new Uri(siteURL), "refresh.html");
                        browser.Url = new Uri(siteURL);
                    }
                }
                await countdownAwait();
            }
        }

        private async Task countdownAwait()
        {
            String time = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            for (int x = frequency; x >= 1; x--)
            {
                if (!changed)
                {
                    status.Text = "No changes as of " + time + ". Next check in " + x + " seconds.";
                }
                await Task.Delay(1000);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (File.Exists("refresh-temp.html"))File.Delete("refresh-temp.html");
            if (File.Exists("refresh.html"))File.Delete("refresh.html");
            if (!running)
            {
                siteURL = txtURL.Text;
                frequency = Int32.Parse(numFreq.Value.ToString());
                running = true;
                checkChanges();
                Text = "Website Monitor - Monitoring: " + siteURL + " per " + frequency + "s";
                btnGo.Text = "END";
            } else
            {
                File.Delete("refresh.html");
                running = false;
                btnGo.Text = "GO";
                Text = "Website Monitor v1.0 by Chlod Aidan Alejandro";
                status.Text = "Ready.";
            }
        }
    }
}
