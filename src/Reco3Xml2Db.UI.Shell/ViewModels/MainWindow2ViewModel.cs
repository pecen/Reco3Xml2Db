using Prism.Commands;
using Prism.Regions;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.UI.Module.ViewModels;
using Unity;

namespace Reco3Xml2Db.UI.Shell.ViewModels {
  public class MainWindow2ViewModel : ViewModelBase {
    private readonly IRegionManager _regionManager;
    private readonly IUnityContainer _container;

    //public DelegateCommand<string> NavigateCommand { get; set; }
    public string ComponentRegion { get; } = WindowRegions.ComponentRegion.ToString();
    //public string SettingsRegion { get; } = WindowRegions.SettingsRegion.ToString();
    public string ListRegion { get; } = WindowRegions.ListRegion.ToString();

    public MainWindow2ViewModel(IRegionManager regionManager, IUnityContainer container)
    {
      Title = "Reco3 Xml to Db";

      _regionManager = regionManager;
      _container = container;

      //NavigateCommand = new DelegateCommand<string>(Navigate);
    }

    //private void Navigate(string uri)
    //{
    //  // using Navigation mechanism (not view discovery or view injection
    //  _regionManager.RequestNavigate(TabRegion, uri);
    //}
  }
}
