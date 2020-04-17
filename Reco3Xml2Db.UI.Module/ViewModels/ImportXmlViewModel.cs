using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Reco3Xml2Db.Library;
using Reco3Xml2Db.UI.Module.Commands;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.UI.Module.Services;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class ImportXmlViewModel : ViewModelBase {
    private readonly IEventAggregator _eventAggregator;
    private readonly IPathProvider _filePathProvider;
    private readonly IXmlProvider _xmlProvider;

    private readonly string _header = "Fill in the information below and press ";

    #region Properties

    public string FileNameToolTip { get; } = "Type in a valid Component filename (*.Xml), or click the button to the right to select file.";
    public string FilePathToolTip { get; } = "Type in a valid path to the Component files, or click the button to the right to select path.";

    //public string SourceComponentExistsInfo { get; } = "This PD number exists in the database. The component will be added with reference to the source component.";
    //public string ComponentExistsInfo { get; } = "This component exists. If you continue, an update will take place.";
    //public string XmlFileDialogButtonToolTip { get; } = "Click the button to open a file dialog to browse for the Xml file(s)";
    //public string AllFilesToolTip { get; } = "Process all files in the chosen directory with the same settings";
    //public string UpdateExistingToolTip { get; } = "This component already exists in the database. An Update will take place by replacing existing data for component, " +
    //  "as opposed to updating an existing component by adding this file, which is not present in the database, and referring to a source component";

    public DelegateCommand GetFilenameCommand { get; set; }
    public DelegateCommand GetFilePathCommand { get; set; }
    public DelegateCommand ImportComponentCommand { get; set; }
    public DelegateCommand UpdateComponentCommand { get; set; }
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
      }
    }

    private string _fileName;
    public string FileName {
      get { return _fileName; }
      set {SetProperty(ref _fileName, value);
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

    public ImportXmlViewModel(IEventAggregator eventAggregator, IPathProvider filePathProvider, IXmlProvider xmlProvider) {
      _eventAggregator = eventAggregator;
      _filePathProvider = filePathProvider;
      _xmlProvider = xmlProvider;

      Title = TabNames.ImportToDb.GetDescription();
      SourceComponentExists = false;
      ReplaceIsChecked = false;

      PDSourceList = new ObservableCollection<string>();
      PDStatusList = new ObservableCollection<string>();
      ComponentTypeList = new ObservableCollection<string>();

      PDSourceList.GetEnumValues<PDSource>();
      PDStatusList.GetEnumValues<PDStatus>();
      ComponentTypeList.GetEnumValues<ComponentType>();

      GetFilePathCommand = new DelegateCommand(GetFolder);
      GetFilenameCommand = new DelegateCommand(GetFileName);
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

    private void GetFolder() {
      var pathProvider = new PathProvider(_eventAggregator);
      var folderPath = string.IsNullOrEmpty(FileName)
                              ? FilePath
                              : Path.GetDirectoryName(FileName);
      pathProvider.FolderPathService(folderPath);
    }

    private void GetFileName() {
      var pathProvider = new PathProvider(_eventAggregator);
      var tmpStream = pathProvider.FilePathService(FilePath);

      if(tmpStream != null) {
        XmlStream = tmpStream;
      }
    }

    private void FilePathReceived(string obj) => FilePath = obj;

    private void FileNameReceived(string obj) {
      ClearValues();
      FileName = obj;

      var dir = Path.GetDirectoryName(obj);
      if (dir != FilePath) {
        _eventAggregator.GetEvent<GetFilePathCommand>().Publish(dir);
      }

      RaisePropertyChanged(nameof(ReplaceIsActive));
    }

    private void UpdateComponentIdReceived(int obj) {
      UpdateComponentId = obj;

      RaisePropertyChanged(nameof(ReplaceIsActive));
    }

    private void ExistingComponentReceived(ComponentInfo obj) {
      if (obj.ComponentId > 0) {
        UpdateComponentId = obj.ComponentId;
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
          try {
            ReplaceComponent();

            MessageBox.Show("Component updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
          }
          catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error when updating component", MessageBoxButton.OK, MessageBoxImage.Error);
          }
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
        RaisePropertyChanged(nameof(ReplaceIsActive));
      }
    }

    private void ReplaceComponent() {
      var component = ComponentEdit.GetComponent(UpdateComponentId);

      component.Description = Description;
      component.PDStatus = SelectedPDStatus;
      component.ComponentType = SelectedComponentType;
      component.PDSource = SelectedPDSource;
      component.Xml = _xmlProvider.GetXmlStringService(XmlStream); 

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
      component.Xml = _xmlProvider.GetXmlStringService(XmlStream); 

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

    private void ClearValues() {
      UpdateComponentId = 0;
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
