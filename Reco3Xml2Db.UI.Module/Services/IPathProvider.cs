using System.IO;

namespace Reco3Xml2Db.UI.Module.Services {
  public interface IPathProvider {
    Stream FilePathService(string initialDirectory = "");
    void FolderPathService(string initialDirectory = "");
  }
}
