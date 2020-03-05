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
      SelectedPDStatus = -1;
      SelectedComponentType = -1;
      SelectedPDSource = -1;

      SearchCommand = new DelegateCommand(GetFilteredComponentList);

      _eventAggregator.GetEvent<GetComponentsCommand>().Subscribe(ComponentListReceived);
      _eventAggregator.GetEvent<GetComponentsCommand>().Publish(ComponentList.GetComponentList());
      _eventAggregator.GetEvent<ImportXmlCommand>().Subscribe(NewComponentReceived);
    }

    private async void GetFilteredComponentList() {
      if (!string.IsNullOrEmpty(SearchText) || Components.Count() != UnFilteredList.Count()) {
        var column = (FilterableColumns)SelectedColumn;
        Func<ComponentInfo, bool> predicate;

        switch (column) {
          case FilterableColumns.ComponentId:
            predicate = c => c.ComponentId.ToString().Contains(SearchText);
            //Components = await ComponentList.GetFilteredListAsync(UnFilteredList.Where(c => c.ComponentId.ToString().Contains(SearchText)));
            break;
          case FilterableColumns.PDNumber:
            predicate = c => c.PDNumber.Contains(SearchText);
            //Components = await ComponentList.GetFilteredListAsync(UnFilteredList.Where(c => c.PDNumber.Contains(SearchText)));
            break;
          case FilterableColumns.PDStatus:
            predicate = c => c.PDStatus.ToString().Contains(SearchText);
            //Components = await ComponentList.GetFilteredListAsync(UnFilteredList.Where(c => c.PDStatus.ToString().Contains(SearchText)));
            break;
          case FilterableColumns.ComponentType:
            predicate = c => c.ComponentType.ToString().Contains(SearchText);
            //Components = await ComponentList.GetFilteredListAsync(UnFilteredList.Where(c => c.ComponentType.ToString().Contains(SearchText)));
            break;
          case FilterableColumns.PDSource:
            predicate = c => c.PDSource.ToString().Contains(SearchText);
            //Components = await ComponentList.GetFilteredListAsync(UnFilteredList.Where(c => c.PDSource.ToString().Contains(SearchText)));
            break;
          case FilterableColumns.SourceComponentId:
            predicate = c => c.SourceComponentId.ToString().Contains(SearchText);
            //Components = await ComponentList.GetFilteredListAsync(UnFilteredList.Where(c => c.SourceComponentId.ToString().Contains(SearchText)));
            break;
          default:
            predicate = c => c.PDNumber.Contains(SearchText);
            break;
        }

        Components = await ComponentList.GetFilteredListAsync(UnFilteredList.Where(predicate));
      }
    }

    private void NewComponentReceived(ComponentEdit obj) {
      //var tmpList = Components;
      //Components = null;
      //var tmpObj = tmpList.AddNew();
      //tmpObj = obj;
      ////tmpList.Add(obj);
      //Components = tmpList;

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
