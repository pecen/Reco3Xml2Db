using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Reco3Xml2Db.UI.Module.Converters {
  public class BoolToGreyForegroundConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return (bool)value
        ? new SolidColorBrush(Colors.Black) 
        : new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
