using Microsoft.Data.ConnectionUI;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Reco3Xml2Db.UI.Module.Commands;
using Reco3Xml2Db.UI.Module.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.UI.Module.Services {
  public class PathProvider : IPathProvider {
    private IEventAggregator _eventAggregator;
    public DelegateCommand GetFilenameCommand { get; set; }

    public PathProvider(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;
    }

    // Publishes the filename through the eventaggregator and returns
    // the Xml-stream
    public Stream FilePathService(string initialDirectory = "") {
      OpenFileDialog openFileDialog = new OpenFileDialog {
        InitialDirectory = initialDirectory,
        Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*",
        FilterIndex = 1,
        RestoreDirectory = true
      };

      if ((bool)openFileDialog.ShowDialog()) {
        _eventAggregator.GetEvent<GetFilenameCommand>()
          .Publish(openFileDialog.FileName);

        return openFileDialog.OpenFile();
      }

      return null;
    }

    // Publishes the filepath through the eventaggregator 
    public void FolderPathService(string initialDirectory = "") {
      var dialog = new CommonOpenFileDialog {
        InitialDirectory = initialDirectory,
        IsFolderPicker = true
      };

      if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
        // dialog.FileName below holds the path and not the filename
        // even though the property FileName is used. 
        _eventAggregator.GetEvent<GetFilePathCommand>()
          .Publish(dialog.FileName);
      }
    }

    public void DbPathService() {
      _eventAggregator
        .GetEvent<GetDbCommand>()
        .Publish(GetDbDialog());
    }

    public Dictionary<string, string> GetDbDialog() {
      DataConnectionDialog dcd = new DataConnectionDialog();
      //DataConnectionConfiguration dcs = new DataConnectionConfiguration(null); 
      //dcs.LoadConfiguration(dcd);

      // The following is used instead of implementing a LoadConfiguration() method
      dcd.DataSources.Add(DataSource.SqlDataSource);
      dcd.DataSources.Add(DataSource.OracleDataSource);

      dcd.UnspecifiedDataSource.Providers.Add(DataProvider.SqlDataProvider);
      dcd.UnspecifiedDataSource.Providers.Add(DataProvider.OracleDataProvider);

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
        var c = SplitString(dcd.ConnectionString, ';');
        var i = c[0].IndexOf("=");
        var j = c[1].IndexOf("=");

        string dataSource = string.Empty;
        string initialCatalog = string.Empty;

        if (c[0].StartsWith("Data Source")) {
          dataSource = c[0].Substring(i + 1, c[0].Length - i - 1);
        }

        if (c[1].StartsWith("Initial Catalog")) {
          initialCatalog = c[1].Substring(j + 1, c[1].Length - j - 1);
        }

        return new Dictionary<string, string>() {
          { ConnectionString.DataSource.ToString(), dataSource },
          { ConnectionString.InitialCatalog.ToString(), initialCatalog }
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
      for (int i = 0; i < strarr.Count(); i++) {
        var v = strarr[i].Split(new char[] { '=' });
        list.Add(v[0], v[1]);
      }

      foreach (var item in list) {

      }

      return line.Split(new char[] { c });
    }
  }
}
