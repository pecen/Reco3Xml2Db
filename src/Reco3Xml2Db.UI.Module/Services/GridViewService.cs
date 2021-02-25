using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Reco3Xml2Db.UI.Module.Services {
  public class GridViewService : IGridViewService {
    public string GetText(DataGridCell cell) {
      if (cell != null) { 
        string result = string.Empty;

        TextBlock provider = cell.Content as TextBlock;

        if (provider.IsNull()) {
          return cell.Content.ToString();
        }

        if (provider is TextBlock && provider.Visibility == Visibility.Visible) {
          return provider.Text;
        }
      }

      return string.Empty;
    }

    /// <summary>
    /// Orders the SelectedItems in the grid to the same
    /// order as is displayed in the view
    /// </summary>
    /// <param name="gridView">The grid</param>
    /// <returns>The ordered collection</returns>
    private static IEnumerable<object> OrderSelectedItems(DataGrid gridView) {
      return gridView.Items.Cast<object>().Where(t => gridView.SelectedItems.Contains(t)).ToList();
    }

    public IList<IEnumerable<string>> GetSelectedRowsData(DataGrid grid, bool includeHeader = false) {
      throw new NotImplementedException();
    }
  }
}
