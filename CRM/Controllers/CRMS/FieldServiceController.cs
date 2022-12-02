using BusinessLogicLayer;
using CRM.Models;
using CRM.Models.DataContext;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using UtilityLayer;

namespace CRM.Controllers.CRMS
{
    public class FieldServiceController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        CRMFieldService objdata = new CRMFieldService();
        public ActionResult Index()
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "FieldService");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanLiterature = rights.CanLiterature;
            ViewBag.CanCreateActivity = rights.CanSalesActivity;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            TempData["DetailsID"] = null;
            TempData["ProductsDetails"] = null;
            var querycrm_cntsource = (from c in dcon.v_crmContacts
                                       select c);
            objdata.crmcontacts = querycrm_cntsource.ToList();

            var querycrm_users = from c in dcon.V_UserLIsts
                                 select c;
            objdata.Users = querycrm_users.ToList();
            var querycrm_Assign = from c in dcon.V_UserLIsts
                                 select c;
            objdata.Assigners = querycrm_Assign.ToList();

            var queryCases_Assign = from c in dcon.V_CaseLists
                                  select c;
            objdata.Caseslist = queryCases_Assign.ToList();

            var querycrm_Project = from c in dcon.V_PROJECTLISTFieldServices
                                   //where c.ProjectStatus == "Approved"
                                  select c;
            objdata.ProjectList = querycrm_Project.ToList();
            var querycrm_Tech = from c in dcon.Technician_Reports
                                   select c;
            objdata.TechList = querycrm_Tech.ToList();

            objdata.OwnerID = Convert.ToInt64(Session["userid"]);
            objdata.Owner = Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]);
            objdata.AssignedID = Convert.ToInt64(Session["userid"]);
            objdata.AssignerName = Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]);
            ViewBag.OwnerAssignID = Convert.ToInt64(Session["userid"]);


            return View(@"~/Views/CRMS/FieldService/Index.cshtml", objdata);
        }
        public PartialViewResult PartialFieldServiceGrid(string frmdate)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "FieldService");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanLiterature = rights.CanLiterature;
            ViewBag.CanCreateActivity = rights.CanSalesActivity;
            return PartialView(@"~/Views/CRMS/FieldService/PartialGrid.cshtml", GetServicecontact(frmdate));
        }

        public ActionResult ExportCRMLeadsList(int type)
        {


            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), GetServicecontact("1"));
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), GetServicecontact("1"));
                //break;
                case 3:
                    return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), GetServicecontact("1"));
                case 4:
                    return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), GetServicecontact("1"));
                case 5:
                    return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), GetServicecontact("1"));
                //break;

                default:
                    break;
            }

            return null;
        }


        public ActionResult EditServiceContact(CRMFieldService newcrmLeadsobj)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            //DataTable Servicedata = objdata.GetDetailsEntryListByID("GetEstimateEntryDetailsData", Convert.ToInt64(objdata.ServiceCnt_Id))
            CRMFieldService CRMLeadsobj = new CRMFieldService();
            DataSet output = CRMLeadsobj.GetDetailsEntryListByID("GetServiceCNTDetails", Convert.ToInt64(newcrmLeadsobj.ServiceCnt_Id));
            TempData["DetailsID"] = Convert.ToInt64(newcrmLeadsobj.ServiceCnt_Id);
            TempData.Keep();
            CustomerDetails clsObj = new CustomerDetails();
            CRMLeadsobj = APIHelperMethods.ToModel<CRMFieldService>(output.Tables[0]);
            //clsObj = APIHelperMethods.ToModel<CustomerDetails>(output.Tables[1]);
            CRMLeadsobj.cntids = output.Tables[1].AsEnumerable()
                           .Select(r => r.Field<string>("Contact_id"))
                           .ToList(); ;
     




            return Json(CRMLeadsobj);
        }
        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "gvPaging";
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "FieldService";


            settings.Columns.Add(column =>
            {
                column.FieldName = "Service_UniqueId";
                column.Caption = "Unique Id";
                
            });



            settings.Columns.Add(column =>
            {
                column.FieldName = "Service_Name";
                column.Caption = "Name";
           
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Customer";
                column.Caption = "Customer";
              
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "ServiceType";
                column.Caption = "Service Type";
               
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Service_Date";
                column.Caption = "Service Date";
               
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "CallAttend_Date";
                column.Caption = "Call Att. Date";
            
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Technician";
                column.Caption = "Technician";
                
            });
        
            settings.Columns.Add(column =>
            {
                column.FieldName = "Proj_Code";
                column.Caption = "JOb Code";
            
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Proj_Name";
                column.Caption = "Job Name";
             
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Status";
                column.Caption = "Status";
               
            });
 

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }
        public IEnumerable GetServicecontact(string frmdate)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);


            if (frmdate != "Ispageload")
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                // Mantis Issue 24051
                //var q = from d in dc.v_CRMFieldServices
                //        // where Convert.ToString(d.Assign_To) == Userid || Convert.ToString(d.Owner_Id) == Userid
                //        orderby d.FieldService_id descending
                //        select d;
                //return q;

                BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
                string ShowFieldServiceDataForAllUsers = ComBL.GetSystemSettingsResult("ShowFieldServiceDataForAllUsers");

                if (ShowFieldServiceDataForAllUsers.ToUpper() == "NO")
                {
                    var q = from d in dc.v_CRMFieldServices
                            where Convert.ToString(d.Assign_Id) == Userid || Convert.ToString(d.Owner_Id) == Userid
                            orderby d.FieldService_id descending
                            select d;
                    return q;
                }
                else
                {
                    var q = from d in dc.v_CRMFieldServices
                            orderby d.FieldService_id descending
                            select d;
                    return q;
                }
                // End of Rev Mantis issue 24051 

            }
            else
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                var q = from d in dc.v_CRMFieldServices
                        where 1 == 0
                        select d;
                return q;
            }


        }

        public ActionResult PopulateSummaryContactGrid(CRMFieldService obj)
        {

            return PartialView(@"~/Views/CRMS/FieldService/PartialFieldSErviceContactGrid.cshtml", GetContactData(obj.Customer_ID));
        }

        public IEnumerable GetContactData(string CustId)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dc = new CRMClassDataContext(connectionString);
            var q = from d in dc.v_crmContacts
                    where Convert.ToString(d.Customer_ID) == CustId
                    orderby d.FirstName
                    select d;
            return q;
        }

        public DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {

                //Setting column names as Property names

                dataTable.Columns.Add(prop.Name);

            }

            foreach (T item in items)
            {

                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {

                    //inserting property values to datatable rows

                    values[i] = Props[i].GetValue(item, null);

                }

                dataTable.Rows.Add(values);

            }

            //put a breakpoint here and check datatable

            return dataTable;

        }

        public ActionResult GetServiceProductEntryList()
        {
            CRM.Models.CRMFieldService.ServiceContactProduct Estimateproductdataobj = new CRM.Models.CRMFieldService.ServiceContactProduct();
            List<CRM.Models.CRMFieldService.ServiceContactProduct> Estimateproductdata = new List<CRM.Models.CRMFieldService.ServiceContactProduct>();
            Int64 DetailsID = 0;

            try
            {
                DataTable dft = new DataTable();
                dft = (DataTable)TempData["ProductsDetails"];
                if (TempData["DetailsID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                    TempData.Keep();
                }
                DataTable dt = new DataTable();
                if (DetailsID > 0 && (dft == null || dft.Rows.Count < 0))
                {
                    DataTable objData = new DataTable();
                    objData = objdata.GetServiceContactProductEntryListByID("GetServiceCNTProductDetails", DetailsID);
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        dt = objData;

                        DataTable dtable = new DataTable();

                        dtable.Clear();
                        dtable.Columns.Add("HIddenID", typeof(System.Guid));
                        dtable.Columns.Add("ProductName", typeof(System.String));
                        dtable.Columns.Add("ProductId", typeof(System.String));

                        dtable.Columns.Add("ProductQty", typeof(System.String));

                        dtable.Columns.Add("Price", typeof(System.String));
                        dtable.Columns.Add("Amount", typeof(System.String));
                        dtable.Columns.Add("ProductDetailsID", typeof(System.String));
                        dtable.Columns.Add("WarrentyStartdate", typeof(System.String));
                        dtable.Columns.Add("WarrentyEnddate", typeof(System.String));
                        String Gid = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            Gid = Guid.NewGuid().ToString();
                            Estimateproductdataobj = new CRM.Models.CRMFieldService.ServiceContactProduct();

                            Estimateproductdataobj.ProductName = Convert.ToString(row["ProductName"]);
                            Estimateproductdataobj.ProductId = Convert.ToString(row["ProductId"]);

                            Estimateproductdataobj.ProductQty = Convert.ToString(row["ProductQty"]);

                            Estimateproductdataobj.Price = Convert.ToString(row["Price"]);
                            Estimateproductdataobj.Amount = Convert.ToString(row["Amount"]);

                            Estimateproductdataobj.ProductDetailsID = Convert.ToString(row["ProductDetailsID"]);
                            Estimateproductdataobj.WarrentyStartdate = Convert.ToString(row["WarrentyStartdate"]);
                            Estimateproductdataobj.WarrentyEnddate = Convert.ToString(row["WarrentyEnddate"]);
                            Estimateproductdataobj.HIddenID = Gid;

                            Estimateproductdata.Add(Estimateproductdataobj);


                            object[] trow = { Gid, Convert.ToString(row["ProductName"]),Convert.ToString(row["ProductID"]),
                                                Convert.ToString(row["ProductQty"]),Convert.ToString(row["Price"]),Convert.ToString(row["Amount"]),
                                    Convert.ToString(row["ProductDetailsID"]),Convert.ToString(row["WarrentyStartdate"]),Convert.ToString(row["WarrentyEnddate"]) };
                            dtable.Rows.Add(trow);
                        }
                        //TempData["ProdAddlDesc"] = prodAddlDesc;
                        //TempData.Keep();

                        dt = dtable;


                    }
                }
                else
                {
                    dt = (DataTable)TempData["ProductsDetails"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Estimateproductdataobj = new CRM.Models.CRMFieldService.ServiceContactProduct();

                            Estimateproductdataobj.ProductName = Convert.ToString(row["ProductName"]);
                            Estimateproductdataobj.ProductId = Convert.ToString(row["ProductId"]);

                            Estimateproductdataobj.ProductQty = Convert.ToString(row["ProductQty"]);

                            Estimateproductdataobj.Price = Convert.ToString(row["Price"]);
                            Estimateproductdataobj.Amount = Convert.ToString(row["Amount"]);


                            Estimateproductdataobj.ProductDetailsID = Convert.ToString(row["ProductDetailsID"]);
                            Estimateproductdataobj.WarrentyStartdate = Convert.ToString(row["WarrentyStartdate"]);
                            Estimateproductdataobj.WarrentyEnddate = Convert.ToString(row["WarrentyEnddate"]);
                            Estimateproductdataobj.HIddenID = Convert.ToString(row["HIddenID"]);
                            Estimateproductdata.Add(Estimateproductdataobj);
                        }

                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();

            }
            catch { }
            // return PartialView("~/Views/PMS/Estimate/_BOMProductEntryGrid.cshtml", Estimateproductdata);
            return PartialView("~/Views/CRMS/FieldService/PartialFieldProductGrid.cshtml", Estimateproductdata);
        }

        public ActionResult AddModeServiceContact()
        {
            TempData["ProductsDetails"] = null;
            TempData["DetailsID"] = null;
            return null;
        }


        [ValidateInput(false)]
        public ActionResult SaveServiceContact(CRMFieldService obj)
        {
            CRMFieldService CRMLeadsobj = new CRMFieldService();
            string output = "";

            DataTable dt_PRODUCTS = (DataTable)TempData["ProductsDetails"];


            List<CRM.Models.CRMFieldService.ServiceContactProduct> udt = new List<CRM.Models.CRMFieldService.ServiceContactProduct>();

            if (dt_PRODUCTS != null && dt_PRODUCTS.Rows.Count > 0)
            {
                foreach (DataRow item in dt_PRODUCTS.Rows)
                {
                    CRM.Models.CRMFieldService.ServiceContactProduct obj1 = new CRM.Models.CRMFieldService.ServiceContactProduct();
                    obj1.ProductName = Convert.ToString(item["ProductName"]);
                    obj1.ProductId = Convert.ToString(item["ProductId"]);
                    obj1.ProductQty = Convert.ToString(item["ProductQty"]); ;

                    obj1.Price = Convert.ToString(item["Price"]);
                    obj1.Amount = Convert.ToString(item["Amount"]);
                    obj1.ProductDetailsID = Convert.ToString(item["ProductDetailsID"]);
                    obj1.WarrentyStartdate = Convert.ToString(item["WarrentyStartdate"]);
                    obj1.WarrentyEnddate = Convert.ToString(item["WarrentyEnddate"]);

                    udt.Add(obj1);
                }
            }

            DataTable Bind_PRODUCTS = new DataTable();
            Bind_PRODUCTS = ToDataTable(udt);


            output = CRMLeadsobj.SaveFieldService(obj.ServiceCnt_Id, obj.Action, (obj.OwnerID), obj.ServiceStatusId, obj.AssignedID, obj.ServiceName, obj.UniqueId, obj.Customer_ID,
                           obj.ServiceDate, obj.Servicetype, obj.CallAttendDate, obj.TechnicianId, obj.JobId, obj.crmcontacts_id, Bind_PRODUCTS,obj.Fault_Description
                           ,obj.Balance,obj.CloseDate,obj.Remarks,obj.CASE_ID,obj.CaseName
                           );


            

            return Json(output, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public ActionResult GetProjectName(string ProjectId)
        {
            string ProjectName = "";
             DBEngine objDb = new DBEngine();
            DataTable dtproj = new DataTable();
            dtproj = objDb.GetDataTable("select top 1 ISNULL(Proj_Name,'') Proj_Name from V_ProjectList where ProjectStatus = 'Approved' and Proj_Id=" + Convert.ToInt64(ProjectId) + "");
            if(dtproj !=null && dtproj.Rows.Count>0)
            {
                ProjectName = Convert.ToString(dtproj.Rows[0]["Proj_Name"]);
            }
            return Json(ProjectName);
        }

        [WebMethod]
        public ActionResult GetCasesName(string CASE_ID)
        {
            string CasesName = "";
            DBEngine objDb = new DBEngine();
            DataTable dtproj = new DataTable();
            dtproj = objDb.GetDataTable("select top 1 ISNULL(TITLE,'')  CasesName  from V_CaseList where CASE_ID=" + Convert.ToInt64(CASE_ID) + "");
            if (dtproj != null && dtproj.Rows.Count > 0)
            {
                CasesName = Convert.ToString(dtproj.Rows[0]["CasesName"]);
            }
            return Json(CasesName);
        }

        [WebMethod]
        public JsonResult DeleteData(string HiddenID)
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        dt.Rows.Remove(item);
                        break;
                    }
                }
            }


            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json("Data Remove Successfully.");
        }
        public ActionResult DeleteServiceContact(Int64 Id)
        {
            CRMFieldService CRMLeadsobj = new CRMFieldService();
            string outPut = CRMLeadsobj.DeleteService(Id);
            return Json("Data Delete Successfully.");
        }

        [WebMethod]
        public JsonResult AddProduct(CRM.Models.CRMFieldService.ServiceContactProduct prod)
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            DataTable dt2 = new DataTable();

            if (dt == null || dt.Rows.Count==0)
            {
                DataTable dtable = new DataTable();

                dtable.Clear();
                dtable.Columns.Add("HIddenID", typeof(System.Guid));
                dtable.Columns.Add("ProductName", typeof(System.String));
                dtable.Columns.Add("ProductId", typeof(System.String));
                dtable.Columns.Add("ProductQty", typeof(System.String));
                dtable.Columns.Add("Price", typeof(System.String));
                dtable.Columns.Add("Amount", typeof(System.String));
                dtable.Columns.Add("ProductDetailsID", typeof(System.String));
                dtable.Columns.Add("WarrentyStartdate", typeof(System.String));
                dtable.Columns.Add("WarrentyEnddate", typeof(System.String));
                object[] trow = { Guid.NewGuid(),prod.ProductName,prod.ProductId,prod.ProductQty,prod.Price,prod.Amount,prod.ProductDetailsID,prod.WarrentyStartdate,
                                prod.WarrentyEnddate};
                dtable.Rows.Add(trow);
                TempData["ProductsDetails"] = dtable;
                TempData.Keep();
            }
            else
            {
                if (string.IsNullOrEmpty(prod.HIddenID))
                {
                    object[] trow = { Guid.NewGuid(), prod.ProductName,prod.ProductId,prod.ProductQty,prod.Price,prod.Amount,prod.ProductDetailsID,prod.WarrentyStartdate,
                                prod.WarrentyEnddate};// Add new parameter Here
                    dt.Rows.Add(trow);
                    TempData["ProductsDetails"] = dt;
                    TempData.Keep();
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (prod.HIddenID.ToString() == item["HIddenID"].ToString())
                            {
                                item["ProductName"] = prod.ProductName;
                                item["ProductId"] = prod.ProductId;

                                item["ProductQty"] = prod.ProductQty;

                                item["Price"] = prod.Price;
                                item["Amount"] = prod.Amount;

                                item["ProductDetailsID"] = prod.ProductDetailsID;
                                item["WarrentyStartdate"] = prod.WarrentyStartdate;
                                item["WarrentyEnddate"] = prod.WarrentyEnddate;
                            }
                        }
                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();
            }


            return Json("");
        }

	}
}