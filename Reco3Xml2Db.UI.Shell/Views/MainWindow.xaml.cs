//using Reco3Xml2Db.UI.Shell.ViewModels;
//using System;
//using System.Runtime.InteropServices;
using System.Windows;
//using System.Windows.Interop;

namespace Reco3Xml2Db.UI.Shell.Views {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    // The commented out code and usings are for adding items to the MainWindow context menu, i.e. right-clicking on
    // the MainWindow title bar. 

    //[DllImport("user32.dll")]
    //private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    //[DllImport("user32.dll")]
    //private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

    //public const Int32 MF_BYPOSITION = 0x400;
    //public const Int32 MF_SEPARATOR = 0x800;

    //public const Int32 ITEMONEID = 1000;
    //public const Int32 ITEMTWOID = 1001;

    //public const Int32 WM_SYSCOMMAND = 0x112;

    public MainWindow() {
      InitializeComponent();
      //Loaded += MainWindow_Loaded;
    }

    //private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
    //  IntPtr windowHandle = new WindowInteropHelper(this).Handle;
    //  HwndSource hwndSource = HwndSource.FromHwnd(windowHandle);

    //  IntPtr systemMenuHandle = GetSystemMenu(windowHandle, false);

    //  InsertMenu(systemMenuHandle, 5, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty); // separator
    //  InsertMenu(systemMenuHandle, 6, MF_BYPOSITION, ITEMONEID, "Item 1"); 
    //  InsertMenu(systemMenuHandle, 7, MF_BYPOSITION, ITEMTWOID, "Item 2"); 

    //  hwndSource.AddHook(new HwndSourceHook(WndProc));
    //}

    //private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
    //  if(msg == WM_SYSCOMMAND) {
    //    switch(wParam.ToInt32()) {
    //      case ITEMONEID: {
    //          MessageBox.Show("Item 1 was clicked");
    //          //var vm = (MainWindowViewModel)this.DataContext;
    //          handled = true;
    //          break;
    //        }
    //      case ITEMTWOID: {
    //          MessageBox.Show("Item 2 was clicked");
    //          handled = true;
    //          break;
    //        }
    //    }
    //  }
    //  return IntPtr.Zero;
    //}
  }
}
