using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Reco3Xml2Db.Utilities.Extensions {
  public static class GridExtensions {
    public static DataGridRow GetSelectedRow(this DataGrid grid) {
      return (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
    }

    public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int column) {
      if (row != null) {
        DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

        if (presenter == null) {
          grid.ScrollIntoView(row, grid.Columns[column]);
          presenter = GetVisualChild<DataGridCellsPresenter>(row);
        }

        DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
        return cell;
      }
      return null;
    }

    public static T GetVisualChild<T>(Visual parent) where T : Visual {
      T child = default;
      int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
      for (int i = 0; i < numVisuals; i++) {
        Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
        child = v as T;
        if (child == null) {
          child = GetVisualChild<T>(v);
        }
        if (child != null) {
          break;
        }
      }
      return child;
    }
  }
}
