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
    class clsReporting
    {


        //public static void loadDataGrid(List<Summary> raSumList)
        //{

        //    var source = new BindingSource(raSumList, null);
            
        //    frmSummary.dataGridView1.DataSource = source;

        //}



        public static DataTable loadSumList(string rsSumFile)
        {


            var connString = string.Format(
                @"Provider=Microsoft.Jet.OleDb.4.0; Data Source={0};Extended Properties=""Text;HDR=YES;FMT=Delimited""",
                Path.GetDirectoryName(rsSumFile)
            );

            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                var query = "SELECT * FROM [" + Path.GetFileName(rsSumFile) + "]";
                using (var adapter = new OleDbDataAdapter(query, conn))
                {

                    var dt = new DataTable("CSV File");
                    adapter.Fill(dt);

                    return dt;

                }



            }

            //frmSummary.dataGridView1.DataSource = dt.DefaultView;
            //dataGrid.Columns[2].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm:ss";
        
        }



        //public static List<Summary> loadSumList(string rsSumFile)
        //{

        //    List<Summary> laSumList = new List<Summary>();

        //    var connString = string.Format(
        //        @"Provider=Microsoft.Jet.OleDb.4.0; Data Source={0};Extended Properties=""Text;HDR=YES;FMT=Delimited""",
        //        Path.GetDirectoryName(rsSumFile)
        //    );

        //    using (var conn = new OleDbConnection(connString))
        //    {
        //        conn.Open();
        //        var query = "SELECT * FROM [" + Path.GetFileName(rsSumFile) + "]";
        //        using (var adapter = new OleDbDataAdapter(query, conn))
        //        {

        //            var dt = new DataTable("CSV File");
        //            adapter.Fill(dt);

        //            //Loop through data table
        //            foreach (DataRow loRow in dt.Rows)
        //            {

        //                laSumList.Add(new Summary()
        //                {

        //                    lsDate = loRow[0].ToString(),
        //                    lsRefNum = loRow[1].ToString(),
        //                    lsTimeSpent = (Convert.ToDateTime(loRow[2])).ToString("HH:mm:ss")



        //                });

        //            }
        //        }
        //    }

        //    return laSumList;

        //}




    }


    public class Summary
    {
        public string lsDate { get; set; }
        public string lsRefNum { get; set; }
        public string lsTimeSpent { get; set; }

    }
}
