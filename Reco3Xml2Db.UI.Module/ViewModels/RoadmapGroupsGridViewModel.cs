﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Reco3Xml2Db.Library;
using Reco3Xml2Db.UI.Module.Commands;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class RoadmapGroupsGridViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;

    #region Properties

    public string DeleteInfo { get; } = "Delete the selected Roadmap Group(s) in the grid";
    public string SearchTextInfo { get; } = "Filter out which Roadmap Group(s) you want to see";

    public DelegateCommand DeleteRoadmapGroupsCommand { get; set; }
    public DelegateCommand SearchCommand { get; set; }

    private int LastSearchLength { get; set; }
    public RoadmapGroupList UnFilteredList { get; set; }

    private bool? _allSelected;
    public bool? AllSelected {
      get => _allSelected;
      set {
        SetProperty(ref _allSelected, value);

        // Set all other CheckBoxes
        AllSelectedChanged();
      }
    }

    private bool _hasCheckedItem;
    private bool HasCheckedItem {
      get => _hasCheckedItem;
      set { SetProperty(ref _hasCheckedItem, value); }
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

    private string _searchText;
    public string SearchText {
      get { return _searchText; }
      set { SetProperty(ref _searchText, value); }
    }

    private ObservableCollection<string> _validationStatus;
    public ObservableCollection<string> ValidationStatusList {
      get { return _validationStatus; }
      set { SetProperty(ref _validationStatus, value); }
    }

    private int _selectedValidationStatus;
    public int SelectedValidationStatus {
      get { return _selectedValidationStatus; }
      set { SetProperty(ref _selectedValidationStatus, value); }
    }

    private ObservableCollection<string> _convertToVehicleStatus;
    public ObservableCollection<string> ConvertToVehicleStatusList {
      get { return _convertToVehicleStatus; }
      set { SetProperty(ref _convertToVehicleStatus, value); }
    }

    private int _selectedConvertToVehicleStatus;
    public int SelectedConvertToVehicleStatus {
      get { return _selectedConvertToVehicleStatus; }
      set { SetProperty(ref _selectedConvertToVehicleStatus, value); }
    }

    private RoadmapGroupList _roadmapGroups;
    public RoadmapGroupList RoadmapGroups {
      get { return _roadmapGroups; }
      set { SetProperty(ref _roadmapGroups, value); }
    }

    #endregion

    public RoadmapGroupsGridViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      Title = TabNames.RoadmapGroupsGrid.GetDescription();

      Columns = new ObservableCollection<string>();
      ValidationStatusList = new ObservableCollection<string>();
      ConvertToVehicleStatusList = new ObservableCollection<string>();

      Columns.GetEnumValues<FilterableRoadmapGroupColumns>();
      ValidationStatusList.GetEnumValues<ValidationStatus>();
      ConvertToVehicleStatusList.GetEnumValues<ConvertToVehicleStatus>();

      SelectedColumn = 0;
      SelectedValidationStatus = -1;
      SelectedConvertToVehicleStatus = -1;
      SearchText = string.Empty;
      HasCheckedItem = false;

      _allSelected = false;

      DeleteRoadmapGroupsCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => HasCheckedItem);
      SearchCommand = new DelegateCommand(GetFilteredRoadmapGroupList);

      _eventAggregator.GetEvent<GetRoadmapGroupsCommand>().Subscribe(RoadmapGroupListReceived);
      //_eventAggregator.GetEvent<GetRoadmapGroupsCommand>().Publish(RoadmapGroupList.GetRoadmapGroups());
      _eventAggregator.GetEvent<GetFilteredRoadmapGroupsCommand>().Subscribe(FilteredVehicleListReceived);
    }

    private void FilteredVehicleListReceived(RoadmapGroupList obj) {
      foreach (var roadmapGroup in obj) {
        roadmapGroup.PropertyChanged += RoadmapGroupOnPropertyChanged;
      }

      RoadmapGroups = obj;
    }

    private bool CanExecute() {
      return RoadmapGroups != null 
          && RoadmapGroups.Any(c => c.IsChecked);
    }

    private void Execute() {
      var count = RoadmapGroups.Where(c => c.IsChecked).Count();

      if (MessageBox.Show($"Are you sure you want to delete {(count > 1 ? "these Roadmap Groups" : "this Roadmap Group")}?",
        "Delete Component?",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning) == MessageBoxResult.Yes) {

        foreach (var item in RoadmapGroups) {
          if (item.IsChecked) {
            RoadmapGroupEdit.DeleteRoadmapGroupAsync(item.RoadmapGroupId);
          }
        }

        _eventAggregator
          .GetEvent<GetComponentsCommand>()
          .Publish(ComponentList.GetComponentList());

        ClearFields();
      }
    }

    private async void GetFilteredRoadmapGroupList() {
      if (SelectedConvertToVehicleStatus == -1
        && SelectedValidationStatus == -1
        && string.IsNullOrEmpty(SearchText)
        && LastSearchLength == 0) {
        return;
      }

      if (AllSelected != null && (bool)AllSelected) {
        AllSelected = false;
      }

      var column = (FilterableRoadmapGroupColumns)SelectedColumn;
      Func<RoadmapGroupInfo, bool> filter;

      Mouse.OverrideCursor = Cursors.Wait;

      if (((FilterableRoadmapGroupColumns)SelectedColumn == FilterableRoadmapGroupColumns.ConvertToVehicleStatus && SelectedConvertToVehicleStatus > -1)
        || ((FilterableRoadmapGroupColumns)SelectedColumn == FilterableRoadmapGroupColumns.ValidationStatus && SelectedValidationStatus > -1)) {
        LastSearchLength = 0;
        SearchText = string.Empty;

        switch (column) {
          case FilterableRoadmapGroupColumns.ValidationStatus:
            filter = r => r.ValidationStatus == SelectedValidationStatus;
            SelectedConvertToVehicleStatus = -1;
            break;
          case FilterableRoadmapGroupColumns.ConvertToVehicleStatus:
            filter = r => r.ConvertToVehicleInputStatus == SelectedConvertToVehicleStatus;
            SelectedValidationStatus = -1;
            break;
          default:
            filter = r => r.RoadmapName.Contains(SearchText);
            break;
        }

        _eventAggregator
          .GetEvent<GetFilteredRoadmapGroupsCommand>()
          .Publish(await RoadmapGroupList.GetFilteredListAsync(UnFilteredList.Where(filter)));
      }
      else {
        SelectedValidationStatus = SelectedConvertToVehicleStatus = -1;

        if (!string.IsNullOrEmpty(SearchText) || RoadmapGroups.Count() != UnFilteredList.Count()) {
          switch (column) {
            case FilterableRoadmapGroupColumns.RoadmapGroupId:
              filter = r => r.RoadmapGroupId.ToString().Contains(SearchText);
              break;
            case FilterableRoadmapGroupColumns.OwnerSss:
              filter = r => r.OwnerSss.Contains(SearchText);
              break;
            default:
              filter = r => r.RoadmapName.Contains(SearchText);
              break;
          }

          if (LastSearchLength < SearchText.Length) {
            _eventAggregator
              .GetEvent<GetFilteredRoadmapGroupsCommand>()
              .Publish(await RoadmapGroupList.GetFilteredListAsync(RoadmapGroups.Where(filter)));
          }
          else {
            _eventAggregator
              .GetEvent<GetFilteredRoadmapGroupsCommand>()
              .Publish(await RoadmapGroupList.GetFilteredListAsync(UnFilteredList.Where(filter)));
          }

          LastSearchLength = SearchText.Length;
        }
      }

      Mouse.OverrideCursor = Cursors.Arrow;
    }

    private void RoadmapGroupListReceived(RoadmapGroupList obj) {
      foreach (var roadmapGroup in obj) {
        roadmapGroup.PropertyChanged += RoadmapGroupOnPropertyChanged;
      }

      UnFilteredList = RoadmapGroups = obj;
      RaisePropertyChanged(nameof(RoadmapGroups));
    }

    private void ClearFields() {
      AllSelected = false;
      SearchText = string.Empty;
      SelectedValidationStatus = SelectedConvertToVehicleStatus = -1;
    }

    public override void OnNavigatedTo(NavigationContext navigationContext) {
      base.OnNavigatedTo(navigationContext);

      Mouse.OverrideCursor = Cursors.Wait;
      _eventAggregator.GetEvent<GetRoadmapGroupsCommand>().Publish(RoadmapGroupList.GetRoadmapGroups());
      Mouse.OverrideCursor = Cursors.Arrow;
    }

    #region Row Select Checkbox handling

    private void RoadmapGroupOnPropertyChanged(object sender, PropertyChangedEventArgs args) {
      // Only re-check if the IsChecked property changed
      if (args.PropertyName == nameof(RoadmapGroupInfo.IsChecked)) {
        RecheckAllSelected();
      }
    }

    private void RecheckAllSelected() {
      // Has this change been caused by some other change?
      // return so we don't mess things up
      if (_allSelectedChanging) return;

      try {
        _allSelectedChanging = true;

        if (RoadmapGroups.All(e => e.IsChecked)) {
          AllSelected = true;
          HasCheckedItem = true;
        }
        else if (RoadmapGroups.All(e => !e.IsChecked)) {
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
          foreach (var component in RoadmapGroups) {
            component.IsChecked = true;
          }
          HasCheckedItem = true;
        }
        else if (AllSelected == false) {
          foreach (var component in RoadmapGroups) {
            component.IsChecked = false;
          }
          HasCheckedItem = false;
        }
      }
      finally {
        _allSelectedChanging = false;
      }
    }
    #endregion
  }
}
