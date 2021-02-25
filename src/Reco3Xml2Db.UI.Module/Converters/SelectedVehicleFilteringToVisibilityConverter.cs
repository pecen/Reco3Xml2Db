using Reco3Xml2Db.UI.Module.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Reco3Xml2Db.UI.Module.Converters {
  class SelectedVehicleFilteringToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var v = (FilterableVehicleColumns)value;
      var columnName = (FilterableVehicleColumns)parameter;

      if (columnName == FilterableVehicleColumns.VehicleMode 
        && v == FilterableVehicleColumns.VehicleMode) {
        return Visibility.Visible;
      }

      return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
