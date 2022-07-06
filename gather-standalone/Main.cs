using System;
using System.Threading;
using System.Windows.Forms;
using DiscordRPC;

namespace gather_standalone
{
    public partial class Main : Form
    {
        bool QuitOnLeaveConfig;
        private Thread send_thread;
        private Guid guid;
        private System.Windows.Forms.Timer QuitTimer;
        private DiscordRpcClient client;
        private bool discord_is_running = false;
        public Main()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.FormClosing += Form_FormClosing;
            this.InitQuitTimer();
        }

        public void InitQuitTimer()
        {
            this.QuitTimer = new System.Windows.Forms.Timer();
            this.QuitTimer.Tick += new EventHandler(QuitTimer_Tick);
            this.QuitTimer.Interval = 10000;
            this.QuitTimer.Start();
        }

        private void QuitTimer_Tick(object sender, EventArgs e)
        {
            if (this.QuitOnLeaveConfig && !Game.IsRunning())
            {
                this.send_thread.Abort();
                this.Close();
                Application.Exit();
            }
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.send_thread.Abort();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.QuitOnLeaveConfig = Properties.Settings.Default.QuitOnLeave;
            QuitOnLeaveOption.Checked = this.QuitOnLeaveConfig;
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

        private void StartStopDiscord()
        {
            if (Game.IsRunning() && !discord_is_running)
            {
                client = new DiscordRpcClient("993783880777744524");
                client.Initialize();
                discord_is_running = true;
            } else if (!Game.IsRunning() && discord_is_running)
            {
                client.Dispose();
                discord_is_running = false;
            }
        }

        private void UpdatePresenceInMenu()
        { 
            if (Game.IsRunning())
            {
                client.SetPresence(new RichPresence()
                {
                    Details = "In the menu's",
                    State = "0 players",
                    Timestamps = new Timestamps()
                    {
                        Start = DateTime.UtcNow.AddSeconds(1)
                    },
                    Assets = new Assets()
                    {
                        LargeImageKey = "bf1",
                        LargeImageText = "Battlefield 1",
                        SmallImageKey = "bf1"
                    }
                });
            }
        }

        private void UpdatePresence(GameReader.CurrentServerReader current_server_reader)
        {
            //Set the rich presence
            //Call this as many times as you want and anywhere in your code.
            client.SetPresence(new RichPresence()
            {
                Details = $"{current_server_reader.ServerName}",
                State = $"{current_server_reader.PlayerLists_All.Count} players",
                Timestamps = new Timestamps()
                {
                    Start = DateTime.UtcNow.AddSeconds(1)
                },
                Assets = new Assets()
                {
                    LargeImageKey = "bf1",
                    LargeImageText = "Battlefield 1",
                    SmallImageKey = "bf1"
                }
            });
        }


        private void SendServerInfo()
        {
            while (true)
            {
                StartStopDiscord();
                GameReader.CurrentServerReader current_server_reader = new GameReader.CurrentServerReader();
                if (current_server_reader.hasResults)
                {
                    if (current_server_reader.PlayerLists_All.Count > 0 && current_server_reader.ServerName != "")
                    {
                        UpdatePresence(current_server_reader);

                        try
                        {
                            Api.PostPlayerlist(current_server_reader, this.guid);
                        }
                        catch (Exception)
                        {

                        }
                    } 
                    else
                    {
                        UpdatePresenceInMenu();
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

        private void QuitOnLeaveOption_CheckedChanged(object sender, EventArgs e)
        {
            this.QuitOnLeaveConfig = QuitOnLeaveOption.Checked;
            Properties.Settings.Default.QuitOnLeave = this.QuitOnLeaveConfig;
            Properties.Settings.Default.Save();
        }
    }
}
