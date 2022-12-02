using BusinessLogicLayer;
using BusinessLogicLayer.MenuBLS;
using BusinessLogicLayer.UserGroupsBLS;
using EntityLayer.CommonELS;
using EntityLayer.MenuHelperELS;
using EntityLayer.UserGroupsEL;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class frmAddUserGroup : ERP.OMS.ViewState_class.VSPage//System.Web.UI.Page
    {
        UserGroupBL userGroupBL = new UserGroupBL();
        MenuBL menuBl = new MenuBL();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        DBEngine oDBEngine = new DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();


        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/root_UserGroups.aspx");
            hdnMessage.Value = "";
            if (!IsPostBack)
            {
                BusinessLogicLayer.CommonBLS.CommonBL.CreateUserRightSession("/management/master/root_UserGroups.aspx");

                // tblCreateModifyForms.Visible = false;
                if (Convert.ToString(Session["GroupId"]) != null && Convert.ToString(Session["GroupId"]) != "")
                {
                    txtGroupName.Text = Convert.ToString(Session["GroupName"]);

                    GetSetGroupAccessValues(Convert.ToInt32(Session["GroupId"]));
                }
                GenerateMenus();

                if (Session["UserGroupUpdateMessage"] != null)
                {
                    hdnMessage.Value = Convert.ToString(Session["UserGroupUpdateMessage"]);
                    Session["UserGroupUpdateMessage"] = null;
                }
            }
        }




        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int? userId = null;

                if (Session["userid"] != null)
                {
                    try
                    {
                        userId = Convert.ToInt32(Session["userid"]);
                    }
                    catch
                    {
                        userId = null;
                    }
                }

                UserGroupSaveModel saveModel = new UserGroupSaveModel();
                saveModel.grp_name = txtGroupName.Text.Trim();
                saveModel.grp_segmentId = 1;
                saveModel.UserGroupRights = GroupUserRights.Value.Trim();
                saveModel.CreateUser = userId;
                saveModel.LastModifyUser = userId;

                if (Session["GroupId"] != null)
                {
                    try
                    {
                        saveModel.grp_id = Convert.ToInt32(Session["GroupId"]);
                        saveModel.mode = PROC_USP_UserGroups_Modes.UPDATE.ToString();
                    }
                    catch
                    {
                        saveModel.grp_id = 0;
                        saveModel.mode = PROC_USP_UserGroups_Modes.INSERT.ToString();
                    }
                }
                else
                {
                    saveModel.mode = PROC_USP_UserGroups_Modes.INSERT.ToString();
                }



                CommonResult stat = userGroupBL.SaveUserGroupData(saveModel);

                if (stat.IsSuccess)
                {
                    ResetAll();
                    tblCreateModifyForms.Visible = false;

                    BusinessLogicLayer.CommonBLS.CommonBL.DestroyUserRightSession();
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/root_UserGroups.aspx");
                    Response.Redirect("root_UserGroups.aspx", false);
                    //bindUserGroups();
                }

                hdnMessage.Value = stat.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("root_UserGroups.aspx", false);
            //ResetAll();
            //tblCreateModifyForms.Visible = false;
        }

        #region Private Usercontrol Methods



        private void GenerateMenus()
        {
            List<MenuEL> AllMenu = menuBl.GetAllMenus(1);
            List<RightEL> rights = menuBl.GetAllRights();
            if (AllMenu != null && AllMenu.Count() > 0)
            {
                string MenuTreeString = "<ul id=\"ulMenuTree\">";

                //---------------------------Body-----------------------


                List<MenuEL> ParentMenus = AllMenu.Where(t => t.mun_parentId == 0).ToList();

                foreach (MenuEL pMenus in ParentMenus.ToList())
                {

                    List<MenuEL> level1Menus = AllMenu.Where(t => t.mun_parentId == pMenus.mnu_id).ToList();

                    MenuTreeString += "<li id=\"0\">";
                    MenuTreeString += "<span>" + pMenus.mnu_menuName + "</span>";
                    if (level1Menus != null && level1Menus.Count() > 0)
                    {

                        MenuTreeString += "<ul>";
                        foreach (MenuEL lvl1 in level1Menus)
                        {
                            List<MenuEL> level2Menus = AllMenu.Where(t => t.mun_parentId == lvl1.mnu_id).ToList();

                            bool stat = !string.IsNullOrWhiteSpace(lvl1.mnu_menuLink) ? true : false;

                            MenuTreeString += "<li id=\"" + ((stat) ? lvl1.mnu_id : 0) + "\">";
                            MenuTreeString += "<span><div style=\"float:left\">" + lvl1.mnu_menuName + "</div>";

                            if (stat)
                            {

                                List<RightEL> allowedRights = menuBl.GetRights(lvl1.RightsToCheck, rights);

                                if (allowedRights == null || allowedRights.Count() <= 0)
                                {
                                    allowedRights = new List<RightEL>();
                                }

                                //MenuTreeString += "<span >";
                                MenuTreeString += "<span style=\"position:relative;left:16px;\">";
                                foreach (var item in rights)
                                {
                                    if (allowedRights.Where(t => t.Id == item.Id).Count() > 0)
                                    {
                                        MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"" + item.Id + "\" data-menuid=\"" + lvl1.mnu_id + "\" />&nbsp;" + item.Rights + "&nbsp;";
                                    }
                                    else
                                    {
                                        MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"" + item.Id + "\" data-menuid=\"" + lvl1.mnu_id + "\"   style=\"display:none\"/><label style=\"display:none\">&nbsp;" + item.Rights + "&nbsp;</label>";
                                    }
                                }
                                MenuTreeString += "</span>";
                                //MenuTreeString += "<span style=\"position:relative;left:31px;\">";

                                //MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"1\" data-menuid=\"" + lvl1.mnu_id + "\" />&nbsp;Add&nbsp;&nbsp;";
                                //MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"2\" data-menuid=\"" + lvl1.mnu_id + "\" />&nbsp;Modify&nbsp;&nbsp;";
                                //MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"3\" data-menuid=\"" + lvl1.mnu_id + "\" />&nbsp;Delete&nbsp;&nbsp;";
                                //MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"4\" data-menuid=\"" + lvl1.mnu_id + "\" />&nbsp;View&nbsp;&nbsp;";

                                //MenuTreeString += "</span>";
                            }

                            MenuTreeString += "</span>";

                            if (level2Menus != null && level2Menus.Count() > 0)
                            {
                                MenuTreeString += "<ul>";
                                foreach (MenuEL lvl2 in level2Menus)
                                {
                                    List<RightEL> allowedRights = menuBl.GetRights(lvl2.RightsToCheck, rights);

                                    if (allowedRights == null || allowedRights.Count() <= 0)
                                    {
                                        allowedRights = new List<RightEL>();
                                    }
                                    MenuTreeString += "<li id=\"" + lvl2.mnu_id + "\">";

                                    MenuTreeString += "<span><div style=\"float:left\">" + lvl2.mnu_menuName + "</div>";

                                    ////MenuTreeString += "<span style=\"position:relative;left:16px;\">";
                                    MenuTreeString += "<span >";

                                    foreach (var item in rights)
                                    {
                                        if (allowedRights.Where(t => t.Id == item.Id).Count() > 0)
                                        {
                                            MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"" + item.Id + "\" data-menuid=\"" + lvl2.mnu_id + "\" />&nbsp;" + item.Rights + "&nbsp;";

                                        }
                                        else
                                        {
                                            MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"" + item.Id + "\" data-menuid=\"" + lvl2.mnu_id + "\"   style=\"display:none\"/><label style=\"display:none\">&nbsp;" + item.Rights + "&nbsp;</label>";

                                        }
                                    }

                                    //MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"1\" data-menuid=\"" + lvl2.mnu_id + "\" />&nbsp;Add&nbsp;&nbsp;";
                                    //MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"2\" data-menuid=\"" + lvl2.mnu_id + "\" />&nbsp;Modify&nbsp;&nbsp;";
                                    //MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"3\" data-menuid=\"" + lvl2.mnu_id + "\" />&nbsp;Delete&nbsp;&nbsp;";
                                    //MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"4\" data-menuid=\"" + lvl2.mnu_id + "\" />&nbsp;View&nbsp;&nbsp;";

                                    MenuTreeString += "</span>";

                                    MenuTreeString += "</span>";

                                    MenuTreeString += "</li>";
                                }
                                MenuTreeString += "</ul>";
                            }

                            MenuTreeString += "</li>";
                        }
                        MenuTreeString += "</ul>";
                    }
                    MenuTreeString += "</li>";
                }

                //---------------------------Body-----------------------



                MenuTreeString += "</ul>";

                dvTreeMenus.InnerHtml = MenuTreeString;
            }
        }

        private void GetSetGroupAccessValues(int GroupId)
        {

            List<TranAccessByGroupModel> accessList = userGroupBL.GetTranAccessByGroup(GroupId);

            if (accessList != null && accessList.Count() > 0)
            {
                string UserGroupRightsString = "";
                foreach (TranAccessByGroupModel model in accessList)
                {
                    string TempString = "";

                    // Mantis Issue 24211 [ model.CreateOpportunities|| model.AutoCloseOpportunities || model.CloseOpportunities || model.ReopenOpportunities  added]
                    // Mantis Issue 24893 [ model.TotalAssigned|| model.RepairingPending || model.ServiceEntered || model.ServicePending  added]
                    // Mantis Issue 25087 [ model.SendSMS  added]
                    // Mantis Issue 0024702 [ model.UpdatePartyInvNoDT  added]
                    if (model.CanAdd || model.CanEdit || model.CanDelete || model.CanView || model.CanIndustry || model.CanCreateActivity || model.CanContactPerson || model.CanHistory || model.CanAddUpdateDocuments || model.CanMembers || model.CanOpeningAddUpdate || model.CanAssetDetails || model.CanExport || model.CanPrint || model.CanBudget || model.CanAssignbranch || model.Cancancelassignmnt || model.CanReassignSupervisor || model.CanReassignSalesman || model.CanClose || model.CanCancel || model.CreateOrder || model.Imagaeupload || model.RePrintBarcode || model.DocumentCollection || model.ClosedSales || model.FutureSales || model.ClarificationRequired || model.CanViewAdjustment || model.Influencer || model.CanRestore || model.CanAssignTo || model.CanConvertTo || model.CanSalesActivity || model.CanApproved || model.CanReadyToInvoice || model.CanMakeInvoice || model.CanUpdateTransporter
                        || model.CanIRN || model.CanEWayBill || model.CanMRCancellation || model.CanWRCancellation || model.CanSTBRequisition || model.CanHolds || model.CanDirectorApproval || model.CanInventoryCancellation || model.CanReturn || model.CanPendingDispatch || model.CanDispatchAcknowledgment || model.CanCreateOpportunities || model.CanAutoCloseOpportunities || model.CanCloseOpportunities || model.CanReopenOpportunities || model.TotalAssigned || model.RepairingPending || model.ServiceEntered
                        || model.ServicePending || model.CanQuotationStatus || model.CanReOpen || model.SendSMS || model.UpdatePartyInvNoDT)
                    {
                        if (model.CanAdd)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|1";
                            }
                            else
                            {
                                TempString += "1";
                            }
                        }

                       

                        if (model.CanView)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|2";
                            }
                            else
                            {
                                TempString += "2";
                            }
                        }

                        if (model.CanEdit)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|3";
                            }
                            else
                            {
                                TempString += "3";
                            }
                        }

                        if (model.CanDelete)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|4";
                            }
                            else
                            {
                                TempString += "4";
                            }
                        }


                        if (model.CanIndustry)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|6";
                            }
                            else
                            {
                                TempString += "6";
                            }
                        }

                        if (model.CanCreateActivity)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|5";
                            }
                            else
                            {
                                TempString += "5";
                            }
                        }

                        if (model.CanContactPerson)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|7";
                            }
                            else
                            {
                                TempString += "7";
                            }
                        }

                        if (model.CanHistory)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|8";
                            }
                            else
                            {
                                TempString += "8";
                            }
                        }
                        if (model.CanAddUpdateDocuments)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|9";
                            }
                            else
                            {
                                TempString += "9";
                            }
                        }
                        if (model.CanMembers)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|10";
                            }
                            else
                            {
                                TempString += "10";
                            }
                        }
                        if (model.CanOpeningAddUpdate)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|11";
                            }
                            else
                            {
                                TempString += "11";
                            }
                        }
                        if (model.CanAssetDetails)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|12";
                            }
                            else
                            {
                                TempString += "12";
                            }
                        }
                        if (model.CanExport)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|13";
                            }
                            else
                            {
                                TempString += "13";
                            }
                        }
                        if (model.CanPrint)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|14";
                            }
                            else
                            {
                                TempString += "14";
                            }
                        }

                        if (model.CanBudget)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|15";
                            }
                            else
                            {
                                TempString += "15";
                            }
                        }

                        if (model.CanAssignbranch)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|16";
                            }
                            else
                            {
                                TempString += "16";
                            }
                        }

                        if (model.Cancancelassignmnt)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|17";
                            }
                            else
                            {
                                TempString += "17";
                            }
                        }
                        if (model.CanReassignSupervisor)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|18";
                            }
                            else
                            {
                                TempString += "18";
                            }
                        }
                       
                        if (model.CanClose)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|19";
                            }
                            else
                            {
                                TempString += "19";
                            }
                        }
                        if (model.CanSpecialEdit)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|20";
                            }
                            else
                            {
                                TempString += "20";
                            }
                        }
                        if (model.CanCancel)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|21";
                            }
                            else
                            {
                                TempString += "21";
                            }
                        }
                        if (model.CreateOrder)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|22";
                            }
                            else
                            {
                                TempString += "22";
                            }
                        }

                        if (model.Imagaeupload)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|23";
                            }
                            else
                            {
                                TempString += "23";
                            }
                        }

                        if (model.RePrintBarcode)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|24";
                            }
                            else
                            {
                                TempString += "24";
                            }
                        }

                        if (model.DocumentCollection)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|25";
                            }
                            else
                            {
                                TempString += "25";
                            }
                        }

                        if (model.ClosedSales)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|26";
                            }
                            else
                            {
                                TempString += "26";
                            }
                        }

                        if (model.FutureSales)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|27";
                            }
                            else
                            {
                                TempString += "27";
                            }
                        }

                        if (model.ClarificationRequired)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|28";
                            }
                            else
                            {
                                TempString += "28";
                            }
                        }

                        if (model.CanReassignSalesman)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|29";
                            }
                            else
                            {
                                TempString += "29";
                            }
                        }

                        if (model.CanViewAdjustment)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|30";
                            }
                            else
                            {
                                TempString += "30";
                            }
                        }

                        if (model.SupervisorFeedback)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|31";
                            }
                            else
                            {
                                TempString += "31";
                            }
                        }

                        if (model.SalesmanFeedback)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|32";
                            }
                            else
                            {
                                TempString += "32";
                            }
                        }

                        if (model.Verified)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|33";
                            }
                            else
                            {
                                TempString += "33";
                            }
                        }
                        if (model.CanRestore)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|35";
                            }
                            else
                            {
                                TempString += "35";
                            }
                        }
                        if (model.CanAssignTo)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|36";
                            }
                            else
                            {
                                TempString += "36";
                            }
                        }
                        if (model.CanConvertTo)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|37";
                            }
                            else
                            {
                                TempString += "37";
                            }
                        }
                        if (model.CanSalesActivity)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|38";
                            }
                            else
                            {
                                TempString += "38";
                            }
                        }
                        if (model.CanApproved)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|39";
                            }
                            else
                            {
                                TempString += "39";
                            }
                        }

                        if (model.CanReadyToInvoice)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|40";
                            }
                            else
                            {
                                TempString += "40";
                            }
                        }
                        if (model.CanMakeInvoice)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|41";
                            }
                            else
                            {
                                TempString += "41";
                            }
                        }
                        if (model.CanUpdateTransporter)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|42";
                            }
                            else
                            {
                                TempString += "42";
                            }
                        }

                        if (model.CanIRN)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|43";
                            }
                            else
                            {
                                TempString += "43";
                            }
                        }
                        if (model.CanEWayBill)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|44";
                            }
                            else
                            {
                                TempString += "44";
                            }
                        }
                        if (model.CanMRCancellation)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|45";
                            }
                            else
                            {
                                TempString += "45";
                            }
                        }
                        if (model.CanWRCancellation)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|46";
                            }
                            else
                            {
                                TempString += "46";
                            }
                        }
                        if (model.CanSTBRequisition)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|47";
                            }
                            else
                            {
                                TempString += "47";
                            }
                        }
                        if (model.CanHolds)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|48";
                            }
                            else
                            {
                                TempString += "48";
                            }
                        }
                        if (model.CanDirectorApproval)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|49";
                            }
                            else
                            {
                                TempString += "49";
                            }
                        }
                        if (model.CanInventoryCancellation)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|50";
                            }
                            else
                            {
                                TempString += "50";
                            }
                        }
                        if (model.CanReturn)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|51";
                            }
                            else
                            {
                                TempString += "51";
                            }
                        }

                        if (model.CanPendingDispatch)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|57";
                            }
                            else
                            {
                                TempString += "57";
                            }
                        }
                        if (model.CanDispatchAcknowledgment)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|58";
                            }
                            else
                            {
                                TempString += "58";
                            }
                        }
                        // Mantis Issue 24211
                        if (model.CanCreateOpportunities)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|59";
                            }
                            else
                            {
                                TempString += "59";
                            }
                        }

                        if (model.CanAutoCloseOpportunities)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|60";
                            }
                            else
                            {
                                TempString += "60";
                            }
                        }
                        if (model.CanCloseOpportunities)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|61";
                            }
                            else
                            {
                                TempString += "61";
                            }
                        }
                        if (model.CanReopenOpportunities)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|62";
                            }
                            else
                            {
                                TempString += "62";
                            }
                        }
                        // End of Mantis Issue 24211
                        // Mantis Issue 24893
                        if (model.TotalAssigned)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|63";
                            }
                            else
                            {
                                TempString += "63";
                            }
                        }

                        if (model.RepairingPending)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|64";
                            }
                            else
                            {
                                TempString += "64";
                            }
                        }
                        if (model.ServiceEntered)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|65";
                            }
                            else
                            {
                                TempString += "65";
                            }
                        }
                        if (model.ServicePending)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|66";
                            }
                            else
                            {
                                TempString += "66";
                            }
                        }
                        // End of Mantis Issue 24893
                        //Mantis Issue 25087
                        if (model.SendSMS)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|69";
                            }
                            else
                            {
                                TempString += "69";
                            }
                        }
                        //End of Mantis Issue 25087
                        //Mantis Issue 0024702
                        if (model.UpdatePartyInvNoDT)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|70";
                            }
                            else
                            {
                                TempString += "70";
                            }
                        }
                        //End of Mantis Issue 0024702
                        TempString = model.MenuId + "^" + TempString;
                    }

                    if (!string.IsNullOrWhiteSpace(TempString))
                    {
                        if (!string.IsNullOrWhiteSpace(UserGroupRightsString))
                        {
                            UserGroupRightsString += "_" + TempString;
                        }
                        else
                        {
                            UserGroupRightsString = TempString;
                        }
                    }
                }

                GroupUserRights.Value = UserGroupRightsString;
            }
        }

        private void ResetAll()
        {
            if (Session["GroupId"] != null)
            {
                Session["GroupId"] = null;
            }

            GroupUserRights.Value = "";
            txtGroupName.Text = "";
            hdnMessage.Value = "";
        }

        #endregion
    }
}