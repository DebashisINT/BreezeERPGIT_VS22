using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class ApprovalPopup : System.Web.UI.Page
    {
        ApprovalBL appbl = new ApprovalBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Init(object sender, EventArgs e)
        {
            dsStatus.ConnectionString = Convert.ToString(Session["ErpConnection"]);
           // dsConfirm.ConnectionString = Convert.ToString(Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/ApprovalPopup.aspx");
            if (!IsPostBack)
            {
                

               
                Session["Approval_Data"] = null;

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt = oDBEngine.GetDataTable(@"SELECT [STATUS_ID], [STATUS_NAME] FROM [MASTER_APPROVAL_STATUS] WHERE ([STATUS_ID] <>'" + 1 + "')");



                ddlConfirm.DataSource = dt;
                ddlConfirm.TextField = "STATUS_NAME";
                ddlConfirm.ValueField = "STATUS_ID";
                ddlConfirm.DataBind();
                ddlConfirm.SelectedIndex = 0;






                //if (Request.QueryString["ModuleName"] == "AccountHead")
                //{
                //    dvHeader.Visible = false;
                //    Session["Approval_Data"] = appbl.GetListData("AccountHead");
                //    grid.DataBind();
                //}
                //else
                //{
                //    dvHeader.Visible = true;
                //}
                dvHeader.Visible = false;
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["Approval_Data"] != null)
            {
                grid.DataSource = (DataTable)Session["Approval_Data"];
            }
        }

        private DataTable GetGridData()
        {

            return null;
            //Prc_Approval
        }
        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            grid.JSProperties["cpStatus"] = null;
            if (e.Parameters.Split('~')[0].ToString() == "BindGrid")
            {
                if (e.Parameters.Split('~')[1].ToString() == "AccountHead")
                {
                    grid.Columns[10].Visible =false;
                    dvHeader.Visible = false;
                    Session["Approval_Data"] = appbl.GetListData("AccountHead", e.Parameters.Split('~')[2].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "Products")
                {
                    grid.Columns[10].Visible = true;
                    dvHeader.Visible = false;
                    Session["Approval_Data"] = appbl.GetListData("Products", e.Parameters.Split('~')[2].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "Customers")
                {
                    grid.Columns[10].Visible = true;
                    dvHeader.Visible = false;
                    Session["Approval_Data"] = appbl.GetListData("Customers", e.Parameters.Split('~')[2].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "Vendor")
                {
                    grid.Columns[10].Visible = true;
                    dvHeader.Visible = false;
                    Session["Approval_Data"] = appbl.GetListData("Vendor", e.Parameters.Split('~')[2].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "AccountGroup")
                {
                    grid.Columns[10].Visible = false;
                    dvHeader.Visible = false;
                    Session["Approval_Data"] = appbl.GetListData("AccountGroup", e.Parameters.Split('~')[2].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "Employees")
                {
                    grid.Columns[10].Visible = false;
                    dvHeader.Visible = false;
                    Session["Approval_Data"] = appbl.GetListData("Employees", e.Parameters.Split('~')[2].ToString());
                    grid.DataBind();
                }


                else
                {
                    dvHeader.Visible = true;
                }
            }

            else if (e.Parameters.Split('~')[0].ToString() == "SaveData")
            {
                if (e.Parameters.Split('~')[1].ToString() == "AccountHead")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        grid.Columns[10].Visible = false;
                        dvHeader.Visible = false;
                        appbl.SaveApproval(ids, "AccountHead", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListData("AccountHead", ddlStatus.Value.ToString());
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }

                    
                }

                else if (e.Parameters.Split('~')[1].ToString() == "Products")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        grid.Columns[10].Visible = true;
                        dvHeader.Visible = false;
                        appbl.SaveApproval(ids, "Products", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListData("Products", ddlStatus.Value.ToString());
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }

                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }
                }

                else if (e.Parameters.Split('~')[1].ToString() == "Customers")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }
                    if (ids != "")
                    {
                        grid.Columns[10].Visible = true;
                        dvHeader.Visible = false;
                        appbl.SaveApproval(ids, "Customers", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListData("Customers", ddlStatus.Value.ToString());
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }

                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }
                   
                }


                else if (e.Parameters.Split('~')[1].ToString() == "Vendor")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        grid.Columns[10].Visible = true;
                        dvHeader.Visible = false;
                        appbl.SaveApproval(ids, "Vendor", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListData("Vendor", ddlStatus.Value.ToString());
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }

                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }

                    
                }

                else if (e.Parameters.Split('~')[1].ToString() == "AccountGroup")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        grid.Columns[10].Visible =false;
                        dvHeader.Visible = false;
                        appbl.SaveApproval(ids, "AccountGroup", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListData("AccountGroup", ddlStatus.Value.ToString());
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }

                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }
                   
                }

                else if (e.Parameters.Split('~')[1].ToString() == "Employees")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        grid.Columns[10].Visible =false;
                        dvHeader.Visible = false;
                        appbl.SaveApproval(ids, "Employees", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListData("Employees", ddlStatus.Value.ToString());
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }

                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }
                    
                }
                else
                {
                    dvHeader.Visible = true;
                }
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dsConfirm.DataBind();
        }

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            ddlConfirm.Items.Clear();
            string status_id = e.Parameter.Split('~')[0].ToString();

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = oDBEngine.GetDataTable(@"SELECT [STATUS_ID], [STATUS_NAME] FROM [MASTER_APPROVAL_STATUS] WHERE ([STATUS_ID] <>'"+ status_id+"')");


           
            ddlConfirm.DataSource = dt;
            ddlConfirm.TextField = "STATUS_NAME";
            ddlConfirm.ValueField = "STATUS_ID";
            ddlConfirm.DataBind();
            ddlConfirm.SelectedIndex = 0;
           
        }

      


    }
}