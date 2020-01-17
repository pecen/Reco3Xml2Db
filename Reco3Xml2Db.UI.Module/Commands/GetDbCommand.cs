using Microsoft.Data.ConnectionUI;
using Prism.Events;
using Reco3Xml2Db.UI.Module.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Reco3Xml2Db.UI.Module.Commands {
  public class GetDbCommand : PubSubEvent<IDictionary<string, string>> {
    public Dictionary<string, string> GetDbDialog() {
      DataConnectionDialog dcd = new DataConnectionDialog();
      //DataConnectionConfiguration dcs = new DataConnectionConfiguration(null); 
      //dcs.LoadConfiguration(dcd);

      // The following is just temporary instead of LoadConfiguration()
      dcd.DataSources.Add(DataSource.SqlDataSource);
      dcd.DataSources.Add(DataSource.OracleDataSource);

      dcd.UnspecifiedDataSource.Providers.Add(DataProvider.SqlDataProvider);
      dcd.UnspecifiedDataSource.Providers.Add(DataProvider.OracleDataProvider);

      //dcd.SelectedDataSource.

      var dataSources = new Dictionary<string, DataSource> {
        { DataSource.SqlDataSource.Name, DataSource.SqlDataSource },
        { DataSource.OracleDataSource.Name, DataSource.OracleDataSource }
  };

      var dataProviders = new Dictionary<string, DataProvider> {
        { DataProvider.SqlDataProvider.Name, DataProvider.SqlDataProvider },
        { DataProvider.OracleDataProvider.Name, DataProvider.OracleDataProvider }
    };

      var dsName = "MicrosoftSqlServer";
      if (!string.IsNullOrEmpty(dsName) && dataSources.TryGetValue(dsName, out DataSource ds)) {
        dcd.SelectedDataSource = ds;
      }

      string dpName = "System.Data.SqlClient";
      if (!string.IsNullOrEmpty(dpName) && dataProviders.TryGetValue(dpName, out DataProvider dp)) {
        dcd.SelectedDataProvider = dp;
      }

      // End of temporary code

      var result = DataConnectionDialog.Show(dcd);

      if (result == System.Windows.Forms.DialogResult.OK) {

        //  // load tables
        //  using (SqlConnection connection = new SqlConnection(dcd.ConnectionString)) {
        //    connection.Open();
        //    SqlCommand cmd = new SqlCommand("SELECT * FROM sys.Tables", connection);
        //    using (SqlDataReader reader = cmd.ExecuteReader()) {
        //      while (reader.Read()) {
        //        Console.WriteLine(reader.HasRows);
        //      }
        //    }
        //  }

        //dcs.SaveConfiguration(dcd);

        var c = SplitString(dcd.ConnectionString, ';');
        var i = c[0].IndexOf("=");
        var j = c[1].IndexOf("=");

        string dataSource = string.Empty;
        string initialCatalog = string.Empty;

        if(c[0].StartsWith("Data Source")) {
          dataSource = c[0].Substring(i + 1, c[0].Length - i - 1);
        }

        if (c[1].StartsWith("Initial Catalog")) {
          initialCatalog = c[1].Substring(j + 1, c[1].Length - j - 1);
        }

        return new Dictionary<string, string>() {
          { ConnectionString.DataSource.ToString(), dataSource }, // c[0].Substring(i + 1, c[0].Length - i - 1) }, 
          { ConnectionString.InitialCatalog.ToString(), initialCatalog } //  c[1].Substring(j + 1, c[1].Length - j - 1) } 
        };
      }

      return null;
    }

    private string[] SplitString(string line, char c) {
      var strarr = line
        .Split(new char[] { c })
        .Where(ch => !string.IsNullOrEmpty(ch))
        .ToArray();

      var list = new Dictionary<string, string>();
      //var list = new ListDictionary<Dictionary<string, string>>();
      for (int i = 0; i < strarr.Count(); i++) {
        var v = strarr[i].Split(new char[] { '=' });
        list.Add(v[0], v[1]);
      }

      foreach (var item in list) {

      }

      return line.Split(new char[] { c });
    }

    private Dictionary<string, string> HandleItems(string connectionString) {
      var items = SplitString(connectionString, ';');

      foreach (var item in items) {

      }

      return new Dictionary<string, string>();
    }
  }
}
