using Reco3Xml2Db.UI.Module.Services;
using Reco3Xml2Db.UI.Module.ViewModels;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
      //AssociatedObject.MouseRightButtonDown += AssociatedObject_OnMouseRightButtonDown;
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
      //ContextMenu menu = AssociatedObject.ContextMenu as ContextMenu;
      var menuItem = AssociatedObject as MenuItem;

      MenuItem menu = AssociatedObject;
      if (menuItem.IsNotNull()) {
        var clickedMenuItem = sender as MenuItem;
        
        
        //DataGridCell cell = menu.GetClickedElement<GridViewCell>();
        //if (cell.IsNotNull()
        //    && cell.Value.IsNotNull()
        //    && cell.IsSelected) {
        //  TextBlock provider = cell.Content as TextBlock;

        //  if (provider.IsNull()) {
        //    AssociatedObject.CommandParameter = cell.Value.ToString();
        //  }
        //  else {
        //    AssociatedObject.CommandParameter = provider.Text == @"\0" ? string.Empty : provider.Text;
        //  }
        //}
      }
    }

    /// <summary>
    /// Gets the cell value
    /// </summary>
    /// <returns></returns>
    private string GetCellValue() {
      var menuItem = AssociatedObject as MenuItem;
      var parent = menuItem.Parent as ContextMenu;
      var grid = parent.PlacementTarget as DataGrid;
      //var textBlock = grid.InputHitTest(e.GetPosition(grid)) as TextBlock;
      //if (textBlock.IsNotNull()) {
      //  return textBlock.Text;
      //}

      //if (menuItem.IsNotNull()) {
      //  var clickedMenuItem = sender as MenuItem;
      //  var dc = clickedMenuItem.DataContext as ComponentsGridViewModel;
      //var xml = dc.SelectedItem.Xml;

      //var parent = clickedMenuItem.Parent as ContextMenu;
      //var grid = parent.PlacementTarget as DataGrid;
      //var cell = grid.CurrentCell;
      //var v = grid.CurrentItem;
      //string s = string.Empty;


      //DataGridCell cell = menu.GetClickedElement<DataGridCell>();

      //if (cell.IsNotNull() && cell.Value.IsNotNull()) {
      //  TextBlock provider = cell.Content as TextBlock;

      //  if (provider.IsNull()) {
      //    return cell.Value.ToString();
      //  }

      //  if (provider.FontFamily.ToString() == NodeListItemTypes.IconFontFamily.GetEnumDescription()) {
      //    return GetIconCellValue(cell.Column.UniqueName, Convert.ToChar(provider.Text));
      //  }

      //  return _gridViewService.GetText(cell);
      //}
    //}
      return string.Empty;
    }
  }
}
