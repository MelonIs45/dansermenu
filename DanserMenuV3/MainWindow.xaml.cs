using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;

namespace DanserMenuV3
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TebSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TebSearch.Text.Length > 3)
            {
                CobMaps.Items.Clear();
                using var connection =
                    new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\danser.db");
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

                var sqlTask = Task<SqliteDataReader>.Factory.StartNew(() => { return command.ExecuteReader(); });
                var res = sqlTask.Result;

                while (res.Read())
                {
                    var name = $"{res.GetString(3)} [{res.GetString(8)}]";

                    CobMaps.Items.Add(name);
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

            var sqlTask = Task<SqliteDataReader>.Factory.StartNew(() => { return command.ExecuteReader(); });
            var res = sqlTask.Result;

            while (res.Read())
            {
                var md5 = res.GetString(21);

                var process = new Process();
                var startInfo = new ProcessStartInfo
                {
                    FileName = "danser.exe",
                    Arguments = $"-md5={md5}"
                };
                process.StartInfo = startInfo;

                try
                {
                    process.Start();
                }
                catch (Win32Exception)
                {
                    MessageBox.Show("danser.exe not found in the same directory as the program!");
                }

                TebMd5.Text = $"Command: {startInfo.Arguments}";
            }
        }

        private void CobMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var utils = new Utils();
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
                    var replayFileDialog = new OpenFileDialog
                    {
                        Filter = "osr files (*.osr)|*.osr",
                        Title = "Open osr file"
                    };
                    var result = replayFileDialog.ShowDialog();

                    if (result == true)
                    {
                        var filename = replayFileDialog.SafeFileName;
                        TebCurReplay.Text = filename;

                        TebCurReplay.Height = 50;

                        if (utils.MeasureString(TebCurReplay).Width < TebCurReplay.ActualWidth)
                            TebCurReplay.Height = 36;
                    }

                    break;
            }
        }

        private void TebCurReplay_TextChanged(object sender, TextChangedEventArgs e)
        {
            var utils = new Utils();
            TebCurReplay.Height = 50;
            if (utils.MeasureString(TebCurReplay).Width < TebCurReplay.ActualWidth) TebCurReplay.Height = 36;
        }

        private void CebRecord_Checked(object sender, RoutedEventArgs e)
        {
            if (CebRecord.IsChecked == true)
            {
                TebOutName.IsEnabled = true;
            }
        }

        private void CebRecord_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CebRecord.IsChecked == false)
            {
                TebOutName.IsEnabled = false;
            }
        }
    }
}