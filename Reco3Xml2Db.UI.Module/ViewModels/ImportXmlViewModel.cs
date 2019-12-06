using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reco3Xml2Db.Library;
using Reco3Xml2Db.UI.Module.Commands;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class ImportXmlViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;
    private readonly string _header = "Fill in the information below and press ";
 
    #region Properties

    public string FilenameToolTip { get; } = "Type in a valid Component filename (*.Xml)";
    public string XmlFileDialogButtonToolTip { get; } = "Click the button to open a file dialog to browse for the Xml file(s)";
    //public string ImportUpdateButtonToolTip {
    //  get {
    //    return ComponentExists
    //      ? "Click the button to update an existing component"
    //      : "Click the button to import the component";
    //  }
    //} 
    public string AuthToolTip { get; } = "Select Authentication method for Sql Server";

    public DelegateCommand GetFilenameCommand { get; set; }
    public DelegateCommand ImportXmlCommand { get; set; }
    public ComponentEdit Component { get; set; }
    public ComponentList Components { get; set; }

    public string PageHeader {
      get { return _header + BtnName; }
    } 

    private string BtnName {
      get {
        return ComponentExists 
          ? ButtonName.Update.ToString() 
          : ButtonName.Import.ToString();
      }
    }

    //private string BtnToolTip {
    //  get {
    //    return ComponentExists
    //      ? 
    //      }
    //}

    private bool _componentExists;
    public bool ComponentExists {
      get { return _componentExists; }
      set { SetProperty(ref _componentExists, value); }
    }

    private string _importBtnContent;
    public string ImportBtnContent {
      get { return _importBtnContent; }
      set { SetProperty(ref _importBtnContent, value); }
    }

    private string _fileName;
    public string FileName {
      get { return _fileName; }
      set { SetProperty(ref _fileName, value); }
    }

    private string _filePath;
    public string FilePath {
      get { return _filePath; }
      set { SetProperty(ref _filePath, value); }
    }

    private ObservableCollection<string> _pdSourceList;
    public ObservableCollection<string> PDSourceList {
      get { return _pdSourceList; }
      private set { SetProperty(ref _pdSourceList, value); }
    }

    private int _selectedPDSource;
    public int SelectedPDSource {
      get { return _selectedPDSource; }
      set { SetProperty(ref _selectedPDSource, value); }
    }

    private ObservableCollection<string> _pdStatusList;
    public ObservableCollection<string> PDStatusList {
      get { return _pdStatusList; }
      private set { SetProperty(ref _pdStatusList, value); }
    }

    private int _selectedPDStatus;
    public int SelectedPDStatus {
      get { return _selectedPDStatus; }
      set { SetProperty(ref _selectedPDStatus, value); }
    }

    private ObservableCollection<string> _componentTypeList;
    public ObservableCollection<string> ComponentTypeList {
      get { return _componentTypeList; }
      private set { SetProperty(ref _componentTypeList, value); }
    }

    private int _selectedComponentType;
    public int SelectedComponentType {
      get { return _selectedComponentType; }
      set { SetProperty(ref _selectedComponentType, value); }
    }

    private string _description;
    public string Description {
      get { return _description; }
      set { SetProperty(ref _description, value); }
    }

    private Stream XmlStream { get; set; }

    #endregion

    public ImportXmlViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      Title = EnumExtensions.GetEnumDescription(TabNames.ImportToDb);
      ComponentExists = false;

      PDSourceList = new ObservableCollection<string>();
      PDStatusList = new ObservableCollection<string>();
      ComponentTypeList = new ObservableCollection<string>();

      GetEnumValues<PDSource>(PDSourceList);
      GetEnumValues<PDStatus>(PDStatusList);
      GetEnumValues<ComponentType>(ComponentTypeList);

      GetFilenameCommand = new DelegateCommand(GetFileName);
      ImportXmlCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => FileName);

      _eventAggregator.GetEvent<GetFilenameCommand>().Subscribe(FileNameReceived);
      _eventAggregator.GetEvent<ImportXmlCommand>().Subscribe(ComponentEditReceived);
      _eventAggregator.GetEvent<GetFilePathCommand>().Subscribe(FilePathReceived);
      _eventAggregator.GetEvent<GetComponentsWSamePDNumberCommand>().Subscribe(ComponentListReceived);
    }

    private void FilePathReceived(string obj) => FilePath = obj;
    private void FileNameReceived(string obj) => FileName = obj;
    private void ComponentEditReceived(ComponentEdit obj) => Component = obj;
    private void ComponentListReceived(ComponentList obj) => Components = obj; 

    private void GetFileName() {
      try {
        var filename = GetFileDialog();

        if (!string.IsNullOrEmpty(filename)) {
          _eventAggregator
            .GetEvent<GetFilenameCommand>()
            .Publish(filename);

          PublishExistingComponent();
        }
      }
      catch (Exception ex) {
        FileName = ex.Message;
      }
    }

    private void Execute() {
      try {
        _eventAggregator
          .GetEvent<ImportXmlCommand>()
          .Publish(ComponentEdit.NewComponentEdit());

        Component.PDNumber = Path.GetFileNameWithoutExtension(FileName);
        Component.DownloadedTimestamp = DateTime.Now;
        Component.Description = Description;
        Component.PDStatus = SelectedPDStatus;
        Component.ComponentType = SelectedComponentType;
        Component.PDSource = SelectedPDSource;
        Component.Xml = GetXml();

        var sourceComponent = ComponentEdit.GetComponent(Component.PDNumber);

        //if (ComponentEdit.Exists(Component.PDNumber)) {
        if (!string.IsNullOrEmpty(sourceComponent.PDNumber)) {
          Component.SourceComponentId = sourceComponent.ComponentId;
        }
        else {
          Component.SourceComponentId = null;
        }

        Component = Component.Save();

        MessageBox.Show("Component saved!", "Reco 3 Import", MessageBoxButton.OK);

        ClearValues();
      }
      catch (Exception ex) {
        FileName = ex.Message;
      }
    }

    private bool CanExecute() {
      if(!string.IsNullOrEmpty(FileName)
        && FileName.Length > 4
        && FileName.Substring(FileName.Length - 4) == ".xml"
        && File.Exists($@"{FilePath}\{FileName}")) {

        PublishExistingComponent();

        return true;
      }
      ClearComponent();
      RaisePropertyChanged(nameof(PageHeader));

      return false;
    }

    private void PublishExistingComponent() {
      var fileNameWOutExt = Path.GetFileNameWithoutExtension(FileName);

      if (ComponentEdit.Exists(fileNameWOutExt)) {
        _eventAggregator
          .GetEvent<GetComponentsWSamePDNumberCommand>()
          .Publish(ComponentList.GetComponentList(fileNameWOutExt));

        var max = Components.Max(c => c.ComponentId);
        var lastComponent = Components.Where(c => c.ComponentId == max).FirstOrDefault();

        SelectedComponentType = lastComponent.ComponentType;
        SelectedPDSource = lastComponent.PDSource;
        SelectedPDStatus = lastComponent.PDStatus;
        Description = lastComponent.Description;

        ComponentExists = true;
      }
      else {
        ClearComponent();
      }

      RaisePropertyChanged(nameof(PageHeader));
    }

    private void ClearComponent() {
        SelectedComponentType = 0;
        SelectedPDSource = 0;
        SelectedPDStatus = 0;
        Description = string.Empty;

        ComponentExists = false;
    }

    private string GetFileDialog() {
      OpenFileDialog openFileDialog = new OpenFileDialog {
        InitialDirectory = FilePath,
        Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*",
        FilterIndex = 1,
        RestoreDirectory = true
      };

      if ((bool)openFileDialog.ShowDialog()) {
        //Read the contents of the file into a stream
        XmlStream = openFileDialog.OpenFile();

        return openFileDialog.SafeFileName;
      }

      return string.Empty;
    }

    private string GetXml() {
      using (StreamReader reader = new StreamReader(XmlStream)) {
        return reader.ReadToEnd();
      }
    }

    private void ClearValues() {
      FileName = string.Empty;
      Description = string.Empty;
      SelectedPDStatus = 0;
      SelectedComponentType = 0;
      SelectedPDSource = 0;
      XmlStream = null;
    }
  }
}
