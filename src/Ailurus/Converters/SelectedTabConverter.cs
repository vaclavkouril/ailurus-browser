using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Ailurus.Converters
{
    /// <summary>
    /// Converts a boolean value indicating the selection status of a tab
    /// to a corresponding background color. This is used to visually differentiate
    /// the selected tab from the others in the UI.
    /// </summary>
    public class SelectedTabConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value indicating whether a tab is selected to a color.
        /// If the tab is selected, it returns a light gray color. If not, it returns a dark gray color.
        /// </summary>
        /// <param name="value">A boolean value indicating whether the tab is selected.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Additional parameter for the converter (not used).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A SolidColorBrush corresponding to the selected or unselected state of the tab.</returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var darkGrey = Color.FromRgb(46, 46, 46);
            if (value is bool isSelected)
            {
                // Return LightGray for selected tabs, DarkGray for unselected tabs
                return new SolidColorBrush(isSelected ? Colors.LightGray : darkGrey);
            }
            // If value is not boolean, return DarkGray as default
            return new SolidColorBrush(darkGrey);
        }

        /// <summary>
        /// Converts back the color to a boolean, but this operation is not supported.
        /// </summary>
        /// <param name="value">The value produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">Additional parameter for the converter (not used).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Throws NotSupportedException because conversion back is not supported.</returns>
        /// <exception cref="NotSupportedException">Thrown always, as conversion back is not supported.</exception>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Converting from color back to selection state is not supported.");
        }
    }
}
