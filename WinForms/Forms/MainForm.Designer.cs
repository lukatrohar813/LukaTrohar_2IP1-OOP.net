using System.ComponentModel;

namespace WinForms.Forms
{
    partial class MainForm
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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            flpFavoritePlayers = new FlowLayoutPanel();
            flpAllPlayers = new FlowLayoutPanel();
            msMenubar = new MenuStrip();
            tsmSettings = new ToolStripMenuItem();
            tsmiLanguage = new ToolStripMenuItem();
            tsmiCroatian = new ToolStripMenuItem();
            tsmiEnglish = new ToolStripMenuItem();
            tsmiLeagueType = new ToolStripMenuItem();
            tsmiFemale = new ToolStripMenuItem();
            tsmiMale = new ToolStripMenuItem();
            tsmRankings = new ToolStripMenuItem();
            tsmRankingsByGoal = new ToolStripMenuItem();
            tsmRankingsByCards = new ToolStripMenuItem();
            tsmAttendance = new ToolStripMenuItem();
            tsmTeamSelection = new ToolStripMenuItem();
            cbTeamSelection = new ToolStripComboBox();
            openWPFProjectToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip = new ContextMenuStrip(components);
            msMenubar.SuspendLayout();
            SuspendLayout();
            // 
            // flpFavoritePlayers
            // 
            flpFavoritePlayers.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(flpFavoritePlayers, "flpFavoritePlayers");
            flpFavoritePlayers.Name = "flpFavoritePlayers";
            flpFavoritePlayers.DragDrop += flpFavoritePlayers_DragDrop;
            flpFavoritePlayers.DragEnter += flpFavoritePlayers_DragEnter;
            // 
            // flpAllPlayers
            // 
            resources.ApplyResources(flpAllPlayers, "flpAllPlayers");
            flpAllPlayers.BorderStyle = BorderStyle.FixedSingle;
            flpAllPlayers.Name = "flpAllPlayers";
            flpAllPlayers.DragDrop += flpAllPlayers_DragDrop;
            flpAllPlayers.DragEnter += flpAllPlayers_DragEnter;
            // 
            // msMenubar
            // 
            msMenubar.Items.AddRange(new ToolStripItem[] { tsmSettings, tsmRankings, tsmAttendance, tsmTeamSelection, openWPFProjectToolStripMenuItem });
            resources.ApplyResources(msMenubar, "msMenubar");
            msMenubar.Name = "msMenubar";
            // 
            // tsmSettings
            // 
            tsmSettings.DropDownItems.AddRange(new ToolStripItem[] { tsmiLanguage, tsmiLeagueType });
            tsmSettings.Name = "tsmSettings";
            resources.ApplyResources(tsmSettings, "tsmSettings");
            // 
            // tsmiLanguage
            // 
            tsmiLanguage.DropDownItems.AddRange(new ToolStripItem[] { tsmiCroatian, tsmiEnglish });
            tsmiLanguage.Name = "tsmiLanguage";
            resources.ApplyResources(tsmiLanguage, "tsmiLanguage");
            // 
            // tsmiCroatian
            // 
            tsmiCroatian.Name = "tsmiCroatian";
            resources.ApplyResources(tsmiCroatian, "tsmiCroatian");
            tsmiCroatian.Tag = "hr";
            tsmiCroatian.Click += tsmiCroatian_Click;
            // 
            // tsmiEnglish
            // 
            tsmiEnglish.Name = "tsmiEnglish";
            resources.ApplyResources(tsmiEnglish, "tsmiEnglish");
            tsmiEnglish.Tag = "en";
            tsmiEnglish.Click += tsmiEnglish_Click;
            // 
            // tsmiLeagueType
            // 
            tsmiLeagueType.DropDownItems.AddRange(new ToolStripItem[] { tsmiFemale, tsmiMale });
            tsmiLeagueType.Name = "tsmiLeagueType";
            resources.ApplyResources(tsmiLeagueType, "tsmiLeagueType");
            // 
            // tsmiFemale
            // 
            tsmiFemale.Name = "tsmiFemale";
            resources.ApplyResources(tsmiFemale, "tsmiFemale");
            tsmiFemale.Tag = "Female";
            tsmiFemale.Click += tsmiFemale_Click;
            // 
            // tsmiMale
            // 
            tsmiMale.Name = "tsmiMale";
            resources.ApplyResources(tsmiMale, "tsmiMale");
            tsmiMale.Tag = "Male";
            tsmiMale.Click += tsmiMale_Click;
            // 
            // tsmRankings
            // 
            tsmRankings.DropDownItems.AddRange(new ToolStripItem[] { tsmRankingsByGoal, tsmRankingsByCards });
            tsmRankings.Name = "tsmRankings";
            resources.ApplyResources(tsmRankings, "tsmRankings");
            // 
            // tsmRankingsByGoal
            // 
            tsmRankingsByGoal.Name = "tsmRankingsByGoal";
            resources.ApplyResources(tsmRankingsByGoal, "tsmRankingsByGoal");
            tsmRankingsByGoal.Click += rankByGoalsToolStripMenuItem_Click;
            // 
            // tsmRankingsByCards
            // 
            tsmRankingsByCards.Name = "tsmRankingsByCards";
            resources.ApplyResources(tsmRankingsByCards, "tsmRankingsByCards");
            tsmRankingsByCards.Click += rankByYellowCardsToolStripMenuItem_Click;
            // 
            // tsmAttendance
            // 
            tsmAttendance.Name = "tsmAttendance";
            resources.ApplyResources(tsmAttendance, "tsmAttendance");
            tsmAttendance.Click += tsmAttendance_Click;
            // 
            // tsmTeamSelection
            // 
            tsmTeamSelection.DropDownItems.AddRange(new ToolStripItem[] { cbTeamSelection });
            tsmTeamSelection.Name = "tsmTeamSelection";
            resources.ApplyResources(tsmTeamSelection, "tsmTeamSelection");
            // 
            // cbTeamSelection
            // 
            cbTeamSelection.Name = "cbTeamSelection";
            resources.ApplyResources(cbTeamSelection, "cbTeamSelection");
            cbTeamSelection.SelectedIndexChanged += cbTeamSelection_SelectedIndexChanged;
            // 
            // openWPFProjectToolStripMenuItem
            // 
            openWPFProjectToolStripMenuItem.Name = "openWPFProjectToolStripMenuItem";
            resources.ApplyResources(openWPFProjectToolStripMenuItem, "openWPFProjectToolStripMenuItem");
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Name = "contextMenuStrip1";
            resources.ApplyResources(contextMenuStrip, "contextMenuStrip");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            Controls.Add(flpAllPlayers);
            Controls.Add(flpFavoritePlayers);
            Controls.Add(msMenubar);
            Name = "MainForm";
            msMenubar.ResumeLayout(false);
            msMenubar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flpFavoritePlayers;
        private FlowLayoutPanel flpAllPlayers;
        private MenuStrip msMenubar;
        private ToolStripMenuItem tsmSettings;
        private ToolStripMenuItem tsmRankings;
        private ToolStripMenuItem tsmAttendance;
        private ToolStripMenuItem tsmRankingsByGoal;
        private ToolStripMenuItem tsmRankingsByCards;
        private ToolStripMenuItem tsmTeamSelection;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem tsmiLanguage;
        private ToolStripMenuItem tsmiCroatian;
        private ToolStripMenuItem tsmiEnglish;
        private ToolStripMenuItem tsmiLeagueType;
        private ToolStripMenuItem tsmiFemale;
        private ToolStripMenuItem tsmiMale;
        private ToolStripComboBox cbTeamSelection;
        private ToolStripMenuItem openWPFProjectToolStripMenuItem;

    }
}