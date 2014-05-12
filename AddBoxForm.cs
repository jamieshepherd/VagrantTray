using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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
                AddBoxProcess();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnAddBox_Click(object sender, EventArgs e)
        {
            AddBoxProcess();
        }
        private void AddBoxProcess()
        {
            // Check that the file is a Vagrantfile, and name is not empty
            if (txtName.Text != "" && Path.GetFileName(txtPath.Text).ToLower() == "vagrantfile" && Path.GetExtension(txtPath.Text) == "" && File.Exists(txtPath.Text))
            {
                MainWindow.vagrantBoxList.id++;
                int newID = MainWindow.vagrantBoxList.id;
                Box objBox = new Box(newID, txtName.Text, txtPath.Text, false);
                MainWindow.vagrantBoxList.AddBox(objBox);
                this.Close();
            }
            else if (txtName.Text == "")
            {
                MessageBox.Show("The name of this box cannot be blank.");
            }
            else
            {
                MessageBox.Show("Sorry, that isn't a valid Vagrantfile.");
            }
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
