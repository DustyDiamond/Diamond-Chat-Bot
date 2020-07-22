namespace Diamond_Chat_Bot
{
    partial class Twitch_Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Twitch_Settings));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.set_oauth = new System.Windows.Forms.TextBox();
            this.set_botuser = new System.Windows.Forms.TextBox();
            this.set_channel = new System.Windows.Forms.TextBox();
            this.set_token = new System.Windows.Forms.Button();
            this.set_save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Twitch Channel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Bot Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Bot oauth-Token";
            // 
            // set_oauth
            // 
            this.set_oauth.Location = new System.Drawing.Point(106, 62);
            this.set_oauth.Name = "set_oauth";
            this.set_oauth.Size = new System.Drawing.Size(267, 20);
            this.set_oauth.TabIndex = 3;
            this.set_oauth.UseSystemPasswordChar = true;
            // 
            // set_botuser
            // 
            this.set_botuser.Location = new System.Drawing.Point(106, 36);
            this.set_botuser.Name = "set_botuser";
            this.set_botuser.Size = new System.Drawing.Size(267, 20);
            this.set_botuser.TabIndex = 2;
            // 
            // set_channel
            // 
            this.set_channel.Location = new System.Drawing.Point(106, 10);
            this.set_channel.Name = "set_channel";
            this.set_channel.Size = new System.Drawing.Size(267, 20);
            this.set_channel.TabIndex = 1;
            // 
            // set_token
            // 
            this.set_token.Location = new System.Drawing.Point(106, 88);
            this.set_token.Name = "set_token";
            this.set_token.Size = new System.Drawing.Size(110, 23);
            this.set_token.TabIndex = 6;
            this.set_token.Text = "Token-Generator";
            this.set_token.UseVisualStyleBackColor = true;
            this.set_token.Click += new System.EventHandler(this.set_token_Click);
            // 
            // set_save
            // 
            this.set_save.Location = new System.Drawing.Point(298, 88);
            this.set_save.Name = "set_save";
            this.set_save.Size = new System.Drawing.Size(75, 23);
            this.set_save.TabIndex = 7;
            this.set_save.Text = "Save";
            this.set_save.UseVisualStyleBackColor = true;
            this.set_save.Click += new System.EventHandler(this.set_save_Click);
            // 
            // Twitch_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 118);
            this.Controls.Add(this.set_save);
            this.Controls.Add(this.set_token);
            this.Controls.Add(this.set_channel);
            this.Controls.Add(this.set_botuser);
            this.Controls.Add(this.set_oauth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Twitch_Settings";
            this.Text = "Twitch_Settings";
            this.Load += new System.EventHandler(this.Twitch_Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox set_oauth;
        private System.Windows.Forms.TextBox set_botuser;
        private System.Windows.Forms.TextBox set_channel;
        private System.Windows.Forms.Button set_token;
        private System.Windows.Forms.Button set_save;
    }
}