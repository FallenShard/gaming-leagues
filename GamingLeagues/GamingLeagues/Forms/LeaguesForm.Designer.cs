namespace GamingLeagues.Forms
{
    partial class LeaguesForm
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
            this.addLeague = new System.Windows.Forms.Button();
            this.editLeague = new System.Windows.Forms.Button();
            this.deleteLeague = new System.Windows.Forms.Button();
            this.detailsLeague = new System.Windows.Forms.Button();
            this.closeLeagues = new System.Windows.Forms.Button();
            this.lvLeagues = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // addLeague
            // 
            this.addLeague.Location = new System.Drawing.Point(12, 12);
            this.addLeague.Name = "addLeague";
            this.addLeague.Size = new System.Drawing.Size(110, 23);
            this.addLeague.TabIndex = 0;
            this.addLeague.Text = "Add League";
            this.addLeague.UseVisualStyleBackColor = true;
            this.addLeague.Click += new System.EventHandler(this.addLeague_Click);
            // 
            // editLeague
            // 
            this.editLeague.Location = new System.Drawing.Point(12, 41);
            this.editLeague.Name = "editLeague";
            this.editLeague.Size = new System.Drawing.Size(110, 23);
            this.editLeague.TabIndex = 1;
            this.editLeague.Text = "Edit League";
            this.editLeague.UseVisualStyleBackColor = true;
            this.editLeague.Click += new System.EventHandler(this.editLeague_Click);
            // 
            // deleteLeague
            // 
            this.deleteLeague.Location = new System.Drawing.Point(12, 70);
            this.deleteLeague.Name = "deleteLeague";
            this.deleteLeague.Size = new System.Drawing.Size(110, 23);
            this.deleteLeague.TabIndex = 2;
            this.deleteLeague.Text = "Delete League";
            this.deleteLeague.UseVisualStyleBackColor = true;
            this.deleteLeague.Click += new System.EventHandler(this.deleteLeague_Click);
            // 
            // detailsLeague
            // 
            this.detailsLeague.Location = new System.Drawing.Point(12, 99);
            this.detailsLeague.Name = "detailsLeague";
            this.detailsLeague.Size = new System.Drawing.Size(110, 23);
            this.detailsLeague.TabIndex = 3;
            this.detailsLeague.Text = "Details For League";
            this.detailsLeague.UseVisualStyleBackColor = true;
            this.detailsLeague.Click += new System.EventHandler(this.detailsLeague_Click);
            // 
            // closeLeagues
            // 
            this.closeLeagues.Location = new System.Drawing.Point(12, 327);
            this.closeLeagues.Name = "closeLeagues";
            this.closeLeagues.Size = new System.Drawing.Size(110, 23);
            this.closeLeagues.TabIndex = 4;
            this.closeLeagues.Text = "Close";
            this.closeLeagues.UseVisualStyleBackColor = true;
            this.closeLeagues.Click += new System.EventHandler(this.closeLeagues_Click);
            // 
            // lvLeagues
            // 
            this.lvLeagues.Location = new System.Drawing.Point(128, 12);
            this.lvLeagues.Name = "lvLeagues";
            this.lvLeagues.Size = new System.Drawing.Size(444, 338);
            this.lvLeagues.TabIndex = 5;
            this.lvLeagues.UseCompatibleStateImageBehavior = false;
            // 
            // LeaguesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.lvLeagues);
            this.Controls.Add(this.closeLeagues);
            this.Controls.Add(this.detailsLeague);
            this.Controls.Add(this.deleteLeague);
            this.Controls.Add(this.editLeague);
            this.Controls.Add(this.addLeague);
            this.Name = "LeaguesForm";
            this.Text = "LeaguesForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addLeague;
        private System.Windows.Forms.Button editLeague;
        private System.Windows.Forms.Button deleteLeague;
        private System.Windows.Forms.Button detailsLeague;
        private System.Windows.Forms.Button closeLeagues;
        private System.Windows.Forms.ListView lvLeagues;
    }
}