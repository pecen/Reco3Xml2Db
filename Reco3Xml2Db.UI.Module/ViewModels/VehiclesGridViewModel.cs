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
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class VehiclesGridViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;

    #region Properties

    public string DeleteInfo { get; } = "Delete the selected vehicles in the grid";

    public DelegateCommand DeleteVehiclesCommand { get; set; }
    public DelegateCommand SearchCommand { get; set; }

    private int LastSearchLength { get; set; }
    public VehicleList UnFilteredList { get; set; }

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

    private VehicleList _vehicles;
    public VehicleList Vehicles {
      get { return _vehicles; }
      set { SetProperty(ref _vehicles, value); }
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

    private ObservableCollection<string> _vehicleModeList;
    public ObservableCollection<string> VehicleModeList {
      get { return _vehicleModeList; }
      private set { SetProperty(ref _vehicleModeList, value); }
    }

    private int _selectedVehicleMode;
    public int SelectedVehicleMode {
      get { return _selectedVehicleMode; }
      set { SetProperty(ref _selectedVehicleMode, value); }
    }

    #endregion

    public VehiclesGridViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      Title = TabNames.VehiclesGrid.GetDescription();

      Columns = new ObservableCollection<string>();
      VehicleModeList = new ObservableCollection<string>();

      Columns.GetEnumValues<FilterableVehicleColumns>();
      VehicleModeList.GetEnumValues<VehicleMode>();

      SelectedColumn = 1;
      SelectedVehicleMode = -1;
      SearchText = string.Empty;
      HasCheckedItem = false;

      _allSelected = false;

      DeleteVehiclesCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => HasCheckedItem);
      SearchCommand = new DelegateCommand(GetFilteredVehicleList);

      _eventAggregator.GetEvent<GetVehiclesCommand>().Subscribe(VehicleListReceived);
      _eventAggregator.GetEvent<GetVehiclesCommand>().Publish(VehicleList.GetVehicleList());
      _eventAggregator.GetEvent<GetFilteredVehiclesCommand>().Subscribe(FilteredVehicleListReceived);
    }

    private void FilteredVehicleListReceived(VehicleList obj) {
      foreach (var vehicle in obj) {
        vehicle.PropertyChanged += VehicleOnPropertyChanged;
      }

      Vehicles = obj;
    }

    private void VehicleListReceived(VehicleList obj) {
      foreach (var vehicle in obj) {
        vehicle.PropertyChanged += VehicleOnPropertyChanged;
      }

      UnFilteredList = Vehicles = obj;
      RaisePropertyChanged(nameof(Vehicles));
    }

    private void VehicleOnPropertyChanged(object sender, PropertyChangedEventArgs args) {
      // Only re-check if the IsChecked property changed
      if (args.PropertyName == nameof(VehicleInfo.IsChecked)) {
        RecheckAllSelected();
      }
    }

    private async void GetFilteredVehicleList() {
      if (SelectedVehicleMode == -1
        && string.IsNullOrEmpty(SearchText)
        && LastSearchLength == 0) {
        return;
      }

      if (AllSelected != null && (bool)AllSelected) {
        AllSelected = false;
      }

      var column = (FilterableVehicleColumns)SelectedColumn;
      Func<VehicleInfo, bool> filter;

      Mouse.OverrideCursor = Cursors.Wait;

      if ((FilterableVehicleColumns)SelectedColumn == FilterableVehicleColumns.VehicleMode && SelectedVehicleMode > -1) {
        LastSearchLength = 0;
        SearchText = string.Empty;

        switch (column) {
          case FilterableVehicleColumns.VehicleMode:
            filter = v => v.VehicleMode == SelectedVehicleMode;
            break;
          default:
            filter = v => v.VIN.Contains(SearchText);
            break;
        }

        _eventAggregator
          .GetEvent<GetFilteredVehiclesCommand>()
          .Publish(await VehicleList.GetFilteredListAsync(UnFilteredList.Where(filter)));
      }
      else {
        SelectedVehicleMode = -1;

        if (!string.IsNullOrEmpty(SearchText) || Vehicles.Count() != UnFilteredList.Count()) {
          switch (column) {
            case FilterableVehicleColumns.VehicleId:
              filter = v => v.VehicleId.ToString().Contains(SearchText);
              break;
            case FilterableVehicleColumns.VIN:
              filter = v => v.VIN.Contains(SearchText);
              break;
            case FilterableVehicleColumns.GroupId:
              filter = v => v.GroupId.ToString().Contains(SearchText);
              break;
            default:
              filter = v => v.VIN.Contains(SearchText);
              break;
          }

          if (LastSearchLength < SearchText.Length) {
            _eventAggregator
              .GetEvent<GetFilteredVehiclesCommand>()
              .Publish(await VehicleList.GetFilteredListAsync(Vehicles.Where(filter)));
          }
          else {
            _eventAggregator
              .GetEvent<GetFilteredVehiclesCommand>()
              .Publish(await VehicleList.GetFilteredListAsync(UnFilteredList.Where(filter)));
          }

          LastSearchLength = SearchText.Length;
        }
      }

      Mouse.OverrideCursor = Cursors.Arrow;
    }

    private bool CanExecute() {
      return Vehicles.Any(v => v.IsChecked);
    }

    private void Execute() {
      var count = Vehicles.Where(c => c.IsChecked).Count();

      if (MessageBox.Show($"Are you sure you want to delete {(count > 1 ? "these vehicles" : "this vehicle")}?",
        "Delete Vehicle?",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning) == MessageBoxResult.Yes) {

        foreach (var item in Vehicles) {
          if (item.IsChecked) {
            VehicleEdit.DeleteVehicleAsync(item.VehicleId);
          }
        }

        _eventAggregator
          .GetEvent<GetVehiclesCommand>()
          .Publish(VehicleList.GetVehicleList());

        ClearFields();
      }
    }

    private void RecheckAllSelected() {
      // Has this change been caused by some other change?
      // return so we don't mess things up
      if (_allSelectedChanging) return;

      try {
        _allSelectedChanging = true;

        if (Vehicles.All(e => e.IsChecked)) {
          AllSelected = true;
          HasCheckedItem = true;
        }
        else if (Vehicles.All(e => !e.IsChecked)) {
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
          foreach (var vehicle in Vehicles) {
            vehicle.IsChecked = true;
          }
          HasCheckedItem = true;
        }
        else if (AllSelected == false) {
          foreach (var vehicle in Vehicles) {
            vehicle.IsChecked = false;
          }
          HasCheckedItem = false;
        }
      }
      finally {
        _allSelectedChanging = false;
      }
    }
    private void ClearFields() {
      AllSelected = false;
      SearchText = string.Empty;
      SelectedVehicleMode = -1;
    }

  }
}
