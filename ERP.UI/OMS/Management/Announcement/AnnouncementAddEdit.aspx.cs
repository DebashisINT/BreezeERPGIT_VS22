using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Announcement
{
    public partial class AnnouncementAddEdit : System.Web.UI.Page
    {
        MasterSettings masterbl = new MasterSettings();

        protected void Page_Init(object sender, EventArgs e)
        {
            userDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string mastersettings = masterbl.GetSettings("isServiceManagementRequred");
            hdnServicemanagement.Value = mastersettings;
            if (mastersettings == "0")
            {
                DivServiceManagement.Style.Add("display", "none");
            }
            else
            {
                DivServiceManagement.Style.Add("display", "!inline-block");
            }
            //Rev for STB Managemnt Tanmoy
            string STBManagementMasterSettings = masterbl.GetSettings("IsSTBManagementRequired");
            hdnSTBManagementMasterSettings.Value = STBManagementMasterSettings;
            if (STBManagementMasterSettings == "0")
            {
                DivSTBManagement.Style.Add("display", "none");
            }
            else
            {
                DivSTBManagement.Style.Add("display", "!inline-block");
            }
            //Rev for STB Managemnt Tanmoy
            if (!IsPostBack)
            {
                FromDate.Date = DateTime.Now;
                ToDate.Date = DateTime.Now;

                if (Request.QueryString["id"] != "Add")
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
                    proc.AddPara("@Action", "Fetch");
                    proc.AddPara("@id", Request.QueryString["id"]);
                    DataSet Ds = proc.GetDataSet();

                    txtTitle.Text = Convert.ToString(Ds.Tables[0].Rows[0]["title"]);
                    onedit.Value = Convert.ToString(Ds.Tables[0].Rows[0]["anninHtml"]);
                    FromDate.Date = Convert.ToDateTime(Ds.Tables[0].Rows[0]["FromDate"]);
                    ToDate.Date = Convert.ToDateTime(Ds.Tables[0].Rows[0]["ToDate"]);
                    iscomment.Checked = Convert.ToBoolean(Ds.Tables[0].Rows[0]["AllowComment"]);
                    //For Service management check box Tanmoy Start
                    isServiceManagement.Checked = Convert.ToBoolean(Ds.Tables[0].Rows[0]["AllowServiceManagement"]);
                    //For Service management check box Tanmoy End

                    //For STB management check box Tanmoy Start
                    isSTBManagement.Checked = Convert.ToBoolean(Ds.Tables[0].Rows[0]["AllowSTBManagement"]);
                    //For STB management check box Tanmoy End

                    userLookUp.DataBind();

                    //For Service management check box Tanmoy Start
                    if (Ds.Tables[0].Rows[0]["AllowServiceManagement"].ToString() == "False")
                    {
                        foreach (DataRow dr in Ds.Tables[1].Rows)
                        {
                            userLookUp.GridView.Selection.SelectRowByKey(Convert.ToString(dr["Userid"]));
                        }
                    }
                    else if (hdnServicemanagement.Value == "0")
                    {
                        foreach (DataRow dr in Ds.Tables[1].Rows)
                        {
                            userLookUp.GridView.Selection.SelectRowByKey(Convert.ToString(dr["Userid"]));
                        }
                    }
                    else
                    {
                        userLookUp.ClientEnabled = false;
                        iscomment.ClientEnabled = false;
                        lblmsg.InnerText = " (All users are Selected.)";
                    }
                    //For Service management check box Tanmoy

                    //For STB management check box Tanmoy Start
                    if (Ds.Tables[0].Rows[0]["AllowSTBManagement"].ToString() == "False")
                    {
                        foreach (DataRow dr in Ds.Tables[1].Rows)
                        {
                            userLookUp.GridView.Selection.SelectRowByKey(Convert.ToString(dr["Userid"]));
                        }
                    }
                    else if (hdnSTBManagementMasterSettings.Value == "0")
                    {
                        foreach (DataRow dr in Ds.Tables[1].Rows)
                        {
                            userLookUp.GridView.Selection.SelectRowByKey(Convert.ToString(dr["Userid"]));
                        }
                    }
                    else
                    {
                        userLookUp.ClientEnabled = false;
                        iscomment.ClientEnabled = false;
                        lblmsg.InnerText = " (All users are Selected.)";
                    }
                    //For STB management check box Tanmoy
                }
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            string userList = "";
            foreach (var usr in userLookUp.GridView.GetSelectedFieldValues("user_id"))
            {
                userList = userList + usr + ',';
            }
            

            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "AddEdit");
            proc.AddPara("@Tittle", txtTitle.Text);
            proc.AddPara("@Body", hdssText.Value);
            proc.AddPara("@html", hdss.Value);
            proc.AddPara("@UserList", userList);
            proc.AddPara("@id", Request.QueryString["id"]);
            proc.AddPara("@FromDate", FromDate.Date);
            proc.AddPara("@Todate", ToDate.Date);
            proc.AddPara("@userid", Convert.ToInt32(Session["userid"]));
            proc.AddPara("@isComment", iscomment.Checked);
            proc.AddPara("@isServiceManagement", isServiceManagement.Checked);
            proc.AddPara("@isSTBManagement", isSTBManagement.Checked);
            proc.RunActionQuery();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ReturnAftermsg()", true);

        }

        protected void isServiceManagement_CheckedChanged(object sender, EventArgs e)
        {
            if (isServiceManagement.Checked == true)
            {
                userLookUp.Style.Add("display", "enable");
            }
            else
            {
                userLookUp.Style.Add("display", "Disable");
            }
        }
    }
}