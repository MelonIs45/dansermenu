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
    }
}
