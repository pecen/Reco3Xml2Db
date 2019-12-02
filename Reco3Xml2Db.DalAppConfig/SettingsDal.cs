using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.DalAppConfig {
  public class SettingsDal : ISettingsDal {
    public SettingsDto Fetch() {
      var connStr = ConfigurationManager.ConnectionStrings["Server"].ConnectionString;
      var dto = HandleItemLine(connStr);

      dto.XmlFilePath = ConfigurationManager.AppSettings["XmlFilePath"];

      return dto;

      //return new AppConfigDto {
      //  Server = ConfigurationManager.AppSettings["DalManagerTypeAuthCode"],
      //  Database = ConfigurationManager.AppSettings["WSBaseUri"],
      //  Authentication = int.Parse(ConfigurationManager.AppSettings["ClientSecret"]),
      //  XmlFilePath = ConfigurationManager.AppSettings["DbInUse"]
      //};
    }

    //public void Insert(AppConfigDto data) {
    //  throw new NotImplementedException();
    //}

    public void Update(SettingsDto data) {
      // Open App.Config of executable
      Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
      // Add Application Settings.
      config.AppSettings.Settings.Remove("XmlFilePath");
      config.AppSettings.Settings.Add("XmlFilePath", data.XmlFilePath);
      // Add the ConnectionString
      var conn = $"Data Source={data.Server};Initial Catalog={data.Database};Integrated Security={(data.Authentication == 0 ? "False" : "True")}";
      config.ConnectionStrings.ConnectionStrings["Server"].ConnectionString = conn;
      config.ConnectionStrings.ConnectionStrings["Server"].ProviderName = "System.Data.SqlClient";
      // Save the configuration file.
      config.Save(ConfigurationSaveMode.Modified);
      // Force a reload of a changed section.
      ConfigurationManager.RefreshSection("appSettings");
      ConfigurationManager.RefreshSection("connectionStrings");
    }

    private SettingsDto HandleItemLine(string line) {
      var strarr = line.Split(new char[] { ';' }).Where(c => !string.IsNullOrEmpty(c)).ToArray();

      ListDictionary list = new ListDictionary();
      //var list = new ListDictionary<Dictionary<string, string>>();
      for(int i = 0; i < strarr.Count(); i++) {
        var v = strarr[i].Split(new char[] { '=' });
        list.Add(v[0],v[1]);
      }



      //var entityType = strarr[0];
      //var key = strarr[1];
      //var content = strarr[2];

      // Newline characters are stated as "<!--NEWLINE-->" in the file
      // Tab characters are stated as "<!--TAB-->"
      //content = content.Replace("<!--NEWLINE-->", Environment.NewLine);
      //content = content.Replace("<!--TAB-->", "\t");

      // Now we have the three relevant things - let developer of receiving system implement what
      // he or she wants...
      //HandleItem(entityType, key, content);

      var auth = (string)list["Integrated Security"];

      return new SettingsDto {
        Server = (string)list["Data Source"],
        Database = (string)list["Initial Catalog"],
        Authentication = auth == "True" ? 1 : 0
      };
    }
  }
}
