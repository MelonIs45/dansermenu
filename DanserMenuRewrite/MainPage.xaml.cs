using System;
using System.IO;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Microsoft.Data.Sqlite;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DanserMenuRewrite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

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

        private async void BuRun_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var mapName = CobMaps.SelectedItem.ToString().Split('[')[0].Replace("'", "''").Trim();
            var diffName = CobMaps.SelectedItem.ToString().Split('[')[1].Split(']')[0].Replace("'", "''");
            
            using (var connection = new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\danser.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    $@"
                    SELECT *
                    FROM beatmaps
                    WHERE title = '{mapName}'
                    AND version = '{diffName}'
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var folderPicker = new Windows.Storage.Pickers.FolderPicker();
                        folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                        folderPicker.FileTypeFilter.Add("*");

                        Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                        if (folder != null)
                        {
                            Windows.Storage.AccessCache.StorageApplicationPermissions.
                                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                            TebMd5.Text = "Picked folder: " + folder.Path;

                            var md5 = reader.GetString(21);

                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = "cmd.exe",
                                Arguments = $"/K danser.exe -md5={md5}",
                                WorkingDirectory = folder.Path
                            };
                            process.StartInfo = startInfo;
                            process.Start();
                        }
                        else
                        {
                            TebMd5.Text = "Operation cancelled.";
                        }
                    }
                }
            }

            
        }

        private void CobMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
