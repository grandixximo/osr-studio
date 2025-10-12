using OsrStudio.Models;
using System;
using System.Globalization;

namespace OsrStudio
{
    public class StateToTrayIconSourceConverter : OneWayConverter
    {        
        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            switch (Value)
            {
                case RecorderState.Recording:
                    return "/Images/record.ico";

                case RecorderState.Paused:
                    return "/Images/pause.ico";

                default:
                    return "/Images/Captura.ico";
            }
        }
    }
}