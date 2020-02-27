using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reco3Xml2Db.Library;
using Reco3Xml2Db.UI.Module.Commands;
using Reco3Xml2Db.UI.Module.Enums;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class ComponentsGridViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;

    //public ComponentList Components { get; set; }

    private ComponentList _components;
    public ComponentList Components {
      get { return _components; }
      set { SetProperty(ref _components, value); }
    }

    public ComponentsGridViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      Title = TabNames.ComponentsGrid.GetDescription();

      _eventAggregator.GetEvent<GetComponentsCommand>().Subscribe(ComponentListReceived);
      _eventAggregator.GetEvent<GetComponentsCommand>().Publish(ComponentList.GetComponentList());
      _eventAggregator.GetEvent<ImportXmlCommand>().Subscribe(ComponentReceived);
    }

    private void ComponentReceived(ComponentEdit obj) {
      //var component = ComponentInfo.GetComponent(obj.PDNumber);

      //var component = ComponentInfo.NewComponent();
      var component = Components.AddNew();

      component.PDNumber = obj.PDNumber;
      component.DownloadedTimestamp = obj.DownloadedTimestamp.ToShortDateString();
      component.Description = obj.Description;
      component.PDStatus = obj.PDStatus;
      component.ComponentType = obj.ComponentType;
      component.PDSource = obj.PDSource;
      component.Xml = obj.Xml;

      //Components.Add(component);
    }

    private void ComponentListReceived(ComponentList obj) => Components = obj;
  }
}
