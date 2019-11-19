using Reco3Xml2Db.UI.Module.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Reco3Xml2Db.UI.Module {
  public class Reco3Xml2DbModule : IModule {
    private IRegionManager _regionManager;
    private IUnityContainer _container;
    public void OnInitialized(IContainerProvider containerProvider) {

    }

    public void RegisterTypes(IContainerRegistry containerRegistry) {

    }
  }
}