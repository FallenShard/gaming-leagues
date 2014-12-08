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
            this.button1 = new System.Windows.Forms.Button();
            this.bGames = new System.Windows.Forms.Button();
            this.bLeagues = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bGames
            // 
            this.bGames.Location = new System.Drawing.Point(62, 160);
            this.bGames.Name = "bGames";
            this.bGames.Size = new System.Drawing.Size(143, 23);
            this.bGames.TabIndex = 1;
            this.bGames.Text = "Games Overview";
            this.bGames.UseVisualStyleBackColor = true;
            this.bGames.Click += new System.EventHandler(this.bGames_Click);
            // 
            // bLeagues
            // 
            this.bLeagues.Location = new System.Drawing.Point(62, 189);
            this.bLeagues.Name = "bLeagues";
            this.bLeagues.Size = new System.Drawing.Size(75, 23);
            this.bLeagues.TabIndex = 2;
            this.bLeagues.Text = "Leagues Overview";
            this.bLeagues.UseVisualStyleBackColor = true;
            this.bLeagues.Click += new System.EventHandler(this.bLeagues_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.bLeagues);
            this.Controls.Add(this.bGames);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.onLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bGames;
        private System.Windows.Forms.Button bLeagues;
    }
}

