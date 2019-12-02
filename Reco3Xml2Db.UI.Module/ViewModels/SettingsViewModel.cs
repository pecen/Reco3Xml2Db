using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
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
    public DelegateCommand GetFilePathCommand { get; set; }
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
      }
    }

    private string _xmlFilePath;
    public string XmlFilePath {
      get { return _xmlFilePath; }
      set {
        SetProperty(ref _xmlFilePath, value);
        //_eventAggregator.GetEvent<UpdateFilePathCommand>().Publish(XmlFilePath);
      }
    }

    private ObservableCollection<string> _authenticationList;
    public ObservableCollection<string> AuthenticationList {
      get { return _authenticationList; }
      set { SetProperty(ref _authenticationList, value); }
    }

    public SettingsEdit Settings { get; private set; }

    public SettingsViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      Title = TabNames.Settings.ToString();

      AuthenticationList = new ObservableCollection<string>();

      GetEnumValues<Authentication>(AuthenticationList);

      GetFilePathCommand = new DelegateCommand(GetFolderName);
      SaveSettingsCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => Server)
        .ObservesProperty(() => DbName)
        .ObservesProperty(() => XmlFilePath);

      _eventAggregator.GetEvent<GetFilePathCommand>().Subscribe(FilePathReceived);
      _eventAggregator.GetEvent<SaveSettingsCommand>().Subscribe(SettingsUpdated);
      //_eventAggregator.GetEvent<GetSettingsCommand>().Subscribe(SettingsLoaded);
      //_eventAggregator.GetEvent<GetSettingsCommand>().Publish(SettingsEdit.GetConfigSettings());

      LoadSettings();
    }

    public void LoadSettings() {
      Settings = SettingsEdit.GetConfigSettings();

      Server = Settings.Server;
      DbName = Settings.Database;
      Authentication = Settings.Authentication;
      XmlFilePath = Settings.XmlFilePath;

      _eventAggregator.GetEvent<GetFilePathCommand>().Publish(XmlFilePath);
    }

    //private void SettingsLoaded(SettingsEdit obj) {
    //  SettingsEdit = obj;

    //  Server = SettingsEdit.Server;
    //  DbName = SettingsEdit.Database;
    //  Authentication = SettingsEdit.Authentication;
    //  XmlFilePath = SettingsEdit.XmlFilePath;
    //
    //  _eventAggregator.GetEvent<GetFilePathCommand>().Publish(XmlFilePath);
    //}

    //private void DbReceived(SettingsEdit obj) => DbName = obj;

    private void FilePathReceived(string obj) => XmlFilePath = obj;
    private void SettingsUpdated(SettingsEdit obj) => Settings = obj;

    private void GetFolderName() {
      try {
        _eventAggregator
          .GetEvent<GetFilePathCommand>()
          .Publish(GetFolderDialog());
      }
      catch (Exception ex) {
        XmlFilePath = ex.Message;
      }
    }

    private bool CanExecute() {
      return (!string.IsNullOrWhiteSpace(Server) 
        && !string.IsNullOrEmpty(DbName) 
        && !string.IsNullOrEmpty(XmlFilePath));
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

    private string GetFolderDialog() {
      var dialog = new CommonOpenFileDialog { 
        InitialDirectory = (string.IsNullOrEmpty(XmlFilePath) ? "C:\\" : XmlFilePath),
        IsFolderPicker = true,
      };

      if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
        //MessageBox.Show("You selected: " + dialog.FileName);
        //XmlFilePath = dialog.FileName;
        return dialog.FileName;
      }

      return XmlFilePath;
    }

    public SettingsEdit SaveConfig(SettingsEdit data) {
      data = data.Save();
      return data;
    }
  }
}
