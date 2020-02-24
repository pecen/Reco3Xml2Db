using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters {
  public class StatusIntToEnumValueConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var s = parameter.ToString();
      string enumValue = string.Empty;
      string desc = string.Empty;

      if (!string.IsNullOrEmpty(s)) {
        s = s.Substring(s.LastIndexOf('.') + 1);
        switch (s) {
          case "PDSource":
            desc = ((PDSource)value).GetDescription();
            enumValue = string.IsNullOrEmpty(desc) ? ((PDSource)value).ToString() : desc;
            break;
          case "PDStatus":
            desc = ((PDStatus)value).GetDescription();
            enumValue = string.IsNullOrEmpty(desc) ? ((PDStatus)value).ToString() : desc;
            break;
          case "ComponentType":
            desc = ((ComponentType)value).GetDescription();
            enumValue = string.IsNullOrEmpty(desc) ? ((ComponentType)value).ToString() : desc;
            break;
        }
      }

      return enumValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
