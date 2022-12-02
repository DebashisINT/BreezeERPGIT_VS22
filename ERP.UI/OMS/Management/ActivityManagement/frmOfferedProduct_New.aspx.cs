using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ActivityManagement
{
    public partial class management_activitymanagement_frmOfferedProduct_New : System.Web.UI.Page
    {
      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        protected void Page_Load(object sender, EventArgs e)
        {
            TxtProduct.Attributes.Add("onkeyup", "call_ajax(this,'getProductByLetters',event)");
            
            TxtComp.Attributes.Add("onkeyup", "call_ajax1(this,'getCompanyByLetters',event)");
            TxtComp_hidden.Text = "";
        }
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            string AddTest;
            lblError.Text = "";
            if (Request.QueryString["Type"] != null)
            {
                AddTest = Request.QueryString["Type"].ToString();
                if (AddTest == "Sales")
                {
                    if (GrdList.Rows.Count == 1)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "Sales", "<script language='JavaScript'>alert('You can only add one product')</script>");
                    }
                    else
                    {
                        ExecuteCode();
                    }
                }
            }
            else
            {
                ExecuteCode();
            }

        }
        private void ExecuteCode()
        {
            if (TxtComp.Text != "" && TxtProduct.Text != "" && TxtProduct_hidden.Text != "")
            {
                string str = "";
                DataTable dt = new DataTable();
                string Product_Id = "";
                if (ViewState["product"] != null)
                {
                    str = ViewState["product"].ToString();
                    if (str != "")
                    {
                        str += ",";
                    }
                }
                string TxtAmount1 = "";
                if (TxtAmount.Text != "")
                {
                    try
                    {
                        int amnt = int.Parse(TxtAmount.Text);
                        TxtAmount1 = TxtAmount.Text;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Amount must be numeric.";
                        lblError.Visible = true;
                        return;
                    }
                }
                else
                {
                    TxtAmount1 = "0";
                }
                string ProductType = DDLProductype.SelectedItem.Value;
                Product_Id = TxtProduct_hidden.Text;
                string Comp_ID = "";
                Comp_ID = TxtComp_hidden.Text;
                if (Comp_ID != "")
                {
                    lblError1.Text = "";
                    lblError1.Visible = false;
                    if (Product_Id != "")
                    {
                        lblError2.Text = "";
                        lblError2.Visible = false;
                        switch (ProductType)
                        {
                            case "Mutual Fund":
                                dt = oDBEngine.GetDataTable("tbl_master_products", "top 1 prds_internalId as ID", " prds_productType = 'MF' and prds_description='" + TxtProduct.Text + "'");
                                str += "Mutual Fund:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "Insurance-Life":
                                dt = oDBEngine.GetDataTable("tbl_master_products", "top 1 prds_internalId as ID", " prds_productType = 'IN' and prds_description='" + TxtProduct.Text + "'");
                                str += "Insurance-Life:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "Insurance-General":
                                dt = oDBEngine.GetDataTable("tbl_master_products", "top 1 prds_internalId as ID", " prds_productType = 'IG' and prds_description='" + TxtProduct.Text + "'");
                                str += "Insurance-General:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "HLO":
                                dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Housing Loan' and cf_pname='" + TxtProduct.Text + "'");
                                str += "Housing Loan:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "LAP":
                                dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Loan Against Property' and cf_pname='" + TxtProduct.Text + "'");
                                str += "Loan Against Property:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "PLO":
                                dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Personal Loan' and cf_pname='" + TxtProduct.Text + "'");
                                str += "Personal Loan:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "TLO":
                                dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Travel Loan' and cf_pname='" + TxtProduct.Text + "'");
                                str += "Travel Loan:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "BLO":
                                dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Business Loan' and cf_pname='" + TxtProduct.Text + "'");
                                str += "Business Loan:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "ELO":
                                dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Education Loan' and cf_pname='" + TxtProduct.Text + "'");
                                str += "Education Loan:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "ALO":
                                dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Auto Loan' and cf_pname='" + TxtProduct.Text + "'");
                                str += "Auto Loan:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "SLO":
                                dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'SME Loan' and cf_pname='" + TxtProduct.Text + "'");
                                if (dt.Rows.Count != 0)
                                    str += "SME Loan:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "LAS":
                                dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Loan Against Securities' and cf_pname='" + TxtProduct.Text + "'");
                                str += "Loan Against Securities:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            case "CRD":
                                dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Credit Cards' and cf_pname='" + TxtProduct.Text + "'");
                                str += "Credit Cards:" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                            default:
                                str += DDLProductype.SelectedItem.Text.ToString() + ":" + TxtAmount1 + ":" + Product_Id + ":" + TxtProduct.Text;
                                break;
                        }


                        ViewState["product"] = str;
                        if (ViewState["product"] != null && ViewState["product"].ToString() != "" && TxtProduct_hidden.Text != "")
                        // if (Product_Id != null && Product_Id.ToString() != "")
                        {
                            DataTable Dt1 = new DataTable();
                            DataColumn Protype = new DataColumn("Product Type");
                            DataColumn Amount = new DataColumn("Product Amount");
                            DataColumn product = new DataColumn("Product");
                            Dt1.Columns.Add(Protype);
                            Dt1.Columns.Add(Amount);
                            Dt1.Columns.Add(product);
                            string[] temp = ViewState["product"].ToString().Split(',');
                            // string[] temp = Product_Id.ToString().Split(',');
                            foreach (string chr in temp)
                            {
                                string[] temp1 = chr.Split(':');
                                DataRow Row = Dt1.NewRow();
                                if (temp1.Length == 4)
                                {
                                    Row["Product Type"] = temp1.GetValue(0);
                                    Row["Product Amount"] = temp1.GetValue(1);
                                    Row["Product"] = temp1.GetValue(3);
                                    Dt1.Rows.Add(Row);
                                }
                                else
                                {
                                    Row["Product Type"] = temp1.GetValue(0);
                                    Row["Product Amount"] = temp1.GetValue(1);
                                    Dt1.Rows.Add(Row);
                                }
                            }
                            GrdList.DataSource = Dt1.DefaultView;
                            GrdList.DataBind();
                            TxtProduct_hidden.Text = "";
                        }
                        BtnSave.Enabled = true;
                        TxtAmount.Text = "";
                        TxtProduct.Text = "";
                    }
                    else
                    {
                        lblError2.Text = "Please Select a Product From List.";
                        lblError1.Visible = true;

                    }

                }
                else
                {
                    lblError1.Text = "Please Select a Company From List.";
                    lblError1.Visible = true;

                }
            }

            else if (TxtComp.Text == "" || TxtProduct.Text == "" || TxtProduct_hidden.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "FillMessage", "<script language='JavaScript'>alert('Please enter company and product');</script>");
            }
        }
        public void SaveData()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "ForClose", "<script language='JavaScript'>parent.editwin.close();</script>");
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string Svalue = "";
            string DataValue = "";
            string columns = "";
            string ActNo = "";
            string ProductId = "";
            string CreateDate = oDBEngine.GetDate().ToString();
            string CreateUser = Session["userid"].ToString();
            if (ViewState["product"] != null)
            {
                string[] str = ViewState["product"].ToString().Split(',');
                columns = "ofp_actId,ofp_leadId,ofp_productTypeId,ofp_probableAmount,ofp_productId,ofp_activityid";
                foreach (string chr1 in str)
                {
                    string[] temp = chr1.Split(':');
                    if (Session["SalesVisitID"] != null)
                    {
                        DataValue += Session["SalesActivityId"].ToString() + ",";
                        DataValue += "'" + Session["InternalId"].ToString() + "'" + ",";
                        DataValue += "'" + temp.GetValue(0) + "'" + ",";
                        DataValue += "'" + temp.GetValue(1) + "'" + ",";
                        if (temp.Length == 4)
                        {
                            DataValue += "'" + temp.GetValue(2) + "'" + ",";
                        }
                        else
                        {
                            DataValue += "0" + ",";
                        }
                        string[,] ActNo1 = oDBEngine.GetFieldValue("tbl_trans_Activies", "act_activityNo", "act_id='" + Session["SalesActivityId"].ToString() + "'", 1);
                        if (ActNo1[0, 0] != "n")
                        {
                            ActNo = ActNo1[0, 0];
                        }
                        DataValue += "'" + ActNo + "'";
                        Svalue += chr1 + ",";
                        oDBEngine.InsurtFieldValue("tbl_trans_offeredProduct", columns, DataValue);

                    }
                    else
                    {
                        if (Session["newactivityid"] != null)
                        {
                            columns = "ofp_actId,ofp_leadId,ofp_productTypeId,ofp_probableAmount,ofp_productId,ofp_activityid";
                            DataValue += Session["newactivityid"].ToString() + ",";
                            DataValue += "'" + Session["LeadId"].ToString() + "'" + ",";
                            DataValue += "'" + temp.GetValue(0) + "'" + ",";
                            DataValue += "'" + temp.GetValue(1) + "'" + ",";
                            if (temp.Length == 4)
                            {
                                DataValue += "'" + temp.GetValue(2) + "'" + ",";
                            }
                            else
                            {
                                DataValue += "0" + ",";
                            }
                            string[,] ActNo1 = oDBEngine.GetFieldValue("tbl_trans_Activies", "act_activityNo", "act_id='" + Session["newactivityid"].ToString() + "'", 1);
                            if (ActNo1[0, 0] != "n")
                            {
                                ActNo = ActNo1[0, 0];
                            }
                            DataValue += "'" + ActNo + "'";
                            Svalue += chr1 + ",";
                            oDBEngine.InsurtFieldValue("tbl_trans_offeredProduct", columns, DataValue);

                        }
                        else
                        {
                            Svalue += chr1 + ",";
                        }
                    }
                    DataValue = "";
                }
                Session["Product"] = Svalue;
            }
            SaveData();

        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            SaveData();
        }
    }
}