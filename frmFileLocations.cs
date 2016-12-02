using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data.OleDb;

namespace timeMachine
{
    public partial class frmFileLocations : Form
    {
        public frmFileLocations()
        {
            InitializeComponent();

            //set constants and filename vars
            string lsFileLocationsXML = AppDomain.CurrentDomain.BaseDirectory + "fileLocations.xml";

            //Set ext ref file
            string lsDirExtRefFile = loadProgFileXML("External Reference Files", lsFileLocationsXML);

            txtExtRefFile.Text = lsDirExtRefFile;
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
            //Also need to figure out the best way to handle the filename. <= Done


            //Here we need to update the /timeMachine/progFiles/progFile node in XML with the user selected file and path




        }


        private string loadProgFileXML(string rsLocName, string rsXMLFile)
        {

            //Load xml values
            //string rsXMLFile = AppDomain.CurrentDomain.BaseDirectory + clsConfigs.gsFileLocationsXML;

            if (File.Exists(rsXMLFile))
            {


                XmlDocument doc = new XmlDocument();
                doc.Load(rsXMLFile);


                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xnlNodes = xelRoot.SelectNodes("/timeMachine/progFiles/progFile");

                //First, create file structure
                //foreach (XmlNode xndNode in xnlNodes)
                //{
                //    //string lsRefNum = xndNode["locName"].InnerText;
                //    string lsLocPath = xndNode["locPath"].InnerText;

                //    //First is a main prog dir. perhaps c:\TimeMachine\
                //    Directory.CreateDirectory(lsLocPath);

                //}


                //Then, find file/location based on param
                //Need to handle initial call for creating file structure...?
                foreach (XmlNode xndNode in xnlNodes)
                {
                    string lsLocName = xndNode["locName"].InnerText;
                    string lsLocPath = xndNode["locPath"].InnerText;

                    if (lsLocName == rsLocName)
                    {

                        return lsLocPath;

                    }
                    else
                    {
                        //return "";
                        continue;
                    }

                    //First is a main prog dir. perhaps c:\TimeMachine\
                    //Directory.CreateDirectory(lsLocPath);

                }

                //Not sure if we will ever get here. Perhaps, If the file does not have any nodes.
                return "";


                //Set picklist default
                //cboTaskList.SelectedIndex = 0;

            }

            else
            {
                //createXML();

                //File should already exist at this point... Throw Error if not found.
                //OR create a new default XML...???

                //for now, just have a message prompt and return blank.
                MessageBox.Show("Ye location map be missin'.", "Yaarrrrr...");
                return "";
            }


        }
    }
}
