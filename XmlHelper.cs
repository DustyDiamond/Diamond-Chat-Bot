using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Diamond_Chat_Bot
{
    class XmlHelper
    {
        const int OutTime = 50;

        public static void NewCounter(string name, string game="", string text = "")
        {
            int id = 0;
            if (File.Exists("counters.xml") == false)
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineOnAttributes = true
                };
                using (XmlWriter xmlWriter = XmlWriter.Create("counters.xml", xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("counters");

                    xmlWriter.WriteStartElement("counter");
                    xmlWriter.WriteElementString("id", id.ToString());
                    xmlWriter.WriteElementString("name", name.ToString());
                    xmlWriter.WriteElementString("text", text);
                    xmlWriter.WriteElementString("count", "0");
                    xmlWriter.WriteElementString("game", game);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    xmlWriter.Close();

                    if (Twitch.Default.BotModeTwitch > 1) { main.client.SendChatMessage("Counter: " + name + " created!"); }
                    if (Twitch.Default.BotModeInternal > 1) { Console.WriteLine("Counter: " + name + " created!"); }
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load("counters.xml");
                XElement root = xDocument.Element("counters");
                IEnumerable<XElement> rows = root.Descendants("counter");
                XElement lastRow = rows.Last();
                int lastId = Int32.Parse(lastRow.Element("id").Value);
                id = lastId + 1;

                //Check if Counter name already exists

                if (xDocument.Elements("counters").Elements("counter").Elements("name").Any(x => x.Value == name))
                {
                    if (Twitch.Default.BotModeTwitch > 1) { main.client.SendChatMessage("Counter: " + name + " already exists!"); }
                    if (Twitch.Default.BotModeInternal > 1) { Console.WriteLine("Counter: " + name + " already exists!"); }
                }
                else
                {
                    lastRow.AddAfterSelf(
                        new XElement("counter",
                        new XElement("id", id.ToString()),
                        new XElement("name", name),
                        new XElement("text", text),
                        new XElement("count", "0"),
                        new XElement("game", game)
                        ));

                    if (Twitch.Default.BotModeTwitch > 1) { main.client.SendChatMessage("Counter: " + name + " created!"); }
                    if (Twitch.Default.BotModeInternal > 1) { Console.WriteLine("Counter: " + name + " created!"); }
                    xDocument.Save("counters.xml");
                }
            }
            Thread.Sleep(OutTime);
        }

        public static void SaveCounter(int id, string name, string text, int count, string game = "")
        {
            XDocument xDocument = XDocument.Load("counters.xml");
            if (xDocument.Elements("counters").Elements("counter").Elements("id").Any(x => x.Value == id.ToString()))
            {
                foreach (XElement element in xDocument.Elements("counters").Elements("counter"))
                {
                    if (Int32.Parse(element.Element("id").Value) == id)
                    {
                        element.Element("name").Value = name;
                        element.Element("text").Value = text;
                        element.Element("count").Value = count.ToString();
                        element.Element("game").Value = game;
                    }
                }
                xDocument.Save("counters.xml");
            }
            Thread.Sleep(OutTime);
        }
        
        //Get Counter ID
        public static int GetId(string name)
        {
            int id = 0;
            XDocument xDocument = XDocument.Load("counters.xml");
            if (xDocument.Elements("counters").Elements("counter").Elements("name").Any(x => x.Value == name))
            {
                foreach (XElement element in xDocument.Elements("counters").Elements("counter"))
                {
                    if (element.Element("name").Value == name)
                    {
                        id = Int32.Parse(element.Element("id").Value);
                    }
                }
            }
            System.Threading.Thread.Sleep(OutTime);
            return id;
        }

        //Get Counter Count
        public static int GetCount(int id)
        {
            int count = 0;
            XDocument xDocument = XDocument.Load("counters.xml");
            if (xDocument.Elements("counters").Elements("counter").Elements("id").Any(x => x.Value == id.ToString()))
            {
                foreach (XElement element in xDocument.Elements("counters").Elements("counter"))
                {
                    if (Int32.Parse(element.Element("id").Value) == id)
                    {
                        count = Int32.Parse(element.Element("count").Value);
                    }
                }
            }
            System.Threading.Thread.Sleep(OutTime);
            return count;
        }

        //Get Counter game
        public static string GetGame(int id)
        {
            string game = "";
            XDocument xDocument = XDocument.Load("counters.xml");
            if (xDocument.Elements("counters").Elements("counter").Elements("id").Any(x => x.Value == id.ToString()))
            {
                foreach (XElement element in xDocument.Elements("counters").Elements("counter"))
                {
                    if (Int32.Parse(element.Element("id").Value) == id)
                    {
                        game = element.Element("game").Value;
                    }
                }
            }
            System.Threading.Thread.Sleep(OutTime);
            return game;
        }

        //Get Counter name
        public static string GetName(int id)
        {
            string name = "";
            XDocument xDocument = XDocument.Load("counters.xml");
            if (xDocument.Elements("counters").Elements("counter").Elements("id").Any(x => x.Value == id.ToString()))
            {
                foreach (XElement element in xDocument.Elements("counters").Elements("counter"))
                {
                    if (Int32.Parse(element.Element("id").Value) == id)
                    {
                        name = element.Element("name").Value;
                    }
                }
            }
            System.Threading.Thread.Sleep(OutTime);
            return name;
        }

        //Get Counter text
        public static string GetText(int id)
        {
            string text = "";
            XDocument xDocument = XDocument.Load("counters.xml");
            if (xDocument.Elements("counters").Elements("counter").Elements("id").Any(x => x.Value == id.ToString()))
            {
                foreach (XElement element in xDocument.Elements("counters").Elements("counter"))
                {
                    if (Int32.Parse(element.Element("id").Value) == id)
                    {
                        text = element.Element("text").Value;
                    }
                }
            }
            System.Threading.Thread.Sleep(OutTime);
            return text;
        }

        public static string[] GetCounters()
        {
            string[] result;

            if (!File.Exists("counters.xml"))
            {
                int id = 0;
                string name = "Dummy";
                string text = "";
                string game = "";
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineOnAttributes = true
                };
                using (XmlWriter xmlWriter = XmlWriter.Create("counters.xml", xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("counters");

                    xmlWriter.WriteStartElement("counter");
                    xmlWriter.WriteElementString("id", id.ToString());
                    xmlWriter.WriteElementString("name", name.ToString());
                    xmlWriter.WriteElementString("text", text);
                    xmlWriter.WriteElementString("count", "0");
                    xmlWriter.WriteElementString("game", game);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    xmlWriter.Close();

                    if (Twitch.Default.BotModeTwitch > 1) { main.client.SendChatMessage("Counter: " + name + " created!"); }
                    if (Twitch.Default.BotModeInternal > 1) { Console.WriteLine("Counter: " + name + " created!"); }
                }

            }
            XDocument xDocument = XDocument.Load("counters.xml");
            int elemList = xDocument.Elements("counters").Elements("counter").Count();

            result = new string[elemList];

            int i = 0;

            foreach (XElement element in xDocument.Elements("counters").Elements("counter"))
            {
                result[i] = element.Element("name").Value;
                i++;
            }
            System.Threading.Thread.Sleep(OutTime);
            return result;

        }

        public static void SetCount(int id, int count)
        {
            XDocument xDocument = XDocument.Load("counters.xml");
            if (xDocument.Elements("counters").Elements("counter").Elements("id").Any(x => x.Value == id.ToString()))
            {
                foreach (XElement element in xDocument.Elements("counters").Elements("counter"))
                {
                    if (Int32.Parse(element.Element("id").Value) == id)
                    {
                        element.Element("count").Value = count.ToString();
                    }
                }
                xDocument.Save("counters.xml");
            }
            Thread.Sleep(OutTime);
        }

    }
}
