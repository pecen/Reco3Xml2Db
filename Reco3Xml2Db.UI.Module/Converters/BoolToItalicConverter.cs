using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters {
  public class BoolToItalicConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return (bool)value == true ? FontStyles.Normal : FontStyles.Italic;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
