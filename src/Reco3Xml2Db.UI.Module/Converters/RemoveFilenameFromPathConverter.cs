using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters {
  public class RemoveFilenameFromPathConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var v = value as string;
      if (!string.IsNullOrEmpty(v)) {
        return Path.GetDirectoryName((string)value);
      }
      else {
        return string.Empty;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
