namespace GamingLeagues.Forms
{
    partial class GamesForm
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
            this.addGame = new System.Windows.Forms.Button();
            this.lvGames = new System.Windows.Forms.ListView();
            this.editGame = new System.Windows.Forms.Button();
            this.deleteGame = new System.Windows.Forms.Button();
            this.detailsGame = new System.Windows.Forms.Button();
            this.closeGames = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addGame
            // 
            this.addGame.Location = new System.Drawing.Point(12, 12);
            this.addGame.Name = "addGame";
            this.addGame.Size = new System.Drawing.Size(110, 23);
            this.addGame.TabIndex = 0;
            this.addGame.Text = "Add Game";
            this.addGame.UseVisualStyleBackColor = true;
            this.addGame.Click += new System.EventHandler(this.addGame_Click);
            // 
            // lvGames
            // 
            this.lvGames.Location = new System.Drawing.Point(128, 12);
            this.lvGames.Name = "lvGames";
            this.lvGames.Size = new System.Drawing.Size(344, 338);
            this.lvGames.TabIndex = 1;
            this.lvGames.UseCompatibleStateImageBehavior = false;
            // 
            // editGame
            // 
            this.editGame.Location = new System.Drawing.Point(12, 42);
            this.editGame.Name = "editGame";
            this.editGame.Size = new System.Drawing.Size(110, 23);
            this.editGame.TabIndex = 2;
            this.editGame.Text = "Edit Game";
            this.editGame.UseVisualStyleBackColor = true;
            this.editGame.Click += new System.EventHandler(this.editGame_Click);
            // 
            // deleteGame
            // 
            this.deleteGame.Location = new System.Drawing.Point(12, 71);
            this.deleteGame.Name = "deleteGame";
            this.deleteGame.Size = new System.Drawing.Size(110, 23);
            this.deleteGame.TabIndex = 3;
            this.deleteGame.Text = "Delete Game";
            this.deleteGame.UseVisualStyleBackColor = true;
            this.deleteGame.Click += new System.EventHandler(this.deleteGame_Click);
            // 
            // detailsGame
            // 
            this.detailsGame.Location = new System.Drawing.Point(12, 100);
            this.detailsGame.Name = "detailsGame";
            this.detailsGame.Size = new System.Drawing.Size(110, 23);
            this.detailsGame.TabIndex = 4;
            this.detailsGame.Text = "Details For Game";
            this.detailsGame.UseVisualStyleBackColor = true;
            this.detailsGame.Click += new System.EventHandler(this.detailsGame_Click);
            // 
            // closeGames
            // 
            this.closeGames.Location = new System.Drawing.Point(12, 327);
            this.closeGames.Name = "closeGames";
            this.closeGames.Size = new System.Drawing.Size(110, 23);
            this.closeGames.TabIndex = 5;
            this.closeGames.Text = "Close";
            this.closeGames.UseVisualStyleBackColor = true;
            this.closeGames.Click += new System.EventHandler(this.closeGames_Click);
            // 
            // GamesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.closeGames);
            this.Controls.Add(this.detailsGame);
            this.Controls.Add(this.deleteGame);
            this.Controls.Add(this.editGame);
            this.Controls.Add(this.lvGames);
            this.Controls.Add(this.addGame);
            this.Name = "GamesForm";
            this.Text = "GamesForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addGame;
        private System.Windows.Forms.ListView lvGames;
        private System.Windows.Forms.Button editGame;
        private System.Windows.Forms.Button deleteGame;
        private System.Windows.Forms.Button detailsGame;
        private System.Windows.Forms.Button closeGames;
    }
}