using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.UI.Module.Services;
using Reco3Xml2Db.UI.Module.Views;
using Unity;

namespace Reco3Xml2Db.UI.Module {
  public class Reco3Xml2DbModule : IModule {
    public static IContainerProvider Container { get; private set; }

    public void OnInitialized(IContainerProvider containerProvider) {
      var regionManager = containerProvider.Resolve<IRegionManager>();
      //regionManager.RegisterViewWithRegion(WindowRegions.ComponentRegion.ToString(), typeof(ImportXml));
      //regionManager.RegisterViewWithRegion(WindowRegions.SettingsRegion.ToString(), typeof(Settings));
      //regionManager.RegisterViewWithRegion(WindowRegions.ListRegion.ToString(), typeof(ComponentsGrid));
      regionManager.RegisterViewWithRegion(WindowRegions.TabRegion.ToString(), typeof(ImportXml));
      regionManager.RegisterViewWithRegion(WindowRegions.TabRegion.ToString(), typeof(ComponentsGrid));
      regionManager.RegisterViewWithRegion(WindowRegions.TabRegion.ToString(), typeof(RoadmapGroupsGrid));
      regionManager.RegisterViewWithRegion(WindowRegions.TabRegion.ToString(), typeof(VehiclesGrid));
      regionManager.RegisterViewWithRegion(WindowRegions.TabRegion.ToString(), typeof(Settings));

      Container = containerProvider;
    }

    public void RegisterTypes(IContainerRegistry containerRegistry) {
      //containerRegistry.RegisterForNavigation(typeof(ImportXml), nameof(ImportXml));
      //containerRegistry.RegisterForNavigation(typeof(ComponentsGrid), nameof(ComponentsGrid));
      //containerRegistry.RegisterForNavigation(typeof(Settings), nameof(Settings));
      //containerRegistry.RegisterForNavigation(typeof(RoadmapGroupsGrid), nameof(RoadmapGroupsGrid));

      containerRegistry.RegisterSingleton<IUnityService, UnityService>();
      containerRegistry.RegisterSingleton<IPathProvider, PathProvider>();
      containerRegistry.RegisterSingleton<IXmlProvider, XmlProvider>();
      containerRegistry.RegisterSingleton<IGridViewService, GridViewService>();
    }
  }
}