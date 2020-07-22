using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using Nancy;


namespace Diamond_Chat_Bot
{

    public partial class main : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        string[] commands =
        {
            "!help",
            "!count",
            "!togglebotmode",
            "!botmode"
        };

        string[] mods;

        public static IrcClient client;
        Pinger pinger;
        Exporter exporter;
        Thread t, t1;
        

        public main()
        {
            InitializeComponent();

            
        }

        private void main_Load(object sender, System.EventArgs e)
        {
            client = new IrcClient("irc.twitch.tv", 6667, Twitch.Default.BotName, Twitch.Default.BotToken, Twitch.Default.ChannelName);
            pinger = new Pinger(client);
            exporter = new Exporter();
            
            

            pinger.Start();
            exporter.Start();

            var reg = callOnlyOnce(RegHelper);
            reg();

            t = new Thread(BotLoop);
            t.IsBackground = true;
            t.Start();
            

            LoadSettings();

            if(Twitch.Default.BotName == "" | Twitch.Default.ChannelName == "" | Twitch.Default.BotToken == "")
            {
                var result = MessageBox.Show("Twitch Channel Settings missing.\r\nDo you want to add them now?\r\n\r\nI strongly recommend integrating Twitch for less bugs.", "Attention!", buttons: MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Twitch_Settings frm = new Twitch_Settings();
                    frm.ShowDialog();
                }
            }
        }

        //####################################################################
        //                      Hotkey Specific START
        //####################################################################

        public void RegHelper()
        {
            RegisHK();
            LoadHotkeys();
        }

        //Hotkey Register
        public void RegisHK()
        {
            UnRegisterChange(0);
            UnRegisterChange(1);

            var hk = Hotkeys.Default;
            string hkup = hk.up1;
            string hkdn = hk.dn1;

            //UP1
            int id = 0;
            int keyM = int.Parse(hkup.Substring(0, 1));
            int key = int.Parse(hkup.Substring(2, hkup.Length - 2));
            RegisterChange(id, keyM, key);

            //DOWN1
            id = 1;
            keyM = int.Parse(hkdn.Substring(0, 1));
            key = int.Parse(hkdn.Substring(2, hkdn.Length - 2));
            RegisterChange(id, keyM, key);


            hkup = hk.up2;
            hkdn = hk.dn2;

            //UP2
            id = 2;
            keyM = int.Parse(hkup.Substring(0, 1));
            key = int.Parse(hkup.Substring(2, hkup.Length - 2));
            RegisterChange(id, keyM, key);

            //DOWN2
            id = 3;
            keyM = int.Parse(hkdn.Substring(0, 1));
            key = int.Parse(hkdn.Substring(2, hkdn.Length - 2));
            RegisterChange(id, keyM, key);


            hkup = hk.up3;
            hkdn = hk.dn3;

            //UP3
            id = 4;
            keyM = int.Parse(hkup.Substring(0, 1));
            key = int.Parse(hkup.Substring(2, hkup.Length - 2));
            RegisterChange(id, keyM, key);

            //DOWN3
            id = 5;
            keyM = int.Parse(hkdn.Substring(0, 1));
            key = int.Parse(hkdn.Substring(2, hkdn.Length - 2));
            RegisterChange(id, keyM, key);
        }
        
        public void RegisterChange(int id, int keyM, int key)
        {
            bool ret = RegisterHotKey(Handle, id, keyM, key);
            Console.WriteLine("Hotkey Nr. " + id + " Register: " + ret.ToString());
        }

        public void UnRegisterChange(int id)
        {
            bool ret = UnregisterHotKey(Handle, id);
            Console.WriteLine("Hotkey Nr. " + id + " Unregister: " + ret.ToString());
        }

