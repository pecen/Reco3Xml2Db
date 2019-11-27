using System;
using System.Globalization;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters {
  public class BoolToOpacityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return ((bool)value == true ? 100 : 50);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
