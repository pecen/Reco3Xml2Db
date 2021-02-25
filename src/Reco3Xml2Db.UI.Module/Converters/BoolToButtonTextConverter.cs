using Reco3Xml2Db.UI.Module.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters {
  public class BoolToButtonTextConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return (bool)value ? ButtonName.Update.ToString() : ButtonName.Import.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
