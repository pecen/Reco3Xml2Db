using Reco3Xml2Db.Dal.Dto;

namespace Reco3Xml2Db.Dal {
  public interface IAppConfigDal {
    AppConfigDto Fetch();
    void Insert(AppConfigDto data);
    void Update(AppConfigDto data);
  }
}
