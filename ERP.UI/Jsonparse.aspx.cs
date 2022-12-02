using DevExpress.Export;
using DevExpress.XtraPrinting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP
{
    public partial class Jsonparse : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Session["gridGstR1"] = null;
            }
        }

        public void LoadJson()
        {
          ///  using (StreamReader r = new StreamReader(Server.MapPath("returns_20102017_R2_19AABCG1741J1ZN.json")))
         
            using (StreamReader r = new StreamReader(Server.MapPath("returns_23102017_R2_19AACFB7296D1ZC.json")))
            {
                string json = r.ReadToEnd();

             //   List<DataJson> UserList = JsonConvert.DeserializeObject<DataJson>(json);

               /// dynamic array = JsonConvert.DeserializeObject(json);
               /// 


                JavaScriptSerializer ser = new JavaScriptSerializer();

                List<DataJsonCollection> movieInfos = ser.Deserialize<List<DataJsonCollection>>(json);

              DataTable dt = new DataTable();
              dt = ConvertJSONToDataTable(json);

           // dt = GetDataTableFromJsonString(json);
               
                Session["gridGstR1"] = dt;
                ShowGrid.DataSource = dt;
                ShowGrid.DataBind();
            }

        }

        public  class DataJsonCollection
        {
            public string gstin { get; set; }
            public string fp { get; set; }

            public List<b2b> b2b_get { get; set; }
        }


        public class b2b
        {

            public string ctin { get; set; }
        }

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {


                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
            }

        }
        public void bindexport(int Filter)
        {
            string filename = "JJJSONN";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();

                    break;
                case 2:
                    exporter.WriteXlsxToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }

        }

        private DataTable ConvertJSONToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            //strip out bad characters
            string[] jsonParts = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");

            //hold column names
            List<string> dtColumns = new List<string>();

            //get columns
            foreach (string jp in jsonParts)
            {
                //only loop thru once to get column names
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",");
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1);
                        string v = rowData.Substring(idx + 1);
                        if (!dtColumns.Contains(n))
                        {
                            dtColumns.Add(n.Replace("\"", ""));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", rowData));
                    }

                }
                break; // TODO: might not be correct. Was : Exit For
            }

            //build dt
            foreach (string c in dtColumns)
            {
                dt.Columns.Add(c);
            }
            //get table data
            foreach (string jp in jsonParts)
            {
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string v = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[n] = v;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
                dt.Rows.Add(nr);
            }
            return dt;
        }




        public DataTable GetDataTableFromJsonString(string json)
        {
            var jsonLinq = JObject.Parse(json);

            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }


        protected void Button_Click(object sender, EventArgs e)
        {
            LoadJson();
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["gridGstR1"] != null)
            {
                ShowGrid.DataSource = (DataTable)Session["gridGstR1"];

            }

        }

    }
}