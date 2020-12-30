Imports DevExpress.Xpf.Core
Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows

Namespace DocumentPrintPreview
	''' <summary>
	''' Interaction logic for App.xaml
	''' </summary>
	Partial Public Class App
		Inherits Application

		Shared Sub New()
			ApplicationThemeHelper.ApplicationThemeName = Theme.Office2016WhiteSEName
			AppDomain.CurrentDomain.SetData("DataDirectory", "..\..\Data")
		End Sub
	End Class
End Namespace
