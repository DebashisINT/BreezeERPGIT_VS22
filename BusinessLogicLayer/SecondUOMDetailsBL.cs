using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessLogicLayer
{
    public class SecondUOMDetailsBL
    {
        public DataTable SaveSencondUOMDetails(List<SecondUOMDetails> list, string modulename, string type,string docid)
        {
            string xmlObject = SerializeToXml(list);


            DataSet dsInst = new DataSet();
            //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            SqlCommand cmd = new SqlCommand("prc_SecondUOMDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "Add");

            cmd.Parameters.AddWithValue("@UomDetails", xmlObject);
            cmd.Parameters.AddWithValue("@Inventory_Type", type);
            cmd.Parameters.AddWithValue("@Doc_Type", modulename);
            cmd.Parameters.AddWithValue("@DocId", docid);
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();



            return null;
        }

        public List<SecondUOMDetails> GetSencondUOMDetails(string productid,string branchid, string modulename, string type,string Warehouseid,string docid)
        {
            List<SecondUOMDetails> lstSecondUOM = new List<SecondUOMDetails>();

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SecondUOMDetails");
            proc.AddVarcharPara("@Action", 100, "GetProductDetails");
            proc.AddVarcharPara("@BranchId", 100, branchid);
            proc.AddVarcharPara("@productid", 100, productid);
            proc.AddVarcharPara("@Inventory_Type",100, type);
            proc.AddVarcharPara("@Doc_Type",100, modulename);
            proc.AddVarcharPara("@Warehouseid", 100, Warehouseid);
            proc.AddVarcharPara("@DocId", 100, docid);
            ds = proc.GetTable();

            lstSecondUOM = DbHelpers.ToModelList<SecondUOMDetails>(ds);
            return lstSecondUOM;
        }
        public string SerializeToXml(object input)
        {
            XmlSerializer ser = new XmlSerializer(input.GetType());
            string result = string.Empty;

            using (MemoryStream memStm = new MemoryStream())
            {
                ser.Serialize(memStm, input);

                memStm.Position = 0;
                result = new StreamReader(memStm).ReadToEnd();
            }

            return result;
        }



    }
    public class SecondUOMDetails
    {
        public string guid { get; set; }
        public string Lenght { get; set; }
        public string width { get; set; }
        public string ProductId { get; set; }
        public string ActualLength { get; set; }
        public string UOM { get; set; }
        public string Branch { get; set; }
        public string BranchName { get; set; }
        public string WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string total { get; set; }
        public string Checked { get; set; }
    }
}
