using Reco3Xml2Db.UI.Module.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;
using Prism.Unity;
using Reco3Xml2Db.UI.Module.Enums;

namespace Reco3Xml2Db.UI.Module {
  public class Reco3Xml2DbModule : IModule {
    public void OnInitialized(IContainerProvider containerProvider) {
      var regionManager = containerProvider.Resolve<IRegionManager>();
      regionManager.RegisterViewWithRegion(WindowRegions.TabRegion.ToString(), typeof(ImportXml));
      regionManager.RegisterViewWithRegion(WindowRegions.TabRegion.ToString(), typeof(Settings));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry) {
      containerRegistry.RegisterForNavigation(typeof(ImportXml), nameof(ImportXml));
      containerRegistry.RegisterForNavigation(typeof(Settings), nameof(Settings));
    }
  }
}