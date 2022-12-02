using BusinessLogicLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Newtonsoft.Json;
using PMS.Models;
using PMS.Models.DataContext;
using PMS.Models.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using UtilityLayer;

namespace PMS.Controllers
{
    public class ScheduleController : Controller
    {
        CommonBL ComBL = new CommonBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        Shedules objdata = new Shedules();
        public ActionResult Index()
        {



            Shedules sc = new Shedules();
            return View("~/Views/PMS/Schedule/ScheduleList.cshtml", sc);
        }

        public ActionResult ScheduleView(string id)
        {
            Session["wbs_id"] = id;

            ViewBag.WBS_ID = id;
            List<HierarchyList> objHierarchy = new List<HierarchyList>();
            WBSInput objWBSModel = new WBSInput();
            Session["ResourceLink"] = null;
            Session["ScheduleList"] = null;
            Session["TreeData"] = null;
            List<BranchUnit> list = new List<BranchUnit>();
            var datasetobj = objdata.DropDownDetailForEstimate("GetUnitDropDownData", Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["userbranchHierarchy"]), 0, 0);
            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                BranchUnit obj = new BranchUnit();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    obj = new BranchUnit();
                    obj.BranchID = Convert.ToString(item["BANKBRANCH_ID"]);
                    obj.BankBranchName = Convert.ToString(item["BANKBRANCH_NAME"]);
                    list.Add(obj);
                }
            }

            string HierarchySelectInEntryModule = ComBL.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    ViewBag.Hierarchy = "1";
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ViewBag.Hierarchy = "0";
                }
            }

            DataTable dt = objdata.GetHeaderData(id);

            if (dt != null && dt.Rows.Count > 0)
            {
                objWBSModel.WBS_ID = Convert.ToInt32(id);
                objWBSModel.WBS_Code = Convert.ToString(dt.Rows[0]["WBS_Code"]);
                objWBSModel.WBS_Name = Convert.ToString(dt.Rows[0]["WBS_Name"]);
                objWBSModel.WBS_HierarchyId = Convert.ToInt32(dt.Rows[0]["WBS_HierarchyId"]);
                objWBSModel.WBS_Branch = Convert.ToString(dt.Rows[0]["WBS_Branch"]);
                objWBSModel.WBS_StartDate = Convert.ToDateTime(dt.Rows[0]["WBS_StartDate"]);
                objWBSModel.WBS_EndTime = Convert.ToDateTime(dt.Rows[0]["WBS_EndTime"]);
                objWBSModel.WBS_Duration = Convert.ToString(dt.Rows[0]["WBS_Duration"]);
                objWBSModel.WBS_Description = Convert.ToString(dt.Rows[0]["WBS_Description"]);
                objWBSModel.WBS_Workunit = Convert.ToString(dt.Rows[0]["WBS_Workunit"]);
                objWBSModel.WBS_ProjectCode = Convert.ToString(dt.Rows[0]["WBS_ProjectCode"]);
                objWBSModel.WBS_Effort = Convert.ToString(dt.Rows[0]["WBS_Effort"]);
                ViewBag.Count = Convert.ToInt32(dt.Rows[0]["cnt"]);

            }

            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
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
            objWBSModel.Hierarchy_List = objHierarchy;

            objWBSModel.UnitList = list;
            return View("~/Views/PMS/Schedule/ScheduleView.cshtml", objWBSModel);

        }

        public PartialViewResult ScheduleEntryView(string Doc_Id)
        {
            List<Shedules> objWBSModel = new List<Shedules>();


            if (!string.IsNullOrEmpty(Doc_Id))
            {
                DataSet dtList = objdata.GetWBSDataSet(Doc_Id);
                if (dtList != null && dtList.Tables.Count > 0)
                {
                    objWBSModel = APIHelperMethods.ToModelList<Shedules>(dtList.Tables[0]);
                    Session["TreeData"] = objWBSModel;
                }
                Session["ScheduleList"] = dtList.Tables[0];
                Session["ResourceLink"] = dtList.Tables[1];

            }
            else
            {
                if (Session["TreeData"] != null)
                {
                    objWBSModel = (List<Shedules>)Session["TreeData"];
                }

            }
            return PartialView("~/Views/PMS/Schedule/_PartialScheduleGrid.cshtml", objWBSModel);

        }

        public JsonResult RenderTreeGrid(Shedules Shedules)
        {
            List<Shedules> objWBSModel = new List<Shedules>();
            if (Session["TreeData"] != null)
            {
                objWBSModel = (List<Shedules>)Session["TreeData"];
            }

            //if (Shedules.AddNew == "1")
            //{
                //if (Shedules.Delete != "1")
                //{
                //    objWBSModel.Add(Shedules);
                //    Session["TreeData"] = objWBSModel;
                //}
                //else
                //{
                    List<Shedules> res = (from d in objWBSModel
                                          where d.Slno != Shedules.Slno orderby d.Slno
                                          select d ).ToList();

                    res.Add(Shedules);
                    Session["TreeData"] = res;
                //}
            //}

            return Json(objWBSModel);

        }

        public JsonResult DeletetreeGrid(string Slno)
        {
            string dtOutput = null;
            List<Shedules> objWBSModel = new List<Shedules>();
            if (Session["TreeData"] != null)
            {
                objWBSModel = (List<Shedules>)Session["TreeData"];

                List<Shedules> resParent = (from d in objWBSModel
                                      where d.ParentId == Slno
                                      select d).ToList();


                if (resParent.Count > 0)
                {
                    dtOutput = "Parent node can not be deleted.";
                }
                else
                {
                    List<Shedules> res = (from d in objWBSModel
                                          where d.Slno != Slno
                                          select d).ToList();

                    Session["TreeData"] = res;
                    dtOutput = "Deleted successfully.";
                }
            }

            return Json(dtOutput, JsonRequestBehavior.DenyGet);

        }
        public JsonResult EdittreeGrid(string Slno)
        {
            List<Shedules> objWBSModel = new List<Shedules>();
            List<Shedules> resParent = new List<Shedules>();
            if (Session["TreeData"] != null)
            {
                objWBSModel = (List<Shedules>)Session["TreeData"];

                resParent = (from d in objWBSModel
                                      where d.Slno == Slno
                                      select d).ToList();
                
            }

            return Json(resParent);

        }

        [ValidateInput(false)]
        public JsonResult grid_Batchupdate(WBSInput options)
        {
            DataTable dt = new DataTable();
            DataTable copydt = new DataTable();

            DataTable dtResource = new DataTable();
            if (Session["ResourceLink"] != null)
            {
                dtResource = (DataTable)Session["ResourceLink"];
            }
            else
            {

                dtResource.Columns.Add("slno", typeof(string));
                dtResource.Columns.Add("Keys", typeof(string));
            }


            //if (Session["ScheduleList"] != null)
            //{
            //    copydt = ((DataTable)Session["ScheduleList"]).Copy();
            //    dt = copydt;

            //    if (dt.Columns.Contains("UpdateEdit"))
            //    {
            //        dt.Columns.Remove("UpdateEdit");
            //    }
            //}
            //else
            //{
                dt.Columns.Add("Slno", typeof(String));
                dt.Columns.Add("ActivityName", typeof(String));
                dt.Columns.Add("Effort", typeof(String));
                dt.Columns.Add("StartDate", typeof(System.DateTime));
                dt.Columns.Add("EndDate", typeof(System.DateTime));
                dt.Columns.Add("Predecessor", typeof(String));
                dt.Columns.Add("Duration", typeof(String));
                dt.Columns.Add("Resources", typeof(String));
                dt.Columns.Add("Description", typeof(String));
            //}



            List<Shedules> objWBSModel = new List<Shedules>();
            if (Session["TreeData"] != null)
            {
                objWBSModel = (List<Shedules>)Session["TreeData"];
            }




            foreach (Shedules item in objWBSModel)
            {
                if (!string.IsNullOrEmpty(item.ActivityName))
                {
                    dt.Rows.Add(item.Slno, item.ActivityName, item.Effort, item.StartDate, item.EndDate, item.Predecessor, item.Duration, item.Resources, item.Description);
                }
            }


            //foreach (Shedules item in updateValues.Update)
            //{
            //    string Slno = item.Slno;
            //    if (!string.IsNullOrEmpty(Slno))
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            if (Convert.ToString(dr["Slno"]) == Slno)
            //            {
            //                //DateTime StartDate = new DateTime(Convert.ToInt32(item.StartDate.Split('-')[2]), Convert.ToInt32(item.StartDate.Split('-')[1]), Convert.ToInt32(item.StartDate.Split('-')[0]));
            //                //DateTime EndDate = new DateTime(Convert.ToInt32(item.EndDate.Split('-')[2]), Convert.ToInt32(item.EndDate.Split('-')[1]), Convert.ToInt32(item.EndDate.Split('-')[0]));

            //                dr["ActivityName"] = Convert.ToString(item.ActivityName);
            //                dr["Effort"] = Convert.ToString(item.Effort);
            //                dr["StartDate"] = null;
            //                dr["EndDate"] = null;
            //                dr["Predecessor"] = Convert.ToString(item.Predecessor);
            //                dr["Duration"] = Convert.ToString(item.Duration);
            //                dr["Resources"] = Convert.ToString(item.Resources);
            //                dr["Description"] = Convert.ToString(item.Description);

            //            }
            //        }

            //    }
            //}


            //foreach (var item in updateValues.DeleteKeys)
            //{
            //    string SLID = Convert.ToString(item);
            //    string SrlNo = "";

            //    for (int i = dt.Rows.Count - 1; i >= 0; i--)
            //    {
            //        DataRow dr = dt.Rows[i];
            //        string delSLID = Convert.ToString(dr["Slno"]);

            //        if (delSLID == SLID)
            //        {
            //            SrlNo = Convert.ToString(dr["Slno"]);
            //            dr.Delete();
            //        }
            //    }
            //    dt.AcceptChanges();

            //    DataRow[] Deldr = dtResource.Select("Slno='" + SLID + "'");

            //    foreach (DataRow dr in Deldr)
            //    {
            //        dr.Delete();
            //    }
            //    dtResource.AcceptChanges();




            //}


            //int j = 1;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    string oldSrlNo = Convert.ToString(dr["Slno"]);
            //    string newSrlNo = j.ToString();

            //    dr["Slno"] = j.ToString();
            //    //UpdateResources(oldSrlNo, newSrlNo);
            //    j++;

            //}

            dt.AcceptChanges();


            string dtOutput = "";
            if (dt.Rows.Count == 0)
            {
                dtOutput = "Please enter atleast one line to proceed.";
            }



            if (options.Isdelete != "1" && dt.Rows.Count > 0)
            {
                dtOutput = objdata.SaveSchedule(options, dt, dtResource);
                ViewBag.OutputID = "1";
            }

            ViewBag.Output = dtOutput;

            List<Shedules> objeSchedules = new List<Shedules>();

            objeSchedules = APIHelperMethods.ToModelList<Shedules>(dt);

            return Json(dtOutput,JsonRequestBehavior.AllowGet );
        }

        public void UpdateResources(string oldSrlNo, string newSrlNo)
        {
            DataTable dtResource = (DataTable)Session["ResourceLink"];
            if (dtResource != null)
            {


                for (int i = 0; i < dtResource.Rows.Count; i++)
                {
                    DataRow dr = dtResource.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["Slno"]);
                    if (oldSrlNo == Product_SrlNo)
                    {
                        dr["Slno"] = newSrlNo;
                    }
                }
                dtResource.AcceptChanges();

                Session["ResourceLink"] = dtResource;
            }
        }

        public ActionResult GetProjectCode(string Project_ID, string Customer_id, String Branchs, String Hierarchy)
        {
            try
            {
                DataTable dtProj = objdata.GetProjectCode(Branchs);
                List<ProjectList> modelProj = new List<ProjectList>();
                modelProj = APIHelperMethods.ToModelList<ProjectList>(dtProj);
                ViewBag.ProjectID = Project_ID;
                ViewBag.Hierarchy = Hierarchy;

                return PartialView("~/Views/PMS/Schedule/_PartialProjectCode.cshtml", modelProj);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult ResourceEntryView(string Slno)
        {
            try
            {
                string strKeys = "";
                DataTable dt = new DataTable();
                string Keys = "";
                if (Session["ResourceLink"] != null)
                {
                    dt = (DataTable)Session["ResourceLink"];

                    DataRow[] dr = dt.Select("slno='" + Slno + "'");

                    foreach (DataRow drr in dr)
                    {
                        strKeys = strKeys + "," + Convert.ToString(drr["Keys"]);
                    }


                }

                ViewBag.Keys = strKeys;



                string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                List<v_employee_detail> crm_CampaignTypelist = new List<v_employee_detail>();
                PMSDataClassesDataContext dc = new PMSDataClassesDataContext(connectionString);
                crm_CampaignTypelist = (from u in dc.v_employee_details
                                        select u).ToList();
                return PartialView("~/Views/PMS/Schedule/_PartialResourceLookup.cshtml", crm_CampaignTypelist);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult ScheduleList()
        {
            Shedules sc = new Shedules();
            return View("~/Views/PMS/Schedule/ScheduleList.cshtml", sc);
        }

        public ActionResult ResourceEntryOkClick(List<string> SelectedKeys, string slno)
        {
            string Output = "";
            DataTable dt = new DataTable();
            if (Session["ResourceLink"] != null)
            {
                dt = (DataTable)Session["ResourceLink"];
            }
            else
            {

                dt.Columns.Add("slno", typeof(string));
                dt.Columns.Add("Keys", typeof(string));
            }

            DataRow[] dr = dt.Select("slno='" + slno + "'");

            foreach (DataRow drr in dr)
            {
                drr.Delete();
            }

            dt.AcceptChanges();

            if (SelectedKeys.Count > 0)
            {
                foreach (string str in SelectedKeys)
                {
                    dt.Rows.Add(slno, str);
                }
            }
            Session["ResourceLink"] = dt;

            return Json(Output, JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult WBSList()
        {
            return PartialView("~/Views/PMS/Schedule/_PartialScheduleListing.cshtml", GetWBSs("frmdate"));
        }

        private IEnumerable GetWBSs(string frmdate)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (frmdate != "Ispageload")
            {
                PMSDataClassesDataContext dc = new PMSDataClassesDataContext(connectionString);
                var q = from d in dc.V_WBs 
                        select d;
                return q;
            }
            else
            {
                PMSDataClassesDataContext dc = new PMSDataClassesDataContext(connectionString);
                var q = from d in dc.V_WBs
                        select d;
                return q;
            }



        }

        public JsonResult DeleteSchedule(string id)
        {
            string dtOutput = objdata.DeleteWBS(id);
            return Json(dtOutput, JsonRequestBehavior.AllowGet);
        }





        // susanta json for grantt
        public JsonResult getGranttData(string id)
        {
            List<GranttChartDetails> dtOutput = objdata.getGranttData(id);
            var str = JsonConvert.SerializeObject(dtOutput);
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEndDateofpredessesor(string Predecessor)
        {
            DateTime? dtOutput = null;
            List<Shedules> objWBSModel = new List<Shedules>();
            if (Session["TreeData"] != null)
            {
                objWBSModel = (List<Shedules>)Session["TreeData"];
                List<Shedules> res = (from d in objWBSModel
                                      where d.Slno == Predecessor
                                      select d).ToList();

                if (res.Count > 0)
                {
                    dtOutput= res[0].EndDate;
                }
            }
            
            




            return Json(dtOutput, JsonRequestBehavior.AllowGet);
        }


        //Tanmoy Hierarchy
        [WebMethod]
        public JsonResult getHierarchyID(string ProjID)
        {
            ReturnData obj = new ReturnData();
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";
            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");
            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                obj.Success = true;
                obj.Message = Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                obj.Success = true;
                obj.Message = Hierarchy_ID;
            }

            return Json(obj);
        }
        //Tanmoy Hierarchy End


        public ActionResult ExportScheduleGridList(int type)
        {


                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetGridView(null),GetWBSs("frmdate"));
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetGridView(null),GetWBSs("frmdate"));
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetGridView(null),GetWBSs("frmdate"));
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetGridView(null),GetWBSs("frmdate"));
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetGridView(null),GetWBSs("frmdate"));
                    default:
                        break;
                }
                return null;
           
        }

        private GridViewSettings GetGridView(object datatable)
        {

            var settings = new GridViewSettings();
            settings.Name = "gridWBSList";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "WBS List";
            settings.Columns.Add(x =>
            {
                x.Caption = "ID";
                x.FieldName = "WBS_ID";
                x.Visible = false;
                x.ShowInCustomizationForm = false;
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Code";
                x.FieldName = "WBS_Code";
                //x.Visible = false;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
                x.ShowInCustomizationForm = false;
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Name";
                x.FieldName = "WBS_Name";
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;

            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Branch";
                x.FieldName = "branch_description";
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;

            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Start Date";
                x.FieldName = "WBS_StartDate";
                x.ColumnType = MVCxGridViewColumnType.DateEdit;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";

            });
            settings.Columns.Add(x =>
            {
                x.Caption = "End Time";
                x.FieldName = "WBS_EndTime";
                x.ColumnType = MVCxGridViewColumnType.DateEdit;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
            });

            // Rev Sayantani

            settings.Columns.Add(x =>
            {
                x.Caption = "Work Unit";
                x.FieldName = "WBS_Workunit";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Duration";
                x.FieldName = "WBS_Duration";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Effort";
                x.FieldName = "Effort";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Description";
                x.FieldName = "WBS_Description";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            // End of Rev Sayantani
            settings.Columns.Add(x =>
            {
                x.Caption = "Project Name";
                x.FieldName = "Proj_Name";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Hierarchy";
                x.FieldName = "H_Name";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Created By";
                x.FieldName = "CREATED";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Last Updated On";
                x.FieldName = "LAST_MODIFIED";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Updated By";
                x.FieldName = "MODIFIED";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

           

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }









    }
}