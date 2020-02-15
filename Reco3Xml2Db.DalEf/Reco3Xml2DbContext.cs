using Reco3Xml2Db.DalEf.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.DalEf {
  public class Reco3Xml2DbContext : DbContext {
    public Reco3Xml2DbContext() : base("Reco3Xml2DbContext") { }

    public Reco3Xml2DbContext(string connectionString) : base(connectionString) {
      // The below row is an ugly hack to make sure all the dll's for Ef is copied
      var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
    }

    public DbSet<Reco3Component> Components { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      // Regarding the row below: The modelBuilder.Conventions.Remove statement in the OnModelCreating 
      // method prevents table names from being pluralized. If you don't do this, the generated tables 
      // in the database will be named Students, Courses, Enrollments and so on, as opposed to having 
      // the table names as Student, Course, and Enrollment. Developers disagree about whether table names
      // should be pluralized or not. If one chooses to use Non-Pluralized, then uncomment the below row. 
      //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }
  }
}
