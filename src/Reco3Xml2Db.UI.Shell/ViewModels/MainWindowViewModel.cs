using Prism.Commands;
using Prism.Regions;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.UI.Module.ViewModels;
using Reco3Xml2Db.Utilities.Extensions;
using Unity;

namespace Reco3Xml2Db.UI.Shell.ViewModels {
  public class MainWindowViewModel : ViewModelBase {
    private readonly IRegionManager _regionManager;
    //private readonly IUnityContainer _container;

    public DelegateCommand<string> NavigateCommand { get; set; }
    public string TabRegion { get; } = WindowRegions.TabRegion.ToString();

    public MainWindowViewModel(IRegionManager regionManager) { //, IUnityContainer container) {
      Title = Titles.AppTitle.GetDescription();

      _regionManager = regionManager;
      //_container = container;

      NavigateCommand = new DelegateCommand<string>(Navigate);
    }

    private void Navigate(string uri) {
      // using Navigation mechanism (not view discovery or view injection
      _regionManager.RequestNavigate(TabRegion, uri);
    }
  }
}
