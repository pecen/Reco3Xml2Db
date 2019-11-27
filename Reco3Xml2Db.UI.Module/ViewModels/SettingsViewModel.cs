using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reco3Xml2Db.Library;
using Reco3Xml2Db.UI.Module.Commands;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.UI.Module.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class SettingsViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;
    public string PageHeader { get; } = "Fill in the configuration settings below";
    public string ServerToolTip { get; } = "Choose a server name or an IP-address";
    public string AuthToolTip { get; } = "Select Authentication method for Sql Server";
    public DelegateCommand SaveSettingsCommand { get; set; }

    private string _server;
    public string Server {
      get { return _server; }
      set {
        SetProperty(ref _server, value);
      }
    }

    private string _dbName;
    public string DbName {
      get { return _dbName; }
      set { SetProperty(ref _dbName, value); }
    }

    private int _authentication;
    public int Authentication {
      get { return _authentication; }
      set {
        SetProperty(ref _authentication, value);
        //_eventAggregator.GetEvent<UpdateFromConfigCommand>().Publish(ClientSecret);
      }
    }

    private string _xmlFilePath;
    public string XmlFilePath {
      get { return _xmlFilePath; }
      set { SetProperty(ref _xmlFilePath, value); }
    }

    private ObservableCollection<string> _authenticationList;
    public ObservableCollection<string> AuthenticationList {
      get { return _authenticationList; }
      set { SetProperty(ref _authenticationList, value); }
    }

    public SettingsEdit SettingsEdit { get; set; }

    public DelegateCommand SaveConfigCommand { get; set; }

    public SettingsViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      Title = TabNames.Settings.ToString();

      AuthenticationList = new ObservableCollection<string>();

      GetEnumValues<Authentication>(AuthenticationList);

      SaveSettingsCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => Server)
        .ObservesProperty(() => DbName)
        .ObservesProperty(() => XmlFilePath);


      _eventAggregator.GetEvent<SaveSettingsCommand>().Subscribe(SettingsUpdated);
    }

    //private void DbReceived(SettingsEdit obj) => DbName = obj;

    private void SettingsUpdated(SettingsEdit obj) {
      SettingsEdit = obj;

      //DalManagers = new ObservableCollection<string>();
      //foreach (DalManagers item in Enum.GetValues(typeof(DalManagers))) {
      //  DalManagers.Add(EnumUtilities.GetEnumDescription(item));
      //}

      //SqlServerInstances = new ObservableCollection<string>();
      //foreach (SQLServerInstances item in Enum.GetValues(typeof(SQLServerInstances))) {
      //  SqlServerInstances.Add(EnumUtilities.GetEnumDescription(item));
      //}

      //if (ConfigEdit.DalManagerType == EnumUtilities.GetEnumDescription(DalManagerConnectionStrings.DalSql)) {
      //  DalManagerInUse = (int)Enums.DalManagers.SQLServer;
      //}
      //else if (ConfigEdit.DalManagerType == EnumUtilities.GetEnumDescription(DalManagerConnectionStrings.DalFile)) {
      //  DalManagerInUse = (int)Enums.DalManagers.File;
      //}

      //BaseUri = ConfigEdit.BaseUri;
      //ClientSecret = ConfigEdit.ClientSecret;

      //if (ConfigEdit.DbInUse == EnumUtilities.GetEnumDescription(SQLServerInstances.Local)) {
      //  DbInUse = (int)SQLServerInstances.Local;
      //}
      //else {
      //  DbInUse = (int)SQLServerInstances.Server;
      //}
    }

    private bool CanExecute() {
      return (!string.IsNullOrWhiteSpace(Server) 
        && !string.IsNullOrEmpty(DbName) 
        && !string.IsNullOrEmpty(XmlFilePath));
    }

    private void Execute() {
      try {
        _eventAggregator
          .GetEvent<SaveSettingsCommand>()
          .Publish(SaveConfig(SettingsEdit));

        MessageBox.Show("Configuration saved!", "Reco 3 Configuration", MessageBoxButton.OK);
      }
      catch (Exception ex) {
        MessageBox.Show($"An error occurred while saving. The application returned: {ex.Message}");
      }
    }

    public SettingsEdit SaveConfig(SettingsEdit data) {
      data = data.Save();
      return data;
    }
  }
}
