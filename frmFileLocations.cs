using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timeMachine
{
    public partial class frmFileLocations : Form
    {
        public frmFileLocations()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select import file...";
            ofd.Filter = "CSV files|*.csv";
            ofd.InitialDirectory = @"C:\";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //lblSelectFile.Text = "" + ofd.FileName + "";
                txtExtRefFile.Text = ofd.FileName.ToString();
            }
        }

        private void btnUpdateFileLocations_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is still under construction.", "Oops");

            //Plan...
            //Get ext ref location and file.
            //update the "External Reference File" node in the locations xml with the location.
            //Also need to figure out the best way to handle the filename.
        }
    }
}
