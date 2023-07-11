using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace DocumentPrintPreview
{
	public partial class EmployeeReport : DevExpress.XtraReports.UI.XtraReport
	{	
		public EmployeeReport()
		{
			InitializeComponent();
		}

        public override void CreateDocument(bool buildForInstantPreview) {
            base.CreateDocument(buildForInstantPreview);
        }

    }
}
