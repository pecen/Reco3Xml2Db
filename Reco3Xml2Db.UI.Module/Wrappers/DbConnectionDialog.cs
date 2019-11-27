using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.VisualStudio.Data;

namespace Reco3Xml2Db.UI.Module.Wrappers
{
    public class DbConnectionDialog {
      [DllImport("Microsoft.VisualStudio.Data.dll")]
      static public extern IntPtr DataConnectionDialog();

      [DllImport("C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Microsoft.VisualStudio.Data.dll")]
      static public extern bool ShowDialog();
  }
}
