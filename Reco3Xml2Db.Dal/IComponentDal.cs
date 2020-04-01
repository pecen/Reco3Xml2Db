using Reco3Xml2Db.Dal.Dto;
using System.Collections.Generic;

namespace Reco3Xml2Db.Dal {
  public interface IComponentDal {
    List<ComponentDto> Fetch();
    ComponentDto Fetch(int componentId);
    ComponentDto Fetch(string pdNumber);
    List<ComponentDto> FetchAllWSamePDNumber(string pdNumber);
    void Insert(ComponentDto data);
    void Update(ComponentDto data);
    void Delete(int componentId);
    bool Exists(string pdNumber);
  }
}
