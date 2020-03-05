using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reco3Xml2Db.Library;
using Reco3Xml2Db.UI.Module.Commands;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.Utilities.Comparers;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class ComponentsGridViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;

    #region Properties

    public DelegateCommand SearchCommand { get; set; }
    public ComponentList UnFilteredList { get; set; }

    private ComponentList _components;
    public ComponentList Components {
      get { return _components; }
      set { SetProperty(ref _components, value); }
    }

    private string _searchText;
    public string SearchText {
      get { return _searchText; }
      set { SetProperty(ref _searchText, value); }
    }

    private ObservableCollection<string> _columns;
    public ObservableCollection<string> Columns {
      get { return _columns; }
      set { SetProperty(ref _columns, value); }
    }

    private int _selectedColumns;
    public int SelectedColumn {
      get { return _selectedColumns; }
      set { SetProperty(ref _selectedColumns, value); }
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

    #endregion

    public ComponentsGridViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      Title = TabNames.ComponentsGrid.GetDescription();

      Columns = new ObservableCollection<string>();
      PDStatusList = new ObservableCollection<string>();
      ComponentTypeList = new ObservableCollection<string>();
      PDSourceList = new ObservableCollection<string>();

      Columns.GetEnumValues<FilterableColumns>();
      PDStatusList.GetEnumValues<PDStatus>();
      ComponentTypeList.GetEnumValues<ComponentType>();
      PDSourceList.GetEnumValues<PDSource>();

      SelectedColumn = 1;
      SelectedPDStatus = 0;
      SelectedComponentType = 0;
      SelectedPDSource = 0;
      SearchText = string.Empty;

      SearchCommand = new DelegateCommand(GetFilteredComponentList);

      _eventAggregator.GetEvent<GetComponentsCommand>().Subscribe(ComponentListReceived);
      _eventAggregator.GetEvent<GetComponentsCommand>().Publish(ComponentList.GetComponentList());
      _eventAggregator.GetEvent<ImportXmlCommand>().Subscribe(NewComponentReceived);
    }

    private async void GetFilteredComponentList() {
      var column = (FilterableColumns)SelectedColumn;
      Func<ComponentInfo, bool> filter;

      if ((FilterableColumns)SelectedColumn == FilterableColumns.ComponentType
        || (FilterableColumns)SelectedColumn == FilterableColumns.PDSource
        || (FilterableColumns)SelectedColumn == FilterableColumns.PDStatus) {
        switch (column) {
          case FilterableColumns.PDStatus:
            filter = c => c.PDStatus == SelectedPDStatus;
            break;
          case FilterableColumns.ComponentType:
            filter = c => c.ComponentType == SelectedComponentType;
            break;
          case FilterableColumns.PDSource:
            filter = c => c.PDSource == SelectedPDSource;
            break;
          default:
            filter = c => c.PDNumber.Contains(SearchText);
            break;
        }

        Components = await ComponentList.GetFilteredListAsync(UnFilteredList.Where(filter));
      }
      else {
        if (!string.IsNullOrEmpty(SearchText) || Components.Count() != UnFilteredList.Count()) {
          switch (column) {
            case FilterableColumns.ComponentId:
              filter = c => c.ComponentId.ToString().Contains(SearchText);
              break;
            case FilterableColumns.PDNumber:
              filter = c => c.PDNumber.Contains(SearchText);
              break;
            case FilterableColumns.SourceComponentId:
              filter = c => c.SourceComponentId.ToString().Contains(SearchText);
              break;
            default:
              filter = c => c.PDNumber.Contains(SearchText);
              break;
          }

          Components = await ComponentList.GetFilteredListAsync(UnFilteredList.Where(filter));
        }
      }
    }

    private void NewComponentReceived(ComponentEdit obj) {
      Components.AddItem(obj);

      // Use the following method to get all the data directly from the database instead 
      // of adding the obj item to the list. 
      //Components = ComponentList.GetComponentList();
    }

    private void UpdateComponents() {

    }

    private void ComponentListReceived(ComponentList obj) => UnFilteredList = Components = obj;
  }
}