        public void LoadHotkeys()
        {
            var hk = Hotkeys.Default;
            string temp = "";
            
            //UP1
            c1_u_Alt.Checked = false;
            c1_u_Ctrl.Checked = false;
            c1_u_Shift.Checked = false;

            string hkup = hk.up1;
            int keyM = int.Parse(hkup.Substring(0, 1));
            int key = int.Parse(hkup.Substring(2, hkup.Length - 2));

            hkup = "";
            if (keyM > 3) { c1_u_Shift.Checked = true; keyM -= 4; hkup += "Shift + "; }
            if (keyM > 1) { c1_u_Ctrl.Checked = true; keyM -= 2; hkup += "Ctrl + "; }
            if (keyM ==1) { c1_u_Alt.Checked = true;  hkup += "Alt + "; }

            if (key > 90)
            {
                temp = GetSB(key);
                hkup += temp;
                c1_u_key.SelectedItem = temp;
            }
            else
            {
                hkup += Convert.ToChar(key);
                c1_u_key.SelectedItem = Convert.ToChar(key).ToString();
            }
            c1_u_Hotkey.Text = hkup;

            //DOWN1
            c1_d_Alt.Checked = false;
            c1_d_Ctrl.Checked = false;
            c1_d_Shift.Checked = false;

            string hkdn = hk.dn1;
            keyM = int.Parse(hkdn.Substring(0, 1));
            key = int.Parse(hkdn.Substring(2, hkdn.Length - 2));
            
            hkdn = "";
            if (keyM > 3) { c1_d_Shift.Checked = true; keyM -= 4; hkdn += "Shift + "; }
            if (keyM > 1) { c1_d_Ctrl.Checked = true; keyM -= 2; hkdn += "Ctrl + "; }
            if (keyM == 1) { c1_d_Alt.Checked = true; hkdn += "Alt + "; }

            if (key > 90)
            {
                temp = GetSB(key);
                hkdn += temp;
                c1_d_key.SelectedItem = temp;
            }
            else
            {
                hkdn += Convert.ToChar(key);
                c1_d_key.SelectedItem = Convert.ToChar(key).ToString();
            }
            c1_d_Hotkey.Text = hkdn;


            //UP2
            c2_u_Alt.Checked = false;
            c2_u_Ctrl.Checked = false;
            c2_u_Shift.Checked = false;

            hkup = hk.up2;
            keyM = int.Parse(hkup.Substring(0, 1));
            key = int.Parse(hkup.Substring(2, hkup.Length - 2));

            hkup = "";
            if (keyM > 3) { c2_u_Shift.Checked = true; keyM -= 4; hkup += "Shift + "; }
            if (keyM > 1) { c2_u_Ctrl.Checked = true; keyM -= 2; hkup += "Ctrl + "; }
            if (keyM == 1) { c2_u_Alt.Checked = true; hkup += "Alt + "; }

            if (key > 90)
            {
                temp = GetSB(key);
                hkup += temp;
                c2_u_key.SelectedItem = temp;
            }
            else
            {
                hkup += Convert.ToChar(key);
                c2_u_key.SelectedItem = Convert.ToChar(key);
            }
            c2_u_Hotkey.Text = hkup;

            //DOWN2
            c2_d_Alt.Checked = false;
            c2_d_Ctrl.Checked = false;
            c2_d_Shift.Checked = false;

            hkdn = hk.dn2;
            keyM = int.Parse(hkdn.Substring(0, 1));
            key = int.Parse(hkdn.Substring(2, hkdn.Length - 2));

            hkdn = "";
            if (keyM > 3) { c2_d_Shift.Checked = true; keyM -= 4; hkdn += "Shift + "; }
            if (keyM > 1) { c2_d_Ctrl.Checked = true; keyM -= 2; hkdn += "Ctrl + "; }
            if (keyM == 1) { c2_d_Alt.Checked = true; hkdn += "Alt + "; }

            if (key > 90)
            {
                temp = GetSB(key);
                hkdn += temp;
                c2_d_key.SelectedItem = temp;
            }
            else
            {
                hkdn += Convert.ToChar(key);
                c2_d_key.SelectedItem = Convert.ToChar(key);
            }
            c2_d_Hotkey.Text = hkdn;


            //UP3
            c3_u_Alt.Checked = false;
            c3_u_Ctrl.Checked = false;
            c3_u_Shift.Checked = false;

            hkup = hk.up3;
            keyM = int.Parse(hkup.Substring(0, 1));
            key = int.Parse(hkup.Substring(2, hkup.Length - 2));

            hkup = "";
            if (keyM > 3) { c3_u_Shift.Checked = true; keyM -= 4; hkup += "Shift + "; }
            if (keyM > 1) { c3_u_Ctrl.Checked = true; keyM -= 2; hkup += "Ctrl + "; }
            if (keyM == 1) { c3_u_Alt.Checked = true; hkup += "Alt + "; }

            if (key > 90)
            {
                temp = GetSB(key);
                hkup += temp;
                c3_u_key.SelectedItem = temp;
            }
            else
            {
                hkup += Convert.ToChar(key);
                c3_u_key.SelectedItem = Convert.ToChar(key);
            }
            c3_u_Hotkey.Text = hkup;

            //DOWN3
            c3_d_Alt.Checked = false;
            c3_d_Ctrl.Checked = false;
            c3_d_Shift.Checked = false;

            hkdn = hk.dn3;
            keyM = int.Parse(hkdn.Substring(0, 1));
            key = int.Parse(hkdn.Substring(2, hkdn.Length - 2));

            hkdn = "";
            if (keyM > 3) { c3_d_Shift.Checked = true; keyM -= 4; hkdn += "Shift + "; }
            if (keyM > 1) { c3_d_Ctrl.Checked = true; keyM -= 2; hkdn += "Ctrl + "; }
            if (keyM == 1) { c3_d_Alt.Checked = true; hkdn += "Alt + "; }

            if (key > 90)
            {
                temp = GetSB(key);
                hkdn += temp;
                c3_d_key.SelectedItem = temp;
            }
            else
            {
                hkdn += Convert.ToChar(key);
                c3_d_key.SelectedItem = Convert.ToChar(key);
            }
            c3_d_Hotkey.Text = hkdn;
        }

