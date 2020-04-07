using Reco3Xml2Db.UI.Module.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters {
  public class SelectedComponentFilteringToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var v = (FilterableComponentColumns)value;
      var columnName = (FilterableComponentColumns)parameter;

      if (columnName == FilterableComponentColumns.PDSource 
        && v == FilterableComponentColumns.PDSource) {
        return Visibility.Visible;
      }
      else if (columnName == FilterableComponentColumns.ComponentType 
        && v == FilterableComponentColumns.ComponentType) {
        return Visibility.Visible;
      }
      else if (columnName == FilterableComponentColumns.PDStatus 
        && v == FilterableComponentColumns.PDStatus) {
        return Visibility.Visible;
      }

      return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
