﻿using System.Globalization;

namespace Maui.FreakyControls.Converters;

public class ContentToInvisibilityConverter : BaseOneWayValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is not null;
    }
}