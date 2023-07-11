Imports DevExpress.DataAccess.Sql
Imports DevExpress.DataAccess.Sql.DataApi
Imports DevExpress.Mvvm.POCO
Imports System.ComponentModel

Namespace DocumentPrintPreview

    Public Class ViewModel

        Public ReadOnly Property DataSource As Object

        Public Overridable Property Report As EmployeeReport

        Public Overridable Property SelectedEmployee As IRow

        Public ReadOnly Property CustomSettings As CustomSettingsViewModel

        Protected Sub New()
            CustomSettings = ViewModelSource.Create(Function() New CustomSettingsViewModel())
            AddHandler CType(CustomSettings, INotifyPropertyChanged).PropertyChanged, AddressOf OnCustomSettingsChanged
            DataSource = CreateDataSource()
        End Sub

        Private Sub OnCustomSettingsChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)
            If Report IsNot Nothing Then
                Report.Parameters("ShowSubordinates").Value = CustomSettings.ShowSubordinates
                Report.CreateDocument(True)
            End If
        End Sub

        Protected Sub OnSelectedEmployeeChanged()
            Dim id = SelectedEmployee.GetValue(Of Long)("EmployeeID")
            Dim report = New EmployeeReport()
            report.Parameters("EmployeeId").Value = id
            report.Parameters("ShowSubordinates").Value = CustomSettings.ShowSubordinates
            report.DataSource = DataSource
            Me.Report = report
        End Sub

        Private Function CreateDataSource() As SqlDataSource
            Dim sqlDataSource = New SqlDataSource()
            sqlDataSource.ConnectionName = "nwind"
            Dim query = SelectQueryFluentBuilder.AddTable("Employees").SelectAllColumns().Build("Employees")
            Dim masterDetailInfo = New MasterDetailInfo("Employees", "Employees", "EmployeeID", "ReportsTo")
            sqlDataSource.Queries.Add(query)
            sqlDataSource.Relations.Add(masterDetailInfo)
            sqlDataSource.Fill()
            Return sqlDataSource
        End Function
    End Class

    Public Class CustomSettingsViewModel

        Public Overridable Property ShowSubordinates As Boolean

        Public Sub New()
            ShowSubordinates = True
        End Sub
    End Class
End Namespace
