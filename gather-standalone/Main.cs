using System;
using System.Threading;
using System.Windows.Forms;

namespace gather_standalone
{
    public partial class Main : Form
    {
        private Thread send_thread;
        private Guid guid;
        public Main()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.FormClosing += Form_FormClosing;
            // this.Resize += new EventHandler(this.Main_Resize);
        }


        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.send_thread.Abort();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.guid = Properties.Settings.Default.Guid;
            if (this.guid == new Guid())
            {
                this.guid = Guid.NewGuid();
                Properties.Settings.Default.Guid = this.guid;
                Properties.Settings.Default.Save();
            }
            IdLabel.Text = this.guid.ToString();

            this.send_thread = new Thread(new ThreadStart(SendServerInfo));
            this.send_thread.Start();
        }

        private void SendServerInfo()
        {
            while (true)
            {
                GameReader.CurrentServerReader current_server_reader = new GameReader.CurrentServerReader();
                if (current_server_reader.hasResults)
                {
                    if (current_server_reader.PlayerLists_All.Count > 0)
                    {
                        try
                        {
                            Api.PostPlayerlist(current_server_reader, this.guid);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                Thread.Sleep(10000);
            }
        }

        private void IdLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(IdLabel.Text);
        }
        
        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            TrayIcon.Visible = false;
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            //hide it from the task bar  
            //and show the system tray icon (represented by the NotifyIcon control) 
            Hide();
            TrayIcon.Visible = true;
            TrayIcon.ShowBalloonTip(1000);
        }
    }
}
