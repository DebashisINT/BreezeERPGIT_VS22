using BusinessLogicLayer;
using BusinessLogicLayer.PMS;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using PMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace PMS.Controllers
{
    public class ProjectController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        ProjectBL ProjectBL = new ProjectBL();
        
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
       

           CommonBL ComBL = new CommonBL();
          

        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult ProjectView()
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProjectView", "Project");
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanAdd = rights.CanAdd;


            string ProjectTermsCondition = ComBL.GetSystemSettingsResult("ShowProjectTCProjectMaster");
            ViewBag.ProjectTermsCondition = ProjectTermsCondition;
            string MultipleBranchProject = ComBL.GetSystemSettingsResult("MultipleBranchProject");
            ViewBag.MultipleBranchProject = MultipleBranchProject;

            //if (!String.IsNullOrEmpty(MultipleBranchProject))
            //{
            //    if (MultipleBranchProject == "Yes")
            //    {
            //        gridProjectList.col
            //    }
            //    else if (MultipleBranchProject.ToUpper().Trim() == "NO")
            //    {
                  
            //    }
            //}


            Session["bankDetails"] = null;

            ProjectViewModel ProjectViewModel = new ProjectViewModel();
            List<Units> Units = new List<Units>();
            List<ProjectmanList> ProjectmanList = new List<ProjectmanList>();
            List<Statues> Statues = new List<Statues>();
            List<UserList> User = new List<UserList>();
            List<ContractsDetails> ConDet = new List<ContractsDetails>();
            List<ddlClass> dlcls = new List<ddlClass>();
            List<HierarchyList> objHierarchy = new List<HierarchyList>();
            var DS = ProjectBL.DropDownDetailForRole();
            var NumSche = ProjectBL.DropDownNumberScheme();
            if (DS.Tables[0].Rows.Count > 0)
            {
                Units obj = new Units();
                foreach (DataRow item in DS.Tables[0].Rows)
                {
                    obj = new Units();
                    obj.branch_id = Convert.ToString(item["branch_id"]);
                    obj.branch_description = Convert.ToString(item["branch_description"]);
                    Units.Add(obj);
                }
            }
            if (DS.Tables[1].Rows.Count > 0)
            {
                ProjectmanList obj = new ProjectmanList();
                foreach (DataRow item in DS.Tables[1].Rows)

                {
                    obj = new ProjectmanList();
                    obj.Promanager_id = Convert.ToString(item["RESOURCE_ID"]);
                    obj.Promanager_Name = Convert.ToString(item["RESOURCE_NAME"]);
                    ProjectmanList.Add(obj);
                }
            }
            if (DS.Tables[2].Rows.Count > 0)
            {
                UserList obj = new UserList();
                //foreach (DataRow item in DS.Tables[2].Rows)
                //{
                   
                    obj.user_id = Convert.ToInt32(System.Web.HttpContext.Current.Session["userid"]);
                    DataTable dtb = oDBEngine.GetDataTable("select user_name from tbl_master_user where user_id='" + obj.user_id + "'");
                    obj.user_name = Convert.ToString(dtb.Rows[0]["user_name"]);
                    User.Add(obj);
                //}
            }
            if (DS.Tables[3].Rows.Count > 0)
            {
                ContractsDetails obj = new ContractsDetails();
                foreach (DataRow item in DS.Tables[3].Rows)
                {
                    obj = new ContractsDetails();
                    obj.Details_ID = Convert.ToInt64(item["Details_ID"]);
                    obj.Contract_No = Convert.ToString(item["Contract_No"]);
                    ConDet.Add(obj);
                }
            }
            if (NumSche.Rows.Count > 0)
            {
                ddlClass obj = new ddlClass();
                foreach (DataRow item in NumSche.Rows)
                {
                    obj = new ddlClass();
                    obj.Id = Convert.ToString(item["Id"]);
                    obj.Name = Convert.ToString(item["SchemaName"]);
                    dlcls.Add(obj);
                }
            }

            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt!=null && hierarchydt.Rows.Count>0)
            {
                 HierarchyList obj = new HierarchyList();
                 foreach (DataRow item in hierarchydt.Rows)
                {
                    obj = new HierarchyList();
                    obj.Hierarchy_id = Convert.ToString(item["ID"]);
                    obj.Hierarchy_Name = Convert.ToString(item["H_Name"]);
                    objHierarchy.Add(obj);
                }
            }

            ProjectViewModel.BranchList = Units;
            ProjectViewModel.ProjectManagerList = ProjectmanList;
            ProjectViewModel.UserList = User;
            ProjectViewModel.ContractDetails = ConDet;
            ProjectViewModel.ddlClass = dlcls;
            ProjectViewModel.Hierarchy_List = objHierarchy;

            // Mantis Issue 25051
            var strShowAddlDetlInProjMast = "";

            DataTable dtShowAddlDetlInProjMast = oDBEngine.GetDataTable("SELECT Variable_Value FROm Config_SystemSettings WHERE Variable_Name='ShowAddlDetlInProjMast'");
            if (dtShowAddlDetlInProjMast != null && dtShowAddlDetlInProjMast.Rows.Count > 0)
            {
               strShowAddlDetlInProjMast  = Convert.ToString( dtShowAddlDetlInProjMast.Rows[0]["Variable_Value"]);
            }
            ProjectViewModel.ShowAddlDetlInProjMast = strShowAddlDetlInProjMast ;
            // End of Mantis Issue 25051

            return View("ProjectView", ProjectViewModel);
        }
        [HttpPost]
        public JsonResult SaveData(ProjectViewModel Project)
        {
            string returns = "";
             string OutputId = "";

           Int64   OutputProjectId = 0;
            
            string OutputText = ProjectBL.SaveProjectManagementData(Project.Action, Project.Proj_Name, Project.Proj_Description, Project.Cnt_InternalId, Project.Proj_Calender, Project.Proj_Bracnchid, Project.Proj_Managerid, Project.Proj_Statuscolor,
                Project.Proj_EstimateStartdate, Project.Proj_EstimateEnddate, Project.Proj_EstimatelabourCost, Project.Proj_EstimateExpenseCost, Project.proj_EstimateTotCost, Project.Proj_ActualStartdate, Project.Proj_ActualEndDate, Project.Proj_Code,
                Project.Proj_Id, Project.NumberSchemaId, Project.Proj_estimateHH, Project.Proj_estimateMM, Project.Proj_Hierarchy, ref OutputId, Project.Doc_No, Project.projStage_Desc, Project.Order_Id, Project.ProjectStatus, ref  OutputProjectId, Project.BranchMap_Id);

            string TermsConditionText = ProjectBL.TermsConditionSave(OutputProjectId, Project.SaveTerms_DefectLibilityPeriodDate, Project.SaveTerms_DefectLibilityPeriodRemarks, Project.SaveTerms_LiqDamage,
                                                                        Project.SaveTerms_LiqDamageAppDate, Project.SaveTerms_Payment, Project.SaveTerms_OrderType, Project.SaveTerms_NatureWork, Project.SaveTerms_DefectLibilityPeriodToDate);

            returns = OutputText + "~" + OutputId + "~" + OutputProjectId;
            return Json(returns);
        }


        public JsonResult SaveCancelCloseRemarks(ProjectViewModel Project)
        {
            string returns = "";

            DataTable OutputText = oDBEngine.GetDataTable("update Master_ProjectManagement set iscancel=1,ProjectStatus='Cancelled',CancelRemarks='" + Project.CancelRemarks + "' where Proj_Id='" + Project.Proj_Id + "'");
           
            //returns = OutputText + "~" + OutputId;
            return Json(returns);
        }


        [HttpPost]
        public JsonResult CancelProjectCheck(string Code)
        {
            string returns = "";

            DataTable valu = oDBEngine.GetDataTable("select isnull(iscancel,0) iscancel,isnull(projStage_Desc,'') projStage_Desc from Master_ProjectManagement where Proj_Id='" + Code + "'");

            //if (Convert.ToBoolean(valu.Rows[0]["iscancel"]) == true || valu.Rows[0]["projStage_Desc"] == "Finish")
            //    {
            //        returns = "1" + "~" + valu.Rows[0]["projStage_Desc"];
            //    }
            if (Convert.ToBoolean(valu.Rows[0]["iscancel"]) == true)
            {
                returns = "1";
            }
            else if (Convert.ToBoolean(valu.Rows[0]["iscancel"]) == false)
            {
                returns = "0";
            }
            if(valu.Rows[0]["projStage_Desc"] == "Finish")
            {
                returns = returns + "~" + valu.Rows[0]["projStage_Desc"];
            }
            else if (valu.Rows[0]["projStage_Desc"] != "Finish")
            {
                returns = returns + "~" + valu.Rows[0]["projStage_Desc"];
            }
            return Json(returns);
        }

        [HttpPost]
        public JsonResult ApprovalSaveData(ProjectViewModel Project)
        {
            string returns = "";

            string valu = ProjectBL.SaveApprovalProData(Project.Action, Project.Proj_Id, Project.Approved_by, Project.Approved_On, Project.Remarks, Project.Proj_Code);
            if (valu == "Data save")
            {
                returns = "Data Save";
            }
            return Json(returns);
        }


      




        [HttpPost]
        public JsonResult RejedctedSaveData(ProjectViewModel Project)
        {
            string returns = "";

            string valu = ProjectBL.SaveRejectedProData(Project.Action, Project.Proj_Id, Project.Proj_Code);
            if (valu == "Data save")
            {
                returns = "Data Save";
            }
            return Json(returns);
        }
        [HttpPost]
        public JsonResult UniqueCodeCheck(string Code)
        {
            DataTable value = oDBEngine.GetDataTable("select COUNT(Proj_Code) Proj_Code from Master_ProjectManagement where Proj_Code='" + Code + "'");
            int returnValue = 0;
            if (Convert.ToInt32(value.Rows[0]["Proj_Code"])>0)
            {
                returnValue=1;
            }

            return Json(returnValue);
        }


        [HttpPost]
        public JsonResult ProjectCodeTransactionCheck(string Code)
        {
            DataTable value = oDBEngine.GetDataTable("select COUNT(Project_Id) Project_Id from Trans_transactionprojectMapping where Project_Id='" + Code + "' ");
            int returnValue = 0;
            if (Convert.ToInt32(value.Rows[0]["Project_Id"]) > 0)
            {
                returnValue = 1;
            }

            return Json(returnValue);
        }

        [HttpPost]
        public JsonResult ProjectCancelCloseCheck(string Code)
        {
            DataTable value = oDBEngine.GetDataTable("select isnull(iscancel,0)  iscancel  from Master_ProjectManagement where Project_Id='" + Code + "' ");
            int returnValue = 0;
            if (Convert.ToInt32(value.Rows[0]["Project_Id"]) > 0)
            {
                returnValue = 1;
            }

            return Json(returnValue);
        }
        [HttpPost]
        public JsonResult ProjectstatusForTransaction(string Code)
        {
            DataTable value = oDBEngine.GetDataTable("select COUNT(Proj_Id) Proj_Id from Master_ProjectManagement   where ProjectStatus  in ('Cancelled','Approved') and Proj_Id='" + Code + "'");
            int returnValue = 0;
            if (Convert.ToInt32(value.Rows[0]["Proj_Id"]) > 0)
            {
                returnValue = 1;
            }

            return Json(returnValue);
        }


        [HttpPost]
        public JsonResult ProjectDeletedForTransaction(string Code)
        {
            DataTable value = oDBEngine.GetDataTable("select COUNT(Proj_Id) Proj_Id from Master_ProjectManagement   where ProjectStatus  in ('Cancelled','Approved') and Proj_Code='" + Code + "'");
            int returnValue = 0;
            if (Convert.ToInt32(value.Rows[0]["Proj_Id"]) > 0)
            {
                returnValue = 1;
            }

            return Json(returnValue);
        }

        public ActionResult GetProjectMasterPartial()
        {

            return View();
        }

        public ActionResult GetProjectPartial()
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProjectView", "Project");
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.CanCancel = rights.CanCancel;
                ViewBag.CanApproved = rights.CanApproved;
                string MultipleBranchProject = ComBL.GetSystemSettingsResult("MultipleBranchProject");
                ViewBag.MultipleBranchProject = MultipleBranchProject;
                String weburl = System.Configuration.ConfigurationSettings.AppSettings["SiteURL"];
                List<ProJectList> omel = new List<ProJectList>();

                DataTable dt = new DataTable();

                dt = ProjectBL.GetProjectList();

                if (dt.Rows.Count > 0)
                {
                    omel = APIHelperMethods.ToModelList<ProJectList>(dt);
                    TempData["ExportProficiency"] = omel;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExportProficiency"] = null;
                    TempData.Keep();
                }
                return PartialView("~/Views/Project/GetProjectPartial.cshtml", omel);
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }


        [HttpPost]
        public JsonResult ViewDataShow(Int64 Proj_Id)
        {
            ProjectViewModel viewMDL = new ProjectViewModel();
            DataTable Projdt = ProjectBL.ViewProjectDetails(Proj_Id);
            if (Projdt != null && Projdt.Rows.Count > 0)
            {
                viewMDL.Proj_Id = Int64.Parse(Projdt.Rows[0]["Proj_Id"].ToString());
                viewMDL.Proj_Name = Projdt.Rows[0]["Proj_Name"].ToString();
                viewMDL.Proj_Description = Projdt.Rows[0]["Proj_Description"].ToString();
                viewMDL.Proj_Code = Projdt.Rows[0]["Proj_Code"].ToString();
                viewMDL.Cnt_InternalId = Projdt.Rows[0]["Cnt_InternalId"].ToString();
                viewMDL.Proj_Calender = Int64.Parse(Projdt.Rows[0]["Proj_Calender"].ToString());
                viewMDL.Proj_Bracnchid = Int64.Parse(Projdt.Rows[0]["Proj_Bracnchid"].ToString());
                viewMDL.Proj_Managerid = Int64.Parse(Projdt.Rows[0]["Proj_Managerid"].ToString());
                viewMDL.Proj_Statuscolor = Projdt.Rows[0]["Proj_Statuscolor"].ToString();
                viewMDL.Proj_EstimateStartdate = DateTime.Parse(Projdt.Rows[0]["Proj_EstimateStartdate"].ToString());
                viewMDL.Proj_EstimateEnddate = DateTime.Parse(Projdt.Rows[0]["Proj_EstimateEnddate"].ToString());
                //viewMDL.Proj_Estimatehours = Decimal.Parse(Projdt.Rows[0]["Proj_Estimatehours"].ToString());
                viewMDL.Proj_estimateHH = Convert.ToInt64(Projdt.Rows[0]["Proj_estimateHH"].ToString());
                viewMDL.Proj_estimateMM = Convert.ToInt64(Projdt.Rows[0]["Proj_estimateMM"].ToString());
                viewMDL.Proj_EstimatelabourCost = Decimal.Parse(Projdt.Rows[0]["Proj_EstimatelabourCost"].ToString());
                viewMDL.Proj_EstimateExpenseCost = Decimal.Parse(Projdt.Rows[0]["Proj_EstimateExpenseCost"].ToString());
                viewMDL.proj_EstimateTotCost = Decimal.Parse(Projdt.Rows[0]["proj_EstimateTotCost"].ToString());
                viewMDL.Proj_ActualStartdate = DateTime.Parse(Projdt.Rows[0]["Proj_ActualStartdate"].ToString());
                viewMDL.Proj_ActualEndDate = DateTime.Parse(Projdt.Rows[0]["Proj_ActualEndDate"].ToString());
                viewMDL.Remarks = Projdt.Rows[0]["Remarks"].ToString();
                if (Projdt.Rows[0]["Approved_On"].ToString() != "")
                {
                    viewMDL.Approved_On = DateTime.Parse(Projdt.Rows[0]["Approved_On"].ToString());
                }
                else
                {
                    viewMDL.Approved_On =Convert.ToDateTime(DateTime.Now);
                }
                viewMDL.Approved_by = Convert.ToInt32(Projdt.Rows[0]["Approved_by"].ToString());
                viewMDL.Customer =Projdt.Rows[0]["Customer"].ToString();
                viewMDL.branch_description = Projdt.Rows[0]["branch_description"].ToString();
                viewMDL.RESOURCE_NAME = Projdt.Rows[0]["RESOURCE_NAME"].ToString();
                viewMDL.projStage_Desc = Projdt.Rows[0]["projStage_Desc"].ToString().Trim();
                viewMDL.Order_Id = Projdt.Rows[0]["OrderList"].ToString().Trim();
                viewMDL.BranchMap_Id = Projdt.Rows[0]["BranchMaplistList"].ToString().Trim();
                viewMDL.ProjectStatus = Projdt.Rows[0]["ProjectStatus"].ToString().Trim();
                viewMDL.Proj_Hierarchy = Int64.Parse(Projdt.Rows[0]["Hierarchy_ID"].ToString());
               
            }
            
            DataSet dt = ProjectBL.GetTermsDetails(Proj_Id, "Project");
            DataTable BGDetails = dt.Tables[0];
            DataTable TermsDetails = dt.Tables[1];
            if (TermsDetails.Rows.Count > 0)
            {
                viewMDL.SaveEditTerms_DefectLibilityPeriodDate = Convert.ToString(TermsDetails.Rows[0]["Terms_DefectLibilityPeriodDate"]);
                viewMDL.SaveEditTerms_DefectLibilityPeriodToDate = Convert.ToString(TermsDetails.Rows[0]["Terms_DefectLibilityPeriodToDate"]);
                viewMDL.SaveTerms_DefectLibilityPeriodRemarks = Convert.ToString(TermsDetails.Rows[0]["Terms_DefectLibilityPeriodRemarks"]);
                viewMDL.SaveTerms_LiqDamage = Convert.ToString(TermsDetails.Rows[0]["Terms_LiqDamage"]);
                viewMDL.SaveEditTerms_LiqDamageAppDate = Convert.ToString(TermsDetails.Rows[0]["Terms_LiqDamageAppDate"]);
                viewMDL.SaveTerms_Payment = Convert.ToString(TermsDetails.Rows[0]["Terms_Payment"]);
                viewMDL.SaveTerms_OrderType = Convert.ToString(TermsDetails.Rows[0]["Terms_OrderType"]);
                viewMDL.SaveTerms_NatureWork = Convert.ToString(TermsDetails.Rows[0]["Terms_NatureWork"]);

              //  GrdBGDetails.JSProperties["cpTermsBind"] = dtDefectPerid.Text + "~" + txtDefectPerid.Text + "~" + txtLiquiDamage.Text + "~" + dtLiqDmgAppliDt.Text + "~" + txtPaymentTerms.Text + "~" + txtOrderType.Text + "~" + txtNatureWork.Text;
            }
            Session["bankDetails"] = BGDetails;



            return Json(viewMDL);
        }


        public ActionResult ExportProjectlist(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["ExportProficiency"] = TempData["ExportProficiency"];
            TempData.Keep();

            if (ViewData["ExportProficiency"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetRoleGridViewSettings(), ViewData["ExportProficiency"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetRoleGridViewSettings(), ViewData["ExportProficiency"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetRoleGridViewSettings(), ViewData["ExportProficiency"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetRoleGridViewSettings(), ViewData["ExportProficiency"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetRoleGridViewSettings(), ViewData["ExportProficiency"]);
                    default:
                        break;
                }
            }
            //TempData["Exportcounterist"] = TempData["Exportcounterist"];
            //TempData.Keep();
            return null;
        }

        private GridViewSettings GetRoleGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Project Model";
            // settings.CallbackRouteValues = new { Controller = "Employee", Action = "ExportEmployee" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Project Model";

            settings.Columns.Add(column =>
            {
                column.Caption = "Project Code";
                column.FieldName = "Proj_Code";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Project Name";
                column.FieldName = "Proj_Name";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Customer";
                column.FieldName = "Customer";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Status";
                column.FieldName = "ProjectStatus";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Branch";
                column.FieldName = "BranchName";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Project Stage";
                column.FieldName = "projStage_Desc";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Hierarchy";
                column.FieldName = "Hierarchy_Name";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Estimated Start Date";
                column.FieldName = "Proj_EstimateStartdate";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Estimated End Date";
                column.FieldName = "Proj_EstimateEnddate";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Actual Start Date";
                column.FieldName = "Proj_ActualStartdate";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Actual End Date";
                column.FieldName = "Proj_ActualEndDate";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            });


            settings.Columns.Add(column =>
            {
                column.Caption = "Entered By";
                column.FieldName = "CreatedName";
                
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Entered On";
                column.FieldName = "CreatedDate";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Modified By";
                column.FieldName = "ModifiedName";
                
            });
             settings.Columns.Add(column =>
            {
                column.Caption = "Modified On";
                column.FieldName = "ModifiedDate";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy hh:mm:ss";
            });
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }
        [HttpPost]
        public JsonResult DeleteData(Int64 Proj_Id)
        {
            string returns = "Data not Deleted please try again later.";

            int val = ProjectBL.DeleteProjectData(Proj_Id);
            if (val ==1)
            {
                returns = "Deleted Successfully.";
            }
            else if (val==-10)
            {
                returns = "Used in other module can not delete.";
            }
            return Json(returns);
        }



        public ActionResult GetSalesOrderList(ProjectViewModel model)
        {
            
            DataTable dtProj=new DataTable();
                try
                {
                    if(model.Cnt_InternalId!=null && model.Proj_Bracnchid!=0)
                        dtProj = ProjectBL.GetSalesOrder(model.Cnt_InternalId, model.Proj_Bracnchid, model.Proj_Id);

                    List<SalesOrderList> modelProj = new List<SalesOrderList>();
                    modelProj = APIHelperMethods.ToModelList<SalesOrderList>(dtProj);

                    return PartialView("~/Views/Project/_PartialSalesOrder.cshtml", modelProj);

                }
                catch
                {
                    return RedirectToAction("Logout", "Login", new { Area = "" });

                }
            
        }

        public ActionResult GetBranchList()
        {

            DataTable dtProj = new DataTable();
            try
            {

                dtProj = ProjectBL.GetBranchList();

                List<BranchMapList> modelProj = new List<BranchMapList>();
                modelProj = APIHelperMethods.ToModelList<BranchMapList>(dtProj);

                return PartialView("~/Views/Project/_partialMultiBranch.cshtml", modelProj);

            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });

            }

        }
        public ActionResult GetTermsConitionsPartial()
        {
            try
            {
                List<BankGuaranteeList> omel = new List<BankGuaranteeList>();

                DataTable dt = new DataTable();
                if (Session["bankDetails"] !=null)
              {
                    dt = (DataTable)Session["bankDetails"];
                }
                else
                {
                    dt = ProjectBL.GetTermsConditionList();
                }
                if (dt.Rows.Count > 0)
                {
                    omel = APIHelperMethods.ToModelList<BankGuaranteeList>(dt);
               
                }
                return PartialView("~/Views/Project/_TermsConditionsBind.cshtml", omel);
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        [HttpPost]
        public JsonResult BankDetailsSave(BankGuaranteeList bank)
        {
           
            //DataTable dtBGDetails = new DataTable();
          
            
            List<BankGuaranteeList> BGList = new List<BankGuaranteeList>();

            DataTable dtBGDetails = (DataTable)Session["bankDetails"];
           
            if (dtBGDetails == null)
            {
                DataTable dtable = new DataTable();
                Session["bankDetails"] = null;
                

                dtable.Clear();
                dtable.Columns.Add("Terms_BankGuaranteeSL", typeof(System.String));
                dtable.Columns.Add("BG_BGGroup", typeof(System.String));
                dtable.Columns.Add("BG_BGType", typeof(System.String));
                dtable.Columns.Add("BG_Percentage", typeof(System.Decimal));
                dtable.Columns.Add("BG_BGValue", typeof(System.Decimal));
                dtable.Columns.Add("BG_BGStatus", typeof(System.String));
                dtable.Columns.Add("BG_BGValidfrom", typeof(System.DateTime));
                dtable.Columns.Add("BG_BGValidUpTo", typeof(System.DateTime));


                object[] trow = { Convert.ToString(Guid.NewGuid()), bank.BG_BGGroup, bank.BG_BGType, bank.BG_Percentage, bank.BG_BGValue, bank.BG_BGStatus, bank.BG_BGValidfrom, bank.BG_BGValidUpTo };// Add new parameter Here
                dtable.Rows.Add(trow);
                Session["bankDetails"] = dtable;
                BGList = APIHelperMethods.ToModelList<BankGuaranteeList>(dtable);

                
            }

            else
            {
                //if (string.IsNullOrEmpty(bank.Terms_BankGuaranteeSL.ToString()))
                //{
                object[] trow = { Convert.ToString(Guid.NewGuid()), bank.BG_BGGroup, bank.BG_BGType, bank.BG_Percentage, bank.BG_BGValue, bank.BG_BGStatus, bank.BG_BGValidfrom, bank.BG_BGValidUpTo };// Add new parameter Here
                    dtBGDetails.Rows.Add(trow);
               // }
                //else
                //{
                //    if (dtBGDetails != null && dtBGDetails.Rows.Count > 0)
                //    {
                //        foreach (DataRow item in dtBGDetails.Rows)
                //        {
                //            if (bank.Terms_BankGuaranteeSL.ToString() == item["Terms_BankGuaranteeSL"].ToString())
                //            {


                //                item["Terms_BankGuaranteeSL"] = bank.Terms_BankGuaranteeSL;
                //                item["BG_BGGroup"] = bank.BG_BGValue;
                //                item["BG_BGType"] = bank.BG_BGType;
                //                item["BG_Percentage"] = bank.BG_Percentage;
                //                item["BG_BGValue"] = bank.BG_BGValue;
                //                item["BG_BGStatus"] = bank.BG_BGStatus;
                //                item["BG_BGValidfrom"] = bank.BG_BGValidfrom;
                //                item["BG_BGValidUpTo"] = bank.BG_BGValidUpTo;

                //            }
                //        }
                //    }

                //}

                Session["BGDetails"] = dtBGDetails;
            }

            return Json(BGList);
        }

        public JsonResult DeleteTermsConditionsData(string Terms_BankGuaranteeSL)
        {
            DataTable dt = (DataTable)Session["bankDetails"];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (Terms_BankGuaranteeSL.ToString() == item["Terms_BankGuaranteeSL"].ToString())
                    {
                        dt.Rows.Remove(item);
                        break;
                    }
                }
            }
            return Json("Bank guarantee  Remove Successfully.");
        }

	}
}