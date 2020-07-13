using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Windows.Forms;

namespace DanserMenu.v2
{
    public partial class Launcher : Form
    {
        public DiscordRpcClient client;
        public Launcher()
        {
            InitializeComponent();

            client = new DiscordRpcClient("727207925222998147");
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            client.Initialize();

            client.SetPresence(new RichPresence()
            {
                Details = $"In the menus",
                State = $"",
                Assets = new Assets()
                {
                    LargeImageKey = "menu",
                    LargeImageText = "DAИSER Menu",
                    SmallImageKey = "coinbig"
                }
            });
        }

        private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandMaker mainMenu = new CommandMaker();
            mainMenu.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settingMenu = new Settings();
            settingMenu.Show();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
