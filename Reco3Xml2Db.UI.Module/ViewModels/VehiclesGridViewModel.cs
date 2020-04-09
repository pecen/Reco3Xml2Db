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

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class VehiclesGridViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;

    #region Properties

    public string DeleteInfo { get; } = "Delete the selected vehicles in the grid";

    public DelegateCommand DeleteVehiclesCommand { get; set; }
    public DelegateCommand SearchCommand { get; set; }

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

      SelectedColumn = 0;
      SelectedVehicleMode = -1;
      SearchText = string.Empty;
      HasCheckedItem = false;

      _allSelected = false;

      DeleteVehiclesCommand = new DelegateCommand(Execute, CanExecute)
        .ObservesProperty(() => HasCheckedItem);
      SearchCommand = new DelegateCommand(GetFilteredVehicleList);

      _eventAggregator.GetEvent<GetVehiclesCommand>().Subscribe(VehicleListReceived);
      _eventAggregator.GetEvent<GetVehiclesCommand>().Publish(VehicleList.GetVehicleList());
    }

    private void VehicleListReceived(VehicleList obj) {
      foreach (var vehicle in obj) {
        vehicle.PropertyChanged += VehicleOnPropertyChanged;
      }

      Vehicles = obj;
      RaisePropertyChanged(nameof(Vehicles));
    }

    private void VehicleOnPropertyChanged(object sender, PropertyChangedEventArgs args) {
      // Only re-check if the IsChecked property changed
      if (args.PropertyName == nameof(VehicleInfo.IsChecked)) {
        RecheckAllSelected();
      }
    }

    private void GetFilteredVehicleList() {
      throw new NotImplementedException();
    }

    private bool CanExecute() {
      return Vehicles.Any(v => v.IsChecked);
    }

    private void Execute() {
      throw new NotImplementedException();
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
  }
}
