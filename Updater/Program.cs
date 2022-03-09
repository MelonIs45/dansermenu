using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;

namespace DanserMenuV3
{
    public class UpdaterHelper
    {
        string AssetUrl { get; set; }
        int ProcessId { get; set; }
        public UpdaterHelper(string url, int processId)
        {
            AssetUrl = url;
            ProcessId = processId;
        }

        public async Task DownloadAsset()
        {
            Process processRunning = Process.GetProcessById(ProcessId);
            processRunning.Kill();

            Console.WriteLine("Danser Menu process killed");

            WebClient webClient = new WebClient();
            webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
            webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            Console.WriteLine("Downloading latest release");
            await webClient.DownloadFileTaskAsync(new Uri(AssetUrl), "temp.zip");

            ExtractAsset();
        }

        private void ExtractAsset()
        {
            Console.WriteLine("Extracting release");
            ZipFile.ExtractToDirectory($"temp.zip", Directory.GetCurrentDirectory(), true);
            Console.WriteLine("Deleting temporary zip file");
            File.Delete($"temp.zip");
            Console.WriteLine("Copying files");
            CopyFilesRecursively($"{Directory.GetCurrentDirectory()}\\release", Directory.GetCurrentDirectory());
            Console.WriteLine("Preparing to launch menu...");
            LaunchMenu();
        }

        private void LaunchMenu()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "DANSER Menu.exe",
                UseShellExecute = true,
                WorkingDirectory = Directory.GetCurrentDirectory()
            };

            process.StartInfo = startInfo;
            Console.WriteLine("Menu Launched");
            process.Start(); // Starts the menu
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }

    public class Updater
    {
        static void Main(string[] args) 
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            string url = args[0];
            int processId = Convert.ToInt32(args[1]);

            UpdaterHelper updater = new UpdaterHelper(url, processId);

            await updater.DownloadAsset();
        }


    }
}