using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reco3Xml2Db.UI.Module.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class XmlViewerViewModel : ViewModelBase {
    private IEventAggregator _eventAggregator;

    public XmlViewerViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;

      _eventAggregator.GetEvent<GetPDNumberCommand>().Subscribe(PDNumberReceived);
    }

    private void PDNumberReceived(string obj) => Title = obj;
  }
}
