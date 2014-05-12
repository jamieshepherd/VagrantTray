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
    public partial class AddBoxForm : Form
    {
        public AddBoxForm()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                MainWindow.vagrantBoxList.id++;
                int newID = MainWindow.vagrantBoxList.id;
                Box objBox = new Box(newID, txtName.Text, txtPath.Text, false);
                MainWindow.vagrantBoxList.AddBox(objBox);
                this.Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnAddBox_Click(object sender, EventArgs e)
        {
            MainWindow.vagrantBoxList.id++;
            int newID = MainWindow.vagrantBoxList.id;
            Box objBox = new Box(newID, txtName.Text, txtPath.Text, false);
            MainWindow.vagrantBoxList.AddBox(objBox);
            this.Close();
        }

        private void AddBoxForm_Load(object sender, EventArgs e)
        {

        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog(); string filename = "";
            ofd.Filter = "Vagrant files (Vagrantfile)|Vagrantfile";
            string path = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = System.IO.Path.GetFileName(ofd.FileName);
                path = System.IO.Path.GetDirectoryName(ofd.FileName);
            }
            txtPath.Text = path + "\\" + filename;
        }
    }
}
