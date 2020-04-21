using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reco3Xml2Db.UI.Module.Commands {
  public class CopyToClipboardCommand : DelegateCommand<string> {
    public CopyToClipboardCommand() : base(Execute, CanExecute) {
    }

    public new static bool CanExecute(string parameter) {
      return !string.IsNullOrEmpty(parameter);
    }

    /// <summary>
    /// Executes the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    public new static void Execute(string parameter) {
      lock (new object()) {
        if (!string.IsNullOrEmpty(parameter)) {
          Clipboard.Clear();
          Clipboard.SetText(parameter.Trim() + Environment.NewLine);
        }
      }
    }
  }
}
