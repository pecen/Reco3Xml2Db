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
    public IList<IEnumerable<string>> GetSelectedRowsData(DataGrid grid, bool includeHeader = false) {
      throw new NotImplementedException();
    }

    public string GetText(DataGridCell cell) {
      if (cell != null) { 
        string result = string.Empty;

        TextBlock provider = cell.Content as TextBlock;

        if (provider.IsNull()) {
          //return cell.Value.ToString();
          return cell.Content.ToString();
        }

        // Handle when provider is Hyperlink
        //if (provider is Hyperlink) {
        //  Hyperlink link = cell.Content as Hyperlink;
        //  //provider.Text = link as TextBlock;
        //  provider.IfNotNull(() => result = provider.Text);
        //  return result;
        //}

        if (provider is TextBlock && provider.Visibility == Visibility.Visible) {
          return provider.Text;
        }
      }

      return string.Empty;
    }
  }
}
