using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Size = System.Drawing.Size;

namespace DanserMenuV3
{
    internal class Utils
    {
        public Size MeasureString(TextBox textBox)
        {
            var formattedText = new FormattedText(
                textBox.Text + "......", // Make string longer to give it some leeway, 6 periods seems give it a really clean transition
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(textBox.FontFamily, textBox.FontStyle, textBox.FontWeight, textBox.FontStretch),
                textBox.FontSize,
                Brushes.Black,
                new NumberSubstitution(),
                1);

            return new Size((int)formattedText.Width, (int)formattedText.Height); // Returns the size of the string passed into the function
        }

        public string FormatCommands(MainWindow mainWindow, string md5)
        {
            var command = "";

            if (md5.Length > 0)
            {
                command += $" -md5={md5}";
            }

            if (mainWindow.CobMode.Text.ToLower() == "replay")
            {
                command += $" -replay=\"{mainWindow.TebCurReplay.Text}\"";
            }
            else
            {
                if (mainWindow.CobMode.Text.ToLower() != "dance")
                {
                    command += $" -{mainWindow.CobMode.Text.ToLower()}";
                }
            }

            command += FormatNumberArgs(mainWindow);

            if (mainWindow.CebRecord.IsChecked == true && mainWindow.CobMode.Text.ToLower() != "play")
            {
                if (mainWindow.TebOutName.Text != "")
                {
                    command += $" -out=\"{mainWindow.TebOutName.Text}\"";
                }
                else
                {
                    command += $" -record";
                }
            }

            if (mainWindow.TebMods.Text.Length > 0)
            {
                command += $" -mods=\"{mainWindow.TebMods.Text}\"";
            }

            if (mainWindow.ChkSkip.IsChecked == true)
            {
                command += $" -skip";
            }

            if (mainWindow.TebSkinName.Text != "")
            {
                command += $" -skin=\"{mainWindow.TebSkinName.Text}\"";
            }

            if (mainWindow.ChkDebug.IsChecked == true)
            {
                command += $" -debug";
            }

            if (mainWindow.ChkNoDb.IsChecked == true)
            {
                command += $" -nodbcheck";
            }

            if (mainWindow.ChkQuickStart.IsChecked == true)
            {
                command += $" -quickstart";
            }

            using var logFile = new StreamWriter("menu.log", true);
            LogCommand(logFile, command);

            //mainWindow.DEBUGCOMMAND.Text = command;

            return command;
        }

        public string FormatNumberArgs(MainWindow mainWindow)
        {
            try
            {
                var numberArgs = "";

                if (Convert.ToInt32(mainWindow.CursorsTextBox.Text) != 1)
                {
                    numberArgs += $" -cursors={Convert.ToInt32(mainWindow.CursorsTextBox.Text)}";
                }

                if (Convert.ToInt32(mainWindow.TagCursorsTextBox.Text) != 1)
                {
                    numberArgs += $" -tag={Convert.ToInt32(mainWindow.TagCursorsTextBox.Text)}";
                }

                if (Convert.ToDouble(mainWindow.SpeedTextBox.Text) != 1)
                {
                    if (!(mainWindow.TebMods.Text.Contains("HT") || mainWindow.TebMods.Text.Contains("DT") || mainWindow.TebMods.Text.Contains("NC")))
                    {
                        numberArgs += $" -speed={Convert.ToDouble(mainWindow.SpeedTextBox.Text)}";
                    }
                }

                if (Convert.ToDouble(mainWindow.PitchTextBox.Text) != 1)
                {
                    if (!(mainWindow.TebMods.Text.Contains("HT") || mainWindow.TebMods.Text.Contains("DT") || mainWindow.TebMods.Text.Contains("NC")))
                    {
                        numberArgs += $" -pitch={Convert.ToDouble(mainWindow.PitchTextBox.Text)}";
                    }
                }

                if (mainWindow.CobMode.Text != "replay" && mainWindow.CobMode.Text != "knockout")
                {
                    if (mainWindow.ArCheckBox.IsChecked == true)
                    {
                        numberArgs += $" -ar={mainWindow.ArTextBox.Text}";
                    }

                    if (mainWindow.OdCheckBox.IsChecked == true)
                    {
                        numberArgs += $" -od={mainWindow.OdTextBox.Text}";
                    }

                    if (mainWindow.CsCheckBox.IsChecked == true)
                    {
                        numberArgs += $" -cs={mainWindow.CsTextBox.Text}";
                    }

                    if (mainWindow.HpCheckBox.IsChecked == true)
                    {
                        numberArgs += $" -hp={mainWindow.HpTextBox.Text}";
                    }
                }

                if (Convert.ToDouble(mainWindow.StartTextBox.Text) != 0)
                {
                    numberArgs += $" -start={mainWindow.StartTextBox.Text}";
                }

                if (Convert.ToDouble(mainWindow.EndTextBox.Text) != 0)
                {
                    numberArgs += $" -end={mainWindow.EndTextBox.Text}";
                }

                if (mainWindow.ChkScreenshot.IsChecked == true)
                {
                    numberArgs += $" -ss={mainWindow.ScreenshotTime.Text}";
                }

                return numberArgs;
            }
            
            catch (Exception ex)
            {
                using var logFile = new StreamWriter("menu.log", true);
                LogError(logFile, ex);

                return string.Empty;
            }
        }

        public void LogError(StreamWriter logFile, Exception ex)
        {
            logFile.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
            logFile.WriteLine($"Error: {ex}");
            logFile.WriteLine("#######################");
            logFile.WriteLine();

            logFile.Close();
        }

        public void LogCommand(StreamWriter logFile, string command)
        {
            logFile.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
            logFile.WriteLine($"Command ran using: {command}");
            logFile.WriteLine("#######################");
            logFile.WriteLine();

            logFile.Close();
        }
    }
}
