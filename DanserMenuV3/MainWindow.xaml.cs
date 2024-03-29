﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using SourceChord.FluentWPF;
using System.Threading;
using System.Globalization;
using System.Windows.Media;

namespace DanserMenuV3
{
	public partial class MainWindow : Window
	{
		readonly Utils _utils = new Utils();
		SettingsWindow settingsWindow;

		public MainWindow()
		{
			InitializeComponent();

			if (!File.Exists("danser.db") || !File.Exists("settings/default.json"))
			{
				StartDanser(" -md5=0", true); // Checks if danser.db exists and makes danser make it if it doesn't
			}

			settingsWindow = new SettingsWindow(this);

			if (_utils.IsLightTheme() == 1)
			{
				Resources["TextColorLight"] = Brushes.Black;
			}

			if (settingsWindow.ObjectJson != null)
			{
				if (settingsWindow.ObjectJson["Language"].ToString() != "English") // If its not english, change the text
				{
					UpdateCulture(settingsWindow.LanguageCodes[settingsWindow.Languages.ToList().IndexOf(settingsWindow.ObjectJson["Language"].ToString())]);
					settingsWindow.ChangeText();
				}
			}

			var logExists = File.Exists($"{Directory.GetCurrentDirectory()}\\menu.log");
			if (logExists)
			{
				File.Delete("menu.log");
				File.Create("menu.log").Close(); // Clears contents of menu.log
			}

			if (!logExists)
			{
				File.Create("menu.log").Close(); // Makes menu.log if it didn't exist before
			}

			TebSearch_TextChanged(null, null);

		}

		private void TebSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				CobMaps.Items.Clear();
				using var connection = new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\danser.db"); // Connects to danser.db
				connection.Open();

				var command = connection.CreateCommand();
				command.CommandText =
					$@"
				SELECT *
				FROM beatmaps
				WHERE dir LIKE '%{TebSearch.Text}%' AND mode = 0
				OR tags LIKE '%{TebSearch.Text}%' AND mode = 0
                OR version LIKE '%{TebSearch.Text}%' AND mode = 0
                OR mapId LIKE '%{TebSearch.Text}%' AND mode = 0
				"; // Selects maps where the name of the map and tags of it match the query

				var res = Task<SqliteDataReader>.Factory.StartNew(() => { return command.ExecuteReader(); }).Result; // Fetches the data from the request

				while (res.Read())
				{
					var name = $"{res.GetString(3)} [{res.GetString(8)}]"; // Formats the map
					CobMaps.Items.Add(name);
				}

				connection.Close();

				LabelPAM.Text = $"{Strings.PAM} ({Strings.MF} {CobMaps.Items.Count}):";

