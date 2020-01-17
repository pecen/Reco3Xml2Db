using Csla.Data;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Reco3Xml2Db.DalSql {
  public class ComponentDal : IComponentDal {
    private readonly string _dbName = "Server";

    public bool Exists(string pdNumber) {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(_dbName)) {
        var cm = ctx.Connection.CreateCommand();
        cm.CommandType = CommandType.Text;
        cm.CommandText = "SELECT PDNumber FROM Reco3Component WHERE PDNumber=@pdNumber";
        cm.Parameters.AddWithValue("@pdNumber", pdNumber);

        string retval = (string)cm.ExecuteScalar();
        return (!string.IsNullOrEmpty(retval));
      }
    }

    public List<ComponentDto> Fetch() {
      throw new NotImplementedException();
    }

    // Fetch the first record with the given PDNumber
    public ComponentDto Fetch(string pdNumber) {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(_dbName)) {
        using (var cm = ctx.Connection.CreateCommand()) {
          cm.CommandType = CommandType.Text;
          cm.CommandText = "SELECT * FROM Reco3Component WHERE PDNumber = @pdNumber";
          cm.Parameters.AddWithValue("@pdNumber", pdNumber);

          using (var dr = cm.ExecuteReader()) {
            if (dr.HasRows) {
              dr.Read();

              var result = new ComponentDto {
                ComponentId = dr.GetInt32(0),
                PDNumber = dr.GetString(1),
                DownloadedTimestamp = dr.GetDateTime(2),
                Description = dr.GetString(3),
                PDStatus = dr.GetInt32(4),
                ComponentType = dr.GetInt32(5),
                Xml = dr.GetString(6),
                PDSource = dr.GetInt32(7)
              };

              if (!dr.IsDBNull(8)) {
                result.SourceComponentId = dr.GetInt32(8);
              }

              return result;
            }
            else {
              return null;
            }
          }
        }
      }
    }

    public List<ComponentDto> FetchAllWSamePDNumber(string pdNumber) {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(_dbName)) {
        using (var cm = ctx.Connection.CreateCommand()) {
          cm.CommandType = CommandType.Text;
          cm.CommandText = "SELECT * FROM Reco3Component WHERE PDNumber = @pdNumber";
          cm.Parameters.AddWithValue("@pdNumber", pdNumber);

          using (var dr = cm.ExecuteReader()) {
            if (dr.HasRows) {
              var result = new List<ComponentDto>();
              while (dr.Read()) {
                var component = new ComponentDto {
                  ComponentId = dr.GetInt32(0),
                  PDNumber = dr.GetString(1),
                  DownloadedTimestamp = dr.GetDateTime(2),
                  Description = dr.GetString(3),
                  PDStatus = dr.GetInt32(4),
                  ComponentType = dr.GetInt32(5),
                  Xml = dr.GetString(6),
                  PDSource = dr.GetInt32(7)
                };

                if (!dr.IsDBNull(8)) {
                  component.SourceComponentId = dr.GetInt32(8);
                }

                result.Add(component);
              }

              return result;
            }
            else {
              return null;
            }
          }
        }
      }
    }

    public void Insert(ComponentDto data) {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(_dbName)) {
        using (var cm = ctx.Connection.CreateCommand()) {
          cm.CommandType = CommandType.Text;

          if (data.SourceComponentId != null) {
            cm.CommandText = "INSERT INTO Reco3Component (PDNumber, DownloadedTimestamp, Description, PD_Status, Component_Type, XML, PD_Source, SourceComponentId) " +
              "VALUES (@pdNumber,@downloadedTimestamp, @description, @pdStatus, @componentType, @xml, @pdSource, @sourceComponentId)";

            cm.Parameters.AddWithValue("@sourceComponentId", data.SourceComponentId);
          }
          else {
            cm.CommandText = "INSERT INTO Reco3Component (PDNumber, DownloadedTimestamp, Description, PD_Status, Component_Type, XML, PD_Source) " +
              "VALUES (@pdNumber,@downloadedTimestamp, @description, @pdStatus, @componentType, @xml, @pdSource)";
          }

          cm.Parameters.AddWithValue("@pdNumber", data.PDNumber);
          cm.Parameters.AddWithValue("@downloadedTimestamp", data.DownloadedTimestamp);
          cm.Parameters.AddWithValue("@description", data.Description);
          cm.Parameters.AddWithValue("@pdStatus", data.PDStatus);
          cm.Parameters.AddWithValue("@componentType", data.ComponentType);
          cm.Parameters.AddWithValue("@xml", data.Xml);
          cm.Parameters.AddWithValue("@pdSource", data.PDSource);

          cm.ExecuteNonQuery();
          cm.Parameters.Clear();
          cm.CommandText = "SELECT @@identity";
          var r = cm.ExecuteScalar();
          var newId = int.Parse(r.ToString());
          data.ComponentId = newId;
        }
      }
    }
  }
}
