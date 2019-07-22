using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Sql.DataApi;
using DevExpress.Mvvm.POCO;
using System.ComponentModel;

namespace DocumentPrintPreview {
    public class ViewModel {
        public object DataSource { get; }
        public virtual EmployeeReport Report { get; protected set; }
        public virtual IRow SelectedEmployee { get; set; }
        public CustomSettingsViewModel CustomSettings { get; }

        protected ViewModel() {
            CustomSettings = ViewModelSource.Create(() => new CustomSettingsViewModel());
            ((INotifyPropertyChanged)CustomSettings).PropertyChanged += OnCustomSettingsChanged;
            DataSource = CreateDataSource();
        }

        void OnCustomSettingsChanged(object sender, PropertyChangedEventArgs e) {
            if(Report!=null) {
                Report.Parameters["ShowSubordinates"].Value = CustomSettings.ShowSubordinates;
                Report.CreateDocument(true);
            }
        }

        protected void OnSelectedEmployeeChanged() {
            var id = SelectedEmployee.GetValue<long>("EmployeeID");
            var report = new EmployeeReport();
            report.Parameters["EmployeeId"].Value = id;
            report.Parameters["ShowSubordinates"].Value = CustomSettings.ShowSubordinates;
            report.DataSource = DataSource;
            Report = report;
        }

        SqlDataSource CreateDataSource() {
            var sqlDataSource = new SqlDataSource();
            sqlDataSource.ConnectionName = "nwind";
            var query = SelectQueryFluentBuilder
                .AddTable("Employees")
                .SelectAllColumns()
                .Build("Employees");
            var masterDetailInfo = new MasterDetailInfo("Employees", "Employees", "EmployeeID", "ReportsTo");
            sqlDataSource.Queries.Add(query);
            sqlDataSource.Relations.Add(masterDetailInfo);
            sqlDataSource.Fill();
            return sqlDataSource;
        }
    }

    public class CustomSettingsViewModel {
        public virtual bool ShowSubordinates { get; set; }
        public CustomSettingsViewModel() {
            ShowSubordinates = true;
        }
    }
}
