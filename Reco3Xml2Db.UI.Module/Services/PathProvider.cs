using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Reco3Xml2Db.UI.Module.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.UI.Module.Services {
  public class PathProvider : IPathProvider {
    private IEventAggregator _eventAggregator;
    public DelegateCommand GetFilenameCommand { get; set; }

    public PathProvider(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;
    }

    // Publishes the filename through the eventaggregator and returns
    // the Xml-stream
    public Stream FilePathService(string initialDirectory = "") {
      OpenFileDialog openFileDialog = new OpenFileDialog {
        InitialDirectory = initialDirectory,
        Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*",
        FilterIndex = 1,
        RestoreDirectory = true
      };

      if ((bool)openFileDialog.ShowDialog()) {
        _eventAggregator.GetEvent<GetFilenameCommand>()
          .Publish(openFileDialog.FileName);

        return openFileDialog.OpenFile();
      }

      return null;
    }

    // Publishes the filepath through the eventaggregator 
    public void FolderPathService(string initialDirectory = "") {
      var dialog = new CommonOpenFileDialog {
        InitialDirectory = initialDirectory,
        IsFolderPicker = true
      };

      if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
        // dialog.FileName below holds the path and not the filename
        // even though the property FileName is used. 
        _eventAggregator.GetEvent<GetFilePathCommand>()
          .Publish(dialog.FileName);
      }
    }
  }
}
