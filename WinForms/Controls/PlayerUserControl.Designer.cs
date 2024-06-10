using System.ComponentModel;

namespace WinForms.Controls
{
    partial class PlayerUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerUserControl));
            pbPlayer = new PictureBox();
            lblPlayerNumberText = new Label();
            lblPlayerName = new Label();
            lblCaptain = new Label();
            lblFavorite = new Label();
            lblPlayerPosition = new Label();
            lblPlayerNumber = new Label();
            ((System.ComponentModel.ISupportInitialize)pbPlayer).BeginInit();
            SuspendLayout();
            // 
            // pbPlayer
            // 
            pbPlayer.BackgroundImage = Properties.Resource.placeholder_img;
            resources.ApplyResources(pbPlayer, "pbPlayer");
            pbPlayer.BorderStyle = BorderStyle.FixedSingle;
            pbPlayer.Image = Properties.Resource.placeholder_img;
            pbPlayer.Name = "pbPlayer";
            pbPlayer.TabStop = false;
            // 
            // lblPlayerNumberText
            // 
            resources.ApplyResources(lblPlayerNumberText, "lblPlayerNumberText");
            lblPlayerNumberText.Name = "lblPlayerNumberText";
            // 
            // lblPlayerName
            // 
            resources.ApplyResources(lblPlayerName, "lblPlayerName");
            lblPlayerName.Name = "lblPlayerName";
            // 
            // lblCaptain
            // 
            resources.ApplyResources(lblCaptain, "lblCaptain");
            lblCaptain.BackColor = SystemColors.ActiveCaption;
            lblCaptain.ForeColor = Color.Firebrick;
            lblCaptain.Name = "lblCaptain";
            // 
            // lblFavorite
            // 
            resources.ApplyResources(lblFavorite, "lblFavorite");
            lblFavorite.ForeColor = Color.Purple;
            lblFavorite.Name = "lblFavorite";
            // 
            // lblPlayerPosition
            // 
            resources.ApplyResources(lblPlayerPosition, "lblPlayerPosition");
            lblPlayerPosition.Name = "lblPlayerPosition";
            // 
            // lblPlayerNumber
            // 
            resources.ApplyResources(lblPlayerNumber, "lblPlayerNumber");
            lblPlayerNumber.Name = "lblPlayerNumber";
            // 
            // PlayerUserControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pbPlayer);
            Controls.Add(lblPlayerNumber);
            Controls.Add(lblPlayerPosition);
            Controls.Add(lblFavorite);
            Controls.Add(lblCaptain);
            Controls.Add(lblPlayerName);
            Controls.Add(lblPlayerNumberText);
            Name = "PlayerUserControl";
            ((System.ComponentModel.ISupportInitialize)pbPlayer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbPlayer;
        private Label lblPlayerNumberText;
        private Label lblPlayerName;
        private Label lblCaptain;
        private Label lblFavorite;
        private Label lblPlayerPosition;
        private Label lblPlayerNumber;
    }
}
