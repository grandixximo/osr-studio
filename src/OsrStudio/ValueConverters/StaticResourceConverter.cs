using System;
using System.Globalization;
using System.Windows;

namespace OsrStudio
{
    public class StaticResourceConverter : OneWayConverter
    {
        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            return Value != null ? Application.Current.Resources[Value] : null;
        }
    }
}