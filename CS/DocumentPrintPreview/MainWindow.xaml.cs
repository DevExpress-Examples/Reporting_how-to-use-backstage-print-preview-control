using DevExpress.Xpf.Core;
using System.Windows;

namespace DocumentPrintPreview {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow {
        public MainWindow() {
            InitializeComponent();
        }

        private void GettingStartedButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            DocumentPresenter.OpenLink("https://docs.devexpress.com/WPF/120325/controls-and-libraries/printing-exporting/concepts/backstage-print-preview");
        }
    }
}
