using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters {
  public class EmptyStringToTooltipConverter : IMultiValueConverter {
    //public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
    //  if(string.IsNullOrEmpty(value as string)) {
    //    return parameter as string;
    //  }

    //  return value;
    //}

    //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
    //  throw new NotImplementedException();
    //}
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      var value = values[0] as string;

      if (string.IsNullOrEmpty(value) || !Path.IsPathRooted(value)) {
        return values[1] as string;
      }

      return values[0];
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
