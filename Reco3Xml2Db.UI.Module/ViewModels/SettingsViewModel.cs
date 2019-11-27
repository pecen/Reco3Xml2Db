using Prism.Commands;
using Prism.Mvvm;
using Reco3Xml2Db.UI.Module.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class SettingsViewModel : ViewModelBase {
    public string PageHeader { get; } = "Type in the configuration settings below:";

    public SettingsViewModel() {
      Title = TabNames.Settings.ToString();
    }
  }
}
