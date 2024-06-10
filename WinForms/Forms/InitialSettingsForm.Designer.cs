using System.ComponentModel;

namespace WinForms.Forms
{
    partial class InitialSettingsForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(InitialSettingsForm));
            gbType = new GroupBox();
            cbFemale = new CheckBox();
            cbMale = new CheckBox();
            gbLanguage = new GroupBox();
            cbCroatian = new CheckBox();
            cbEnglish = new CheckBox();
            btnSubmit = new Button();
            gbType.SuspendLayout();
            gbLanguage.SuspendLayout();
            SuspendLayout();
            // 
            // gbType
            // 
            gbType.Controls.Add(cbFemale);
            gbType.Controls.Add(cbMale);
            resources.ApplyResources(gbType, "gbType");
            gbType.Name = "gbType";
            gbType.TabStop = false;
            // 
            // cbFemale
            // 
            resources.ApplyResources(cbFemale, "cbFemale");
            cbFemale.Name = "cbFemale";
            cbFemale.Tag = "Female";
            cbFemale.UseVisualStyleBackColor = true;
            cbFemale.CheckedChanged += cbFemale_CheckedChanged;
            // 
            // cbMale
            // 
            resources.ApplyResources(cbMale, "cbMale");
            cbMale.Name = "cbMale";
            cbMale.Tag = "Male";
            cbMale.UseVisualStyleBackColor = true;
            cbMale.CheckedChanged += cbMale_CheckedChanged;
            // 
            // gbLanguage
            // 
            gbLanguage.Controls.Add(cbCroatian);
            gbLanguage.Controls.Add(cbEnglish);
            resources.ApplyResources(gbLanguage, "gbLanguage");
            gbLanguage.Name = "gbLanguage";
            gbLanguage.TabStop = false;
            // 
            // cbCroatian
            // 
            resources.ApplyResources(cbCroatian, "cbCroatian");
            cbCroatian.Name = "cbCroatian";
            cbCroatian.Tag = "hr";
            cbCroatian.UseVisualStyleBackColor = true;
            cbCroatian.CheckedChanged += cbCroatian_CheckedChanged;
            // 
            // cbEnglish
            // 
            resources.ApplyResources(cbEnglish, "cbEnglish");
            cbEnglish.Name = "cbEnglish";
            cbEnglish.Tag = "En";
            cbEnglish.UseVisualStyleBackColor = true;
            cbEnglish.CheckedChanged += cbEnglish_CheckedChanged;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = SystemColors.ControlLightLight;
            resources.ApplyResources(btnSubmit, "btnSubmit");
            btnSubmit.Name = "btnSubmit";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // InitialSettingsForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(btnSubmit);
            Controls.Add(gbLanguage);
            Controls.Add(gbType);
            Name = "InitialSettingsForm";
            FormClosing += Settings_FormClosing;
            gbType.ResumeLayout(false);
            gbType.PerformLayout();
            gbLanguage.ResumeLayout(false);
            gbLanguage.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbType;
        private CheckBox cbFemale;
        private CheckBox cbMale;
        private GroupBox gbLanguage;
        private CheckBox cbCroatian;
        private CheckBox cbEnglish;
        private Button btnSubmit;
    }
}