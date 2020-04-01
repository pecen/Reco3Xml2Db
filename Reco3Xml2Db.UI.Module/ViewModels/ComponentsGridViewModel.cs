using Csla;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reco3Xml2Db.Library;
using Reco3Xml2Db.UI.Module.Commands;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.UI.Module.Services;
using Reco3Xml2Db.Utilities.Comparers;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class ComponentsGridViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;
    private IPathProvider _filePathProvider;

    #region Properties

    public string DeleteInfo { get; } = "Delete the selected components in the grid";

    public DelegateCommand SearchCommand { get; set; }
    public DelegateCommand UpdateComponentSetCommand { get; set; }
    public DelegateCommand DeleteComponentsCommand { get; set; }

    private int LastSearchLength { get; set; }
    public ComponentList UnFilteredList { get; set; }

    private bool _hasCheckedItem;
    private bool HasCheckedItem {
      get => _hasCheckedItem;
      set { SetProperty(ref _hasCheckedItem, value); }
    }

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

    public ComponentsGridViewModel(IEventAggregator eventAggregator, IPathProvider filePathProvider) {
      _eventAggregator = eventAggregator;
      _filePathProvider = filePathProvider;

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
      SearchText = string.Empty;
      LastSearchLength = 0;
      HasCheckedItem = false;

      SearchCommand = new DelegateCommand(GetFilteredComponentList);
      UpdateComponentSetCommand = new DelegateCommand(PublishComponentId);
      DeleteComponentsCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => HasCheckedItem);

      _eventAggregator.GetEvent<GetComponentsCommand>().Subscribe(ComponentListReceived);
      _eventAggregator.GetEvent<GetComponentsCommand>().Publish(ComponentList.GetComponentList());
      _eventAggregator.GetEvent<ImportComponentCommand>().Subscribe(NewComponentReceived);
      _eventAggregator.GetEvent<UpdateComponentCommand>().Subscribe(UpdateComponentReceived);
      _eventAggregator.GetEvent<GetFilenameCommand>().Subscribe(FilenameReceived);
      _eventAggregator.GetEvent<GetFilteredComponentsCommand>().Subscribe(FilteredComponentListReceived);

      _allSelected = false;
    }

    private bool CanExecute() {
      return Components.Any(c => c.IsChecked);
    }

    /// <summary>
    /// Delete Components
    /// </summary>
    private void Execute() {
      var count = Components.Where(c => c.IsChecked).Count();

      if (MessageBox.Show($"Are you sure you want to delete {(count > 1 ? "these components" : "this component")}?", 
        "Delete Component?", 
        MessageBoxButton.YesNo, 
        MessageBoxImage.Warning) == MessageBoxResult.Yes) {

        foreach (var item in Components) {
          if (item.IsChecked) {
            ComponentEdit.DeleteComponentAsync(item.ComponentId);
            //Components.RemoveItem(item);
            //UnFilteredList.RemoveItem(item);
            //if (Components.Count() == 0) break;
          }
        }

        _eventAggregator
          .GetEvent<GetComponentsCommand>()
          .Publish(ComponentList.GetComponentList());

        ClearFields();
        //RaisePropertyChanged(nameof(Components));
      }
    }

    private void FilenameReceived(string obj) {
      SearchText = string.Empty;
    }

    private void UpdateComponentReceived(ComponentEdit obj) {
      if (obj.IsDirty) {
        var item = UnFilteredList.First(c => c.ComponentId == obj.ComponentId);
        UnFilteredList.RemoveItem(item);
        UnFilteredList.AddItem(obj);
      }
      SearchText = string.Empty;
      LastSearchLength = 0;
    }

    private void PublishComponentId() {
      int id = 0;

      if (Components.Count() == 1) {
        id = Components[0].ComponentId;
      }

      _eventAggregator
        .GetEvent<UpdateComponentSetCommand>()
        .Publish(id);
    }

    private async void GetFilteredComponentList() {
      if(SelectedPDSource == -1 
        && SelectedPDStatus == -1 
        && SelectedComponentType == -1 
        && string.IsNullOrEmpty(SearchText)
        && LastSearchLength == 0) {
        return;
      }

      if ((bool)AllSelected) {
        AllSelected = false;
      }
      var column = (FilterableColumns)SelectedColumn;
      Func<ComponentInfo, bool> filter;

      Mouse.OverrideCursor = Cursors.Wait;

      if (((FilterableColumns)SelectedColumn == FilterableColumns.ComponentType && SelectedComponentType > -1)
        || ((FilterableColumns)SelectedColumn == FilterableColumns.PDSource && SelectedPDSource > -1)
        || ((FilterableColumns)SelectedColumn == FilterableColumns.PDStatus && SelectedPDStatus > -1)) {

        LastSearchLength = 0;
        SearchText = string.Empty;

        switch (column) {
          case FilterableColumns.PDStatus:
            filter = c => c.PDStatus == SelectedPDStatus;
            SelectedPDSource = SelectedComponentType = -1;
            break;
          case FilterableColumns.ComponentType:
            filter = c => c.ComponentType == SelectedComponentType;
            SelectedPDSource = SelectedPDStatus = -1;
            break;
          case FilterableColumns.PDSource:
            filter = c => c.PDSource == SelectedPDSource;
            SelectedPDStatus = SelectedComponentType = -1;
            break;
          default:
            filter = c => c.PDNumber.Contains(SearchText);
            break;
        }

        _eventAggregator
          .GetEvent<GetFilteredComponentsCommand>()
          .Publish(await ComponentList.GetFilteredListAsync(UnFilteredList.Where(filter)));
      }
      else {
        SelectedPDSource = SelectedPDStatus = SelectedComponentType = -1;

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

          if (LastSearchLength < SearchText.Length) {
            _eventAggregator
              .GetEvent<GetFilteredComponentsCommand>()
              .Publish(await ComponentList.GetFilteredListAsync(Components.Where(filter)));
          }
          else {
            _eventAggregator
              .GetEvent<GetFilteredComponentsCommand>()
              .Publish(await ComponentList.GetFilteredListAsync(UnFilteredList.Where(filter)));
          }

          LastSearchLength = SearchText.Length;
        }

        UpdateComponentSetCommand.Execute();
      }

      Mouse.OverrideCursor = Cursors.Arrow;
    }

    private void ClearFields() {
      AllSelected = false;
      SearchText = string.Empty;
      SelectedPDSource = SelectedPDStatus = SelectedComponentType = -1;
    }

    private void NewComponentReceived(ComponentEdit obj) {
      obj.PropertyChanged += ComponentOnPropertyChanged;
      UnFilteredList.AddItem(obj);

      // Use the following method to get all the data directly from the database instead 
      // of adding the obj item to the list. 
      //Components = ComponentList.GetComponentList();
    }

    private void ComponentOnPropertyChanged(object sender, PropertyChangedEventArgs args) {
      // Only re-check if the IsChecked property changed
      if (args.PropertyName == nameof(ComponentInfo.IsChecked)) {
        RecheckAllSelected();
      }
    }

    private void ComponentListReceived(ComponentList obj) {
      foreach (var component in obj) {
        component.PropertyChanged += ComponentOnPropertyChanged;
      }

      UnFilteredList = Components = obj;
    }

    private void FilteredComponentListReceived(ComponentList obj) {
      foreach (var component in obj) {
        component.PropertyChanged += ComponentOnPropertyChanged;
      }

      Components = obj;
    }

    private bool? _allSelected;
    public bool? AllSelected {
      get => _allSelected;
      set {
        SetProperty(ref _allSelected, value);

        // Set all other CheckBoxes
        AllSelectedChanged();
        //OnPropertyChanged();
      }
    }

    private void RecheckAllSelected() {
      // Has this change been caused by some other change?
      // return so we don't mess things up
      if (_allSelectedChanging) return;

      try {
        _allSelectedChanging = true;

        if (Components.All(e => e.IsChecked)) {
          AllSelected = true;
          HasCheckedItem = true;
        }
        else if (Components.All(e => !e.IsChecked)) {
          AllSelected = false;
          HasCheckedItem = false;
        }
        else {
          AllSelected = null;
          HasCheckedItem = true;
        }
      }
      finally {
        _allSelectedChanging = false;
      }
    }

    private bool _allSelectedChanging;
    private void AllSelectedChanged() {
      // Has this change been caused by some other change?
      // return so we don't mess things up
      if (_allSelectedChanging) return;

      try {
        _allSelectedChanging = true;

        // this can of course be simplified
        if (AllSelected == true) {
          foreach (var component in Components) {
            component.IsChecked = true;
          }
          HasCheckedItem = true;
        }
        else if (AllSelected == false) {
          foreach (var component in Components) {
            component.IsChecked = false;
          }
          HasCheckedItem = false;
        }
      }
      finally {
        _allSelectedChanging = false;
      }
    }
  }
}
