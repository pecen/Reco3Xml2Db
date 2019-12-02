using Reco3Xml2Db.Dal.Dto;

namespace Reco3Xml2Db.Dal {
  public interface ISettingsDal {
    SettingsDto Fetch();
    //void Insert(AppConfigDto data);
    void Update(SettingsDto data);
  }
}
