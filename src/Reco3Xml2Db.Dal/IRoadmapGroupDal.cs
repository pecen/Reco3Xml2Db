using Reco3Xml2Db.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Dal {
  public interface IRoadmapGroupDal {
    IList<RoadmapGroupDto> Fetch();
    void Delete(int roadmapGroupId);
  }
}
