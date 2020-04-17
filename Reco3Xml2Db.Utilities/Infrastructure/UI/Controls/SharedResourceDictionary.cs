using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace Reco3Xml2Db.Utilities.Infrastructure.UI.Controls {
  /// <summary>
  /// The shared resource dictionary is a specialized resource dictionary
  /// that loads it content only once. If a second instance with the same source
  /// is created, it only merges the resources from the cache.
  /// </summary>
  [Localizability(LocalizationCategory.Ignore)]
  [Ambient]
  [UsableDuringInitialization(true)]
  public class SharedResourceDictionary : ResourceDictionary {
    /// <summary>
    /// Internal cache of loaded dictionaries
    /// </summary>
    private static Dictionary<Uri, ResourceDictionary> sharedDictionaries = new Dictionary<Uri, ResourceDictionary>();

    /// <summary>
    /// Gets a value indicating whether this instance is in design mode.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is in design mode; otherwise, <c>false</c>.
    /// </value>
    public static bool IsInDesignMode => (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;

    /// <summary>
    /// The shared source
    /// </summary>
    private string _sharedSource;

    /// <summary>
    /// Gets or sets the shared source.
    /// </summary>
    /// <value>
    /// The shared source.
    /// </value>
    public string SharedSource {
      get {
        return _sharedSource;
      }
      set {
        Uri sourceUri = new Uri(value, UriKind.RelativeOrAbsolute);
        if (IsInDesignMode) {
          Source = sourceUri;
        }
        else if (_sharedSource != value) {
          _sharedSource = value;
          OnSharedSourceChange(sourceUri);
        }
      }
    }

    /// <summary>
    /// Called when shared source has changed.
    /// </summary>
    private void OnSharedSourceChange(Uri value) {
      lock (((ICollection)sharedDictionaries).SyncRoot) {
        if (!sharedDictionaries.TryGetValue(value, out ResourceDictionary resourceDictionary)) {
          Source = value;
          sharedDictionaries.Add(value, this);
        }
      }
    }
  }
}
