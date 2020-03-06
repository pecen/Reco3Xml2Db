using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Utilities.Extensions {
  public static class TimeStampExtensions {
    public static bool Matches(this DateTime stamp1, DateTime stamp2) {
      if (stamp1 != null && stamp2 != null) {
        if (!stamp1.Equals(stamp2)) {
          return false;
        }
        return true;
      }
      return false;
    }
  }
}
