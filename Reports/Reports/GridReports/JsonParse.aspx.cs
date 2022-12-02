using DevExpress.Web;
using EntityLayer.CommonELS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;


namespace Reports.Reports.GridReports
{
    public partial class JsonParse : System.Web.UI.Page
    {

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/JsonParse.aspx");

            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "GSTR-2 Reconciliation [With JSON]";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                Session["gridJson"] = null;
            }
        }

        #region    ******************   Classes Properties          ******************
        public class DataJsonCollection
        {
            public string gstin { get; set; }
            public string fp { get; set; }

            public List<b2bList> b2b { get; set; }
        }

        public class b2bList
        {

            public string ctin { get; set; }
            public string cfs { get; set; }
            public List<Invlist> inv { get; set; }

        }

        public class Invlist
        {

            public List<ItemsList> itms { get; set; }
            public string val { get; set; }
            public string inv_typ { get; set; }

            public string flag { get; set; }
            public string pos { get; set; }
            public string updby { get; set; }
            public string idt { get; set; }
            public string rchrg { get; set; }
            public string inum { get; set; }
            public string cflag { get; set; }


        }

        public class ItemsList
        {
            public string num { get; set; }
            public Itcclass itc { get; set; }

            public ItemDetails itm_det { get; set; }
        }

        public class Itcclass
        {

            public string tx_cs { get; set; }
            public string elg { get; set; }
            public string tx_i { get; set; }
            public string tx_c { get; set; }
            public string tx_s { get; set; }


        }

        public class ItemDetails
        {

            public string samt { get; set; }
            public string csamt { get; set; }
            public string rt { get; set; }
            public string txval { get; set; }
            public string camt { get; set; }
            public string iamt { get; set; }

        }

        public class InsertedParameters
        {

            public string ctin { get; set; }
            public string tx_i { get; set; }
            public string tx_c { get; set; }
            public string tx_s { get; set; }
            public string rchrg { get; set; }

            public string cflag { get; set; }


            public string samt { get; set; }
            public string csamt { get; set; }
            public string rt { get; set; }
            public string txval { get; set; }
            public string camt { get; set; }
            public string iamt { get; set; }

            public string val { get; set; }
            public string inv_typ { get; set; }


            public string idt { get; set; }

            public string inum { get; set; }



        }


        #endregion
     

        protected void Button_Click(object sender, EventArgs e)
        {
            drdExport.SelectedValue = "0";
            if (fileuploadjson.PostedFile != null && fileuploadjson.PostedFile.ContentLength > 0)
            {
                string foldername = "JsonFiles";
                string fileName = Path.GetFileName(fileuploadjson.PostedFile.FileName);
               
                Guid id = Guid.NewGuid();
                fileName = fileName + HttpContext.Current.Session.SessionID + id;
                string folder = Server.MapPath("~/CommonFolder/JsonFiles/");
                //Directory.CreateDirectory(folder);
                fileuploadjson.PostedFile.SaveAs(Path.Combine(folder, fileName));
                try
                {
                    LoadJson(foldername, fileName);
                }
                catch
                {

                }
            }


        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["gridJson"] != null)
            {

                ShowGrid.DataSource = (DataTable)Session["gridJson"];

            }

        }

        public void LoadJson(string folder, string fileName)
        {
            ///  using (StreamReader r = new StreamReader(Server.MapPath("returns_20102017_R2_19AABCG1741J1ZN.json")))

            using (StreamReader r = new StreamReader(Server.MapPath("~/CommonFolder/" + folder + "/" + fileName)))
            {
                string json = r.ReadToEnd().ToString();


                //   List<DataJson> UserList = JsonConvert.DeserializeObject<DataJson>(json);

                /// dynamic array = JsonConvert.DeserializeObject(json);
                /// 


                JavaScriptSerializer ser = new JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                DataJsonCollection movieInfos = ser.Deserialize<DataJsonCollection>(json);


                // DataTable dt = new DataTable();
                // dt = ConvertJSONToDataTable(json);
                // dt = GetDataTableFromJsonString(json);
                ///Session["gridGstR1"] = dt;

                InsertedParameters omedl = new InsertedParameters();
                List<InsertedParameters> omedl2 = new List<InsertedParameters>();
                foreach (var s in movieInfos.b2b)
                {
                    omedl.ctin = s.ctin;
                    foreach (var s1 in s.inv)
                    {

                        omedl.val = s1.val;
                        omedl.inv_typ = s1.inv_typ;
                        omedl.idt = s1.idt;
                        omedl.inum = s1.inum;
                        omedl.rchrg = s1.rchrg;
                        omedl.cflag = s1.cflag;



                        foreach (var s2 in s1.itms)
                        {
                            //Rev Rajdip For commeting ITC check as it is not applicable in GSTR2 reconciliation as discussed with Bhaskar da and Pijush da
                            //if (s2.itc != null)
                            //{
                            //    omedl.tx_c = s2.itc.tx_c;
                            //    omedl.tx_i = s2.itc.tx_i;
                            //    omedl.tx_s = s2.itc.tx_s;
                            //}
                            if (s2.itm_det != null)
                            {
                                omedl.tx_c = s2.itm_det.camt;
                                omedl.tx_i = s2.itm_det.iamt;
                                omedl.tx_s = s2.itm_det.samt;
                            }


                            omedl.samt = s2.itm_det.samt;

                            omedl.rt = s2.itm_det.rt;
                            omedl.txval = s2.itm_det.txval;
                            omedl.camt = s2.itm_det.camt;
                            omedl.iamt = s2.itm_det.iamt;


                            omedl2.Add(new InsertedParameters()
                            {
                                val = omedl.val,
                                inv_typ = omedl.inv_typ,
                                idt = omedl.idt,
                                inum = omedl.inum,
                                rchrg = omedl.rchrg,
                                cflag = omedl.cflag,

                                tx_c = omedl.tx_c,
                                tx_i = omedl.tx_i,
                                tx_s = omedl.tx_s,
                                samt = omedl.samt,
                                ctin = omedl.ctin,
                                rt = omedl.rt,
                                txval = omedl.txval,
                                camt = omedl.camt,
                                iamt = omedl.iamt,
                            });

                        }



                    }


                }


                string JsonXML = ConvertToXml(omedl2, 0);

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Proc_JSON_Reconsile", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@JsonXML", JsonXML);


                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);


                cmd.Dispose();
                con.Dispose();


                Session["gridJson"] = ds.Tables[0];
                ShowGrid.DataSource = ds.Tables[0];
                ShowGrid.DataBind();

                // object s3 = omedl2;

            }
            System.IO.File.Delete(Server.MapPath("~/CommonFolder/" + folder + "/" + fileName));


        }


        #region ******************************************** Xml Conversion  ********************************************
        public static string ConvertToXml<T>(List<T> table, int metaIndex = 0)
        {
            XmlDocument ChoiceXML = new XmlDocument();
            ChoiceXML.AppendChild(ChoiceXML.CreateElement("root"));
            Type temp = typeof(T);

            foreach (var item in table)
            {
                XmlElement element = ChoiceXML.CreateElement("data");

                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    element.AppendChild(ChoiceXML.CreateElement(pro.Name)).InnerText = Convert.ToString(item.GetType().GetProperty(pro.Name).GetValue(item, null));
                }
                ChoiceXML.DocumentElement.AppendChild(element);
            }

            return ChoiceXML.InnerXml.ToString();
        }


        #endregion


        #region  Extra  Reuseble funcions
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

        #endregion


        #region  ****************** Export  Functionality *********************

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {

               
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                
            }

        }
        public void bindexport(int Filter)
        {
            string filename = "GSTR-2 Reconciliation(JSON)";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
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
        #endregion


        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

          
                e.Text = string.Format("{0}", e.Value);


        }

    }
}