using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.IO;
using EntityLayer.CommonELS; 
using System.Configuration;
 
using System.Data.SqlClient;
 
using System.Web.UI.WebControls; 
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
 
using ERP.OMS.ViewState_class;
using ERP.Models;
using System.Linq;
using BusinessLogicLayer;
namespace ERP.OMS.Management.Master
{
    public partial class frm_PortCode : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        //GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.GenericMethod oGenericMethod;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        PortCodeBL objPortCodeBL = new PortCodeBL();
       


        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            dsCountry.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/frm_PortCode.aspx");
            
            PortCode.JSProperties["cpEdit"] = null;
            PortCode.JSProperties["cpinsert"] = null;
            PortCode.JSProperties["cpUpdate"] = null;
            PortCode.JSProperties["cpDelete"] = null;
            PortCode.JSProperties["cpExists"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

           // BindPortCodeDtl();

            if(!IsPostBack)
            {
                Session["exportval"] = null;
            }
        }


        protected void BindPortCodeDtl()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oGenericMethod.GetDataTable("SELECT * FROM tbl_master_country order by cou_country");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                PortCode.DataSource = dtFillGrid;
                PortCode.DataBind();
            }
        }


        protected void btnSearch(object sender, EventArgs e)
        {
            PortCode.Settings.ShowFilterRow = true;
        }
        //protected void PortCode_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType == GridViewRowType.Data)
        //    {
        //        int commandColumnIndex = -1;
        //        for (int i = 0; i < PortCode.Columns.Count; i++)
        //            if (PortCode.Columns[i] is GridViewCommandColumn)
        //            {
        //                commandColumnIndex = i;
        //                break;
        //            }
        //        if (commandColumnIndex == -1)
        //            return;
        //        //____One colum has been hided so index of command column will be leass by 1 
        //        commandColumnIndex = commandColumnIndex - 1;
        //        DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
        //        for (int i = 0; i < cell.Controls.Count; i++)
        //        {
        //            DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
        //            if (button == null) return;
        //            DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

        //            if (hyperlink.Text == "Delete")
        //            {
        //                if (Session["PageAccess"].ToString().Trim() == "DelAdd" || Session["PageAccess"].ToString().Trim() == "Delete" || Session["PageAccess"].ToString().Trim() == "All")
        //                {
        //                    hyperlink.Enabled = true;
        //                    continue;
        //                }
        //                else
        //                {
        //                    hyperlink.Enabled = false;
        //                    continue;
        //                }
        //            }


        //        }

        //    }

        //}
        protected void PortCode_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!PortCode.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = PortCode.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void PortCode_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            bool result = false;
            int userid = 0;
            int deletecont = 0;
            int updtcnt = 0;
            int insertcount = 0;
            if(Session["userid"]!=null)
            {
                userid=Convert.ToInt32(Session["userid"]);
            }
            PortCode.JSProperties["cpEdit"] = null;
            PortCode.JSProperties["cpinsert"] = null;
            PortCode.JSProperties["cpUpdate"] = null;
            PortCode.JSProperties["cpDelete"] = null;
            PortCode.JSProperties["cpExists"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                PortCode.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                PortCode.FilterExpression = string.Empty;
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = objPortCodeBL.GetPortCodeById(Convert.ToInt32(WhichType ));
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string portid = string.Empty;
                    string portcode = string.Empty;
                    string portdesc = string.Empty;
                    string StateId = string.Empty;
                    string StateName = string.Empty;
                    string CountryId = string.Empty;

                     portid = Convert.ToString(dtEdit.Rows[0]["Port_Id"]);
                     portcode = Convert.ToString(dtEdit.Rows[0]["Port_Code"]);
                     portdesc = Convert.ToString(dtEdit.Rows[0]["Port_Description"]);
                     StateId = Convert.ToString(dtEdit.Rows[0]["Port_StateId"]);
                     StateName = Convert.ToString(dtEdit.Rows[0]["StateName"]);
                     CountryId = Convert.ToString(dtEdit.Rows[0]["Port_CountryId"]);

                     PortCode.JSProperties["cpEdit"] = portcode + "~" + portdesc + "~" + WhichType + "~" + StateId + "~" + StateName + "~" + CountryId;

                }
            }

            if (WhichCall == "updatePortCode")
            {
                //updtcnt = oGenericMethod.Update_Table("tbl_master_country", "cou_country='" + txtCountryName.Text + "',Country_NSECode='" + txtNseCode.Text + "',Country_BSECode='" + txtBseCode.Text + "',Country_MCXCode='" + txtMcxCode.Text + "',Country_MCXSXCode='" + txtMcsxCode.Text + "',Country_NCDEXCode='" + txtNcdexCode.Text + "',Country_CDSLID=case when '" + txtCdslCode.Text + "'='' then null else '" + txtCdslCode.Text + "' end,Country_NSDLID=case when '" + txtNsdlCode.Text + "'='' then null else '" + txtNsdlCode.Text + "' end,Country_NdmlID=case when '" + txtNdmlCode.Text + "'='' then null else '" + txtNdmlCode.Text + "' end,Country_DotExID=case when '" + txtDotexidCode.Text + "'='' then null else '" + txtDotexidCode.Text + "' end,Country_CvlID=case when '" + txtCvlidCode.Text + "'='' then null else '" + txtCvlidCode.Text + "' end", "cou_id=" + WhichType + "");
                result = objPortCodeBL.updatePortCode(Convert.ToInt32(WhichType), txt_PorctDesc.Text.Trim(), Convert.ToString(txt_Country.Value), hdStateId.Value, userid);
                if (result)
                {
                    PortCode.JSProperties["cpUpdate"] = "Success";
                    //BindPortCodeDtl();
                }
                else
                    PortCode.JSProperties["cpUpdate"] = "fail";

            }
            if (WhichCall == "savePortCode")
            {

                result = objPortCodeBL.insertPortCode(txt_PorctCode.Text.Trim(), txt_PorctDesc.Text.Trim(),Convert.ToString(txt_Country.Value), hdStateId.Value, userid);
                if (result)
                {
                    PortCode.JSProperties["cpinsert"] = "Success";
                    //BindPortCodeDtl();
                }
                else
                {
                    PortCode.JSProperties["cpinsert"] = "fail";
                }
            }

            if (WhichCall == "Delete")
            {
                MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
                int portid = Convert.ToInt32(WhichType);
                int i = objPortCodeBL.deletePortCode(portid);
                if (i == 1)
                {
                    PortCode.JSProperties["cpDelete"] = "Successfully Deleted";
                }
                else
                {
                    PortCode.JSProperties["cpDelete"] = "Used in other modules. Cannot Delete.";

                }
                 
            }
        } 
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            } 
        }

        public void bindexport(int Filter)
        {
            PortCode.Columns[2].Visible = false;

            string filename = "PortCode";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "PortCode";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
            if (stream.Length > 0)
                Page.Response.BinaryWrite(stream.ToArray());
            //Page.Response.End();
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Port_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //string connectionString = ConfigurationManager.ConnectionStrings["GECORRECTIONConnectionString"].ConnectionString;

            //string IsFilter = Convert.ToString(hfIsFilter.Value);
            //string strFromDate = Convert.ToString(hfFromDate.Value);
            //string strToDate = Convert.ToString(hfToDate.Value);
            //string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            //List<int> branchidlist;

            //if (IsFilter == "Y")
            //{
            //    if (strBranchID == "0")
            //    {
                    //string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    //branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.tbl_master_PortCodes 
                            orderby d.Port_Code
                            select d;

                    e.QueryableSource = q;
                    //var cnt = q.Count();
            //    }
            //    else
            //    {
            //        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

            //        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            //        var q = from d in dc.v_PBLists
            //                where
            //                d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate) &&
            //                branchidlist.Contains(Convert.ToInt32(d.branchid)) && d.invoicefor == "DV"
            //                orderby d.Invoice_Id descending
            //                select d;
            //        e.QueryableSource = q;
            //    }
            //}
            //else
            //{
            //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            //    var q = from d in dc.v_PBLists
            //            where d.branchid == 0
            //            orderby d.Invoice_Id descending
            //            select d;
            //    e.QueryableSource = q;
            //}
        }
       
        [WebMethod]
        public static bool CheckUniqueName(string portcode)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (portcode != "" && Convert.ToString(portcode).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(portcode).Trim(), "0", "PortCode_Check");
            }
            return status;
        }
    }
}
