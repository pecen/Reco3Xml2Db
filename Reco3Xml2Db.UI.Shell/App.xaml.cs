using Reco3Xml2Db.UI.Shell.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using Prism.Unity;
using Reco3Xml2Db.UI.Module;

namespace Reco3Xml2Db.UI.Shell {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : PrismApplication {
    
    protected override Window CreateShell() {
      return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry) {
      
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) {
      base.ConfigureModuleCatalog(moduleCatalog);

      moduleCatalog.AddModule(typeof(Reco3Xml2DbModule));
    }
  }
}
