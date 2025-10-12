using OsrStudio.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OsrStudio
{
    public class StateToRecordButtonGeometryConverter : OneWayConverter
    {
        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            var icons = ServiceProvider.Get<IIconSet>();

            if (Value is RecorderState state)
            {
                return Geometry.Parse(state == RecorderState.NotRecording
                    ? icons.Record
                    : icons.Stop);
            }

            return Binding.DoNothing;
        }
    }
}