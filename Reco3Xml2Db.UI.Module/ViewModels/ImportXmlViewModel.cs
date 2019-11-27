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

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class ImportXmlViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;
    public string PageHeader { get; } = "Fill in the information below and press Import";
    public DelegateCommand GetFilenameCommand { get; set; }
    public DelegateCommand ImportXmlCommand { get; set; }

    private string _fileName;
    public string FileName {
      get { return _fileName; }
      set { SetProperty(ref _fileName, value); }
    }

    private ComponentEdit _component;
    public ComponentEdit Component {
      get { return _component; }
      set { SetProperty(ref _component, value); }
    }

    private ObservableCollection<string> _pdSourceList;
    public ObservableCollection<string> PDSourceList {
      get { return _pdSourceList; }
      private set { SetProperty(ref _pdSourceList, value); }
    }


    private ObservableCollection<string> _pdStatusList;
    public ObservableCollection<string> PDStatusList {
      get { return _pdStatusList; }
      private set { SetProperty(ref _pdStatusList, value); }
    }

    private ObservableCollection<string> _componentTypeList;
    public ObservableCollection<string> ComponentTypeList {
      get { return _componentTypeList; }
      private set { SetProperty(ref _componentTypeList, value); }
    }

    public ImportXmlViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;
      Title = EnumExtensions.GetEnumDescription(TabNames.ImportToDb);

      PDSourceList = new ObservableCollection<string>();
      PDStatusList = new ObservableCollection<string>();
      ComponentTypeList = new ObservableCollection<string>();

      GetEnumValues<PDSource>(PDSourceList);
      GetEnumValues<PDStatus>(PDStatusList);
      GetEnumValues<ComponentType>(ComponentTypeList);

      GetFilenameCommand = new DelegateCommand(GFCExecute);
      ImportXmlCommand = new DelegateCommand(IXCExecute, IXCCanExecute)
        .ObservesProperty(() => FileName);

      _eventAggregator.GetEvent<GetFilenameCommand>().Subscribe(FileNameReceived);
      _eventAggregator.GetEvent<ImportXmlCommand>().Subscribe(ComponentEditReceived);
    }

    private void FileNameReceived(string obj) => FileName = obj;
    private void ComponentEditReceived(ComponentEdit obj) => Component = obj;

    private void GFCExecute() {
      try {
        _eventAggregator
          .GetEvent<GetFilenameCommand>()
          .Publish(GetFileName());
      }
      catch (Exception ex) {
        FileName = ex.Message;
      }
    }

    private void IXCExecute() {
      _eventAggregator
        .GetEvent<ImportXmlCommand>()
        .Publish(ComponentEdit.NewComponentEdit());

      //Component
    }

    private bool IXCCanExecute() {
      return !string.IsNullOrEmpty(FileName)
        && FileName.Length > 4
        && FileName.Substring(FileName.Length - 4) == ".xml";
    }

    private string GetFileName() {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = "c:\\";
      openFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
      openFileDialog.FilterIndex = 1;
      openFileDialog.RestoreDirectory = true;

      if ((bool)openFileDialog.ShowDialog()) {
        //Get the path of specified file
        //filePath = openFileDialog.FileName;

        //Read the contents of the file into a stream
        //var fileStream = openFileDialog.OpenFile();

        //using (StreamReader reader = new StreamReader(fileStream)) {
        //fileContent = reader.ReadToEnd();
        //}

        return openFileDialog.SafeFileName;
      }

      return string.Empty;
    }

    //private ComponentEdit GetComponentEdit() {
    //  return ComponentEdit.NewComponentEdit();
    //} 
  }
}
