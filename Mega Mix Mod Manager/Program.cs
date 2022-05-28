using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mega_Mix_Mod_Manager.Objects;
using YamlDotNet.Serialization;

namespace Mega_Mix_Mod_Manager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        
    }

    internal static class Global
    {
        internal static Settings MainSettings { get; set; } = new Settings();
        
        public static void LoadSettings()
        {
            if (File.Exists($"settings.yaml"))
            {
                string yaml = File.ReadAllText($"settings.yaml");
                var deserializer = new DeserializerBuilder().Build();
                Settings setting = deserializer.Deserialize<Settings>(yaml);
                MainSettings = setting;
            }
        }
    }
}
