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

    public string ComponentExistsInfo { get; } = "This component exists. If you continue, an update will take place.";
    public string SourceComponentExistsInfo { get; } = "This PD number exists in the database. The component will be added with reference to the source component.";

    public string FileNameToolTip { get; } = "Type in a valid Component filename (*.Xml), or click the button to the right to select file.";
    public string FilePathToolTip { get; } = "Type in a valid path to the Component files, or click the button to the right to select path.";
    public string XmlFileDialogButtonToolTip { get; } = "Click the button to open a file dialog to browse for the Xml file(s)";
    public string AuthToolTip { get; } = "Select Authentication method for Sql Server";
    public string AllFilesToolTip { get; } = "Process all files in the chosen directory with the same settings";
    public string UpdateExistingToolTip { get; } = "This component already exists in the database. An Update will take place by replacing existing data for component, " +
      "as opposed to updating an existing component by adding this file, which is not present in the database, and referring to a source component";

    public DelegateCommand GetFilenameCommand { get; set; }
    public DelegateCommand ImportComponentCommand { get; set; }
    public DelegateCommand UpdateComponentCommand { get; set; }
    //public ComponentEdit Component { get; set; }
    //public ComponentList Components { get; set; }
    public DelegateCommand PublishedCommand { get; set; }

    private Stream XmlStream { get; set; }
    private bool IsPublished { get; set; }
    private int UpdateComponentId { get; set; }

    public bool ReplaceIsActive {
      get { return SourceComponentExists == true && UpdateComponentId > 0; }
    }

    public string PageHeader {
      get { return _header + BtnName; }
    }

    private string BtnName {
      get {
        return SourceComponentExists
          ? ButtonName.Update.ToString()
          : ButtonName.Import.ToString();
      }
    }

    private bool _sourceComponentExists;
    public bool SourceComponentExists {
      get { return _sourceComponentExists; }
      set { SetProperty(ref _sourceComponentExists, value); }
    }

    private string _importBtnContent;
    public string ImportBtnContent {
      get { return _importBtnContent; }
      set { SetProperty(ref _importBtnContent, value); }
    }

    private string _filePath;
    public string FilePath {
      get { return _filePath; }
      set {
        SetProperty(ref _filePath, value);
        //SetProperty(ref _filePath, Directory.Exists(value)
        //  ? value
        //  : Environment.CurrentDirectory);
      }
    }

    private string _fileName;
    public string FileName {
      get { return _fileName; }
      set {
        SetProperty(ref _fileName, value);
        //SetProperty(ref _fileName, Path.IsPathRooted(value)
        //  ? value
        //  : string.Empty);
        ////: string.IsNullOrEmpty(FilePath) 
        ////  ? value
        ////  : FilePath + "\\" + value);
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

    private bool _allFilesIsChecked;
    public bool AllFilesIsChecked {
      get { return _allFilesIsChecked; }
      set {
        SetProperty(ref _allFilesIsChecked, value);
        if(value == true) ReplaceIsChecked = false;
        RaisePropertyChanged(nameof(AllFilesIsNotChecked));
      }
    }

    public bool AllFilesIsNotChecked {
      get { return !_allFilesIsChecked; }
    }

    private bool _replacedIsChecked;
    public bool ReplaceIsChecked {
      get { return _replacedIsChecked; }
      set {
        SetProperty(ref _replacedIsChecked, value);
        if (value == true) AllFilesIsChecked = false;
      }
    }

    #endregion

    public ImportXmlViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      Title = TabNames.ImportToDb.GetDescription();
      SourceComponentExists = false;
      ReplaceIsChecked = false;

      PDSourceList = new ObservableCollection<string>();
      PDStatusList = new ObservableCollection<string>();
      ComponentTypeList = new ObservableCollection<string>();

      PDSourceList.GetEnumValues<PDSource>();
      PDStatusList.GetEnumValues<PDStatus>();
      ComponentTypeList.GetEnumValues<ComponentType>();

      GetFilePathCommand = new DelegateCommand(GetFolderDialog);
      GetFilenameCommand = new DelegateCommand(GetFileDialog);
      ImportComponentCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => FileName)
        .ObservesProperty(() => FilePath)
        .ObservesProperty(() => AllFilesIsChecked);
      UpdateComponentCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => FileName);

      PublishedCommand = new DelegateCommand(SetPublishedStatus);

      _eventAggregator.GetEvent<GetFilePathCommand>().Subscribe(FilePathReceived);
      _eventAggregator.GetEvent<GetFilenameCommand>().Subscribe(FileNameReceived);
      _eventAggregator.GetEvent<ComponentExistsCommand>().Subscribe(ExistingComponentReceived);
      _eventAggregator.GetEvent<UpdateComponentSetCommand>().Subscribe(UpdateComponentIdReceived);
    }

    private void UpdateComponentIdReceived(int obj) {
      UpdateComponentId = obj;

      RaisePropertyChanged(nameof(ReplaceIsActive));
    }

    private void FilePathReceived(string obj) {
      FilePath = obj;
    }

    private void FileNameReceived(string obj) {
      FileName = obj;

      var dir = Path.GetDirectoryName(obj);
      if (dir != FilePath) {
        _eventAggregator.GetEvent<GetFilePathCommand>().Publish(dir);
      }

      ReplaceIsChecked = false;
    }

    private void ExistingComponentReceived(ComponentInfo obj) {
      if (obj.ComponentId > 0) {
        SourceComponentExists = true;
        SelectedComponentType = obj.ComponentType;
        SelectedPDSource = obj.PDSource;
        SelectedPDStatus = obj.PDStatus;
        Description = obj.Description;

        RaisePropertyChanged(nameof(PageHeader));
        RaisePropertyChanged(nameof(ReplaceIsActive));
      }
      else {
        SourceComponentExists = false;
      }
    }

    private void Execute() {
      try {
        if (ReplaceIsChecked) {
          ReplaceComponent();
        }
        else if (AllFilesIsChecked) {
          ExecuteMany();
          var files = Directory.EnumerateFiles(FilePath).Count();

          MessageBox.Show($"{files} Component{(files > 1 ? "s" : string.Empty)} saved to the Database", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else {
          ExecuteOne();

          MessageBox.Show("Component saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "Error when saving component", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally {
        ClearValues();
      }
    }

    private void ReplaceComponent() {
      var component = ComponentEdit.GetComponent(UpdateComponentId);

      component.Description = Description;
      component.PDStatus = SelectedPDStatus;
      component.ComponentType = SelectedComponentType;
      component.PDSource = SelectedPDSource;
      component.Xml = GetXml();

      component = component.Save();

      _eventAggregator
        .GetEvent<UpdateComponentCommand>()
        .Publish(component);
    }

    private void ExecuteMany() {
      foreach (var item in Directory.EnumerateFiles(FilePath)) {
        FileName = item;
        XmlStream = File.OpenRead(item);
        ExecuteOne();
      }
    }

    private void ExecuteOne() {
      var component = ComponentEdit.NewComponentEdit();

      component.PDNumber = CheckPDNumber(Path.GetFileNameWithoutExtension(FileName));
      component.DownloadedTimestamp = DateTime.Now;
      component.Description = Description;
      component.PDStatus = SelectedPDStatus;
      component.ComponentType = SelectedComponentType;
      component.PDSource = SelectedPDSource;
      component.Xml = GetXml();

      var sourceComponent = ComponentEdit.GetComponent(component.PDNumber);

      if (!string.IsNullOrEmpty(sourceComponent.PDNumber)) {
        component.SourceComponentId = sourceComponent.ComponentId;
      }
      else {
        component.SourceComponentId = null;
      }

      component = component.Save();

      _eventAggregator
        .GetEvent<ImportComponentCommand>()
        .Publish(component);
    }

    private string CheckPDNumber(string pdNumber) {
      if (!int.TryParse(pdNumber, out int parsedPd)) {
        throw new ApplicationException($"Wrong format in PDNumber. The filename is: {pdNumber}");
      }

      return pdNumber;
    }

    private bool CanExecute() {
      if ((AllFilesIsChecked
        && Directory.Exists(FilePath))
        || (AllFilesIsNotChecked
        && !string.IsNullOrEmpty(FileName)
        && FileName.Length > 4
        && FileName.Substring(FileName.Length - 4) == ".xml"
        && File.Exists($"{FileName}"))) {

        if (!IsPublished) {
          PublishSourceComponent();
        }

        return true;
      }

      SourceComponentExists = false;

      return false;
    }

    public void SetPublishedStatus() {
      IsPublished = false;
    }

    private void PublishSourceComponent() {
      var fileNameWOutExt = Path.GetFileNameWithoutExtension(FileName);

      _eventAggregator
        .GetEvent<ComponentExistsCommand>()
        .Publish(ComponentInfo.GetComponent(fileNameWOutExt));

      IsPublished = true;
    }

    private void GetFileDialog() {
      OpenFileDialog openFileDialog = new OpenFileDialog {
        //InitialDirectory = string.IsNullOrEmpty(FileName)
        //                    //? SavedFilePath
        //                    ? FilePath
        //                    : Directory.Exists(FilePath + "\\" + FileName)
        //                      ? Path.GetDirectoryName(FileName)
        //                      : Environment.CurrentDirectory,
        InitialDirectory = FilePath,
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
                              ? FilePath
                              : Path.GetDirectoryName(FileName)),
        IsFolderPicker = true,
      };

      if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
        // dialog.FileName below holds the path and not the filename
        // even though the property FileName is used. 
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
      SourceComponentExists = false;
      ReplaceIsChecked = false;
    }
  }
}
