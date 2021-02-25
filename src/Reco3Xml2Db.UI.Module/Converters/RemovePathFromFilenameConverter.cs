using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters {
  public class RemovePathFromFilenameConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      //return Path.GetFileName((string)value);

      var path = value as string;

      if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
        return Path.GetFileName(path);
      }

      return path?.TrimEnd(new char[] { '\\' });
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      return value;
    }
  }
}
