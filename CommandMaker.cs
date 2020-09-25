using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DiscordRPC;
using DiscordRPC.Logging;
using Newtonsoft.Json.Linq;

namespace DanserMenu.v2
{
    public partial class CommandMaker : Form
    {
        public string command = "";
        public DiscordRpcClient client { get; set; }

        public CommandMaker()
        {
            InitializeComponent();
            comboBox3.Text = "flower";
        }

        public void songAdder(string query)
        {
            JObject settingsJson = JObject.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}\settings.json"));
            string osuSongPath = settingsJson["General"]["OsuSongsDir"].ToString();

            string[] songPath = Directory.GetDirectories(osuSongPath, "*", SearchOption.TopDirectoryOnly);
            foreach (string song in songPath)
            {
                bool contains = song.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains == true)
                {
                    List<string> osuSong = new List<string>(song.Split(new string[] { osuSongPath }, StringSplitOptions.None));
                    comboBox1.Items.Add(osuSong[1]);
                }
                else
                {
                    continue;
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var curDir = Directory.GetCurrentDirectory();
            string[] filePaths = Directory.GetFiles(curDir, "*.exe", SearchOption.TopDirectoryOnly);

            foreach (string file in filePaths) {
                List<string> splitFilePath = new List<string>(file.Split(new string[] { "danser" }, StringSplitOptions.None));
                foreach (string name in splitFilePath)
                {
                    if (name.EndsWith(".exe") != name.Contains("menu"))
                    {
                        command = "danser" + name;
                        label11.Text = command;
                    }
                }
            }

            richTextBox1.Text = command;

            songAdder(textBox1.Text);
        }

        private void OnApplicationExit(Object sender, FormClosingEventArgs e)
        {
            client.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> splitTitle = new List<string>(comboBox1.Text.Split(new string[] { "- " }, 2, StringSplitOptions.None));

            string title = splitTitle[1];

            string extraSettings = "";

            if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                extraSettings = " -fps -debug";
            }
            else if (checkBox1.Checked == true && checkBox2.Checked == false)
            {
                extraSettings = " -fps";
            }
            else if (checkBox1.Checked == false && checkBox2.Checked == true)
            {
                extraSettings = " -debug";
            }

            string commandString = label11.Text + " -t=" + '"' + title + '"' + " -d=" + '"' + comboBox2.Text + '"' + " -cursors=" + numericUpDown1.Value + " -tag=" + numericUpDown4.Value + " -speed=" + numericUpDown2.Value + " -pitch=" + numericUpDown3.Value + " -mover=" + '"' + comboBox3.Text + '"' + extraSettings;
            richTextBox1.Text = commandString;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            songAdder(textBox1.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            JObject settingsJson = JObject.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}\settings.json"));
            string osuSongPath = settingsJson["General"]["OsuSongsDir"].ToString();

            string[] songDiffs = Directory.GetFiles(osuSongPath + comboBox1.Text + "\\", "*.osu", SearchOption.TopDirectoryOnly);

            foreach (string diffs in songDiffs)
            {
                var count = diffs.Count(x => x == '[');
                if (count > 1)
                {
                    List<string> attrs = new List<string>(diffs.Split(new string[] { ") " }, StringSplitOptions.None));
                    string difficultyName = attrs[1];
                    List<string> difficulty = new List<string>(difficultyName.Split(new string[] { "[", "]" }, StringSplitOptions.None));
                    comboBox2.Items.Add(difficulty[1]);
                }
                else
                {
                    List<string> difficulty = new List<string>(diffs.Split(new string[] { "[", "]" }, StringSplitOptions.None));
                    comboBox2.Items.Add(difficulty[1]);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (client != null)
            //{
            //    client.Dispose();
            //}

            List<string> splitTitle = new List<string>(comboBox1.Text.Split(new string[] { "- " }, 2, StringSplitOptions.None));
            string title = splitTitle[1];

            //client = new DiscordRpcClient("727207925222998147");
            //client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            //client.Initialize();

            //client.SetPresence(new RichPresence()
            //{
            //    Details = $"Playing: {title} [{comboBox2.Text}]",
            //    State = $"Cursors: {numericUpDown1.Value}",
            //    Assets = new Assets()
            //    {
            //        LargeImageKey = "menu",
            //        LargeImageText = "DAИSER Menu",
            //        SmallImageKey = "coinbig"
            //    }
            //});

            string strCmdText = "/C" + richTextBox1.Text;
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            
        }

        private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
