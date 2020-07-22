using System;
using System.IO;
using System.Threading;

namespace Diamond_Chat_Bot
{
    class Exporter
    {
        private Thread thr;
        private main main;

        public Exporter()
        {
            main = new main();
            thr = new Thread(new ThreadStart(Run));
        }

        public void Start()
        {
            thr.IsBackground = true;
            thr.Start();
        }

        public void Run()
        {
            while (true)
            {
                string c1 = counter1.Default.Text + counter1.Default.Count.ToString();
                File.WriteAllText(path: "counter1.txt", contents: c1);
                string c2 = counter2.Default.Text + counter2.Default.Count.ToString();
                File.WriteAllText(path: "counter2.txt", contents: c2);
                string c3 = counter3.Default.Text + counter3.Default.Count.ToString();
                File.WriteAllText(path: "counter3.txt", contents: c3);

                Thread.Sleep(500);
            }
        }
    }
}
