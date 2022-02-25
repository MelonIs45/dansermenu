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

namespace DanserMenuV3
{
    public partial class SettingsWindow : Window
    {
        public Settings SettingsObject;
        public string SerializedJson;
        public JObject ObjectJson;
        public MainWindow MainWindow;
        public string[] LanguageCodes;
        public string[] Languages;

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

        protected void Window_Closing(object sender, CancelEventArgs e)
        {
            Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
