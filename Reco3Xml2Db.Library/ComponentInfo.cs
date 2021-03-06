﻿using Csla;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using Reco3Xml2Db.Dal.Enums;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class ComponentInfo : ReadOnlyBase<ComponentInfo>, INotifyPropertyChanged {
    #region IsChecked

    private bool _isChecked;
    public bool IsChecked {
      get => _isChecked;
      set {
        if (value == _isChecked) return;
        _isChecked = value;
        OnPropertyChanged(nameof(IsChecked));
      }
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<int> ComponentIdProperty = RegisterProperty<int>(c => c.ComponentId);
    public int ComponentId {
      get { return GetProperty(ComponentIdProperty); }
      set { LoadProperty(ComponentIdProperty, value); }
    }

    public static readonly PropertyInfo<string> PDNumberProperty = RegisterProperty<string>(c => c.PDNumber);
    public string PDNumber {
      get { return GetProperty(PDNumberProperty); }
      set { LoadProperty(PDNumberProperty, value); }
    }

    public static readonly PropertyInfo<string> DownloadedTimestampProperty = RegisterProperty<string>(c => c.DownloadedTimestamp);
    public string DownloadedTimestamp {
      get { return GetProperty(DownloadedTimestampProperty); }
      set { LoadProperty(DownloadedTimestampProperty, value); }
    }

    public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(c => c.Description);
    public string Description {
      get { return GetProperty(DescriptionProperty); }
      set { LoadProperty(DescriptionProperty, value); }
    }

    public static readonly PropertyInfo<int> PDStatusProperty = RegisterProperty<int>(c => c.PDStatus);
    public int PDStatus {
      get { return GetProperty(PDStatusProperty); }
      set { LoadProperty(PDStatusProperty, value); }
    }

    public static readonly PropertyInfo<int> ComponentTypeProperty = RegisterProperty<int>(c => c.ComponentType);
    public int ComponentType {
      get { return GetProperty(ComponentTypeProperty); }
      set { LoadProperty(ComponentTypeProperty, value); }
    }

    public static readonly PropertyInfo<string> XmlProperty = RegisterProperty<string>(c => c.Xml);
    public string Xml {
      get { return GetProperty(XmlProperty); }
      set { LoadProperty(XmlProperty, value); }
    }

    public static readonly PropertyInfo<int> PDSourceProperty = RegisterProperty<int>(c => c.PDSource);
    public int PDSource {
      get { return GetProperty(PDSourceProperty); }
      set { LoadProperty(PDSourceProperty, value); }
    }

    public static readonly PropertyInfo<int?> SourceComponentIdProperty = RegisterProperty<int?>(c => c.SourceComponentId);
    public int? SourceComponentId {
      get { return GetProperty(SourceComponentIdProperty); }
      set { LoadProperty(SourceComponentIdProperty, value); }
    }

    #endregion

    #region Factory Methods

    public static ComponentInfo NewComponent() {
      return DataPortal.Create<ComponentInfo>();
    }

    public static ComponentInfo GetComponent(string pdNumber) {
      return DataPortal.Fetch<ComponentInfo>(pdNumber);
    }

    public static implicit operator ComponentInfo(ComponentEdit obj) {
      var component = NewComponent();

      component.ComponentId = obj.ComponentId;
      component.PDNumber = obj.PDNumber;
      component.DownloadedTimestamp = obj.DownloadedTimestamp.ToShortDateString();
      component.Description = obj.Description;
      component.PDStatus = obj.PDStatus;
      component.ComponentType = obj.ComponentType;
      component.PDSource = obj.PDSource;
      component.Xml = obj.Xml;
      component.SourceComponentId = obj.SourceComponentId;

      return component;
    }

    #endregion

    #region Data Access

    [Create]
    [RunLocal]
    private void Create() {

    }

    [CreateChild]
    private void Child_Create() { }

    [Fetch]
    private void Fetch(string pdNumber) {
      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
        var dal = dalManager.GetProvider<IComponentDal>();
        var data = dal.Fetch(pdNumber);
        if (data != null) {
          ComponentId = data.ComponentId;
          PDNumber = data.PDNumber;
          DownloadedTimestamp = data.DownloadedTimestamp.ToShortDateString();
          Description = data.Description;
          PDStatus = data.PDStatus;
          ComponentType = data.ComponentType;
          Xml = data.Xml;
          PDSource = data.PDSource;
          SourceComponentId = data.SourceComponentId;
        }
      }
    }

    [FetchChild]
    private void Child_Fetch(ComponentDto item) {
      ComponentId = item.ComponentId;
      PDNumber = item.PDNumber;
      DownloadedTimestamp = item.DownloadedTimestamp.ToShortDateString();
      Description = item.Description;
      PDStatus = item.PDStatus;
      ComponentType = item.ComponentType;
      Xml = item.Xml.UnformatXml();
      PDSource = item.PDSource;
      SourceComponentId = item.SourceComponentId;
    }

    [FetchChild]
    private void Child_Fetch(ComponentInfo item) {
      ComponentId = item.ComponentId;
      PDNumber = item.PDNumber;
      DownloadedTimestamp = item.DownloadedTimestamp;
      Description = item.Description;
      PDStatus = item.PDStatus;
      ComponentType = item.ComponentType;
      Xml = item.Xml.UnformatXml();
      PDSource = item.PDSource;
      SourceComponentId = item.SourceComponentId;
    }

    #endregion
  }
}
