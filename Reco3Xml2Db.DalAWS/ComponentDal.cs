using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.DalAWS {
  public class ComponentDal : IComponentDal {
    public void Delete(int componentId) {
      throw new NotImplementedException();
    }

    public bool Exists(string pdNumber) {
      throw new NotImplementedException();
    }

    public List<ComponentDto> Fetch() {
      throw new NotImplementedException();
    }

    public ComponentDto Fetch(int componentId) {
      throw new NotImplementedException();
    }

    public ComponentDto Fetch(string pdNumber) {
      throw new NotImplementedException();
    }

    public List<ComponentDto> FetchAllWSamePDNumber(string pdNumber) {
      throw new NotImplementedException();
    }

    public void Insert(ComponentDto data) {
      throw new NotImplementedException();
    }

    public void Update(ComponentDto data) {
      throw new NotImplementedException();
    }
  }
}
