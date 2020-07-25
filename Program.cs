using System;
using System.IO;
using System.Windows.Forms;

namespace Diamond_Chat_Bot
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string path = Directory.GetCurrentDirectory().ToString();
            Directory.CreateDirectory(path + "/bin/");
            if (!File.Exists(path + "/bin/Diamond Chat Bot.exe"))
            {
                File.Copy(path + "/Diamond Chat Bot.exe", path + "/bin/Diamond Chat Bot.exe");
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new main());
        }
    }
}
