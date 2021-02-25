using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters
{
  public class BoolToButtonToolTipTextConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return (bool)value
        ? ToolTipText.UpdateButtonToolTip.GetDescription()
        : ToolTipText.ImportButtonToolTip.GetDescription();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