        //Save Hotkeys
        private void c_hk_save_Click(object sender, EventArgs e)
        {
            //UP1
            int id = 0;
            int keyM = 0;
            string tempKey = c1_u_key.SelectedItem.ToString();
            byte[] key;

            if (c1_u_Shift.Checked) { keyM += 4; }
            if (c1_u_Ctrl.Checked) { keyM += 2; }
            if (c1_u_Alt.Checked) { keyM += 1; }

            key = Encoding.ASCII.GetBytes(s: tempKey);
            if(key.Length >1 | key[0] < 48)
            {
                key = CheckKey(tempKey);
            }
            UnRegisterChange(id);
            RegisterChange(id, keyM, key[0]);
            Hotkeys.Default.up1 = keyM.ToString() + "," + key[0];
            

            //DOWN1
            id = 1;
            keyM = 0;
            tempKey = c1_d_key.SelectedItem.ToString();

            if (c1_d_Shift.Checked) { keyM += 4; }
            if (c1_d_Ctrl.Checked) { keyM += 2; }
            if (c1_d_Alt.Checked) { keyM += 1; }

            key = Encoding.ASCII.GetBytes(s: tempKey);
            if (key.Length > 1 | key[0] < 48)
            {
                key = CheckKey(tempKey);
            }
            UnRegisterChange(id);
            RegisterChange(id, keyM, key[0]);
            Hotkeys.Default.dn1 = keyM.ToString() + "," + key[0];

            //UP2
            id = 2;
            keyM = 0;
            tempKey = c2_u_key.SelectedItem.ToString();

            if (c2_u_Shift.Checked) { keyM += 4; }
            if (c2_u_Ctrl.Checked) { keyM += 2; }
            if (c2_u_Alt.Checked) { keyM += 1; }

            key = Encoding.ASCII.GetBytes(s: tempKey);
            if (key.Length > 1 | key[0] < 48)
            {
                key = CheckKey(tempKey);
            }
            UnRegisterChange(id);
            RegisterChange(id, keyM, key[0]);
            Hotkeys.Default.up2 = keyM.ToString() + "," + key[0];


            //DOWN2
            id = 3;
            keyM = 0;
            tempKey = c2_d_key.SelectedItem.ToString();

            if (c2_d_Shift.Checked) { keyM += 4; }
            if (c2_d_Ctrl.Checked) { keyM += 2; }
            if (c2_d_Alt.Checked) { keyM += 1; }

            key = Encoding.ASCII.GetBytes(s: tempKey);
            if (key.Length > 1 | key[0] < 48)
            {
                key = CheckKey(tempKey);
            }
            UnRegisterChange(id);
            RegisterChange(id, keyM, key[0]);
            Hotkeys.Default.dn2 = keyM.ToString() + "," + key[0];

            //UP3
            id = 4;
            keyM = 0;
            tempKey = c3_u_key.SelectedItem.ToString();

            if (c3_u_Shift.Checked) { keyM += 4; }
            if (c3_u_Ctrl.Checked) { keyM += 2; }
            if (c3_u_Alt.Checked) { keyM += 1; }

            key = Encoding.ASCII.GetBytes(s: tempKey);
            if (key.Length > 1 | key[0] < 48)
            {
                key = CheckKey(tempKey);
            }
            UnRegisterChange(id);
            RegisterChange(id, keyM, key[0]);
            Hotkeys.Default.up3 = keyM.ToString() + "," + key[0];


            //DOWN3
            id = 5;
            keyM = 0;
            tempKey = c1_d_key.SelectedItem.ToString();

            if (c3_d_Shift.Checked) { keyM += 4; }
            if (c3_d_Ctrl.Checked) { keyM += 2; }
            if (c3_d_Alt.Checked) { keyM += 1; }

            key = Encoding.ASCII.GetBytes(s: tempKey);
            if (key.Length > 1 | key[0] < 48)
            {
                key = CheckKey(tempKey);
            }
            UnRegisterChange(id);
            RegisterChange(id, keyM, key[0]);
            Hotkeys.Default.dn3 = keyM.ToString() + "," + key[0];

            Hotkeys.Default.Save();
            LoadHotkeys();
        }

