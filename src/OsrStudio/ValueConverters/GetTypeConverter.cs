using System;
using System.Globalization;

namespace OsrStudio
{
    public class GetTypeConverter : OneWayConverter
    {
        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            return Value?.GetType();
        }
    }
}