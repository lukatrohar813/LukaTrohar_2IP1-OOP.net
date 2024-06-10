using System.ComponentModel;

namespace WinForms.Controls
{
    partial class MatchUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatchUserControl));
            lblLocation = new Label();
            lblAttendance = new Label();
            lblHomeTeam = new Label();
            lblAwayTeam = new Label();
            lblLocationText = new Label();
            lblAttendanceText = new Label();
            lblHomeTeamText = new Label();
            lblAwayTeamText = new Label();
            SuspendLayout();
            // 
            // lblLocation
            // 
            resources.ApplyResources(lblLocation, "lblLocation");
            lblLocation.AutoSize = true;
            lblLocation.Location = new Point(13, 24);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(54, 21);
            lblLocation.TabIndex = 0;
           
            // 
            // lblAttendance
            // 
            resources.ApplyResources(lblAttendance, "lblAttendance");
            lblAttendance.AutoSize = true;
            lblAttendance.Location = new Point(13, 58);
            lblAttendance.Name = "lblAttendance";
            lblAttendance.Size = new Size(69, 21);
            lblAttendance.TabIndex = 1;
          
            // 
            // lblHomeTeam
            // 
            resources.ApplyResources(lblHomeTeam, "lblHomeTeam");
            lblHomeTeam.AutoSize = true;
            lblHomeTeam.Location = new Point(13, 88);
            lblHomeTeam.Name = "lblHomeTeam";
            lblHomeTeam.Size = new Size(72, 21);
            lblHomeTeam.TabIndex = 2;
           
            // 
            // lblAwayTeam
            // 
            resources.ApplyResources(lblAwayTeam, "lblAwayTeam");
            lblAwayTeam.AutoSize = true;
            lblAwayTeam.Location = new Point(13, 118);
            lblAwayTeam.Name = "lblAwayTeam";
            lblAwayTeam.Size = new Size(70, 21);
            lblAwayTeam.TabIndex = 3;
            // 
            // lblLocationText
            // 
            resources.ApplyResources(lblLocationText, "lblLocationText");
            lblLocationText.AutoSize = true;
            lblLocationText.Location = new Point(136, 24);
            lblLocationText.Name = "lblLocationText";
            lblLocationText.Size = new Size(0, 21);
            lblLocationText.TabIndex = 4;
            // 
            // lblAttendanceText
            // 
            resources.ApplyResources(lblAttendanceText, "lblAttendanceText");   
            lblAttendanceText.AutoSize = true;
            lblAttendanceText.Location = new Point(136, 58);
            lblAttendanceText.Name = "lblAttendanceText";
            lblAttendanceText.Size = new Size(0, 21);
            lblAttendanceText.TabIndex = 5;
            // 
            // lblHomeTeamText
            // 
            resources.ApplyResources(lblHomeTeamText, "lblHomeTeamText");
            lblHomeTeamText.AutoSize = true;
            lblHomeTeamText.Location = new Point(136, 88);
            lblHomeTeamText.Name = "lblHomeTeamText";
            lblHomeTeamText.Size = new Size(0, 21);
            lblHomeTeamText.TabIndex = 6;
            // 
            // lblAwayTeamText
            // 
            resources.ApplyResources(lblAwayTeamText, "lblAwayTeamText");
            lblAwayTeamText.AutoSize = true;
            lblAwayTeamText.Location = new Point(136, 118);
            lblAwayTeamText.Name = "lblAwayTeamText";
            lblAwayTeamText.Size = new Size(0, 21);
            lblAwayTeamText.TabIndex = 7;
            // 
            // MatchUserControl
            // 
            AutoScaleDimensions = new SizeF(6F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(lblAwayTeamText);
            Controls.Add(lblHomeTeamText);
            Controls.Add(lblAttendanceText);
            Controls.Add(lblLocationText);
            Controls.Add(lblAwayTeam);
            Controls.Add(lblHomeTeam);
            Controls.Add(lblAttendance);
            Controls.Add(lblLocation);
            Font = new Font("Dubai", 8.999999F, FontStyle.Regular, GraphicsUnit.Point);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MatchUserControl";
            Size = new Size(270, 164);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblLocation;
        private Label lblAttendance;
        private Label lblHomeTeam;
        private Label lblAwayTeam;
        private Label lblLocationText;
        private Label lblAttendanceText;
        private Label lblHomeTeamText;
        private Label lblAwayTeamText;
    }
}
