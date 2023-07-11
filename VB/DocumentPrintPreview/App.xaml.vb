Imports DevExpress.Xpf.Core
Imports System
Imports System.Windows

Namespace DocumentPrintPreview

    ''' <summary>
    ''' Interaction logic for App.xaml
    ''' </summary>
    Public Partial Class App
        Inherits Application

        Shared Sub New()
            AppDomain.CurrentDomain.SetData("DataDirectory", "..\..\Data")
        End Sub
    End Class
End Namespace
