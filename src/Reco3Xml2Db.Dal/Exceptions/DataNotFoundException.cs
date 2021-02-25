using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Reco3Xml2Db.Dal.Exceptions {
  [Serializable]
  public class DataNotFoundException : Exception {
    public DataNotFoundException(string message)
      : base(message) { }

    public DataNotFoundException(string message, Exception innerException)
      : base(message, innerException) { }

    protected DataNotFoundException(
      SerializationInfo info,
      StreamingContext context)
      : base(info, context) { }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(
      SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);
    }
  }
}
