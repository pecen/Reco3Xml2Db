using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.ObjectModel;

namespace Reco3Xml2Db.UI.Module.ViewModels {
  public class ViewModelBase : BindableBase {
    public DelegateCommand GetFilePathCommand { get; set; }

    string _title;
    public string Title {
      get { return _title; }
      set { SetProperty(ref _title, value); }
    }

    public virtual bool IsNavigationTarget(NavigationContext navigationContext) {
      return true;
    }

    // Making this one virtual to be able to override in the child classes
    public virtual void OnNavigatedFrom(NavigationContext navigationContext) {
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {

    }

    protected void GetEnumValues<T>(ObservableCollection<string> list) where T : Enum {
      foreach (T item in Enum.GetValues(typeof(T))) {
        var description = EnumExtensions.GetEnumDescription(item);

        if (string.IsNullOrEmpty(description)) {
          list.Add(item.ToString());
        }
        else {
          list.Add(description);
        }
      }
    }
  }
}
