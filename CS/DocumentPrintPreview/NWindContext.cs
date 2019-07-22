using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentPrintPreview {
    public class Employee {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
    public static class NWindDataProvider {
        public static IList<Employee> Employees {
            get { return (IList<Employee>)new NWindDataLoader().Employees; }
        }
    }
    public class NWindDataLoader {
        NWindContext context;
        public NWindDataLoader() {
            if(!ViewModelBase.IsInDesignMode)
                context = NWindContext.Create();
        }
        public object Employees {
            get {
                if(ViewModelBase.IsInDesignMode)
                    return new List<Employee>();
                context.Employees.Load();
                return context.Employees.Local;
            }
        }
    }
    public partial class NWindContext : DbContext {
        public NWindContext() : base(CreateConnection(), true) { }
        public NWindContext(string connectionString) : base(connectionString) { }
        public NWindContext(DbConnection connection) : base(connection, true) { }

        static NWindContext() {
            Database.SetInitializer<NWindContext>(null);
        }

        static DbConnection CreateConnection() {
            var connection = DbProviderFactories.GetFactory("System.Data.SQLite.EF6").CreateConnection();
            connection.ConnectionString = new SQLiteConnectionStringBuilder { DataSource = @"|DataDirectory|\nwind.db" }.ConnectionString;
            return connection;
        }

        public override int SaveChanges() {
            throw new Exception("Readonly context");
        }

        public static Task Load() {
            Action action = () => {
                var context = new NWindContext();
                var prop = typeof(NWindContext).GetProperties()
                    .Where(p => p.PropertyType.IsGenericType &&
                                p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                    .FirstOrDefault();
                if(prop == null)
                    return;
                var query = (IQueryable<object>)prop.GetValue(context, null);
                query.Count();
            };
            return new TaskFactory().StartNew(action);
        }

        public static NWindContext Create() {
            Load().Wait();
            return new NWindContext();
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Configurations.Add(new EmployeeMap());
        }
    }
    public class EmployeeMap : EntityTypeConfiguration<Employee> {
        public EmployeeMap() {
            HasKey(t => t.EmployeeId);
            Property(t => t.EmployeeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(20);
            Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(10);
            Property(t => t.Title)
                .HasMaxLength(30);
            ToTable("Employees");
            Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.Title).HasColumnName("Title");
        }
    }
}
