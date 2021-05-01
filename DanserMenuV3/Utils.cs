using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Size = System.Drawing.Size;

namespace DanserMenuV3
{
    class Utils
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

            return new Size((int)formattedText.Width, (int)formattedText.Height);
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
                command += $" -{mainWindow.CobMode.Text.ToLower()}";
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
                command += $" -mods={mainWindow.TebMods.Text}";
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
                command += $" - debug";
            }

            return command;
        }

        public string FormatNumberArgs(MainWindow mainWindow)
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

            return numberArgs;
        }
    }
}
