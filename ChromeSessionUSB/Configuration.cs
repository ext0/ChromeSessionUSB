using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeSessionUSB
{
    public static class Configuration
    {
        public static Dictionary<String, String> config = new Dictionary<String, String>();
        public static void loadConfiguration(String path = ".config", bool clear = false)
        {
            if (clear)
            {
                config.Clear();
            }
            if (!File.Exists(path))
            {
                writeConfiguration(path);
            }
            foreach (String line in File.ReadAllLines(path))
            {
                String text = line.Trim();
                int seperator = text.IndexOf(":");
                if ((text.Length > 0) && (seperator > 0))
                {
                    addToConfiguration(text.Substring(0, seperator), text.Substring(seperator + 1));
                }
            }
        }
        public static void writeConfiguration(String path = ".config")
        {
            List<String> data = new List<String>();
            foreach (KeyValuePair<String, String> pair in config)
            {
                data.Add(pair.Key + ":" + pair.Value);
            }
            File.WriteAllLines(path, data);
        }
        public static void addToConfiguration(String value, String key)
        {
            modifyConfiguration(value, key);
        }
        public static void modifyConfiguration(String value, String key)
        {
            config[value] = key;
        }
        public static String getValue(String key)
        {
            return (config.ContainsKey(key))?config[key]:null;
        }
    }
}
