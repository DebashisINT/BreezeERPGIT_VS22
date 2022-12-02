using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_ShowDocument : ERP.OMS.ViewState_class.VSPage, System.Web.UI.ICallbackEventHandler
    {

        string data;
        string BranchId = null;
        string Clients;
        string Group = null;

        clsDropDownList clsdropdown = new clsDropDownList();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();

        BusinessLogicLayer.Documents ODocuments = new BusinessLogicLayer.Documents();

        protected void Page_Load(object sender, EventArgs e)
        {


            //Procedure();

            if (!IsPostBack)
            {
                Session["docid"] = "";
                if (cmbDate.SelectedItem.Value == "Renewal")
                {
                    dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                    dtFrom.Value = oDBEngine.GetDate();
                    dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                    dtTo.Value = oDBEngine.GetDate().AddDays(7);

                }
                else
                {
                    dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                    dtFrom.Value = oDBEngine.GetDate().AddDays(-7);
                    dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                    dtTo.Value = oDBEngine.GetDate();
                }

                fillDropDownlist();

            }
            if (Session["docid"].ToString().Contains("$"))
            {
                Procedure();
            }
            else
            {
                //if (HiddenField_EntityChange.Value != "T")//This indicate That No Need To Bind Grid Again.When Entry DropDown Change
                BindGrid();
                //else
                //    HiddenField_EntityChange.Value = null;

            }
            Session["docid"] = "";
            Page.ClientScript.RegisterStartupScript(GetType(), "CallingHeight", "<script language='JavaScript'>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script language='JavaScript'>Page_Load();</script>");

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    str = "'" + val[0] + "'";
                    str1 = val[0] + ";" + val[1];
                }
                else
                {
                    str += ",'" + val[0] + "'";
                    str1 += "," + val[0] + ";" + val[1];
                }
            }

            if (idlist[0] == "Clients")
            {
                Clients = str;
                data = "Clients~" + str;
                ViewState["Clients"] = Clients;
            }

            else if (idlist[0] == "Group")
            {
                Group = str;
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                BranchId = str;
                data = "Branch~" + str;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                BindGroup();
            }
        }

        protected void btnDt_Click(object sender, EventArgs e)
        {
            if (cmbDate.SelectedItem.Value == "Renewal")
            {
                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtFrom.Value = oDBEngine.GetDate();
                dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.Value = oDBEngine.GetDate().AddDays(7);

            }
            else
            {
                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtFrom.Value = oDBEngine.GetDate().AddDays(-7);
                dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.Value = oDBEngine.GetDate();
            }

        }

        protected void btnEntity_Click(object sender, EventArgs e)
        {
            fillDropDownlist();
        }

        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }

        }

        protected void fillDropDownlist()
        {
            drpDocumentType.Items.Clear();
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            string[,] Data = objEngine.GetFieldValue(" tbl_master_documentType", " dty_id,dty_documentType ", " dty_applicableFor='" + drpDocumentEntity.SelectedValue.ToString() + "'", 2, "dty_documentType");
            if (Data[0, 0] != "n")
            {
                clsdropdown.AddDataToDropDownList(Data, drpDocumentType);
                drpDocumentType.Items.Insert(0, new ListItem("--ALL--", "0"));
            }
            else
            {
                drpDocumentType.Items.Insert(0, new ListItem("--ALL--", "0"));
            }


        }



        //protected void drpDocumentEntity_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fillDropDownlist();
        //}



        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            Procedure();
        }


        protected void Procedure()
        {
            DataTable DT = new DataTable();

            string Branch = Session["userbranchHierarchy"].ToString();
            string Group = "Group";
            string Client = "";
            string GroupType = "N";
            String DocType = "";
            string DateRange = "N";
            string param = "";
            if (RadDateRangeS.Checked == true)
                DateRange = "Y";

            if (drpDocumentType.SelectedItem.Value == "0")
                DocType = "A";
            else
                DocType = drpDocumentType.SelectedItem.Value;


            if (ddlGroup.SelectedItem.Value == "0")
            {
                if (rdbranchAll.Checked == true)
                {
                    Branch = Session["userbranchHierarchy"].ToString();
                }
                else
                {
                    if (HdnBranchId.Value.ToString() != "")
                    {
                        Branch = HdnBranchId.Value;
                    }
                    else
                    {
                        Branch = Session["userbranchHierarchy"].ToString();
                    }
                }
            }
            else
            {
                if (rdddlgrouptypeAll.Checked == true)
                {
                    if (ddlgrouptype.SelectedItem.Value != "0")
                    {
                        GroupType = "Y";
                        Group = ddlgrouptype.SelectedItem.Value;
                    }
                    else
                    {
                        Group = "Group";
                    }
                }
                else
                {
                    if (HdnGroup.Value.ToString() != "")
                    {
                        Group = HdnGroup.Value;
                    }
                    else
                    {
                        Group = "Group";
                    }
                }

            }

            if (rdbALLSCL.Checked == true)
            {
                Client = "A";
            }
            else if (rdbSCL.Checked == true)
            {
                if (HdnClients.Value.ToString() != "")
                {
                    Client = HdnClients.Value;
                }
                else
                {
                    Client = "A";
                }
            }
            if (drpDocumentEntity.SelectedItem.Value == "Accounts")
            {

                if (rdmainaccount.Checked == true)
                {
                    param = "main";
                }

                else if (rdcustom.Checked == true)
                {
                    param = "custom";
                }
                //else if (rdother.Checked == true)
                //{
                //    param = "oth";
                //}
            }
            else
            {
                param = "";
            }
            DataSet ds = new DataSet();

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlDataAdapter da = new SqlDataAdapter("Fetch_DocumentMaster", con))
            //    {
            //       string xx =drpDocumentEntity.SelectedItem.Value.ToString(),DocType,Branch,Group,GroupType,Client,
            //           DateRange,cmbDate.SelectedItem.Value.ToString(),dtFrom.Value,dtTo.Value,param


            //        da.SelectCommand.Parameters.AddWithValue("@DocumentFor", drpDocumentEntity.SelectedItem.Value);
            //        da.SelectCommand.Parameters.AddWithValue("@DocumentType", DocType);
            //        da.SelectCommand.Parameters.AddWithValue("@Branch", Branch);
            //        da.SelectCommand.Parameters.AddWithValue("@Group", Group);
            //        da.SelectCommand.Parameters.AddWithValue("@GroupType", GroupType);
            //        da.SelectCommand.Parameters.AddWithValue("@Client", Client);

            //        da.SelectCommand.Parameters.AddWithValue("@DateRange", DateRange);
            //        da.SelectCommand.Parameters.AddWithValue("@DateRangeType", cmbDate.SelectedItem.Value);
            //        da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
            //        da.SelectCommand.Parameters.AddWithValue("@ToDate", dtTo.Value);
            //        da.SelectCommand.Parameters.AddWithValue("@param", param);
            //        da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //        da.SelectCommand.CommandTimeout = 0;

            //        if (con.State == ConnectionState.Closed)
            //            con.Open();
            //        ds.Reset();
            //        da.Fill(ds);



            ds = ODocuments.Get_DocumentMaster(drpDocumentEntity.SelectedItem.Value.ToString(), DocType, Branch, Group, GroupType, Client,
                     DateRange, cmbDate.SelectedItem.Value.ToString(), dtFrom.Value.ToString(), dtTo.Value.ToString(), param);

            if (ds != null)
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Height4", "alert('No Record Found!..');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Height4", "alert('No Record Found!..');", true);


            ViewState["DatasetMain"] = ds;
            //Session["ds"] = ViewState["DatasetMain"];
            BindGrid();

            //    }
            //}




        }
        protected void BindGrid()
        {
            bool IsFilterHide = false;
            if (ViewState["DatasetMain"] != null)
            {
                DataSet dsNew = (DataSet)ViewState["DatasetMain"];
                if (dsNew != null)
                    if (dsNew.Tables.Count > 0)
                    {
                        if (dsNew.Tables[0].Rows.Count > 0)
                        {
                            gridStatus.DataSource = dsNew.Tables[0];
                            gridStatus.DataBind();
                            IsFilterHide = true;
                        }
                        else
                        {
                            gridStatus.DataSource = null;
                            gridStatus.DataBind();
                            IsFilterHide = false;
                        }
                    }
                    else
                    {
                        gridStatus.DataSource = null;
                        gridStatus.DataBind();
                        IsFilterHide = false;
                    }
                dsNew.Dispose();

                if (IsFilterHide)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Height", "height();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "HideShowFilterMsg", "HideShowFilter('H');", true);
                }
            }
        }

        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString() == "s")
            {
                gridStatus.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters.ToString() == "All")
            {
                gridStatus.FilterExpression = string.Empty;
            }
            BindGrid();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Procedure();
            exporter.WriteXlsToResponse();
        }
        protected void gridStatus_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            int rowindex = e.VisibleIndex;
            string Verify = gridStatus.GetRowValues(rowindex, "vrfy").ToString();
            string ContactID = e.GetValue("doc_source").ToString();
            string Rowid = e.GetValue("doc_id").ToString();
            if (Verify != "Not Verified")
            {
                DataTable dt = oDBEngine.GetDataTable("select doc_verifyremarks from tbl_master_document where doc_id=" + Rowid + "");
                string tooltip = dt.Rows[0][0].ToString();
                e.Row.Cells[0].Style.Add("cursor", "hand");
                e.Row.Cells[0].ToolTip = "View Document!";
                e.Row.Cells[0].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[1].Style.Add("cursor", "hand");
                e.Row.Cells[1].ToolTip = "View Document!";
                e.Row.Cells[1].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[2].Style.Add("cursor", "hand");
                e.Row.Cells[2].ToolTip = "View Document!";
                e.Row.Cells[2].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[3].Style.Add("cursor", "hand");
                e.Row.Cells[3].ToolTip = "View Document!";
                e.Row.Cells[3].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[4].Style.Add("cursor", "hand");
                e.Row.Cells[4].ToolTip = "View Document!";
                e.Row.Cells[4].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[5].Style.Add("cursor", "hand");
                e.Row.Cells[5].ToolTip = "View Document!";
                e.Row.Cells[5].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[6].Style.Add("cursor", "hand");
                e.Row.Cells[6].ToolTip = "View Document!";
                e.Row.Cells[6].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[7].Style.Add("cursor", "hand");
                e.Row.Cells[7].ToolTip = "View Document!";
                e.Row.Cells[7].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[8].Style.Add("cursor", "hand");
                e.Row.Cells[8].ToolTip = "View Document!";
                e.Row.Cells[8].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[9].Style.Add("cursor", "hand");
                e.Row.Cells[9].ToolTip = "View Document!";
                e.Row.Cells[9].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[10].Style.Add("cursor", "hand");
                e.Row.Cells[10].ToolTip = "View Document!";
                e.Row.Cells[10].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");


                e.Row.Cells[11].Style.Add("cursor", "hand");
                e.Row.Cells[11].ToolTip = tooltip.ToString();
            }
            if (Verify == "Not Verified")
            {
                DataTable dt = oDBEngine.GetDataTable("select doc_verifyremarks from tbl_master_document where doc_id=" + Rowid + "");
                string tooltip = dt.Rows[0][0].ToString();
                e.Row.Cells[0].Style.Add("cursor", "hand");
                e.Row.Cells[0].ToolTip = "View Document!";
                e.Row.Cells[0].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[1].Style.Add("cursor", "hand");
                e.Row.Cells[1].ToolTip = "View Document!";
                e.Row.Cells[1].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[2].Style.Add("cursor", "hand");
                e.Row.Cells[2].ToolTip = "View Document!";
                e.Row.Cells[2].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[3].Style.Add("cursor", "hand");
                e.Row.Cells[3].ToolTip = "View Document!";
                e.Row.Cells[3].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[4].Style.Add("cursor", "hand");
                e.Row.Cells[4].ToolTip = "View Document!";
                e.Row.Cells[4].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[5].Style.Add("cursor", "hand");
                e.Row.Cells[5].ToolTip = "View Document!";
                e.Row.Cells[5].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[6].Style.Add("cursor", "hand");
                e.Row.Cells[6].ToolTip = "View Document!";
                e.Row.Cells[6].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[7].Style.Add("cursor", "hand");
                e.Row.Cells[7].ToolTip = "View Document!";
                e.Row.Cells[7].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[8].Style.Add("cursor", "hand");
                e.Row.Cells[8].ToolTip = "View Document!";
                e.Row.Cells[8].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[9].Style.Add("cursor", "hand");
                e.Row.Cells[9].ToolTip = "View Document!";
                e.Row.Cells[9].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[10].Style.Add("cursor", "hand");
                e.Row.Cells[10].ToolTip = "View Document!";
                e.Row.Cells[10].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[11].Style.Add("cursor", "hand");
                e.Row.Cells[11].ToolTip = "Click here to Verify !";
                e.Row.Cells[11].Attributes.Add("onclick", "javascript:Changestatus('" + Rowid + "');");
                e.Row.Cells[11].Style.Add("color", "Red");
            }
            //if (Verify.Contains("["))
            //{
            //    DataTable dt = oDBEngine.GetDataTable("select doc_verifyremarks from tbl_master_document where doc_id=" + Rowid + "");
            //    string tooltip = dt.Rows[0][0].ToString();

            //    e.Row.Cells[7].Style.Add("cursor", "hand");
            //    e.Row.Cells[7].ToolTip = tooltip.ToString();
            //    //e.Row.Cells[7].Attributes.Add("onclick", "javascript:Changestatus('" + Rowid + "');");
            //}
        }
    }
}