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

        public void SongAdder(string query)
        {
            var settingsJson = JObject.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}\settings.json"));
            var osuSongPath = settingsJson["General"]["OsuSongsDir"].ToString();

            var songPath = Directory.GetDirectories(osuSongPath, "*", SearchOption.TopDirectoryOnly);
            foreach (var song in songPath)
            {
                var contains = song.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains)
                {
                    var osuSong = new List<string>(song.Split(new string[] { osuSongPath + "\\" }, StringSplitOptions.None));
                    try { comboBox1.Items.Add(osuSong[1]); }
                    catch
                    {
                        // ignored
                    }
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
            var filePaths = Directory.GetFiles(curDir, "*.exe", SearchOption.TopDirectoryOnly);
            var settingsJson = JObject.Parse(File.ReadAllText($@"{curDir}\settings.json"));

            foreach (var file in filePaths)
            {
                var splitFilePath = new List<string>(file.Split(new string[] { "\\" }, StringSplitOptions.None));

                try { comboBox4.Items.Add(splitFilePath[splitFilePath.Count - 1]); }
                catch
                {
                    // ignored
                }
            }

            richTextBox1.Text = command;
            label14.Text = @$".{settingsJson["Recording"]["Container"]}";

            SongAdder(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var splitTitle = new List<string>(comboBox1.Text.Split(new string[] { "- " }, 2, StringSplitOptions.None));

            var title = "";
            try { title = splitTitle[1]; }
            catch
            {
                // ignored
            }


            var extraSettings = "";
            var recordSettings = "";
            var skinSettings = "";
            var timeSettings = "";
            var modSettings = "";
            
            if (checkBox2.Checked)
            {
                extraSettings = " -debug";
            }

            if (recordButton.Checked)
            {
                if (outputName.Text != "")
                {
                    recordSettings = $" -out=\"{outputName.Text}\"";
                }
                else
                {
                    recordSettings = " -record";
                }
                
            }

            if (SkinName.Text != "")
            {
                skinSettings = $" -skin=\"{SkinName.Text}\"";
            }

            if (startTime.Value != 0)
            {
                timeSettings += $" -start={startTime.Value}";
            }

            if (endTime.Value != 0)
            {
                timeSettings += $" -end={endTime.Value}";
            }

            if (skipBox.Checked)
            {
                timeSettings += " -skip";
            }

            if (modName.Text != "")
            {
                modSettings += $" -mods=\"{modName.Text}\"";
            }

            var spo = "";
            if (po)
            {
                spo = " -play";
            }
            if (ko)
            {
                spo = " -knockout";
            }
            else
            {
                spo = $"{FormatExtraCommands(numericUpDown1.Value, numericUpDown4.Value, numericUpDown2.Value, numericUpDown3.Value)}{extraSettings}";
            }

            var commandString = $"{label11.Text} -t=\"{title}\" -d=\"{comboBox2.Text}\"{spo}{recordSettings}{skinSettings}{modSettings}{timeSettings}";

            if (replayButton.Checked)
            {
                commandString = $"{label11.Text} -r=\"{replayPath.Text}\"{FormatExtraCommands(numericUpDown1.Value, numericUpDown4.Value, numericUpDown2.Value, numericUpDown3.Value)}{recordSettings}{extraSettings}{skinSettings}{modSettings}{timeSettings}";
            }
            
            richTextBox1.Text = commandString;
        }

        private string FormatExtraCommands(decimal cursors, decimal tag, decimal speed, decimal pitch)
        {
            var extraCommand = "";

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
                if (!(modName.Text.Contains("HT") || modName.Text.Contains("DT") || modName.Text.Contains("NC")))
                {
                    extraCommand += $" -speed={speed}";
                }
            }

            if (pitch != 1)
            {
                if (!(modName.Text.Contains("HT") || modName.Text.Contains("DT") || modName.Text.Contains("NC")))
                {
                    extraCommand += $" -pitch={pitch}";
                }
            }

            return extraCommand;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            SongAdder(textBox1.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            var settingsJson = JObject.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}\settings.json"));
            var osuSongPath = settingsJson["General"]["OsuSongsDir"].ToString();

            var songDiffs = Directory.GetFiles(osuSongPath + "\\" + comboBox1.Text + "\\", "*.osu", SearchOption.TopDirectoryOnly);

            foreach (var diffs in songDiffs)
            {
                var count = diffs.Count(x => x == '[');
                if (count > 1)
                {
                    var attrs = new List<string>(diffs.Split(new string[] { ") " }, StringSplitOptions.None));
                    var difficultyName = attrs[1];
                    var difficulty = new List<string>(difficultyName.Split(new string[] { "[", "]" }, StringSplitOptions.None));
                    try { comboBox2.Items.Add(difficulty[1]); }
                    catch
                    {
                        // ignored
                    }
                }
                else
                {
                    var difficulty = new List<string>(diffs.Split(new string[] { "[", "]" }, StringSplitOptions.None));
                    try { comboBox2.Items.Add(difficulty[1]); }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var strCmdText = "/C" + richTextBox1.Text;
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
        
        private void recordButton_CheckedChanged(object sender, EventArgs e)
        {
            if (recordButton.Checked)
            {
                outputName.Enabled = true;
            }
            else
            {
                outputName.Enabled = false;
            }
        }
        
        private void replayButton_CheckedChanged(object sender, EventArgs e)
        {
            if (replayButton.Checked)
            {
                replayPath.Enabled = true;
                browseReplay.Enabled = true;
            }
            else
            {
                replayPath.Enabled = false;
                browseReplay.Enabled = false;
            }
        }

        private void browseReplay_Click(object sender, EventArgs e)
        {
            JObject settingsJson = JObject.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}\settings.json"));

            openFileDialog1 = new OpenFileDialog()
            {
                InitialDirectory = settingsJson["General"]["OsuSongsDir"].ToString().Replace("Songs", "Replays"),
                FileName = "Select a .osr file",
                Filter = @"Osr files (*.osr)|*.osr",
                Title = @"Open osr file"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    replayPath.Text = openFileDialog1.FileName;
                }
                catch
                {
                    // ignored
                }
            }
        }
        
        private void skinButton_Click(object sender, EventArgs e)
        {
            var settingsJson = JObject.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}\settings.json"));

            folderBrowserDialog1 = new FolderBrowserDialog()
            {
                SelectedPath = settingsJson["General"]["OsuSkinsDir"].ToString(),
            };

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SkinName.Text = folderBrowserDialog1.SelectedPath.Split(new char[] {'\\'}).Last();
                }
                catch
                {
                    // ignored
                }
            }
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


        private void label1_Click(object sender, EventArgs e)
        {
            
        }


        
    }
}
