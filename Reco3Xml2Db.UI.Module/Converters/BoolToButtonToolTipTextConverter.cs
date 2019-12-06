using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters
{
  public class BoolToButtonToolTipTextConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return (bool)value
        ? EnumExtensions.GetEnumDescription(ToolTipText.UpdateButtonToolTip)
        : EnumExtensions.GetEnumDescription(ToolTipText.ImportButtonToolTip);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
