namespace GamingLeagues
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.btnPlayers = new System.Windows.Forms.Button();
            this.btnTeams = new System.Windows.Forms.Button();
            this.btnLeagues = new System.Windows.Forms.Button();
            this.btnSponsors = new System.Windows.Forms.Button();
            this.btnGames = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPlayers
            // 
            this.btnPlayers.Location = new System.Drawing.Point(13, 13);
            this.btnPlayers.Name = "btnPlayers";
            this.btnPlayers.Size = new System.Drawing.Size(75, 23);
            this.btnPlayers.TabIndex = 0;
            this.btnPlayers.Text = "Players";
            this.btnPlayers.UseVisualStyleBackColor = true;
            this.btnPlayers.Click += new System.EventHandler(this.btnPlayers_Click);
            // 
            // btnTeams
            // 
            this.btnTeams.Location = new System.Drawing.Point(13, 43);
            this.btnTeams.Name = "btnTeams";
            this.btnTeams.Size = new System.Drawing.Size(75, 23);
            this.btnTeams.TabIndex = 1;
            this.btnTeams.Text = "Teams";
            this.btnTeams.UseVisualStyleBackColor = true;
            this.btnTeams.Click += new System.EventHandler(this.btnTeams_Click);
            // 
            // btnLeagues
            // 
            this.btnLeagues.Location = new System.Drawing.Point(13, 73);
            this.btnLeagues.Name = "btnLeagues";
            this.btnLeagues.Size = new System.Drawing.Size(75, 23);
            this.btnLeagues.TabIndex = 2;
            this.btnLeagues.Text = "Leagues";
            this.btnLeagues.UseVisualStyleBackColor = true;
            this.btnLeagues.Click += new System.EventHandler(this.btnLeagues_Click);
            // 
            // btnSponsors
            // 
            this.btnSponsors.Location = new System.Drawing.Point(13, 103);
            this.btnSponsors.Name = "btnSponsors";
            this.btnSponsors.Size = new System.Drawing.Size(75, 23);
            this.btnSponsors.TabIndex = 3;
            this.btnSponsors.Text = "Sponsors";
            this.btnSponsors.UseVisualStyleBackColor = true;
            this.btnSponsors.Click += new System.EventHandler(this.btnSponsors_Click);
            // 
            // btnGames
            // 
            this.btnGames.Location = new System.Drawing.Point(13, 133);
            this.btnGames.Name = "btnGames";
            this.btnGames.Size = new System.Drawing.Size(75, 23);
            this.btnGames.TabIndex = 4;
            this.btnGames.Text = "Games";
            this.btnGames.UseVisualStyleBackColor = true;
            this.btnGames.Click += new System.EventHandler(this.btnGames_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnGames);
            this.Controls.Add(this.btnSponsors);
            this.Controls.Add(this.btnLeagues);
            this.Controls.Add(this.btnTeams);
            this.Controls.Add(this.btnPlayers);
            this.Name = "MainForm";
            this.Text = "Gaming Leagues";
            this.Load += new System.EventHandler(this.onLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPlayers;
        private System.Windows.Forms.Button btnTeams;
        private System.Windows.Forms.Button btnLeagues;
        private System.Windows.Forms.Button btnSponsors;
        private System.Windows.Forms.Button btnGames;
    }
}

