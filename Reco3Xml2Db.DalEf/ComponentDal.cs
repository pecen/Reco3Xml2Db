﻿using Csla.Data.EF6;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using Reco3Xml2Db.Dal.Exceptions;
using Reco3Xml2Db.DalEf.Entities;
using Reco3Xml2Db.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Reco3Xml2Db.DalEf {
  public class ComponentDal : IComponentDal {
    private readonly string _dbName = "Server";

    public void Delete(int componentId) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var data = (from r in ctx.DbContext.Components
                    where r.ComponentId == componentId
                    select r).FirstOrDefault();
        if (data != null) {
          ctx.DbContext.Components.Remove(data);
          ctx.DbContext.SaveChanges();
        }
      }
    }

    public bool Exists(string pdNumber) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = (from r in ctx.DbContext.Components
                      where r.PDNumber == pdNumber
                      select r.ComponentId).Count() > 0;
        return result;
      }
    }

    public List<ComponentDto> Fetch() {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = from r in ctx.DbContext.Components
                     select new ComponentDto {
                       ComponentId = r.ComponentId,
                       PDNumber = r.PDNumber,
                       DownloadedTimestamp = r.DownloadedTimestamp,
                       Description = r.Description,
                       PDStatus = r.PD_Status,
                       ComponentType = r.Component_Type,
                       Xml = r.XML,
                       PDSource = r.PD_Source,
                       SourceComponentId = r.SourceComponentId
                     };
        return result.ToList();
      }
    }

    public ComponentDto Fetch(int componentId) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = (from r in ctx.DbContext.Components
                      where r.ComponentId == componentId
                      select new ComponentDto {
                        ComponentId = r.ComponentId,
                        PDNumber = r.PDNumber,
                        DownloadedTimestamp = r.DownloadedTimestamp,
                        Description = r.Description,
                        PDStatus = r.PD_Status,
                        ComponentType = r.Component_Type,
                        Xml = r.XML,
                        PDSource = r.PD_Source,
                        SourceComponentId = r.SourceComponentId
                      }).FirstOrDefault();
        return result;
      }
    }

    public ComponentDto Fetch(string pdNumber) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = (from r in ctx.DbContext.Components
                      where r.PDNumber == pdNumber
                      select new ComponentDto {
                        ComponentId = r.ComponentId,
                        PDNumber = r.PDNumber,
                        DownloadedTimestamp = r.DownloadedTimestamp,
                        Description = r.Description,
                        PDStatus = r.PD_Status,
                        ComponentType = r.Component_Type,
                        Xml = r.XML,
                        PDSource = r.PD_Source,
                        SourceComponentId = r.SourceComponentId
                      }).FirstOrDefault();
        return result;
      }
    }

    public List<ComponentDto> FetchAllWSamePDNumber(string pdNumber) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = from r in ctx.DbContext.Components
                     where r.PDNumber.Contains(pdNumber)
                     select new ComponentDto {
                       ComponentId = r.ComponentId,
                       PDNumber = r.PDNumber,
                       DownloadedTimestamp = r.DownloadedTimestamp,
                       Description = r.Description,
                       PDStatus = r.PD_Status,
                       ComponentType = r.Component_Type,
                       Xml = r.XML,
                       PDSource = r.PD_Source,
                       SourceComponentId = r.SourceComponentId
                     };
        return result.ToList();
      }
    }

    public void Insert(ComponentDto data) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var item = new Reco3Component {
          ComponentId = data.ComponentId,
          PDNumber = data.PDNumber,
          DownloadedTimestamp = data.DownloadedTimestamp,
          Description = data.Description,
          PD_Status = data.PDStatus,
          Component_Type = data.ComponentType,
          XML = data.Xml,
          PD_Source = data.PDSource,
          SourceComponentId = data.SourceComponentId
        };

        ctx.DbContext.Components.Add(item);
        ctx.DbContext.SaveChanges();
        data.ComponentId = item.ComponentId;
      }
    }

    public void Update(ComponentDto data) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var item = (from r in ctx.DbContext.Components
                    where r.ComponentId == data.ComponentId
                    select r).FirstOrDefault();
        if (item == null)
          throw new DataNotFoundException("Component not found exception.");
        if (!item.DownloadedTimestamp.Matches(data.DownloadedTimestamp))
          throw new ConcurrencyException("ConcurrencyException: DownloadedTimeStamp mismatch.");

        item.Description = data.Description;
        item.PD_Status = data.PDStatus;
        item.Component_Type = data.ComponentType;
        item.XML = data.Xml;
        item.PD_Source = data.PDSource;

        var count = ctx.DbContext.SaveChanges();
        if (count == 0)
          throw new UpdateFailureException("Failed to save Component.");
      }
    }
  }
}
