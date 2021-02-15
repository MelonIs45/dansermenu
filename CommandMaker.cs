using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;

namespace DanserMenu.v2
{
    public partial class CommandMaker : Form
    {
        public string command = "";
        public bool po = false;
        public bool ko = false;

        public CommandMaker()
        {
            InitializeComponent();
        }

        public void songAdder(string query)
        {
            JObject settingsJson = JObject.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}\settings.json"));
            string osuSongPath = settingsJson["General"]["OsuSongsDir"].ToString();

            string[] songPath = Directory.GetDirectories(osuSongPath, "*", SearchOption.TopDirectoryOnly);
            foreach (string song in songPath)
            {
                bool contains = song.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains)
                {
                    List<string> osuSong = new List<string>(song.Split(new string[] { osuSongPath + "\\" }, StringSplitOptions.None));
                    try { comboBox1.Items.Add(osuSong[1]); }
                    catch { }
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

            foreach (string file in filePaths)
            {
                List<string> splitFilePath = new List<string>(file.Split(new string[] { "\\" }, StringSplitOptions.None));

                try { comboBox4.Items.Add(splitFilePath[splitFilePath.Count - 1]); }
                catch { }
            }

            richTextBox1.Text = command;

            songAdder(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> splitTitle = new List<string>(comboBox1.Text.Split(new string[] { "- " }, 2, StringSplitOptions.None));

            string title = "";
            try { title = splitTitle[1]; }
            catch { }


            string extraSettings = "";

            if (checkBox1.Checked && checkBox2.Checked)
            {
                extraSettings = " -fps -debug";
            }
            else if (checkBox1.Checked && checkBox2.Checked == false)
            {
                extraSettings = " -fps";
            }
            else if (checkBox1.Checked == false && checkBox2.Checked)
            {
                extraSettings = " -debug";
            }

            string spo;
            if (po)
            {
                spo = " -play";
            }
            else if (ko)
            {
                spo = " -knockout";
            }
            else
            {
                spo = $"{formatExtraCommands(numericUpDown1.Value, numericUpDown4.Value, numericUpDown2.Value, numericUpDown3.Value)}{extraSettings}";
            }

            string commandString = $"{label11.Text} -t=\"{title}\" -d=\"{comboBox2.Text}\"{spo}";
            richTextBox1.Text = commandString;
        }

        private string formatExtraCommands(decimal cursors, decimal tag, decimal speed, decimal pitch)
        {
            string extraCommand = "";

            if (cursors != 1)
            {
                extraCommand += $" -cursors={cursors}";
            }
            if (tag != 1)
            {
                extraCommand += $" -tag={tag}";
            }
            if (speed != 1)
            {
                extraCommand += $" -speed={speed}";
            }
            if (pitch != 1)
            {
                extraCommand += $" -pitch={pitch}";
            }

            return extraCommand;
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

            string[] songDiffs = Directory.GetFiles(osuSongPath + "\\" + comboBox1.Text + "\\", "*.osu", SearchOption.TopDirectoryOnly);

            foreach (string diffs in songDiffs)
            {
                var count = diffs.Count(x => x == '[');
                if (count > 1)
                {
                    List<string> attrs = new List<string>(diffs.Split(new string[] { ") " }, StringSplitOptions.None));
                    string difficultyName = attrs[1];
                    List<string> difficulty = new List<string>(difficultyName.Split(new string[] { "[", "]" }, StringSplitOptions.None));
                    try { comboBox2.Items.Add(difficulty[1]); }
                    catch { }
                }
                else
                {
                    List<string> difficulty = new List<string>(diffs.Split(new string[] { "[", "]" }, StringSplitOptions.None));
                    try { comboBox2.Items.Add(difficulty[1]); }
                    catch { }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strCmdText = "/C" + richTextBox1.Text;
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) { label11.Text = comboBox4.Text; }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked) { po = true; }
            else { po = false; }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { ko = true; }
            else { ko = false; }
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

        private void label10_Click(object sender, EventArgs e)
        {

        }

        
    }
}
