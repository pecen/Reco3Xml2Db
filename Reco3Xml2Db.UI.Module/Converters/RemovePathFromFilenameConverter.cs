﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters {
  public class RemovePathFromFilenameConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return Path.GetFileName((string)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
