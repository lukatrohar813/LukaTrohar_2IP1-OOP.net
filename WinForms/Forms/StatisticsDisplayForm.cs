using System.Drawing.Printing;
using WinForms.HelperClasses;

namespace WinForms.Forms
{
    public partial class StatisticsDisplayForm : Form
    {
        readonly PrintPageSetup _printHelper = new PrintPageSetup();
        public StatisticsDisplayForm()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ppdDisplayForm.Document = pdDisplayForm;
            ppdDisplayForm.ShowDialog();
        }

        private void pdDisplayForm_PrintPage(object sender, PrintPageEventArgs e)
        {
            _printHelper.PrintControls(e, flpDisplayForm);
        }
        public void ResizeFormToFitControls()
        {
            int maxWidth = 0;
            int maxHeight = 0;

            foreach (Control control in Controls)
            {
                if (control.Right > maxWidth)
                {
                    maxWidth = control.Right;
                }
                if (control.Bottom > maxHeight)
                {
                    maxHeight = control.Bottom;
                }
            }
            int padding = 20;
            ClientSize = new Size(maxWidth + padding, maxHeight + padding);
        }

    }
}
