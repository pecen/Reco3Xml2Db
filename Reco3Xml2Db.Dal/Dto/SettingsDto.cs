namespace Reco3Xml2Db.Dal.Dto {
  public class SettingsDto {
    public string Server { get; set; }
    public string Database { get; set; }
    public int Authentication { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string XmlFilePath { get; set; }
  }
}
