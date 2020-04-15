using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.UI.Module.Services {
  public interface IXmlProvider {
    void XmlViewService(string filePath);
    void XmlViewService(Stream xml);
    string GetXmlStringService(Stream xml);
  }
}
