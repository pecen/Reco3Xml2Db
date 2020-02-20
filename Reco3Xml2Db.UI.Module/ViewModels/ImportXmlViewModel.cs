using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Reco3Xml2Db.Library;
using Reco3Xml2Db.UI.Module.Commands;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class ImportXmlViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;
    private readonly string _header = "Fill in the information below and press ";
 
    #region Properties

    public string FilenameToolTip { get; } = "Type in a valid Component filename (*.Xml), or click the button to the right to select file.";
    public string XmlFileDialogButtonToolTip { get; } = "Click the button to open a file dialog to browse for the Xml file(s)";
    public string AuthToolTip { get; } = "Select Authentication method for Sql Server";
    public string AllFilesToolTip { get; } = "Process all files in the chosen directory with the same settings";

    public DelegateCommand GetFilenameCommand { get; set; }
    public DelegateCommand ImportXmlCommand { get; set; }
    public ComponentEdit Component { get; set; }
    public ComponentList Components { get; set; }

    private Stream XmlStream { get; set; }


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

    private string _filePath;
    public string FilePath {
      get { return _filePath; }
      set { SetProperty(ref _filePath, value); }
    }

    private string _fileName;
    public string FileName {
      get { return _fileName; }
      set {
        SetProperty(ref _fileName, Path.IsPathRooted(value) 
          ? value 
          : FilePath + "\\" + value);
      }
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

    private bool _isChecked;
    public bool IsChecked {
      get { return _isChecked; }
      set {
        SetProperty(ref _isChecked, value);
        RaisePropertyChanged(nameof(IsNotChecked));
      }
    }

    public bool IsNotChecked {
      get { return !_isChecked; }
    }

    #endregion

    public ImportXmlViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      Title = TabNames.ImportToDb.GetDescription();
      ComponentExists = false;

      PDSourceList = new ObservableCollection<string>();
      PDStatusList = new ObservableCollection<string>();
      ComponentTypeList = new ObservableCollection<string>();

      PDSourceList.GetEnumValues<PDSource>();
      PDStatusList.GetEnumValues<PDStatus>();
      ComponentTypeList.GetEnumValues<ComponentType>();

      GetFilePathCommand = new DelegateCommand(GetFolderDialog);
      GetFilenameCommand = new DelegateCommand(GetFileDialog);
      ImportXmlCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => FileName)
        .ObservesProperty(() => IsChecked);

      _eventAggregator.GetEvent<GetFilePathCommand>().Subscribe(FilePathReceived);
      _eventAggregator.GetEvent<GetFilenameCommand>().Subscribe(FileNameReceived);
      _eventAggregator.GetEvent<ImportXmlCommand>().Subscribe(ComponentEditReceived);
      _eventAggregator.GetEvent<GetComponentsWSamePDNumberCommand>().Subscribe(ComponentListReceived);
    }

    private void FilePathReceived(string obj) {
      FilePath = obj;
      FileName = string.Empty;
    }
    private void FileNameReceived(string obj) {
      FileName = obj;

      var dir = Path.GetDirectoryName(obj);
      if (dir != FilePath) {
        FilePath = dir;
      }
    }
    private void ComponentEditReceived(ComponentEdit obj) => Component = obj;
    private void ComponentListReceived(ComponentList obj) => Components = obj; 

    private void Execute() {
      try {
        if (IsChecked) {
          ExecuteMany();
          var files = Directory.EnumerateFiles(FilePath).Count();

          MessageBox.Show($"{files} Component{(files > 1 ? "s" : string.Empty)} saved to the Database", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else {
          ExecuteOne();

          MessageBox.Show("Component saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        ClearValues();
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "Error when saving component", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void ExecuteMany() {
      foreach(var item in Directory.EnumerateFiles(FilePath)) {
        FileName = item;
        XmlStream = File.OpenRead(item);
        ExecuteOne();
      }
    }

    private void ExecuteOne() {
        _eventAggregator
          .GetEvent<ImportXmlCommand>()
          .Publish(ComponentEdit.NewComponentEdit());

        Component.PDNumber = CheckPDNumber(Path.GetFileNameWithoutExtension(FileName));
        Component.DownloadedTimestamp = DateTime.Now;
        Component.Description = Description;
        Component.PDStatus = SelectedPDStatus;
        Component.ComponentType = SelectedComponentType;
        Component.PDSource = SelectedPDSource;
        Component.Xml = GetXml();

        var sourceComponent = ComponentEdit.GetComponent(Component.PDNumber);

        if (!string.IsNullOrEmpty(sourceComponent.PDNumber)) {
          Component.SourceComponentId = sourceComponent.ComponentId;
        }
        else {
          Component.SourceComponentId = null;
        }

        Component = Component.Save();
    }

    private string CheckPDNumber(string pdNumber) {
      if(!int.TryParse(pdNumber, out int parsedPd)) {
        throw new ApplicationException($"Wrong format in PDNumber. The filename is: {pdNumber}");
      }

      return pdNumber;
    }

    private bool CanExecute() {
      if (IsChecked
        || (!string.IsNullOrEmpty(FileName)
        && FileName.Length > 4
        && FileName.Substring(FileName.Length - 4) == ".xml"
        && File.Exists($"{FileName}"))) {

        PublishExistingComponent();
        RaisePropertyChanged(nameof(PageHeader));

        return true;
      }

      ComponentExists = false;

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

        RaisePropertyChanged(nameof(PageHeader));
      }
    }

    private void GetFileDialog() {
      OpenFileDialog openFileDialog = new OpenFileDialog {
        InitialDirectory = string.IsNullOrEmpty(FileName)
                            //? SavedFilePath
                            ? FilePath
                            : Path.GetDirectoryName(FileName),
        Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*",
        FilterIndex = 1,
        RestoreDirectory = true
      };

      if ((bool)openFileDialog.ShowDialog()) {
        ClearValues();
        
        //Read the contents of the file into a stream
        XmlStream = openFileDialog.OpenFile();

        _eventAggregator.GetEvent<GetFilenameCommand>()
          .Publish(openFileDialog.FileName);
      }
    }

    private void GetFolderDialog() {
      var dialog = new CommonOpenFileDialog {
        InitialDirectory = (string.IsNullOrEmpty(FileName) 
                              //? SavedFilePath 
                              ? FilePath
                              : Path.GetDirectoryName(FileName)),
        IsFolderPicker = true,
      };

      if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
        _eventAggregator.GetEvent<GetFilePathCommand>()
          .Publish(dialog.FileName);
      }
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
      ComponentExists = false;
    }
  }
}
