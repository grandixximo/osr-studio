using OsrStudio.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace OsrStudio
{
    public class NotRecordingConverter : OneWayConverter
    {
        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            if (Value is RecorderState state)
                return state == RecorderState.NotRecording;

            return Binding.DoNothing;
        }
    }
}