        private byte[] CheckKey(String text)
        {
            byte[] temp = new byte[1];
            if (text.Contains("Num"))
            {
                text = text.Substring(4, 1);
                int asc = Int32.Parse(text);
                asc += 96;
                temp[0] = byte.Parse(asc.ToString());
                return temp;
            }
            else if (text == "+")
            {
                int asc = 107;
                temp[0] = byte.Parse(asc.ToString());
                return temp;
            }
            else if (text == "-")
            {
                int asc = 109;
                temp[0] = byte.Parse(asc.ToString());
                return temp;
            }
            else if (text == "*")
            {
                int asc = 106;
                temp[0] = byte.Parse(asc.ToString());
                return temp;
            }
            else if (text == "/")
            {
                int asc = 111;
                temp[0] = byte.Parse(asc.ToString());
                return temp;
            }
            else
            {
                int asc = Int32.Parse(text);
                temp[0] = byte.Parse(asc.ToString());
                return temp;
            }

        }

        public string GetSB(int asc)
        {
            string temp = "";
            switch (asc)
            {
                case 107:
                    temp = "+";
                    break;
                case 109:
                    temp = "-";
                    break;
                case 106:
                    temp = "*";
                    break;
                case 111:
                    temp = "/";
                    break;
                default:
                    asc -= 96;
                    temp = "Num " + asc.ToString();
                    break;
            }
            return temp;
        }

        //Hotkey Listener
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {

                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);
                int id = m.WParam.ToInt32();

