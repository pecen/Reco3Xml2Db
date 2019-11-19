using Prism.Commands;
using Prism.Regions;
using Reco3Xml2Db.UI.Module.ViewModels;
using Unity;

namespace Reco3Xml2Db.UI.Shell.ViewModels {
  public class MainWindowViewModel : ViewModelBase {
    private readonly IRegionManager _regionManager;
    private readonly IUnityContainer _container;

    public DelegateCommand<string> NavigateCommand { get; set; }

    private string _contentRegion; // = "ContentRegion";
    public string ContentRegion {
      get { return _contentRegion; }
      set { SetProperty(ref _contentRegion, value); }
    }

    private string _tabRegionRegion; // = "TabRegion";
    public string TabRegion {
      get { return _tabRegionRegion; }
      set { SetProperty(ref _tabRegionRegion, value); }
    }

    public MainWindowViewModel(IRegionManager regionManager, IUnityContainer container) {
      Title = "Reco3 Xml to Db";

      _regionManager = regionManager;
      _container = container;

      NavigateCommand = new DelegateCommand<string>(Navigate);

      ContentRegion = "ContentRegion"; // EnumExtensions.GetEnumDescription(WindowRegions.ContentRegion);
      //TabRegion = EnumExtensions.GetEnumDescription(WindowRegions.TabRegion);
      TabRegion = "TabRegion"; // WindowRegions.TabRegion.ToString();
    }

    private void Navigate(string uri) {
      // using Navigation mechanism (not view discovery or view injection
      //_regionManager.RequestNavigate(_contentRegion, uri);
      _regionManager.RequestNavigate(TabRegion, uri);
    }
  }
}
