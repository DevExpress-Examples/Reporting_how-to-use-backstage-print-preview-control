Imports DevExpress.Xpf.Core

Namespace DocumentPrintPreview

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Public Partial Class MainWindow
        Inherits ThemedWindow

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub GettingStartedButton_ItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            DocumentPresenter.OpenLink("https://docs.devexpress.com/WPF/120325/controls-and-libraries/printing-exporting/concepts/backstage-print-preview")
        End Sub
    End Class
End Namespace
