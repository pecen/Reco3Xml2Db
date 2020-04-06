using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Reco3Xml2Db.Library;
using Reco3Xml2Db.UI.Module.Commands;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.UI.Module.Services;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Windows;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class SettingsViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;

    #region Properties

    public string PageHeader { get; } = "Fill in the configuration settings below";
    public string ServerToolTip { get; } = "Choose a server name or an IP-address where the Sql Server database resides";
    public string DbToolTip { get; } = "Type in the name of the Sql Server database";
    public string AuthToolTip { get; } = "Select Authentication method for Sql Server";
    public string DbDialogButtonToolTip { get; } = "Click the button to set the database servername and database name";
    public string XmlFilePathToolTip { get; } = "Type in the filepath where the Xml files resides";
    public string XmlFileDialogButtonToolTip { get; } = "Click the button to browse for the location of the Xml file(s)";
    public string SaveButtonToolTip { get; } = "Click the button to save the configuration settings";
    public string UserNameToolTip { get; } = "Enter a user name";
    public string PasswordToolTip { get; } = "Enter your password";

    public DelegateCommand GetDbCommand { get; set; }
    public DelegateCommand SaveSettingsCommand { get; set; }

    public bool HasServerValue {
      get { return !string.IsNullOrEmpty(Server); }
    }

    private string _server;
    public string Server {
      get { return _server; }
      set {
        SetProperty(ref _server, value);
        RaisePropertyChanged(nameof(HasServerValue));
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
        if (value == (int)AuthMethod.Windows) {
          IsUidPwdAuth = false;
        }
        else {
          IsUidPwdAuth = true;
        }
      }
    }

    private bool _isUidPwdAuth;
    public bool IsUidPwdAuth {
      get { return _isUidPwdAuth; }
      set {
        SetProperty(ref _isUidPwdAuth, value);
      }
    }

    private string _xmlFilePath;
    public string XmlFilePath {
      get { return _xmlFilePath; }
      set {
        SetProperty(ref _xmlFilePath, value);
      }
    }

    private ObservableCollection<string> _authenticationList;
    public ObservableCollection<string> AuthenticationList {
      get { return _authenticationList; }
      set { SetProperty(ref _authenticationList, value); }
    }

    private string _userName;
    public string UserName {
      get { return _userName; }
      set {
        SetProperty(ref _userName, value);
      }
    }

    private string _password;
    public string Password {
      get { return _password; }
      set {
        SetProperty(ref _password, value);
      }
    }

    public SettingsEdit Settings { get; private set; }

    #endregion

    public SettingsViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      Title = TabNames.Settings.ToString();

      AuthenticationList = new ObservableCollection<string>();
      AuthenticationList.GetEnumValues<AuthMethod>();

      GetFilePathCommand = new DelegateCommand(GetFolderName);
      GetDbCommand = new DelegateCommand(GetDbName);
      SaveSettingsCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => Server)
        .ObservesProperty(() => DbName)
        .ObservesProperty(() => XmlFilePath);

      _eventAggregator.GetEvent<GetFilePathCommand>().Subscribe(FilePathReceived);
      _eventAggregator.GetEvent<GetDbCommand>().Subscribe(DbNameReceived);
      _eventAggregator.GetEvent<SaveSettingsCommand>().Subscribe(SettingsUpdated);

      LoadSettings();
    }

    public void LoadSettings() {
      Settings = SettingsEdit.GetConfigSettings();

      Server = Settings.Server;
      DbName = Settings.Database;
      Authentication = Settings.Authentication;
      XmlFilePath = Settings.XmlFilePath;

      if (Directory.Exists(XmlFilePath)) {

        _eventAggregator.GetEvent<GetFilePathCommand>().Publish(XmlFilePath);
      }
      else {
        XmlFilePath = string.Empty;
      }
    }

    private void FilePathReceived(string obj) => XmlFilePath = obj;

    private void DbNameReceived(IDictionary<string, string> obj) {
      if (obj != null) {
        Server = obj[ConnectionString.DataSource.ToString()];
        DbName = obj[ConnectionString.InitialCatalog.ToString()];
      }
    }

    private void SettingsUpdated(SettingsEdit obj) => Settings = obj;

    private void GetFolderName() 
      => new PathProvider(_eventAggregator)
      .FolderPathService(XmlFilePath);

    //private void GetDbName() {
    //  try {
    //    _eventAggregator
    //      .GetEvent<GetDbCommand>()
    //      .Publish(_eventAggregator
    //        .GetEvent<GetDbCommand>()
    //        .GetDbDialog());
    //  }
    //  catch (Exception ex) {
    //    DbName = ex.Message;
    //  }
    //}

    private void GetDbName()
      => new PathProvider(_eventAggregator)
      .DbPathService();

    private bool CanExecute() {
      return (!string.IsNullOrWhiteSpace(Server)
        && !string.IsNullOrEmpty(DbName)
        && !string.IsNullOrEmpty(XmlFilePath))
        && Directory.Exists(XmlFilePath);
    }

    private void Execute() {
      Settings.Server = Server;
      Settings.Database = DbName;
      Settings.Authentication = Authentication;
      Settings.XmlFilePath = XmlFilePath;

      try {
        var isDirty = Settings.IsDirty;

        _eventAggregator
          .GetEvent<SaveSettingsCommand>()
          .Publish(SaveConfig(Settings));

        if (isDirty) {
          MessageBox.Show("Configuration saved!", "Reco 3 Configuration", MessageBoxButton.OK);
        }
        else {
          MessageBox.Show("Nothing was changed so nothing to be saved!", "Reco 3 Configuration", MessageBoxButton.OK);
        }
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
