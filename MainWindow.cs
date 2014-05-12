using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VagrantTray
{
    public partial class MainWindow : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        public static BoxList vagrantBoxList = new BoxList();

        public MainWindow()
        {
            InitializeComponent();

            if (String.IsNullOrEmpty(Properties.Settings.Default.Boxes))
            {
                Console.WriteLine("We don't have values");
            }
            else
            {
                Console.WriteLine("We have values:");
                Console.WriteLine(Properties.Settings.Default.Boxes);
                Console.WriteLine("Trying to load them now...");
                Program.LoadSettings();
            }

            // Create tray menu
            trayMenu = new ContextMenu();
            trayIcon = new NotifyIcon();
            trayIcon.Text = "VagrantTray";
            trayIcon.Icon = new Icon(VagrantTray.Properties.Resources.Icon, 40, 40);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            RefreshMenu();

        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
        }
        public void RefreshMenuClick(object sender, EventArgs e)
        {
            // See if there have been any changes to vagrant status
            RefreshMenu();
        }
        public void RefreshMenu()
        {
            // Clear for a new menu
            trayMenu.MenuItems.Clear();

            // Begin standard menu items
            trayMenu.MenuItems.Add("Add New", AddBox);
            trayMenu.MenuItems.Add("Refresh", RefreshMenuClick);
            trayMenu.MenuItems.Add("-");

            Console.WriteLine("## Building menu");
            // List the boxes
            foreach(Box box in vagrantBoxList)
            {
                Console.WriteLine("Current index: "+box.boxID);
                String boxName = (box.boxStatus == true) ? "✔ " + box.boxName : box.boxName;
                MenuItem DropDown = new MenuItem(boxName);

                MenuItem cmdVagrantSsh = new MenuItem();
                cmdVagrantSsh.Text = "vagrant ssh";
                DropDown.MenuItems.Add(cmdVagrantSsh);
                cmdVagrantSsh.Click += (sender, e) => RunCommand("vagrant ssh", box.boxPath, box.boxID);

                DropDown.MenuItems.Add("-", OnExit);

                MenuItem cmdVagrantUp = new MenuItem();
                cmdVagrantUp.Text = "vagrant up";
                DropDown.MenuItems.Add(cmdVagrantUp);
                cmdVagrantUp.Click += (sender, e) => RunCommand("vagrant up", box.boxPath, box.boxID);

                MenuItem cmdVagrantReload = new MenuItem();
                cmdVagrantReload.Text = "vagrant reload";
                DropDown.MenuItems.Add(cmdVagrantReload);
                cmdVagrantReload.Click += (sender, e) => RunCommand("vagrant reload", box.boxPath, box.boxID);

                MenuItem cmdVagrantHalt = new MenuItem();
                cmdVagrantHalt.Text = "vagrant halt";
                DropDown.MenuItems.Add(cmdVagrantHalt);
                cmdVagrantHalt.Click += (sender, e) => RunCommand("vagrant halt", box.boxPath, box.boxID);

                MenuItem cmdVagrantDestroy = new MenuItem();
                cmdVagrantDestroy.Text = "vagrant destroy";
                DropDown.MenuItems.Add(cmdVagrantDestroy);
                cmdVagrantDestroy.Click += (sender, e) => RunCommand("vagrant destroy", box.boxPath, box.boxID);

                if (box.boxStatus == false)
                {
                    cmdVagrantUp.Enabled = true;
                    // rest should be false
                    cmdVagrantSsh.Enabled = false;
                    cmdVagrantReload.Enabled = false;
                    cmdVagrantHalt.Enabled = false;
                    cmdVagrantDestroy.Enabled = false;
                }
                else
                {
                    cmdVagrantUp.Enabled = false;
                    // rest should be true
                    cmdVagrantSsh.Enabled = true;
                    cmdVagrantReload.Enabled = true;
                    cmdVagrantHalt.Enabled = true;
                    cmdVagrantDestroy.Enabled = true;
                }

                DropDown.MenuItems.Add("-", OnExit);

                MenuItem removeFromList = new MenuItem();
                removeFromList.Text = "Remove From List";
                DropDown.MenuItems.Add(removeFromList);
                removeFromList.Click += (sender, e) => RemoveFromList(box.boxID);

                trayMenu.MenuItems.Add(DropDown);
            }

            // Finish standard menu items
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Halt All", OnExit);
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Preferences", OnExit);
            trayMenu.MenuItems.Add("Quit", OnExit);

            Program.SaveSettings();
        }
        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
            base.OnLoad(e);
        }
        private void RunCommand(String command, String path, int thisBox)
        {
            Console.WriteLine(command);
            // Let's cut out the vagrantfile part so we can cd easily
            String directoryPath = path.Remove(path.Length-12);
            Console.WriteLine(directoryPath);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            // Get the current box object so we can change things if we need
            var thisBoxObject = vagrantBoxList.list.FirstOrDefault(box => box.boxID == thisBox);

            // Take the command, and do specific things based upon what was sent
            switch (command)
            {
                case "vagrant ssh":
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    break;
                case "vagrant up":                    
                    thisBoxObject.boxStatus = true;
                    break;
                case "vagrant halt":                    
                    thisBoxObject.boxStatus = false;
                    break;
            }

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C cd "+directoryPath+" && "+command;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            RefreshMenu();
        }
        private void RemoveFromList(int index)
        {
            // In preferences, could give option to go silent and remove dialogs?
            DialogResult result = MessageBox.Show("Remove this Vagrant box from the list?\n\n(This will only affect the bookmark, your Vagrantfile will be untouched.)", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (result == DialogResult.Yes)
            {
                // Remove the index from the list
                //vagrantBoxList.list.RemoveAt(index);
                //
                vagrantBoxList.list.RemoveAll(box => box.boxID == index);
                // Do some cleaning (is this automated?)
                vagrantBoxList.list.TrimExcess();
                // Refresh menu
                RefreshMenu();
            }
        }
        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Icon = null;
            Application.Exit();
        }
        private void AddBox(object sender, EventArgs e)
        {
            AddBoxForm addBox = new AddBoxForm();
            addBox.Show();
        }
    }
}
