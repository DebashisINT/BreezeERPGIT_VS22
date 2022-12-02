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
    public class ResourceController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        ResourceMasterBL bl = new ResourceMasterBL();
        public ActionResult ResourceIndex()
        {
            ResourceViewModel objPO = new ResourceViewModel();
            List<Units> listUnits = new List<Units>();
            List<ResourecType> resource = new List<ResourecType>();
            var datasetobj = bl.DropDownDetailForResource();
            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                Units obj = new Units();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    obj = new Units();
                    obj.branch_id = Convert.ToString(item["branch_id"]);
                    obj.branch_description = Convert.ToString(item["branch_description"]);
                    listUnits.Add(obj);
                }
            }
            if (datasetobj.Tables[1].Rows.Count > 0)
            {
                ResourecType obj = new ResourecType();
                foreach (DataRow item in datasetobj.Tables[1].Rows)
                {
                    obj = new ResourecType();
                    obj.ID = Convert.ToString(item["ID"]);
                    obj.RESOURCE_NAME = Convert.ToString(item["RESOURCE_NAME"]);
                    resource.Add(obj);
                }
            }

            objPO.BranchList = listUnits;
            objPO.resourceTypeList = resource;

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ResourceIndex", "Resource");
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;

            return View("~/Views/PMS/Resource/ResourceIndex.cshtml", objPO);
        }

        [HttpPost]
        public JsonResult SaveData(ResourceViewModel Resc)
        {
            string returns = "Not saved please try again later.";
            int valu = bl.InsertRoleMaster(Resc.resource_id, Resc.resourceType, Resc.Contact, Resc.resourceName, Resc.Branch);
            if (valu > 0)
            {
                returns = "Saved Successfully.";
            }
            return Json(returns);
        }

        public ActionResult GetResourceMasterPartial()
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ResourceIndex", "Resource");
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
                ViewBag.Verified = rights.Verified;

                String weburl = System.Configuration.ConfigurationSettings.AppSettings["SiteURL"];
                List<ResourecView> omel = new List<ResourecView>();

                DataTable dt = new DataTable();

                dt = bl.GetResourceList();

                if (dt!=null)
                {
                    omel = APIHelperMethods.ToModelList<ResourecView>(dt);
                    TempData["ExportResourceMaster"] = omel;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExportResourceMaster"] = null;
                    TempData.Keep();
                }
                return PartialView("~/Views/PMS/Resource/PartialResourceGrid.cshtml", omel);
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        [HttpPost]
        public JsonResult ViewDataShow(string resourceID)
        {
            ResourceViewModel viewMDL = new ResourceViewModel();
            DataTable Resourcedt = bl.ViewResourceMaster(resourceID);
            if (Resourcedt != null && Resourcedt.Rows.Count > 0)
            {
                viewMDL.resource_id = Resourcedt.Rows[0]["RESOURCE_ID"].ToString();
                viewMDL.resourceName = Resourcedt.Rows[0]["RESOURCE_NAME"].ToString();
                viewMDL.resourceType = Resourcedt.Rows[0]["RESOURCE_TypeID"].ToString();
                viewMDL.Contact = Resourcedt.Rows[0]["CONTACT"].ToString();
                viewMDL.Branch = Resourcedt.Rows[0]["BRANCH"].ToString();
            }
            return Json(viewMDL);
        }

        [HttpPost]
        public JsonResult DeleteData(string resourceID)
        {
            string returns = "Not Deleted please try again later.";
            int val = bl.DeleteResoureceMaster(resourceID);
            if (val > 0)
            {
                returns = "Deleted Successfully.";
            }
            return Json(returns);
        }

        public ActionResult ExportResourcelist(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["ExportResourceMaster"] = TempData["ExportResourceMaster"];
            TempData.Keep();

            if (ViewData["ExportResourceMaster"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), ViewData["ExportResourceMaster"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), ViewData["ExportResourceMaster"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), ViewData["ExportResourceMaster"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), ViewData["ExportResourceMaster"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), ViewData["ExportResourceMaster"]);
                    default:
                        break;
                }
            }
            //TempData["Exportcounterist"] = TempData["Exportcounterist"];
            //TempData.Keep();
            return null;
        }

        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Resource";
            // settings.CallbackRouteValues = new { Controller = "Employee", Action = "ExportEmployee" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Resource";

            settings.Columns.Add(column =>
            {
                column.Caption = "Resource Type";
                column.FieldName = "RESTYPE_NAME";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Name";
                column.FieldName = "RESOURCE_NAME";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Unit";
                column.FieldName = "BRANCH_NAME";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Entered By";
                column.FieldName = "CREATE_NAME";

            });


            settings.Columns.Add(column =>
            {
                column.Caption = "Entered On";
                column.FieldName = "CREATE_DATE";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Modified By";
                column.FieldName = "UPDATE_NAME";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Modified On";
                column.FieldName = "UPDATE_DATE";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy hh:mm:ss";
            });

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

        public ActionResult GetcrmProducts(string SearchKey)
        {
            List<pmsPosProductModel> listCust = new List<pmsPosProductModel>();
            if (Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");


                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable("select top 10  sProductsID as Products_ID,Products_Name ,Products_Description ,HSNSAC  from v_Product_MargeDetailsPOS where Products_Name like '%" + SearchKey + "%'  or Products_Description  like '%" + SearchKey + "%' order by Products_Name,Products_Description");


                listCust = (from DataRow dr in cust.Rows
                            select new pmsPosProductModel()
                            {
                                id = dr["Products_ID"].ToString(),
                                Na = dr["Products_Name"].ToString(),
                                Des = Convert.ToString(dr["Products_Description"]),
                                HSN = Convert.ToString(dr["HSNSAC"])
                            }).ToList();
            }

            return Json(listCust);
        }

        public ActionResult GetCustomer(string SearchKey, string contactType)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();

                //if (contactType == "Customer")
                //{
                //    cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid where pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' ) as t order by t.Name");
                //}
                //else if (contactType == "Contact")
                //{
                //    cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_VendorTransporterDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                //}
                //else
                //{

                //}

                cust = oDBEngine.GetDataTable(" select distinct top 10  cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing from v_pos_customerDetailsForPMS where Name like '%" + SearchKey + "%' AND Type='" + contactType + "'");
                listCust = (from DataRow dr in cust.Rows
                            select new CustomerModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return Json(listCust);
        }
    }
}