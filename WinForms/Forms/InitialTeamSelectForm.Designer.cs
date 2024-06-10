using System.ComponentModel;

namespace WinForms.Forms
{
    partial class InitialTeamSelectForm
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(InitialTeamSelectForm));
            cbFavoriteTeam = new ComboBox();
            btnContinue = new Button();
            lblStatus = new Label();
            lblFavoriteTeam = new Label();
            SuspendLayout();
            // 
            // cbFavoriteTeam
            // 
            cbFavoriteTeam.FormattingEnabled = true;
            resources.ApplyResources(cbFavoriteTeam, "cbFavoriteTeam");
            cbFavoriteTeam.Name = "cbFavoriteTeam";
            // 
            // btnContinue
            // 
            resources.ApplyResources(btnContinue, "btnContinue");
            btnContinue.Name = "btnContinue";
            btnContinue.UseVisualStyleBackColor = true;
            btnContinue.Click += btnContinue_Click;
            // 
            // lblStatus
            // 
            resources.ApplyResources(lblStatus, "lblStatus");
            lblStatus.Name = "lblStatus";
            // 
            // lblFavoriteTeam
            // 
            resources.ApplyResources(lblFavoriteTeam, "lblFavoriteTeam");
            lblFavoriteTeam.Name = "lblFavoriteTeam";
            // 
            // InitialTeamSelectForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(lblFavoriteTeam);
            Controls.Add(lblStatus);
            Controls.Add(btnContinue);
            Controls.Add(cbFavoriteTeam);
            Name = "InitialTeamSelectForm";
            FormClosing += InitialTeamSelectForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbFavoriteTeam;
        private Button btnContinue;
        private Label lblStatus;
        private Label lblFavoriteTeam;
    }
}