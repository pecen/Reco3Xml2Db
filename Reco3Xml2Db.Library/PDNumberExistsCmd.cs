using Csla;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Enums;
using System;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class PDNumberExistsCmd : CommandBase<PDNumberExistsCmd> {
    public static readonly PropertyInfo<string> PDNumberProperty = RegisterProperty<string>(c => c.PDNumber);
    public string PDNumber {
      get { return ReadProperty(PDNumberProperty); }
      set { LoadProperty(PDNumberProperty, value); }
    }

    public static readonly PropertyInfo<bool> PDNumberExistsProperty = RegisterProperty<bool>(c => c.PDNumberExists);
    public bool PDNumberExists {
      get { return ReadProperty(PDNumberExistsProperty); }
      private set { LoadProperty(PDNumberExistsProperty, value); }
    }

    public static readonly PropertyInfo<string> ErrorMessageProperty = RegisterProperty<string>(c => c.ErrorMessage);
    public string ErrorMessage {
      get { return ReadProperty(ErrorMessageProperty); }
      private set { LoadProperty(ErrorMessageProperty, value); }
    }

    public PDNumberExistsCmd() { }

    [RunLocal]
    [Create]
    protected void Create(string id) {
      PDNumber = id;
    }

    [Execute]
    protected override void DataPortal_Execute() {
      try {
        using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
          var dal = ctx.GetProvider<IComponentDal>();
          PDNumberExists = dal.Exists(PDNumber);
        }
      }
      catch (Exception ex) {
        ErrorMessage = ex.Message;
      }
    }
  }
}
