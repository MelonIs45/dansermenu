using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace DanserMenuV3
{
    public partial class SettingsWindow : Window
    {
        public Settings SettingsObject;
        public string SerializedJson;
        public JObject ObjectJson;
        public JObject DanserJson;
        public MainWindow MainWindow;
        public string[] LanguageCodes;
        public string[] Languages;
        private Utils _utils = new Utils();

        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();


            MainWindow = mainWindow;
            SettingsObject = new Settings();
            LanguageCodes = new string[] { "en", "fr", "es", "pl", "nl", "de", "ru" };
            Languages = new string[] { "English", "Français", "Español", "Polski", "Nederlands", "Deutsch", "Pусский" };

            bool settingsExists = File.Exists($"{Directory.GetCurrentDirectory()}\\menu-settings.json");
            if (!settingsExists) // Default settings
            {
                File.Create("menu-settings.json").Close();
                SerializedJson = Newtonsoft.Json.JsonConvert.SerializeObject(SettingsObject);
                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\menu-settings.json", SerializedJson);
            }

            ObjectJson = JObject.Parse(File.ReadAllText($"{Directory.GetCurrentDirectory()}\\menu-settings.json"));
            DanserJson = JObject.Parse(File.ReadAllText($"{Directory.GetCurrentDirectory()}\\settings\\default.json"));
            //MessageBox.Show(File.ReadAllText($"{Directory.GetCurrentDirectory()}\\settings\\default.json"));

            for (int i = 0; i < Languages.Length; i++)
            {
                CobLanguage.Items.Add(Languages[i]);
            }

            CobLanguage.SelectedIndex = CobLanguage.Items.IndexOf(ObjectJson["Language"].ToString());

        }

        public void ChangeText() // Big BLOB, dont mind this
        {
            MainWindow.LabelMSG.Text = Strings.MSG;
            MainWindow.LabelSFAM.Text = Strings.SFAM;
            MainWindow.LabelPAM.Text = Strings.PAM;
            MainWindow.LabelPAMD.Text = Strings.PAMD;
            MainWindow.LabelCUR.Content = Strings.CUR;
            MainWindow.LabelTAG.Content = Strings.TAG;
            MainWindow.LabelSPEED.Content = Strings.SPEED;
            MainWindow.LabelPITCH.Content = Strings.PITCH;
            MainWindow.LabelRP.Content = Strings.RP;
            MainWindow.LabelST.Content = Strings.ST;
            MainWindow.LabelET.Content = Strings.ET;
            MainWindow.LabelMOD.Content = Strings.MOD;
            MainWindow.LabelSKIN.Content = Strings.SKIN;
            MainWindow.ChkSkip.Content = Strings.SKIP;
            MainWindow.ChkDebug.Content = Strings.DEBUG;
            MainWindow.ChkQuickStart.Content = Strings.QS;
            MainWindow.ChkNoDb.Content = Strings.NDC;
            MainWindow.ChkNoUpdateCheck.Content = Strings.NUC;
            MainWindow.ChkRecord.Content = Strings.REC;
            MainWindow.ChkScreenshot.Content = Strings.SCT;
            MainWindow.LabelCFN.Content = Strings.CFN;
            MainWindow.LabelCS.Content = Strings.CS;
            MainWindow.LabelCM.Content = Strings.CM;
            MainWindow.BtnSkinBrowse.Content = Strings.BROWSE;
            MainWindow.BtnSettingsBrowse.Content = Strings.BROWSE;

            LabelLANG.Content = Strings.LANG;
            BtnSave.Content = Strings.SAVE;
        }

        private void CobLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingsObject.Language = CobLanguage.SelectedItem.ToString();
        }

        private void BuSave_Click(object sender, RoutedEventArgs e)
        {
            bool settingsExists = File.Exists($"{Directory.GetCurrentDirectory()}\\menu-settings.json");
            if (!settingsExists)
            {
                File.Create("menu-settings.json").Close();
            }
            SerializedJson = Newtonsoft.Json.JsonConvert.SerializeObject(SettingsObject);
            File.WriteAllText($"{Directory.GetCurrentDirectory()}\\menu-settings.json", SerializedJson);

            MainWindow.UpdateCulture(LanguageCodes[Languages.ToList().IndexOf(SettingsObject.Language)]);
            ChangeText();
        }

        protected void SettingsClosing(object sender, CancelEventArgs e)
        { 
            Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        private void BtnSettingsBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var settingsFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "setting files (*.json)|*.json",
                    Title = "Open settings file",
                    InitialDirectory = Directory.GetCurrentDirectory() + "\\settings"
                };
                var result = settingsFileDialog.ShowDialog(); // Opens a file dialog to select a settings file

                if (result == true)
                {
                    var filename = settingsFileDialog.FileName;
                    TebSettingsName.Text = filename.Split($"{Directory.GetCurrentDirectory() + "\\settings\\"}")[1].Split(".json")[0];
                    DanserJson = JObject.Parse(File.ReadAllText(filename));
                    //MessageBox.Show(filename + "###" + $"{Directory.GetCurrentDirectory()}\\settings\\default.json");
                    UpdateValues();
                }
            }
            catch (Exception ex)
            {
                using var logFile = new StreamWriter("menu.log", true);
                _utils.LogError(logFile, ex);
            }
        }

        public void UpdateValues()
        {
            TebSongsPath.Text = DanserJson["General"]["OsuSongsDir"].ToString().Replace("\\", "\\\\");
            TebSkinsPath.Text = DanserJson["General"]["OsuSkinsDir"].ToString().Replace("\\", "\\\\");
            ChkDiscordRPC.IsChecked = bool.Parse(DanserJson["General"]["DiscordPresenceOn"].ToString());
            ChkFullscreen.IsChecked = bool.Parse(DanserJson["Graphics"]["Fullscreen"].ToString());
            TBxMasterVolume.Text = DanserJson["Audio"]["GeneralVolume"].ToString();
            TBxMusicVolume.Text = DanserJson["Audio"]["MusicVolume"].ToString();
            TBxHitsoundVolume.Text = DanserJson["Audio"]["SampleVolume"].ToString();
            ChkSkinCursor.IsChecked = bool.Parse(DanserJson["Skin"]["Cursor"]["UseSkinCursor"].ToString());
            TBxBackgroundDim.Text = DanserJson["Playfield"]["Background"]["Dim"]["Normal"].ToString();
            CobKnockoutMode.SelectedIndex = Convert.ToInt32(DanserJson["Knockout"]["Mode"].ToString());
            ChkAddDanser.IsChecked = bool.Parse(DanserJson["Knockout"]["AddDanser"].ToString());
            TebDanserName.Text = DanserJson["Knockout"]["DanserName"].ToString();
            TBxRecordingWidth.Text = DanserJson["Recording"]["FrameWidth"].ToString();
            TBxRecordingHeight.Text = DanserJson["Recording"]["FrameHeight"].ToString();
            TBxRecordingFps.Text = DanserJson["Recording"]["FPS"].ToString();
            TBxRecordingExtension.Text = DanserJson["Recording"]["Container"].ToString();

            if (ChkAddDanser.IsChecked == true)
            {
                TebDanserName.IsEnabled = true;
            }
            else
            {
                TebDanserName.IsEnabled = false;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void BtnSongsBrowse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var songsFolderDialog = new System.Windows.Forms.FolderBrowserDialog()
                {
                    SelectedPath = Directory.GetCurrentDirectory()
                };

                var result = songsFolderDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    TebSongsPath.Text = songsFolderDialog.SelectedPath.Replace("\\", "\\\\");
                }
            }
            catch (Exception ex)
            {
                using var logFile = new StreamWriter("menu.log", true);
                _utils.LogError(logFile, ex);
            }
        }

        private void BtnSkinsBrowse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var songsFolderDialog = new System.Windows.Forms.FolderBrowserDialog()
                {
                    SelectedPath = Directory.GetCurrentDirectory()
                };

                var result = songsFolderDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    TebSkinsPath.Text = songsFolderDialog.SelectedPath.Replace("\\", "\\\\");
                }
            }
            catch (Exception ex)
            {
                using var logFile = new StreamWriter("menu.log", true);
                _utils.LogError(logFile, ex);
            }
        }

        private void ChkAddDanser_Checked(object sender, RoutedEventArgs e)
        {
            if (ChkAddDanser.IsChecked == true)
            {
                TebDanserName.IsEnabled = true;
            }
            else
            {
                TebDanserName.IsEnabled = false;
            }
        }
    }
}
