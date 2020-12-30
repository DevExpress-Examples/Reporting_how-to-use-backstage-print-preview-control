Imports DevExpress.DataAccess.Sql
Imports DevExpress.DataAccess.Sql.DataApi
Imports DevExpress.Mvvm.POCO
Imports System.ComponentModel

Namespace DocumentPrintPreview
	Public Class ViewModel
		Public ReadOnly Property DataSource() As Object
		Private privateReport As EmployeeReport
		Public Overridable Property Report() As EmployeeReport
			Get
				Return privateReport
			End Get
			Protected Set(ByVal value As EmployeeReport)
				privateReport = value
			End Set
		End Property
		Public Overridable Property SelectedEmployee() As IRow
		Public ReadOnly Property CustomSettings() As CustomSettingsViewModel

		Protected Sub New()
			CustomSettings = ViewModelSource.Create(Function() New CustomSettingsViewModel())
			AddHandler DirectCast(CustomSettings, INotifyPropertyChanged).PropertyChanged, AddressOf OnCustomSettingsChanged
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
'INSTANT VB NOTE: The variable report was renamed since Visual Basic does not handle local variables named the same as class members well:
			Dim report_Conflict = New EmployeeReport()
			report_Conflict.Parameters("EmployeeId").Value = id
			report_Conflict.Parameters("ShowSubordinates").Value = CustomSettings.ShowSubordinates
			report_Conflict.DataSource = DataSource
			Report = report_Conflict
		End Sub

		Private Function CreateDataSource() As SqlDataSource
			Dim sqlDataSource As New SqlDataSource()
			sqlDataSource.ConnectionName = "nwind"
			Dim query = SelectQueryFluentBuilder.AddTable("Employees").SelectAllColumns().Build("Employees")
			Dim masterDetailInfo As New MasterDetailInfo("Employees", "Employees", "EmployeeID", "ReportsTo")
			sqlDataSource.Queries.Add(query)
			sqlDataSource.Relations.Add(masterDetailInfo)
			sqlDataSource.Fill()
			Return sqlDataSource
		End Function
	End Class

	Public Class CustomSettingsViewModel
		Public Overridable Property ShowSubordinates() As Boolean
		Public Sub New()
			ShowSubordinates = True
		End Sub
	End Class
End Namespace