				if (CobMaps.Items.Count < 50)
				{
					CobMaps.Style = CobMode.Style;
				}
				else
				{
					CobMaps.Style = null;
				}
			}
			catch (Exception ex)
			{
				using var logFile = new StreamWriter("menu.log", true);
				_utils.LogError(logFile, ex);
			}
		}

		private void BuRun_Click(object sender, RoutedEventArgs e)
		{
        try
            {
            var res = GetComboboxMapInfo();
			var selectedItem = (ComboBoxItem)CobMode.SelectedItem;
			var md5 = "";

			if (selectedItem.Content.ToString() == "Replay")
			{
				StartDanser(_utils.FormatCommands(this, md5), false);
				return;
			}

			while (res.Read())
			{
				md5 = res.GetString(21);

			}

			res.Close();

			StartDanser(_utils.FormatCommands(this, md5), false);
        }
		catch (Exception ex)
		{
			using var logFile = new StreamWriter("menu.log", true);
			_utils.LogError(logFile, ex);
		}
	}

		private SqliteDataReader GetComboboxMapInfo()
		{
			var selectedItem = (ComboBoxItem)CobMode.SelectedItem;
			if (CobMaps.Items.Count == 0 || selectedItem.Content.ToString() == "Replay")
			{
				return null;
			}
			var mapName = CobMaps.SelectedItem.ToString().Split('[')[0].Replace("'", "''").Trim();
			var diffName = CobMaps.SelectedItem.ToString().Split('[')[1].Split(']')[0].Replace("'", "''");

			var connection = new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\danser.db");
			connection.Open();
			var command = connection.CreateCommand();
			command.CommandText =
				$@"
					SELECT *
					FROM beatmaps
					WHERE title = '{mapName}'
					AND version = '{diffName}'
				";

			return Task<SqliteDataReader>.Factory.StartNew(() => { return command.ExecuteReader(); }).Result;

		}

		private void CobMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				var utils = new Utils();
				var selectedItem = (ComboBoxItem)CobMode.SelectedItem;
				switch (selectedItem.Content.ToString())
				{
					case "Knockout":

						break;
					case "Play":

						break;
					case "Replay":
						var replayFileDialog = new Microsoft.Win32.OpenFileDialog
						{
							Filter = "osr files (*.osr)|*.osr",
							Title = "Open osr file"
						};
						var result = replayFileDialog.ShowDialog(); // Opens a file dialog to select a replay file

						if (result == true)
						{
							var filename = replayFileDialog.FileName;
							TebCurReplay.Text = filename;

							TebCurReplay.Height = 50;

							if (utils.MeasureString(TebCurReplay).Width < TebCurReplay.ActualWidth)
							{
								TebCurReplay.Height = 36; // Formats the textbox size based on its contents size
							}
						}

						break;
				}
			}
			catch (Exception ex)
			{
				using var logFile = new StreamWriter("menu.log", true);
				_utils.LogError(logFile, ex);
			}
		}

		private void CobMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (e.AddedItems.Count > 0)
                {
					var res = GetComboboxMapInfo();

					if (res != null)
					{
						while (res.Read())
						{
							var maximum = Convert.ToDouble(res.GetString(33)) / 1000;
							SliStartTime.Maximum = maximum;
							SliEndTime.Maximum = maximum;

							ArTextBox.Text = res.GetString(12); // AR Column
							CsTextBox.Text = res.GetString(11); // CS Column
							OdTextBox.Text = res.GetString(26); // OD Column
							HpTextBox.Text = res.GetString(25); // HP Column
						}
					}
					res.Close();
				}
			}
			catch (Exception ex)
			{
				using var logFile = new StreamWriter("menu.log", true);
				_utils.LogError(logFile, ex);
			}
		}

		private void TebCurReplay_TextChanged(object sender, TextChangedEventArgs e)
		{
			var utils = new Utils();
			TebCurReplay.Height = 50;
			if (utils.MeasureString(TebCurReplay).Width < TebCurReplay.ActualWidth) TebCurReplay.Height = 36;
		}

		private void TebSkinName_TextChanged(object sender, TextChangedEventArgs e)
		{
			TebSkinName.Height = 50;
			if (_utils.MeasureString(TebSkinName).Width < TebSkinName.ActualWidth) TebSkinName.Height = 36;
		}

		private void CebRecord_Checked(object sender, RoutedEventArgs e)
		{
			if (ChkRecord.IsChecked == true)
			{
				TebOutName.IsEnabled = true;
			}
		}

		private void CebRecord_Unchecked(object sender, RoutedEventArgs e)
		{
			if (ChkRecord.IsChecked == false)
			{
				TebOutName.IsEnabled = false;
			}
		}

		private void ChkScreenshot_Checked(object sender, RoutedEventArgs e)
		{
			if (ChkScreenshot.IsChecked == true)
			{
				ScreenshotTime.IsEnabled = true;
				TebOutName.IsEnabled = true;
			}
		}

		private void ChkScreenshot_Unchecked(object sender, RoutedEventArgs e)
		{
			if (ChkScreenshot.IsChecked == false)
			{
				ScreenshotTime.IsEnabled = false;
				TebOutName.IsEnabled = false;
			}
		}

		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			var regex = new Regex("[^0-9]+"); // Only allows numbers 0 to 9, eg: 14
			e.Handled = regex.IsMatch(e.Text);
		}

		private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			var regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$"); // Only allows numbers and periods, eg: 0.122
			e.Handled = !regex.IsMatch(e.Text);
		}

		//private void NegativeDecimalValidationTextBox(object sender, TextCompositionEventArgs e)
		//{
		//    var regex = new Regex("^-?[0-9]\\d*(\\.\\d+)?$"); // Only allows numbers, dashes and periods, eg: -2.4 // This does not work for some reason
		//    e.Handled = !regex.IsMatch(e.Text);
		//}

		private void BtnSkinBrowse_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var settingsJson = JObject.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}\settings\default.json"));

				var skinFolderDialog = new System.Windows.Forms.FolderBrowserDialog()
				{
					SelectedPath = settingsJson["General"]["OsuSkinsDir"].ToString()
				};
				var result = skinFolderDialog.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK)
				{
					TebSkinName.Text = skinFolderDialog.SelectedPath.Split('\\').Last();

					TebSkinName.Height = 50;

					if (_utils.MeasureString(TebSkinName).Width < TebSkinName.ActualWidth)
					{
						TebSkinName.Height = 36;
					}
				}
			}
			catch (Exception ex)
			{
				using var logFile = new StreamWriter("menu.log", true);
				_utils.LogError(logFile, ex);
			}
		}

		private void StartDanser(string args, bool firstTime)
		{
			var process = new Process();
			var startInfo = new ProcessStartInfo
			{
				FileName = "danser.exe",
				Arguments = args
			};
			process.StartInfo = startInfo;
			Debug.WriteLine(Directory.GetCurrentDirectory());
			try
			{
				process.Start();
				
				if (firstTime)
                {
					process.WaitForExit();
					TebSearch_TextChanged(null, null);
				}
			}
			catch (Win32Exception)
			{
				MessageBox.Show("danser.exe not found in the same directory as the program!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void BtnUpdateDb_Click(object sender, RoutedEventArgs e)
		{
			StartDanser(" -md5=0", true);
		}

		private void BtnSettingsBrowse_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var utils = new Utils();
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

					TebSettingsName.Height = 50;

					if (utils.MeasureString(TebSettingsName).Width < TebSettingsName.ActualWidth)
					{
						TebSettingsName.Height = 36; // Formats the textbox size based on its contents size
					}
				}
			}
			catch (Exception ex)
			{
				using var logFile = new StreamWriter("menu.log", true);
				_utils.LogError(logFile, ex);
			}
		}

		private void StyleCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			AcrylicWindow.SetEnabled(this, false); // Doesn't work? :(
		}

        private void BuSettings_Click(object sender, RoutedEventArgs e)
		{
			settingsWindow.Show();
			settingsWindow.UpdateValues();
        }

		public void UpdateCulture(string langCode)
        {
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(langCode);
		}

		public void MainClosing(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}
    }
}