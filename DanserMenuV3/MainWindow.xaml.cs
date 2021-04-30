using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;

namespace DanserMenuV3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TebSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CobMaps.Items.Clear();
            using (var connection = new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\danser.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    $@"
                    SELECT *
                    FROM beatmaps
                    WHERE dir LIKE '%{TebSearch.Text}%'
                    OR tags LIKE '%{TebSearch.Text}%'
                    AND mode = 0
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = $"{reader.GetString(3)} [{reader.GetString(8)}]";

                        CobMaps.Items.Add(name);
                    }
                }
            }
        }

        private void BuRun_Click(object sender, RoutedEventArgs e)
        {
            var mapName = CobMaps.SelectedItem.ToString().Split('[')[0].Replace("'", "''").Trim();
            var diffName = CobMaps.SelectedItem.ToString().Split('[')[1].Split(']')[0].Replace("'", "''");

            using var connection = new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\danser.db");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                $@"
                    SELECT *
                    FROM beatmaps
                    WHERE title = '{mapName}'
                    AND version = '{diffName}'
                ";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var md5 = reader.GetString(21);

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = $"danser.exe",
                    Arguments = $"-md5={md5}",
                };
                process.StartInfo = startInfo;

                try
                {
                    process.Start();
                } catch (System.ComponentModel.Win32Exception)
                {
                    MessageBox.Show("danser.exe not found in the same directory as the program!");
                }
                

                TebMd5.Text = $"Command: {startInfo.Arguments}";
                Debug.WriteLine(TebMd5.Text);
            }
        }

        private void CobMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem) CobMode.SelectedItem;
            Debug.WriteLine(selectedItem.Content.ToString());
            switch (selectedItem.Content.ToString())
            {
                case "Knockout":

                    break;
                case "Play":

                    break;
                case "Replay":
                    Debug.WriteLine("TEST");
                    var replayFileDialog = new OpenFileDialog()
                    {
                        Filter = "osr files (*.osr)|*.osr",
                        Title = "Open osr file"
                    };
                    var result = replayFileDialog.ShowDialog();

                    if (result == true)
                    {
                        // Open document 
                        var filename = replayFileDialog.SafeFileName;
                        TebCurReplay.Text = filename;
                    }
                    break;
            }
        }
    }
}
