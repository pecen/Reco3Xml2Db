using Microsoft.Data.ConnectionUI;
using Prism.Events;
using Reco3Xml2Db.UI.Module.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Reco3Xml2Db.UI.Module.Commands {
  public class GetDbCommand : PubSubEvent<IDictionary<string, string>> {
  }
}
