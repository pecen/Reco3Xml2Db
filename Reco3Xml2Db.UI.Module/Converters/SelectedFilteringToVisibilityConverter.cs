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
  public class SelectedFilteringToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var v = (FilterableColumns)value;
      var columnName = parameter as string;

      if (columnName == FilterableColumns.PDSource.ToString()
        && v == FilterableColumns.PDSource) {
        return Visibility.Visible;
      }
      else if (columnName == FilterableColumns.ComponentType.ToString()
        && v == FilterableColumns.ComponentType) {
        return Visibility.Visible;
      }
      else if (columnName == FilterableColumns.PDStatus.ToString()
        && v == FilterableColumns.PDStatus) {
        return Visibility.Visible;
      }

      return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
