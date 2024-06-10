using System.ComponentModel;
using System.Drawing.Printing;

namespace WinForms.Forms
{
    partial class StatisticsDisplayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(StatisticsDisplayForm));
            flpDisplayForm = new FlowLayoutPanel();
            btnPrint = new Button();
            ppdDisplayForm = new PrintPreviewDialog();
            pdDisplayForm = new PrintDocument();
            SuspendLayout();
            // 
            // flpDisplayForm
            // 
            resources.ApplyResources(flpDisplayForm, "flpDisplayForm");
            flpDisplayForm.Name = "flpDisplayForm";
            // 
            // btnPrint
            // 
            resources.ApplyResources(btnPrint, "btnPrint");
            btnPrint.Name = "btnPrint";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Click += btnPrint_Click;
            // 
            // ppdDisplayForm
            // 
            resources.ApplyResources(ppdDisplayForm, "ppdDisplayForm");
            ppdDisplayForm.Name = "ppdDisplayForm";
            // 
            // pdDisplayForm
            // 
            pdDisplayForm.PrintPage += pdDisplayForm_PrintPage;
            // 
            // StatisticsDisplayForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            Controls.Add(btnPrint);
            Controls.Add(flpDisplayForm);
            Name = "StatisticsDisplayForm";
            ResumeLayout(false);
        }

        #endregion

        public FlowLayoutPanel flpDisplayForm;
        private Button btnPrint;
        private PrintPreviewDialog ppdDisplayForm;
        private PrintDocument pdDisplayForm;
    }
}