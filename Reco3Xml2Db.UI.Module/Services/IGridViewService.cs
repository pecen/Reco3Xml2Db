using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Reco3Xml2Db.UI.Module.Services {
  public interface IGridViewService {
    /// <summary>
    /// Gets the text from current cell.
    /// </summary>
    /// <param name="cell">The cell.</param>
    /// <returns>A string with a value if any value exist; Otherwise an empty string.</returns>
    string GetText(DataGridCell cell);

    /// <summary>
    /// Gets the selected rows data.
    /// </summary>
    /// <param name="grid">The grid.</param>
    /// <param name="includeHeader">If set to true include header.</param>
    /// <returns>Selected rows data.</returns>
    IList<IEnumerable<string>> GetSelectedRowsData(DataGrid grid, bool includeHeader = false);
  }
}
