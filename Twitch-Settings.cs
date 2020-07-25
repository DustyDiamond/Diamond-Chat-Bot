using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diamond_Chat_Bot
{
    public partial class Twitch_Settings : Form
    {
        public Twitch_Settings()
        {
            InitializeComponent();
        }

        private void set_token_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitchtokengenerator.com/quick/k2AYDAMxBl");
        }

        private void set_save_Click(object sender, EventArgs e)
        {
            Twitch.Default.BotName = set_botuser.Text;
            Twitch.Default.ChannelName = set_channel.Text;
            Twitch.Default.BotToken = "oauth:" + set_oauth.Text;
            Twitch.Default.Save();
            MessageBox.Show("Diamond Chat Bot has to be restarted manually. \r\nPlease Wait.","Attention!", MessageBoxButtons.OK);
            Close();
            Application.Restart();
            Environment.Exit(0);
        }

        private void Twitch_Settings_Load(object sender, EventArgs e)
        {
            set_botuser.Text = Twitch.Default.BotName;
            set_channel.Text = Twitch.Default.ChannelName;
            if (Twitch.Default.BotToken != "") { 
            set_oauth.Text = Twitch.Default.BotToken.Substring(6, Twitch.Default.BotToken.Length -6);}
        }
    }
}
