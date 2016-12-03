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
    public partial class frmTimeMachine : Form
    {
        public frmTimeMachine()
        {
            InitializeComponent();

            updateStatus("Loading Program...");


            //set constants and filename vars
            string lsFileLocationsXML = AppDomain.CurrentDomain.BaseDirectory + "fileLocations.xml";

            string lsStaticRefsXML = AppDomain.CurrentDomain.BaseDirectory + "staticRefs.xml";
            //string lsExtRefFile = "report.csv";
            

            //Build directory tree
            createFileStructure(lsFileLocationsXML);

            //Set file location vars
            string lsDirFileRoot = loadFileLocationXML("File Root", lsFileLocationsXML);
            string lsDirFileHistory = loadFileLocationXML("File History", lsFileLocationsXML);
            string lsDirSummaryFiles = loadFileLocationXML("Summary Files", lsFileLocationsXML);
            string lsDirTimesheetFiles = loadFileLocationXML("Timesheet Files", lsFileLocationsXML);
            string lsDirCurDayFiles = loadFileLocationXML("Current Day Files", lsFileLocationsXML);
            string lsDirUnprocessedFiles = loadFileLocationXML("Unprocessed Files", lsFileLocationsXML);

            //Set ext ref file
            string lsDirExtRefFile = loadProgFileXML("External Reference Files", lsFileLocationsXML);


            //Set up filename for TODAYS file

            DateTime lcToday = System.DateTime.Now;
            string lcMonth = lcToday.ToString("MM");
            string lcDay = lcToday.ToString("dd");
            string lsDestFile = lsDirCurDayFiles + "\\Timesheet_" + lcMonth + "_" + lcDay + ".csv";

            //Set up filename for the today's summary file
            //Get date from filename

            char[] delChars = { '_', '.' };
            string[] laDateSplit = (Path.GetFileName(lsDestFile)).Split(delChars);

            string lsDate = laDateSplit[1].ToString() + "_" + laDateSplit[2].ToString();

            //Then pass that list into a linq query function

            //Need a summary filename based on date
            string lsSummaryFile = lsDirSummaryFiles + "\\" + lsDate + "_SUMMARY.csv";

            //Check for previous file
            //validatePreviousFile(lsDestFile, lsDirUnprocessedFiles, lsDirCurDayFiles, lsDirTimesheetFiles, lsSummaryFile);
            validatePreviousFile(lsDestFile, lsDirUnprocessedFiles, lsDirCurDayFiles, lsDirTimesheetFiles, lsDirSummaryFiles);

            //Check for existing file summary
            //validateSummaryExistance(lsDestFile);

            //Check for today's file
            validateFileExistance(lsDestFile, lsDirTimesheetFiles);

            //Prepopulate text field defaults
            populateDefaults();

            //Populate reference list with static references
            loadStaticRefXML(lsStaticRefsXML);

            // Bind the SelectedValueChanged event to our handler for it.
            cboTaskList.SelectedValueChanged +=
                new EventHandler(cboTaskList_SelectedValueChanged);

            //Populate ref list with exported RFCs, etc.
            //loadExtRef(lsDirExtRefFile + "\\report.csv");
            loadExtRef(lsDirExtRefFile);

            updateStatus("Ready. Please start your task...");

        }


        //**********************
        //
        //  GENERAL MAINTENANCE
        //
        //**********************
        #region maintenance


        private void createFileStructure(string rsXMLFile)
        {


            updateStatus("Checking file structure...");
            //Load xml values
            //string rsXMLFile = AppDomain.CurrentDomain.BaseDirectory + clsConfigs.gsFileLocationsXML;

            if (File.Exists(rsXMLFile))
            {


                XmlDocument doc = new XmlDocument();
                doc.Load(rsXMLFile);


                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xnlNodes = xelRoot.SelectNodes("/timeMachine/fileLocations/location");

                //First, create file structure
                foreach (XmlNode xndNode in xnlNodes)
                {
                    //string lsRefNum = xndNode["locName"].InnerText;

                    //Set this up to pull the dir path only, not the filename (if applicable)
                    string lsLocPath = xndNode["locPath"].InnerText;

                    ////Check to see if path has a filename attached
                    //if (Right(lsLocPath, 4) == ".csv")
                    //{
                    //    //get only the file path, not filename
                    //    Directory.CreateDirectory(Path.GetDirectoryName(lsLocPath));
                    //}
                    //else
                    //{ 
                    //    //Just create the path
                    //    Directory.CreateDirectory(lsLocPath);
                    //}

                    //First is a main prog dir. perhaps c:\TimeMachine\
                    Directory.CreateDirectory(lsLocPath);

                }


                //Then, find file/location based on param
                //Need to handle initial call for creating file structure...?
                //foreach (XmlNode xndNode in xnlNodes)
                //{
                //    string lsLocName = xndNode["locName"].InnerText;
                //    string lsLocPath = xndNode["locPath"].InnerText;

                //    if (lsLocName = rsLocName)
                //    {



                //    }

                //    //First is a main prog dir. perhaps c:\TimeMachine\
                //    Directory.CreateDirectory(lsLocPath);

                //}




                //Set picklist default
                //cboTaskList.SelectedIndex = 0;

            }

            else
            {
                //createXML();

                //File should already exist at this point... Throw Error if not found.
                //OR create a new default XML...???
            }
        }



        private void updateStatus(string rsMessage)
        {
            toolStripStatusLabel1.Text = rsMessage;
        }

        private void populateDefaults()
        {

            //Default the date to today's date
            dtpDate.Value = DateTime.Now;
            //txtTime.Text = DateTime.Now.ToString("hh:mm  tt");
            txtTime.Text = DateTime.Now.ToString("HH:mm");

            //btnStartTask.Focus();
            this.ActiveControl = txtTaskDesc;
            this.AcceptButton = btnStartTask;

            //Set default text fields
            txtExtRefNum.Text = "999999";
            txtTaskTitle.Text = "Internal Misc.";
            txtTaskDesc.Text = "Odds and Ends";

        }

        private void cboTaskList_SelectedValueChanged(object sender, EventArgs e)
        {
            //Update reference text fields when combobox is changed

            string[] laTask = cboTaskList.Text.Split(':');

            txtExtRefNum.Text = laTask[0].Trim();
            txtTaskTitle.Text = laTask[1].Trim();

            txtTaskDesc.Text = "";

            txtTaskDesc.Focus();

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




        private string loadFileLocationXML(string rsLocName, string rsXMLFile)
        {

            //Load xml values
            //string rsXMLFile = AppDomain.CurrentDomain.BaseDirectory + clsConfigs.gsFileLocationsXML;

            if (File.Exists(rsXMLFile))
            {
                

                XmlDocument doc = new XmlDocument();
                doc.Load(rsXMLFile);


                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xnlNodes = xelRoot.SelectNodes("/timeMachine/fileLocations/location");

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
        





        private void loadStaticRefXML(string rsStaticRefFile)
        {

            //Check to see if the file currently exists
            //if not, create it - call createXML()
            //XML should reside in program root
            //XML should be called staticRefs.XML

            updateStatus("Updating static references...");

            //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + clsConfigs.gsStaticRefsXML))
            if (File.Exists(rsStaticRefFile))
            {
                //Load xml values
                //string rsStaticRefFile = AppDomain.CurrentDomain.BaseDirectory + clsConfigs.gsStaticRefsXML;

                XmlDocument doc = new XmlDocument();
                doc.Load(rsStaticRefFile);


                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xnlNodes = xelRoot.SelectNodes("/timeMachine/staticRefs/reference");


                foreach (XmlNode xndNode in xnlNodes)
                {
                    string lsRefNum = xndNode["refNum"].InnerText;
                    string lsRefName = xndNode["refName"].InnerText;

                    cboTaskList.Items.Add(lsRefNum + " : " + lsRefName);

                }

                //Set picklist default
                cboTaskList.SelectedIndex = 0;

            }
            else
            {
                //createXML();

                //File should already exist at this point... Throw Error if not found.
                //OR create a new default XML...???
            }

        }




        private void loadExtRef(string rsExtRefFile)
        {

            updateStatus("Updating external references...");

            //CHECK TO SEE IF FILE EXISTS FIRST

            //var filename = @"C:\Temp\report.csv";
            //var rsExtRefFile = clsConfigs.gsExtRefLoc + clsConfigs.gsExtRefFile;

            if (File.Exists(rsExtRefFile))
            {
                var connString = string.Format(
                @"Provider=Microsoft.Jet.OleDb.4.0; Data Source={0};Extended Properties=""Text;HDR=YES;FMT=Delimited""",
                Path.GetDirectoryName(rsExtRefFile)
                );

                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    var query = "SELECT * FROM [" + Path.GetFileName(rsExtRefFile) + "]";
                    using (var adapter = new OleDbDataAdapter(query, conn))
                    {

                        var dt = new DataTable("CSV File");
                        adapter.Fill(dt);

                        //Loop through data table
                        foreach (DataRow loRow in dt.Rows)
                        {
                            cboTaskList.Items.Add(loRow[0].ToString() + " : " + loRow[2].ToString());
                        }
                    }
                }

            }
            else
            {

                //Prompt that file does not exist.
                //Dont load the file, but prompt stating that the file wasnt found.

                MessageBox.Show("External reference file not found.", "FYI...");
            
            }


            
            




        }





        public List<Task> loadTaskList(string rsDestFile)
        {

            updateStatus("Processing hours...");
            //Build task list

            //string lsDestFile = clsConfigs.gsOutputLocation + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";

            List<Task> laTaskList = new List<Task>();

            var connString = string.Format(
                @"Provider=Microsoft.Jet.OleDb.4.0; Data Source={0};Extended Properties=""Text;HDR=YES;FMT=Delimited""",
                Path.GetDirectoryName(rsDestFile)
            );

            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                var query = "SELECT * FROM [" + Path.GetFileName(rsDestFile) + "]";
                using (var adapter = new OleDbDataAdapter(query, conn))
                {

                    var dt = new DataTable("CSV File");
                    adapter.Fill(dt);

                    //Loop through data table
                    foreach (DataRow loRow in dt.Rows)
                    {

                        //string zsStartTime;
                        //string zsEndTime;
                        //string zsTimeSpent;

                        //Create the time format conversion thing here...
                        DateTime zsEndTime = Convert.ToDateTime(loRow[6].ToString());
                        DateTime zsStartTime = Convert.ToDateTime(loRow[1].ToString());
                        //TimeSpan zsTimeSpent = zsEndTime - zsStartTime;
                        double zsTimeSpent = (zsEndTime - zsStartTime).TotalHours;

                        //DateTime ltEndTime = DateTime.Parse(loRow[6].ToString(), "HH:mm");
                        //zsTimeSpent = (timeEnd - timeStart);
                        

                        //cboTaskList.Items.Add(loRow[0].ToString() + " : " + loRow[2].ToString());
                        laTaskList.Add(new Task() { 

                            lsDate = loRow[0].ToString(), 
                            lsStartTime = loRow[1].ToString(),
                            lsRefName = loRow[2].ToString(),
                            lsRefNum = loRow[3].ToString(),
                            lsTitle = loRow[4].ToString(),
                            lsTaskDesc = loRow[5].ToString(),
                            lsEndTime = loRow[6].ToString(),
                            //lsTimeSpent = zsTimeSpent.ToString()
                            lsTimeSpent = zsTimeSpent
                            
                            
                        
                        });

                    }
                }
            }

            return laTaskList;
                  
        }













        private void addToFile(bool rsFirstRow, string rsDestFile)
        {

            //Code here to add record entry line to output csv file.

            //Vars
            string lsDate = dtpDate.Text;
            string lsTime = txtTime.Text;
            string lsTask = cboTaskList.Text;
            string lsRefNum = txtExtRefNum.Text;
            string lsRefName = txtTaskTitle.Text;
            string lsDesc = txtTaskDesc.Text;
            string lsEndTime = txtTime.Text;
            //string rsDestFile = clsConfigs.gsOutputLocation + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";

            using (StreamWriter outfile = new StreamWriter(rsDestFile, true))
            {

                //Need to check to see if the file already has a record.
                //If it does not (first record), it will not write out lsEnd time and new line.

                if (rsFirstRow == true)
                {
                    //Second row and greater
                    //outfile.Write(lsEndTime + "\"" + System.Environment.NewLine + lsDate + "," + lsTime + ",\"" + lsTask + "\",\"" + lsRefNum + "\",\"" + lsRefName + "\",\"" + lsDesc + "\"");
                    outfile.Write(lsEndTime + System.Environment.NewLine + lsDate + "," + lsTime + ",\"" + lsTask + "\",\"" + lsRefNum + "\",\"" + lsRefName + "\",\"" + lsDesc + "\",");
                }
                else
                {
                    //First row only
                    outfile.Write(System.Environment.NewLine + lsDate + "," + lsTime + ",\"" + lsTask + "\",\"" + lsRefNum + "\",\"" + lsRefName + "\",\"" + lsDesc + "\",");
                }
            }
        }












        private void writeFinalEntry(string rsDestFile, string rsEndTime)
        {

            //Write out the end time for the last entry in the file.

            //string lsEndTime = txtTime.Text;

            //string lsDestFile = clsConfigs.gsOutputLocation + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";

            using (StreamWriter outfile = new StreamWriter(rsDestFile, true))
            {
                outfile.Write(rsEndTime);
            }

        }


        private void createSummary(List<Task> raTaskList, string rsDate, string rsSummaryFile)
        {

            updateStatus("Creating summary...");
            //This is the function that will create the summary file

            //First create output csv



            //************************
            //
            //      NOTE:
            //      Cannot save negative values for time span
            //      Determine how to validate this issue
            //
            //************************
            

            //Logic: (use linq)

            //This can be done on either previous day's file or current day's file.

            //Need to build in some sort of failsafe so if I accidentally click the clock out button, i can "undo" it.


            //Process each distinct ext ref num 

            //var laRefNums = raTaskList.Select(x => x.lsRefNum).Distinct();

            //Then get total hours for the day per ref num

            //string lsDate = dtpDate.Text;

            //string rsSummaryFile = clsConfigs.gsSummaryLocation + rsDate + "_SUMMARY.csv";

            using (StreamWriter sw = new StreamWriter(rsSummaryFile, true))
            {
                //sw.Write("Date,StartTime,Task,RefNum,Title,Description" + System.Environment.NewLine);
                sw.Write("Date,RefNum,TimeSpent" + System.Environment.NewLine);
                sw.Close();
            }

            var laRefHours = raTaskList.GroupBy(a => a.lsRefNum).Select(p => new { lsRefNum = p.Key, lsTimeSpent = p.Sum(q => q.lsTimeSpent) });

            foreach (var lsRefNum in laRefHours)
            {

                lsRefNum.lsRefNum.ToString();
                lsRefNum.lsTimeSpent.ToString();


                //updateStatus("Creating new file...");
                using (StreamWriter outfile = new StreamWriter(rsSummaryFile, true))
                {

                    //outfile.Write(lsEndTime + "\"" + System.Environment.NewLine + lsDate + "," + lsTime + ",\"" + lsTask + "\",\"" + lsRefNum + "\",\"" + lsRefName + "\",\"" + lsDesc + "\"");
                    outfile.Write(rsDate + "," + lsRefNum.lsRefNum.ToString() + "," + TimeSpan.FromHours((double)lsRefNum.lsTimeSpent).ToString() + "," + System.Environment.NewLine);


                }
                


                //FUTURE JOEL...
                //THIS IS WHERE YOU LEFT OFF.
                //THE LINES IN THE LOOP ARE WHERE YOU SHOULD BE WRITING OUT TO THE SUMMARY CSV FILE.
                //ALSO, YOU'RE AWESOME, AND EVEN THOUGH THIS WEEK KIND OF SUCKED, IT REALLY WASNT THAT BAD.
                //TOMORROW IS A NEW DAY.
                //ALSO, GOOD LUCK WITH THANKSGIVING.
                //JUST WAIT UNTIL FRIDAY AND THEN GET TRASHED.
                //LOVE YOU!
                //SINCERELY, JOEL.

            }


            updateStatus("Summary created...");

            //remember that internal misc is 99999

            //Processed file gets written out to new file

            //not sure of format
            //Either ext refs as headers or as records

            //Dont forget to do file handling....
        
        }





        #endregion




        




        //**********************
        //
        //  VALIDATIONS
        //
        //**********************
        #region validations


        private bool validateRecord()
        { 
        
            //After button is clicked, ensure that the data was written out properly.
            updateStatus("Validating...");

            //This is a test... replace the lines below with actual validation code.
            bool lbIsValidated = false;
            lbIsValidated = true;
            return lbIsValidated;

        }


        private bool validateFirstRecord(string rsDestFile)
        { 

            //Check to see if the file already has the first record written out.
            //This will affect how the first row is written.

            //string rsDestFile = clsConfigs.gsOutputLocation + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";

            int liRecords = File.ReadLines(rsDestFile).Count();

            if (liRecords > 1)
            {
                return true;
            }
            else 
            {
                return false;
            }
            
        
        
        }


        //private bool validateSummaryExistance(string rsDestFile, string rsDirTimesheets)
        //{

        //    //string lsDestFile = clsConfigs.gsOutputLocation + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";

        //    //First check to see if today has already been processed. If so, prompt, but dont create a new file.

        //    //This filepath needs hella updating yo
        //    if (File.Exists(@"E:\TimeMachine\Historical\Timesheets\" + Path.GetFileName(rsDestFile)))
        //    {
        //        MessageBox.Show("The timesheet file for today has already been processed. \r\n a new file will not be created. \r\n Please re-open the existing timesheet to make manual changes.", "Attention!");
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        
        
        //}


        private bool validateFileExistance(string rsDestFile, string rsDirTimesheets)
        {

            updateStatus("Checking for file...");

            //Check when program starts.

            //First check to see if today has already been processed. If so, prompt, but dont create a new file.
            //if (File.Exists(@"E:\TimeMachine\Historical\Timesheets\" + Path.GetFileName(rsDestFile)))
            if (File.Exists(rsDirTimesheets  + "\\" + Path.GetFileName(rsDestFile)))
            {
                //MessageBox.Show("The timesheet file for today has already been processed. \r\n a new file will not be created. \r\n Please re-open the existing timesheet to make manual changes.", "Attention!");
                MessageBox.Show("The timesheet file for today has already been processed. \r\n a new file will not be created. \r\n Please re-open the existing timesheet to make manual changes.", "Attention!");
                return true;
            }

            //Also perhaps check to see if TODAYS file is already in the UNPROCESSED FOLDER. This shouldnt ever happen, but just in case, do it anyways.



            //If TODAYS file exists, continue on. Otherwise create new file.

            //string lsDestFile = clsConfigs.gsOutputLocation + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";

            if (File.Exists(rsDestFile))
            {

                updateStatus("File found...");
                return true;

            }
            else
            { 
            
                //create new file

                updateStatus("Creating new file...");

                using (StreamWriter sw = new StreamWriter(rsDestFile, true))
                {
                    //sw.Write("Date,StartTime,Task,RefNum,Title,Description" + System.Environment.NewLine);
                    sw.Write("Date,StartTime,Task,RefNum,Title,Description,EndTime");
                    sw.Close();
                }

                return true;

            }

        }


        protected virtual bool IsFileLocked(FileInfo rsDestFile)
        {
            FileStream stream = null;

            try
            {
                stream = rsDestFile.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }


        //private bool validatePreviousFile(string rsDestFile, string rsDirUnprocessed, string rsDirToday, string rsDirTimesheets, string rsSummaryFile)
        private bool validatePreviousFile(string rsDestFile, string rsDirUnprocessed, string rsDirToday, string rsDirTimesheets, string rsDirSummaryFiles)
        {

            //First move previous file(s) (if any) from the Today folder to the Unprocessed folder... 
            //compares filename to today, if not match, then move.)

            //string rsDestFile = clsConfigs.gsOutputLocation + System.DateTime.Now.Month + "_" + (System.DateTime.Now.Day) + ".csv";

            string[] laFileList = Directory.GetFiles(rsDirToday);
            foreach (string lsFileName in laFileList)
            {

                if (lsFileName != rsDestFile)
                {

                    File.Move(lsFileName, rsDirUnprocessed + "\\" + Path.GetFileName(lsFileName));
                }
            
            }
                


            //Check previous files - not based on filename
            //previous unprocessed files should already be moved to unprocessed folder 

            //also possibly check to see if summary file for previous day(s) exist...?

            //Need to check if the file from the previous day(s) is still "active"
            //check the unprocessed folder and loop through any and all files


            //string lsPreviousFile = clsConfigs.gsOutputLocation + System.DateTime.Now.Month + "_" + (System.DateTime.Now.Day - 1) + ".csv";



            int liFileCount = Directory.GetFiles(rsDirUnprocessed, "*", SearchOption.TopDirectoryOnly).Length;

            if (liFileCount > 0)
            {

                updateStatus("File found...");

                //show yes/no/cancel prompt


                DialogResult result = MessageBox.Show("You have unprocessed hours from previous day(s). \r\n\r\n Do you wish to process the file(s)? \r\n\r\n Note: A 'Clock Out' time of 17:00 will be appended to the last record. You may update manually if needed.", "Action Required...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //code for Yes

                    //if yes, call  writeFinalEntry() and createSummary() functions for yesterday's file. 

                    //Possibly send in the filename for the previous file...?


                    //Perhaps add a validation to see if the file has any time records written out. Otherwise it might throw an error if there are headers but no data.

                    string[] laUnprocessedFileList = Directory.GetFiles(rsDirUnprocessed);

                    foreach (string lsDestFile in laUnprocessedFileList)
                    {

                        //This will error since the txtTime field has not populated yet.
                        //Consider sending in the time as a param instead.
                        writeFinalEntry(lsDestFile, "17:00");

                        //Function to loop through csv and write to list of class
                        List<Task> laTaskList = loadTaskList(lsDestFile);

                        //Get date from filename
                        char[] delChars = { '_', '.'};
                        string[] laDateSplit = (Path.GetFileName(lsDestFile)).Split(delChars);

                        string lsDate = laDateSplit[1].ToString() + "_" + laDateSplit[2].ToString();

                        string lsSummaryFile = rsDirSummaryFiles + "\\" + lsDate + "_SUMMARY.csv";

                        //Then pass that list into a linq query function
                        createSummary(laTaskList, lsDate, lsSummaryFile);

                        //Don't forget to move the daily file to the historical folder after processing.
                        File.Move(lsDestFile, rsDirTimesheets + "\\" + Path.GetFileName(lsDestFile));

                    }

                    //string lsDestFile = clsConfigs.gsOutputLocation + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";


                    //Might also want to send in a time indicatior/prompt... or else it will write out the current time.
                    //Ex: if you forgot to clock out at 5:00 pm the previous day, you dont want it to write out 8:00 am today.

                    

                    //Do any necessary file handling as well
                    // ^ Move or rename or both
                    // ^ Move/Rename after processing hours


                }
                else if (result == DialogResult.No)
                {
                    //code for No
                    //no and cancel do the same thing
                    //user will continue to be prompted until the files are processed

                }
                else if (result == DialogResult.Cancel)
                {
                    //code for Cancel
                    //no and cancel do the same thing
                    //user will continue to be prompted until the files are processed
                }
            
            }













                
            return true;





            
            


            //return true;


        }



        //private void processHours()
        //{ 





        
        //}




        #endregion






        //**********************
        //
        //  MENU AND BUTTON CLICKS
        //
        //**********************
        #region menuButtonClicks



        private void btnStartTask_Click(object sender, EventArgs e)
        {

            updateStatus("Saving to file...");


            //set constants and filename vars
            string lsFileLocationsXML = AppDomain.CurrentDomain.BaseDirectory + "fileLocations.xml";
            //string lsStaticRefsXML = AppDomain.CurrentDomain.BaseDirectory + "staticRefs.xml";


            //Set file location vars
            //string lsDirFileRoot = loadFileLocationXML("File Root", lsFileLocationsXML);
            //string lsDirFileHistory = loadFileLocationXML("File History", lsFileLocationsXML);
            //string lsDirSummaryFiles = loadFileLocationXML("Summary Files", lsFileLocationsXML);
            //string lsDirTimesheetFiles = loadFileLocationXML("Timesheet Files", lsFileLocationsXML);
            string lsDirCurDayFiles = loadFileLocationXML("Current Day Files", lsFileLocationsXML);
            //string lsDirUnprocessedFiles = loadFileLocationXML("Unprocessed Files", lsFileLocationsXML);
            //string lsDirExtRefFile = loadFileLocationXML("External Reference File", lsFileLocationsXML);


            //Set up filename for TODAYS file
            //string lsDestFile = lsDirCurDayFiles + "\\Timesheet_" + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";

            //Set up filename for TODAYS file

            DateTime lcToday = System.DateTime.Now;
            string lcMonth = lcToday.ToString("MM");
            string lcDay = lcToday.ToString("dd");


            string lsDestFile = lsDirCurDayFiles + "\\Timesheet_" + lcMonth + "_" + lcDay + ".csv";




            //Check to see if file is currently open/in use
            FileInfo lsDestFileInfo = new FileInfo(lsDestFile);
            if (IsFileLocked(lsDestFileInfo) == true)
            {
                MessageBox.Show("There was an issue writing to the file. /r/n Please ensure the file is currently closed.","File Error...");
                return;
            }



            //Check to see if file already has the first record.
            bool lbFirstRow = validateFirstRecord(lsDestFile);



            //If text boxes are empty, still allow save to log the time. Maybe write out "UNKNOWN" OR "BLANK".
            addToFile(lbFirstRow, lsDestFile);

            //Maybe just put addToFile inside a trycatch instead of below...?

            //Validate
            if (validateRecord() == true)
            {
                updateStatus("File saved. Closing program...");
            }


            //Close prog if everything is good.
            Application.Exit();

        }



        private void btnClockOut_Click(object sender, EventArgs e)
        {

            updateStatus("Clocking out...");
            //Close out the day ("Clock Out")



            //set constants and filename vars
            string lsFileLocationsXML = AppDomain.CurrentDomain.BaseDirectory + "fileLocations.xml";
            //string lsStaticRefsXML = AppDomain.CurrentDomain.BaseDirectory + "staticRefs.xml";


            //Set file location vars
            //string lsDirFileRoot = loadFileLocationXML("File Root", lsFileLocationsXML);
            //string lsDirFileHistory = loadFileLocationXML("File History", lsFileLocationsXML);
            string lsDirSummaryFiles = loadFileLocationXML("Summary Files", lsFileLocationsXML);
            string lsDirTimesheetFiles = loadFileLocationXML("Timesheet Files", lsFileLocationsXML);
            string lsDirCurDayFiles = loadFileLocationXML("Current Day Files", lsFileLocationsXML);
            string lsDirUnprocessedFiles = loadFileLocationXML("Unprocessed Files", lsFileLocationsXML);
            //string lsDirExtRefFile = loadFileLocationXML("External Reference File", lsFileLocationsXML);


            //Set up filename for TODAYS file
            //string lsDestFile = lsDirCurDayFiles + "\\Timesheet_" + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";

            //Set up filename for TODAYS file

            DateTime lcToday = System.DateTime.Now;
            string lcMonth = lcToday.ToString("MM");
            string lcDay = lcToday.ToString("dd");


            string lsDestFile = lsDirCurDayFiles + "\\Timesheet_" + lcMonth + "_" + lcDay + ".csv";



            //string lsDestFile = clsConfigs.gsOutputLocation + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";

            //Validate to see if already processed...
            //if (validateFileExistance(lsDestFile) == true)
            //{
            //    return;
            //}


            

            //Move the file from the TODAY folder to the unprocessed folder.
            File.Move(lsDestFile, lsDirUnprocessedFiles + "\\" + Path.GetFileName(lsDestFile));

            lsDestFile = lsDirUnprocessedFiles + "\\" + Path.GetFileName(lsDestFile);


            //IMPORTANT... CLOSE OUT LAST ENTRY IN FILE.
            writeFinalEntry(lsDestFile, txtTime.Text);


            //Function to loop through csv and write to list of class
            List<Task> laTaskList = loadTaskList(lsDestFile);


            //Get date from filename

            char[] delChars = { '_', '.' };
            string[] laDateSplit = (Path.GetFileName(lsDestFile)).Split(delChars);

            string lsDate = laDateSplit[1].ToString() + "_" + laDateSplit[2].ToString();

            //Then pass that list into a linq query function

            //Need a summary filename based on date
            string lsSummaryFile = lsDirSummaryFiles + "\\" + lsDate + "_SUMMARY.csv";
            createSummary(laTaskList, lsDate, lsSummaryFile);

            //call datagridview shenanigans


            //Don't forget to move the daily file to the historical folder after processing.
            File.Move(lsDestFile, lsDirTimesheetFiles + "\\" + Path.GetFileName(lsDestFile));



            updateStatus("Ready.");


        }






        private void manualTimeEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is still under construction.", "Oops");
        }


        private void addStaticReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {


            //Opens screen for updating static reference numbers

            MessageBox.Show("This is still under construction.", "Oops");

            frmUserDefinedRefs UserDefinedRefs = new frmUserDefinedRefs();
            UserDefinedRefs.Show();

        }





        private void summariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSummary frmSummary = new frmSummary();
            frmSummary.Show();
        }



        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MessageBox.Show(
                "The Time Machine \r\n Version: 1.1.2 \r\n \r\n Copyright: (c) 2016 \r\n Author: Joel K. Stienstra \r\n All Rights Reserved.  \r\n \r\n Icon courtesy of: Oxygen Team \r\n http://www.oxygen-icons.org/", "Product Information"
                );

        }



        private void fileLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Show new form
            //auto pop form with xml values

            frmFileLocations frmLocations = new frmFileLocations();
            frmLocations.Show();

        }



        private void restoreDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is still under construction.", "Oops");
        }



        private void docsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/jstienstra/timeMachine/wiki");
        }




        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        









        

       
    }


    public class Task
    {
        public string lsDate { get; set; }
        public string lsStartTime { get; set; }
        public string lsEndTime { get; set; }
        public string lsRefNum { get; set; }
        public string lsRefName { get; set; }
        public string lsTitle { get; set; }
        public string lsTaskDesc { get; set; }
        public double lsTimeSpent { get; set; }
        
    }


}
