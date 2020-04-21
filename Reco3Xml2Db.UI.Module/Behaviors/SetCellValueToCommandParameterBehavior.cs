using Reco3Xml2Db.Library;
using Reco3Xml2Db.UI.Module.Services;
using Reco3Xml2Db.Utilities.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Reco3Xml2Db.UI.Module.Behaviors {
  public class SetCellValueToCommandParameterBehavior : Behavior<MenuItem> {
    /// <summary>
    /// The grid view service.
    /// </summary>
    private readonly IGridViewService _gridViewService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetCellValueToCommandParameterBehavior"/> class.
    /// </summary>
    public SetCellValueToCommandParameterBehavior() {
      try {
        _gridViewService = UnityService.Get().Resolve<IGridViewService>();
      }
      catch {
      }
    }

    /// <summary>
    /// Called after the behavior is attached to an AssociatedObject.
    /// </summary>
    protected override void OnAttached() {
      base.OnAttached();
      AssociatedObject.Click += AssociatedObject_Click;
      AssociatedObject.Loaded += AssociatedObject_Loaded; 
    }

    /// <summary>
    /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
    /// </summary>
    /// <remarks>
    /// Override this to unhook functionality from the AssociatedObject.
    /// </remarks>
    protected override void OnDetaching() {
      base.OnDetaching();
      AssociatedObject.Click -= AssociatedObject_Click;
      AssociatedObject.Loaded -= AssociatedObject_Loaded;
    }

    private void AssociatedObject_Click(object sender, RoutedEventArgs e) {
      AssociatedObject.CommandParameter = GetCellValue();
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e) {
    }

    /// <summary>
    /// Gets the cell value
    /// </summary>
    /// <returns></returns>
    private string GetCellValue() {
      var menuItem = AssociatedObject as MenuItem;
      var parent = menuItem.Parent as ContextMenu;
      var grid = parent.PlacementTarget as DataGrid;

      if (grid.IsNotNull()) {
        var componentInfo = grid.CurrentItem as ComponentInfo;
        if (componentInfo.IsNotNull()) {
          return componentInfo.Xml;
        }
      }

      return string.Empty;
    }
  }
}