                switch (id)
                {
                    case 0:
                        CountUp(1);
                        break;
                    case 1:
                        CountDown(1);
                        break;
                    case 2:
                        CountUp(2);
                        break;
                    case 3:
                        CountDown(2);
                        break;
                    case 4:
                        CountUp(3);
                        break;
                    case 5:
                        CountDown(3);
                        break;
                    default:

                        break;

                }
            }
        }
        
        //Action for RegHK only Calling once...
        Action callOnlyOnce(Action action)
        {
            var context = new ContextCallOnlyOnce();
            Action ret = () => {
                if (false == context.AlreadyCalled)
                {
                    action();
                    context.AlreadyCalled = true;
                }
            };

            return ret;
        }

        //####################################################################
        //                      Hotkey Specific END
        //####################################################################


        //####################################################################
        //               Twitch Chat specific Methods START
        //####################################################################

        public string[] GetMods()
        {
            
            string[] mods = { };
            if (File.Exists("mods.txt"))
            {
                mods = File.ReadAllLines("mods.txt");
                mods_list.Items.Clear();
                foreach (string line in mods)
                {
                    if (!line.StartsWith("#")) { 
                        mods_list.Items.Add(line + "\r\n");
                    }
                }
            }
            else
            {
                File.WriteAllText("mods.txt", "#dustydiamondbot\r\n#This is a command.\r\n#Start by adding Users by their Username.");
            }
            return mods;
        }

        private void mods_add_Click(object sender, EventArgs e)
        {
            string name = mod_new.Text;
            string mods = File.ReadAllText("mods.txt");

            File.WriteAllText("mods.txt", mods + "\r\n" + name);
            mod_new.Text = "";
            GetMods();
        }

        private void mods_del_Click(object sender, EventArgs e)
        {
            string mods = File.ReadAllText("mods.txt");
            string name = mods_list.SelectedItem.ToString();
            mods = mods.Replace(name, "");
            File.WriteAllText("mods.txt", mods);
            GetMods();
        }

        public void BotLoop()
        {
            while (true)
            {
                //Console.WriteLine("Reading Message...");
                var message = client.ReadMessage();

                if(message != null) { 

                    //For Debugging Purposes
                    //Console.WriteLine($"Message: {message}");
                    message = message.ToLower();
                    foreach (string line in commands)
                    {
                        if(message.Contains(line) == true)
                        {
                            Console.WriteLine($"Message: {message}");
                            MessageHandler(message);
                        }
                        if (message.StartsWith("tmi.twitch.tv PONG"))
                        {
                            MethodInvoker mi;
                            string time = DateTime.Now.ToString();
                            if (Twitch.Default.BotModeInternal > 1)
                            {
                                mi = delegate () { chatBox.AppendText(time.Substring(11, time.Length - 14) + ": Successfully Connected to " + Twitch.Default.ChannelName + "\r\n"); };
                                Invoke(mi);
                            }
                            if (Twitch.Default.BotModeTwitch > 1)
                            {
                                mi = delegate () { client.SendChatMessage(time.Substring(11, time.Length - 14) + ": Successfully Connected to " + Twitch.Default.ChannelName + "\r\n"); };
                                Invoke(mi);
                            }

                        }
                    }
                }
            }
        }

        private void MessageHandler(string message)
        {
            int lineNumber = 0;
            int index = message.IndexOf("privmsg", 0);
            int index2 = message.IndexOf("!", 0);
            MethodInvoker mi;
            string userName = message.Substring(1, index2 - 1);
            message = message.Substring(index, message.Length - index);

            index = message.IndexOf(":", 0) + 1;
            message = message.Substring(index, message.Length - index);
            if (message.StartsWith("!count"))
            {
                lineNumber = 1;
            }
            else
            {
                lineNumber = Array.IndexOf(commands, message);
            }

            foreach (string line in commands)
            {
                if (message.StartsWith(line) == true)
                {
                    if (Array.Exists(mods, element => element == userName))
                    {
                        string time = DateTime.Now.ToString();
                        mi = delegate () { chatBox.AppendText( time.Substring(11,time.Length-14) + ": " + userName + ": " + message + "\r\n"); };
                        if (Twitch.Default.BotModeInternal > 1) {Invoke(mi); }

                        switch (lineNumber)
                        {
                            case 0:
                                //Help Command
                                client.SendChatMessage("I am a Counting Bot :)");
                                break;
                            case 1:
                                //Count Command
                                //Syntax !count <Counter-Nr> <amount>
                                if (message.Length > 7)
                                {
                                    try
                                    {
                                        int amount;
                                        if (message.Substring(6, 1) == " " & message.Substring(8, 1) == " ")
                                        {
                                            int nr = int.Parse(message.Substring(7, 1));
                                            if (message.Length > 8)
                                            {
                                                amount = int.Parse(message.Substring(8, message.Length - 8));
                                                if (amount > 0)
                                                {
                                                    CountUp(nr, amount);
                                                }
                                                else
                                                {
                                                    amount = amount * -1;
                                                    CountDown(nr, amount);
                                                }
                                            }
                                            else
                                            {
                                                amount = 1;
                                                CountUp(nr, amount);
                                            }
                                        }
                                        else
                                        {
                                            
                                        }
                                    }
                                    catch
                                    {
                                        mi = delegate () {
                                            chatBox.AppendText("Hoppala! Da ist was schief gelaufen...");
                                        };
                                        Invoke(mi);
                                    }
                                }
                                else if(message.Length < 7)
                                {
                                    CountUp(1);
                                }
                                else
                                {
                                    if (message.Substring(6, 1) == "+")
                                    {
                                        CountUp(1);
                                    }
                                    else if (message.Substring(6, 1) == "-")
                                    {
                                        CountDown(1);
                                    }
                                    else
                                    {
                                        mi = delegate () {
                                            chatBox.AppendText("Hoppala! Da ist was schief gelaufen...");
                                        };
                                        Invoke(mi);
                                    }
                                }
                                break;
                            case 2:
                                //Bot-Mode Toggled
                                var modeNow = Twitch.Default.BotModeTwitch;
                                if (modeNow == 0)
                                {
                                    modeNow = 1;
                                }
                                else if (modeNow == 1)
                                {
                                    modeNow = 2;
                                }
                                else
                                {
                                    modeNow = 0;
                                }
                                Twitch.Default.BotModeTwitch = modeNow;
                                mi = delegate () { loadBotLevels(); 
                                if (Twitch.Default.BotModeTwitch > 0) { client.SendChatMessage("Bot Silent Mode is now " + modeNow + "\r\n"); }
                                if (Twitch.Default.BotModeInternal > 0) { chatBox.AppendText("Bot Silent Mode is now " + modeNow + "\r\n"); }
                                };
                                Invoke(mi);
                                break;
                            case 3:
                                //Current Bot Mode
                                client.SendChatMessage("Bot Silent Mode is Currently " + Twitch.Default.BotModeTwitch);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        
        //####################################################################
        //               Twitch Chat specific Methods END
        //####################################################################


        //####################################################################
        //                  Counter specific Methods START
        //####################################################################

        public void loadCounters()
        {
            string[] counters = XmlHelper.GetCounters();
            c1_NameC.Items.Clear();
            c1_NameC.Items.AddRange(counters);

            c2_NameC.Items.Clear();
            c2_NameC.Items.AddRange(counters);

            c3_NameC.Items.Clear();
            c3_NameC.Items.AddRange(counters);
        }
        
        //Counter 1
        private void c1_NameC_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var c = counter1.Default;
            if (c1_NameC.SelectedItem != null)
            {
                c.Name = c1_NameC.SelectedItem.ToString();
                c.ID = XmlHelper.GetId(c.Name);
                c.Text = XmlHelper.GetText(c.ID);
                c.Count = XmlHelper.GetCount(c.ID);
                c.Game = XmlHelper.GetGame(c.ID);
                c1_Name.Text = c.Name;
                c1_Game.Text = c.Game;
                c1_ID.Text = c.ID.ToString();
                c1_Text.Text = c.Text;
                c1_Count.Text = c.Count.ToString();
            }
        }

        private void c1_add_Click(object sender, System.EventArgs e)
        {
            CountUp(1);
        }

        private void c1_sub_Click(object sender, System.EventArgs e)
        {
            CountDown(1);
        }

        private void c1_save_Click(object sender, System.EventArgs e)
        {
            if (c1_ID.Text == "")
            {
                XmlHelper.NewCounter(c1_Name.Text, c1_Game.Text, c1_Text.Text);
                c1_Name.Text = "";
                c1_Game.Text = "";
                c1_ID.Text = "";
                c1_Text.Text = "";
                c1_Count.Text = "";
                c1_NameC.Text = "";
                c1_NameC.SelectedIndex = -1;
            }
            else
            {
                var c = counter1.Default;
                c.Name = c1_Name.Text;
                c.Text = c1_Text.Text;
                c.Count = int.Parse(c1_Count.Text);
                c.Game = c1_Game.Text;
                XmlHelper.SaveCounter(c.ID, c.Name, c.Text, c.Count, c.Game);
            }
            loadCounters();
        }

        private void c1_clear_Click(object sender, System.EventArgs e)
        {
            c1_Name.Text = "";
            c1_Game.Text = "";
            c1_ID.Text = "";
            c1_Text.Text = "";
            c1_Count.Text = "";
            c1_NameC.Text = "";
            c1_NameC.SelectedIndex = -1;

        }

        //Counter 2
        private void c2_NameC_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var c = counter2.Default;
            if (c2_NameC.SelectedItem != null)
            {
                c.Name = c2_NameC.SelectedItem.ToString();
                c.ID = XmlHelper.GetId(c.Name);
                c.Text = XmlHelper.GetText(c.ID);
                c.Count = XmlHelper.GetCount(c.ID);
                c.Game = XmlHelper.GetGame(c.ID);
                c2_Name.Text = c.Name;
                c2_Game.Text = c.Game;
                c2_ID.Text = c.ID.ToString();
                c2_Text.Text = c.Text;
                c2_Count.Text = c.Count.ToString();
            }
        }

        private void c2_add_Click(object sender, System.EventArgs e)
        {
            CountUp(2);
        }

        private void c2_sub_Click(object sender, System.EventArgs e)
        {
            CountDown(2);
        }

        private void c2_save_Click(object sender, System.EventArgs e)
        {
            if (c2_ID.Text == "")
            {
                XmlHelper.NewCounter(c2_Name.Text, c2_Game.Text, c2_Text.Text);
                c2_Name.Text = "";
                c2_Game.Text = "";
                c2_ID.Text = "";
                c2_Text.Text = "";
                c2_Count.Text = "";
                c2_NameC.Text = "";
                c2_NameC.SelectedIndex = -1;
            }
            else
            {
                var c = counter2.Default;
                c.Name = c2_Name.Text;
                c.Text = c2_Text.Text;
                c.Count = int.Parse(c2_Count.Text);
                c.Game = c2_Game.Text;
                XmlHelper.SaveCounter(c.ID, c.Name, c.Text, c.Count, c.Game);
            }
            loadCounters();
        }

        private void c2_clear_Click(object sender, System.EventArgs e)
        {
            c2_Name.Text = "";
            c2_Game.Text = "";
            c2_ID.Text = "";
            c2_Text.Text = "";
            c2_Count.Text = "";
            c2_NameC.Text = "";
            c2_NameC.SelectedIndex = -1;

        }
        
        //Counter 3
        private void c3_NameC_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var c = counter3.Default;
            if (c3_NameC.SelectedItem != null)
            {
                c.Name = c3_NameC.SelectedItem.ToString();
                c.ID = XmlHelper.GetId(c.Name);
                c.Text = XmlHelper.GetText(c.ID);
                c.Count = XmlHelper.GetCount(c.ID);
                c.Game = XmlHelper.GetGame(c.ID);
                c3_Name.Text = c.Name;
                c3_Game.Text = c.Game;
                c3_ID.Text = c.ID.ToString();
                c3_Text.Text = c.Text;
                c3_Count.Text = c.Count.ToString();
            }
        }

        private void c3_add_Click(object sender, System.EventArgs e)
        {
            CountUp(3);
        }

        private void c3_sub_Click(object sender, System.EventArgs e)
        {
            CountDown(3);
        }

        private void c3_save_Click(object sender, System.EventArgs e)
        {
            if (c3_ID.Text == "")
            {
                XmlHelper.NewCounter(c3_Name.Text, c3_Game.Text, c3_Text.Text);
                c3_Name.Text = "";
                c3_Game.Text = "";
                c3_ID.Text = "";
                c3_Text.Text = "";
                c3_Count.Text = "";
                c3_NameC.Text = "";
                c3_NameC.SelectedIndex = -1;
            }
            else
            {
                var c = counter3.Default;
                c.Name = c3_Name.Text;
                c.Text = c3_Text.Text;
                c.Count = int.Parse(c3_Count.Text);
                c.Game = c3_Game.Text;
                XmlHelper.SaveCounter(c.ID, c.Name, c.Text, c.Count, c.Game);
            }
            loadCounters();
        }

        private void c3_clear_Click(object sender, System.EventArgs e)
        {
            c3_Name.Text = "";
            c3_Game.Text = "";
            c3_ID.Text = "";
            c3_Text.Text = "";
            c3_Count.Text = "";
            c3_NameC.Text = "";
            c3_NameC.SelectedIndex = -1;

        }


        //Counting Counters
        public void CountUp(int nr,  int amount = 1)
        {
            MethodInvoker mi;
            switch (nr)
            {
                case 1:
                    var c1 = counter1.Default;
                    c1.Count += amount;
                    mi = delegate () { c1_Count.Text = c1.Count.ToString(); };
                    Invoke(mi);
                    XmlHelper.SetCount(c1.ID, c1.Count);
                    break;
                case 2:
                    var c2 = counter2.Default;
                    c2.Count += amount;
                    mi = delegate () { c2_Count.Text = c2.Count.ToString(); };
                    Invoke(mi);
                    XmlHelper.SetCount(c2.ID, c2.Count);
                    break;
                case 3:
                    var c3 = counter3.Default;
                    c3.Count += amount;
                    mi = delegate () { c3_Count.Text = c3.Count.ToString(); };
                    Invoke(mi);
                    XmlHelper.SetCount(c3.ID, c3.Count);
                    break;
            }
            string time = DateTime.Now.ToString();
            if (Twitch.Default.BotModeInternal > 1)
            {
                mi = delegate () { chatBox.AppendText(time.Substring(11, time.Length - 14) + ": Counted Counter #" + nr + " Up.\r\n"); };
                Invoke(mi);
            }
            if (Twitch.Default.BotModeTwitch > 1)
            {
                mi = delegate () { client.SendChatMessage(time.Substring(11, time.Length - 14) + ": Counted Counter #" + nr + " Up.\r\n"); };
                Invoke(mi);
            }
        }

        public void CountDown(int nr, int amount = 1)
        {
            MethodInvoker mi;
            switch (nr)
            {
                case 1:
                    var c1 = counter1.Default;
                    c1.Count -= amount;
                    mi = delegate () { c1_Count.Text = c1.Count.ToString(); };
                    Invoke(mi);
                    XmlHelper.SetCount(c1.ID, c1.Count);
                    break;
                case 2:
                    var c2 = counter2.Default;
                    c2.Count -= amount;
                    mi = delegate () { c2_Count.Text = c2.Count.ToString(); };
                    Invoke(mi);
                    XmlHelper.SetCount(c2.ID, c2.Count);
                    break;
                case 3:
                    var c3 = counter3.Default;
                    c3.Count -= amount;
                    mi = delegate () { c3_Count.Text = c3.Count.ToString(); };
                    Invoke(mi);
                    XmlHelper.SetCount(c3.ID, c3.Count);
                    break;
            }
            string time = DateTime.Now.ToString();
            if (Twitch.Default.BotModeInternal > 1)
            {
                mi = delegate () { chatBox.AppendText(time.Substring(11, time.Length - 14) + ": Counted Counter #" + nr + " Down.\r\n"); };
                Invoke(mi);
            }
            if (Twitch.Default.BotModeTwitch > 1)
            {
                mi = delegate () { client.SendChatMessage(time.Substring(11, time.Length - 14) + ": Counted Counter #" + nr + " Down.\r\n"); };
                Invoke(mi);
            }
        }


        //####################################################################
        //                  Counter specific Methods END
        //####################################################################

        private void LoadSettings()
        {
            loadCounters();
            mods = GetMods();

            loadBotLevels();

        }

        private void loadBotLevels()
        {
            int internal_lv = Twitch.Default.BotModeInternal;
            int twitch_lv = Twitch.Default.BotModeTwitch;

            int_lv0.Checked = false;
            int_lv1.Checked = false;
            int_lv2.Checked = false;
            twi_lv0.Checked = false;
            twi_lv1.Checked = false;
            twi_lv2.Checked = false;
            switch (internal_lv)
            {
                case 0:
                    int_lv0.Checked = true;
                    break;
                case 1:
                    int_lv1.Checked = true;
                    break;
                case 2:
                    int_lv2.Checked = true;
                    break;
            }
            switch (twitch_lv)
            {
                case 0:
                    twi_lv0.Checked = true;
                    break;
                case 1:
                    twi_lv1.Checked = true;
                    break;
                case 2:
                    twi_lv2.Checked = true;
                    break;
            }
        }

        private void int_lv0_CheckedChanged(object sender, EventArgs e)
        {
            internal_CheckChange();
        }

        private void int_lv1_CheckedChanged(object sender, EventArgs e)
        {
            internal_CheckChange();
        }

        private void int_lv2_CheckedChanged(object sender, EventArgs e)
        {
            internal_CheckChange();
        }

        private void twi_lv0_CheckedChanged(object sender, EventArgs e)
        {
            twitch_CheckChange();
        }

        private void twi_lv1_CheckedChanged(object sender, EventArgs e)
        {
            twitch_CheckChange();
        }

        private void twi_lv2_CheckedChanged(object sender, EventArgs e)
        {
            twitch_CheckChange();
        }

        public void internal_CheckChange()
        {
            ushort int_lv;
            if (int_lv0.Checked)
            {
                int_lv = 0;
            }
            else if (int_lv1.Checked)
            {
                int_lv = 1;
            }
            else
            {
                int_lv = 2;
            }
            Twitch.Default.BotModeInternal = int_lv;
            Twitch.Default.Save();
        }

        public void twitch_CheckChange()
        {
            ushort tw_lv;
            if (twi_lv0.Checked)
            {
                tw_lv = 0;
            }
            else if (twi_lv1.Checked)
            {
                tw_lv = 1;
            }
            else
            {
                tw_lv = 2;
            }
            Twitch.Default.BotModeTwitch = tw_lv;
            Twitch.Default.Save();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("© 2020 by DustyDiamond \r\nContact: julian@dustydiamond.de \r\nDiscord: DustyDiamond#5227");
        }

        private void main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void channelSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Twitch_Settings frm = new Twitch_Settings();
            frm.ShowDialog();
        }

        private void startWebServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("Nancy gestartet...");
            
        }
    }
    class ContextCallOnlyOnce
    {
        public bool AlreadyCalled;
    }
}
