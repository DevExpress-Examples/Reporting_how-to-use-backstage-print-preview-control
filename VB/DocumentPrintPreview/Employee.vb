Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports DevExpress.XtraReports.UI

Namespace DocumentPrintPreview
	Partial Public Class EmployeeReport
		Inherits DevExpress.XtraReports.UI.XtraReport

		Public Sub New()
			InitializeComponent()
		End Sub

		Public Overrides Sub CreateDocument(ByVal buildForInstantPreview As Boolean)
			MyBase.CreateDocument(buildForInstantPreview)
		End Sub

	End Class
End Namespace
