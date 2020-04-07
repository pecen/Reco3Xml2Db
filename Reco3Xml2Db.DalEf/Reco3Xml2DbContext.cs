using Reco3Xml2Db.DalEf.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.DalEf {
  public class Reco3Xml2DbContext : DbContext {
    public Reco3Xml2DbContext() : base("Reco3Xml2DbContext") { }

    public Reco3Xml2DbContext(string connectionString) : base(connectionString) {
      // The below row is an ugly hack to make sure all the dll's for Ef are copied
      var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
    }

    public DbSet<Reco3Component> Components { get; set; }
    public DbSet<Vehicle> Vehicles
      { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      // Regarding the row below: The modelBuilder.Conventions.Remove statement in the OnModelCreating 
      // method prevents table names from being pluralized. If you don't do this, the generated tables 
      // in the database will be named Students, Courses, Enrollments and so on, as opposed to having 
      // the table names as Student, Course, and Enrollment. This is true not only for creating tables,
      // but also when Fetching data, i.e. if you have a table Vehicle and you're fetching a list of 
      // vehicles EF will automatically look for a Vehicles table, although your "Vehicles" table is
      // called Vehicle. Developers disagree about whether table names
      // should be pluralized or not. If one chooses to use Pluralized, then comment the below row out. 
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }
  }
}
