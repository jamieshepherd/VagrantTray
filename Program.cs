using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VagrantTray
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainWindow = new MainWindow();
            Application.Run(mainWindow);
            // Application.Run(new MainWindow());
        }
        public static MainWindow mainWindow
        {
            get;
            set;
        }
        public static void SaveSettings()
        {
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(MainWindow.vagrantBoxList.list);
            Properties.Settings.Default.Boxes = json;
            Properties.Settings.Default.Save();
            Console.WriteLine(json);
        }
        public static void LoadSettings()
        {
            var json = Properties.Settings.Default.Boxes;
            var jsonSerialiser = new JavaScriptSerializer();            
            // Deserialise
            var deserialised = jsonSerialiser.Deserialize<Box[]>(json);
            foreach (Box box in deserialised)
            {
                MainWindow.vagrantBoxList.id++;
                int newID = MainWindow.vagrantBoxList.id;
                Box objBox = new Box(newID, box.boxName, box.boxPath, false);
                MainWindow.vagrantBoxList.list.Add(box);
            }
        }
    }
}
