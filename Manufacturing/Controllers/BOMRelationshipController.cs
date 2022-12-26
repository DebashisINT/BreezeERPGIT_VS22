using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using Manufacturing.Models;
using Manufacturing.Models.ViewModel;
using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using UtilityLayer;

namespace Manufacturing.Controllers
{
    public class BOMRelationshipController : Controller
    {
        BomRelationshipModel objModel = null;
        BomRelationshipViewModel objdata = null;
        DBEngine oDBEngine = new DBEngine();
        UserRightsForPage rights = new UserRightsForPage();
        Int64 BOMRelationID = 0;
        public BOMRelationshipController()
        {
            objModel = new BomRelationshipModel();
            objdata = new BomRelationshipViewModel();
        }  
        public ActionResult BOMRelationship()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BOMRelationship", "BOMRelationship");
            BomRelationshipViewModel obj = new BomRelationshipViewModel();           
            ViewBag.CanAdd = rights.CanAdd;
            return View("~/Views/BOMRelationship/BOMRelationship.cshtml", obj);
        }
        public ActionResult BomEntry(string ActionType, Int64 BOMRelationID = 0)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BOMRelationship", "BOMRelationship");
            string EntryType = Request.QueryString["ActionType"];
            try
            {
            TempData["BOMRelationID"] = null;
            TempData["BOMChildDetails"] = null;
            List<BranchUnit> list = new List<BranchUnit>();
            var datasetobj = objModel.DropDownDetailForBOMRelation("GetUnitDropDownData", Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["userbranchHierarchy"]), 0, 0);
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
            objdata.UnitList = list;
            if (ActionType != "ADD")
                    {
                        if (BOMRelationID != null)
                        {
                           
                            if (ActionType == "View")
                            {
                                TempData["View"] = ActionType;
                                ViewBag.View = Convert.ToString(TempData["View"]);
                            }
                            else
                            if (ActionType == "EDIT")
                            {
                                TempData["Edit"] = ActionType;
                                ViewBag.View = Convert.ToString(TempData["Edit"]);
                            }


                            TempData["BOMRelationID"] = BOMRelationID;
                            objdata.BOMRelation_ID = Convert.ToString(TempData["BOMRelationID"]);                           
                            
                            TempData.Keep();

                            if (Convert.ToInt64(objdata.BOMRelation_ID) > 0)
                            {
                                DataTable objData = objModel.GetBOMRelationHeaderListByID("GetBOMHeaderData", Convert.ToInt64(objdata.BOMRelation_ID));
                                if (objData != null && objData.Rows.Count > 0)
                                {
                                    DataTable dt = objData;
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        objdata.BOMRelation_ID = Convert.ToString(row["BOMRelation_ID"]);

                                        objdata.BomRelationshipNo = Convert.ToString(row["BOMRelation_No"]);
                                        objdata.BomRelationshipName = Convert.ToString(row["BOMRelation_Name"]);                                        
                                        objdata.Unit = Convert.ToString(row["BRANCH_ID"]);
                                        objdata.ParentBOMID = Convert.ToString(row["ParentBOM_ID"]);
                                        objdata.ParentBOMNo = Convert.ToString(row["BOM_No"]);
                                        objdata.ParentBOMFG = Convert.ToString(row["ParentBOM_FG"]);
                                        objdata.ParentBOMREV = Convert.ToString(row["ParentBOM_REV"]);

                                        ViewBag.ParentBOMID = Convert.ToString(row["Details_ID"]);
                                        ViewBag.Unit = Convert.ToString(row["BRANCH_ID"]);                                    

                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
            return View("~/Views/BOMRelationship/BOMEntry.cshtml", objdata);
        }
        public ActionResult GetBomCenterList()
        {
            List<BomRelationshipViewModel> list = new List<BomRelationshipViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BOMRelationship", "BOMRelationship");
            try
            {
                Int64 BranchID = 0;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                DataTable dt = new DataTable();
                if (TempData["BranchID"] != null && TempData["FromDate"] != null && TempData["ToDate"] != null)
                {
                    BranchID = Convert.ToInt64(TempData["BranchID"]);
                    FromDate = Convert.ToDateTime(TempData["FromDate"]);
                    ToDate = Convert.ToDateTime(TempData["ToDate"]);
                    TempData.Keep();
                }
                if (TempData["BranchID"] != null && TempData["FromDate"] != null && TempData["ToDate"] != null)
                {
                    if (BranchID > 0)
                    {
                        dt = oDBEngine.GetDataTable("select * from V_BOMRelationDetailsList where BRANCH_ID =" + BranchID + " AND (CreateDate BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "')  ORDER BY BOMRelation_ID DESC ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_BOMRelationDetailsList where CreateDate BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "'  ORDER BY BOMRelation_ID DESC ");
                    }

                }


                TempData["EstimateDetailsListDataTable"] = dt;

                if (dt.Rows.Count > 0)
                {
                    BomRelationshipViewModel obj = new BomRelationshipViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new BomRelationshipViewModel();
                        obj.BOMRelation_ID = Convert.ToString(item["BOMRelation_ID"]);
                        obj.BomRelationshipNo = Convert.ToString(item["BOMRelation_No"]);
                        obj.BomRelationshipName = Convert.ToString(item["BOMRelation_Name"]);                     

                        obj.Unit = Convert.ToString(item["BranchDescription"]);
                        obj.ParentBOMNo = Convert.ToString(item["BOM_No"]);
                        obj.ParentBOMFG = Convert.ToString(item["ParentBOM_FG"]);
                        obj.ParentBOMREV = Convert.ToString(item["ParentBOM_REV"]);
                        obj.CreatedBy = Convert.ToString(item["CreatedBy"]);
                        obj.ModifyBy = Convert.ToString(item["ModifyBy"]);
                        obj.CreateDate = Convert.ToDateTime(item["CreateDate"]);
                        if (Convert.ToString(item["ModifyDate"]) != "")
                        {
                            obj.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                        }
                        else
                        {
                            obj.ModifyDate = null;
                        }
                       
                        list.Add(obj);
                    }
                }
            }
            catch { }
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            //ViewBag.CanAddUpdateDocuments = rights.CanAddUpdateDocuments;
            return PartialView("~/Views/BOMRelationship/_bomList.cshtml", list);
        }
        
        [WebMethod]
        public JsonResult AddBOMChild(BOMChild prod)
        {
            DataTable dt = (DataTable)TempData["BOMChildDetails"];
            DataTable dt2 = new DataTable();

            if (dt == null)
            {
                DataTable dtable = new DataTable();              

                dtable.Clear();
                dtable.Columns.Add("HIddenID", typeof(System.Guid));
                dtable.Columns.Add("SlNO", typeof(System.String));
                dtable.Columns.Add("BOMChildID", typeof(System.String));
                dtable.Columns.Add("ChildBOM", typeof(System.String));
                dtable.Columns.Add("ChildBOMFG", typeof(System.String));
                dtable.Columns.Add("ChildBOMREV", typeof(System.String));                
                dtable.Columns.Add("UpdateEdit", typeof(System.String));
                dtable.Columns.Add("ChildBOM_ID", typeof(System.String));

                object[] trow = { Guid.NewGuid(), 1,prod.BOMChildID,prod.ChildBOM,prod.ChildBOMFG,prod.ChildBOMREV,prod.UpdateEdit,prod.ChildBOM_ID
                                    };
                dtable.Rows.Add(trow);
                TempData["BOMChildDetails"] = dtable;
                TempData.Keep();
            }
            else
            {
                if (string.IsNullOrEmpty(prod.Guids))
                {
                    object[] trow = { Guid.NewGuid(), Convert.ToInt32(dt.Rows.Count)+1,prod.BOMChildID,prod.ChildBOM,prod.ChildBOMFG,prod.ChildBOMREV,prod.UpdateEdit,prod.ChildBOM_ID
                                     };// Add new parameter Here
                    dt.Rows.Add(trow);
                    TempData["BOMChildDetails"] = dt;
                    TempData.Keep();
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (prod.Guids.ToString() == item["HIddenID"].ToString())
                            {

                                item["BOMChildID"] = prod.BOMChildID;
                                item["ChildBOM"] = prod.ChildBOM;
                                item["ChildBOMFG"] = prod.ChildBOMFG;
                                item["ChildBOMREV"] = prod.ChildBOMREV;                                
                                item["UpdateEdit"] = "1";
                                item["ChildBOM_ID"] = prod.ChildBOM_ID;                                                                  
                            }                  
                        }
                    }
                }
                TempData["BOMChildDetails"] = dt;
                TempData.Keep();
            }
            return Json("");
        }

        public ActionResult GetChildBOMEntryList()
        {
            BOMChild BOMChilddataobj = new BOMChild();
            List<BOMChild> BOMChilddata = new List<BOMChild>();
            Int64 BOMRelationID = 0;
            try
            {

                if (TempData["BOMRelationID"] != null)
                {
                    BOMRelationID = Convert.ToInt64(TempData["BOMRelationID"]);
                    TempData.Keep();
                }
                DataTable dt = new DataTable();
                if (BOMRelationID > 0 && TempData["BOMChildDetails"] == null)
                {
                    DataTable objData = objModel.GetChildEntryListByID("GetEntryChildBOMData", BOMRelationID);
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        dt = objData;

                        DataTable dtable = new DataTable();

                        dtable.Clear();
                        dtable.Columns.Add("HIddenID", typeof(System.Guid));
                        dtable.Columns.Add("SlNO", typeof(System.String));
                        dtable.Columns.Add("BOMChildID", typeof(System.String));
                        dtable.Columns.Add("ChildBOM", typeof(System.String));
                        dtable.Columns.Add("ChildBOMFG", typeof(System.String));
                        dtable.Columns.Add("ChildBOMREV", typeof(System.String));                        
                        dtable.Columns.Add("UpdateEdit", typeof(System.String));
                        dtable.Columns.Add("ChildBOM_ID", typeof(System.String));
                        dtable.Columns.Add("ParentBOM_ID", typeof(System.String));

                        String Gid = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            Gid = Guid.NewGuid().ToString();
                            BOMChilddataobj = new BOMChild();
                            BOMChilddataobj.SlNO = Convert.ToString(row["SlNO"]);
                            BOMChilddataobj.BOMChildID = Convert.ToString(row["Details_ID"]);
                            BOMChilddataobj.ChildBOM = Convert.ToString(row["BOM_No"]);
                            BOMChilddataobj.ChildBOMFG = Convert.ToString(row["ChildBOM_FG"]);
                            BOMChilddataobj.ChildBOMREV = Convert.ToString(row["ChildBOM_REV"]);                           
                            BOMChilddataobj.Guids = Gid;
                            BOMChilddataobj.ChildBOM_ID = Convert.ToString(row["ChildBOM_ID"]); 
                            BOMChilddata.Add(BOMChilddataobj);



                            object[] trow = { Gid, row["SlNO"],Convert.ToString(row["Details_ID"]),Convert.ToString(row["BOM_No"]),
                                                Convert.ToString(row["ChildBOM_FG"]),Convert.ToString(row["ChildBOM_REV"]),"1" ,Convert.ToString(row["ChildBOM_ID"]),Convert.ToString(row["ParentBOM_ID"])                                                  
                             
                                            };
                            dtable.Rows.Add(trow);
                        }                      

                        dt = dtable;                       
                    }
                }
                else
                {
                    dt = (DataTable)TempData["BOMChildDetails"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            BOMChilddataobj = new BOMChild();
                            BOMChilddataobj.SlNO = Convert.ToString(row["SlNO"]);
                            BOMChilddataobj.BOMChildID = Convert.ToString(row["BOMChildID"]);
                            BOMChilddataobj.ChildBOM = Convert.ToString(row["ChildBOM"]);
                            BOMChilddataobj.ChildBOMFG = Convert.ToString(row["ChildBOMFG"]);
                            BOMChilddataobj.ChildBOMREV = Convert.ToString(row["ChildBOMREV"]);
                            BOMChilddataobj.Guids = Convert.ToString(row["HIddenID"]);
                            BOMChilddataobj.ChildBOM_ID = Convert.ToString(row["ChildBOM_ID"]); 
                            BOMChilddata.Add(BOMChilddataobj);
                           
                        }
                        
                    }
                }
                TempData["BOMChildDetails"] = dt;
                TempData.Keep();

            }
            catch { }
            return PartialView("~/Views/BOMRelationship/_BOMProductEntryGrid.cshtml", BOMChilddata);
        }
        public class BOMChild
        {
            public String SlNO { get; set; }     

            public String BOMChildID { get; set; }
            public String ChildBOM { get; set; }
            public String ChildBOMFG { get; set; }

            public String ChildBOMREV { get; set; }            
            public String UpdateEdit { get; set; }         

            public String Guids { get; set; }

            public String ChildBOM_ID { get; set; }
        }

        public class BOMChildList
        {
            public String SlNO { get; set; }
            public String BOMChildID { get; set; }
            public String ChildBOM { get; set; }
            public String ChildBOMFG { get; set; }
            public String ChildBOMREV { get; set; }
            public String UpdateEdit { get; set; }
            public String Guids { get; set; }
            public String ChildBOM_ID { get; set; }

        }

        //public JsonResult ParentBOM()
        //{
        //    List<ParentBOMList> list = new List<ParentBOMList>();
        //    DataTable ParentBOMdt = objModel.GetParentBOM();
        //    if (ParentBOMdt.Rows.Count > 0 )
        //    {
        //        ParentBOMList obj = new ParentBOMList();
        //        foreach (DataRow item in ParentBOMdt.Rows)
        //        {
        //            obj = new ParentBOMList();
        //            obj.ID = Convert.ToString(item["ID"]);
        //            obj.BOM_No = Convert.ToString(item["BOM_No"]);                    
        //            list.Add(obj);
        //        }
        //    }

        //    return Json(list);
        //}

        public ActionResult GetParentBOM(BomRelationshipViewModel model, string ParentBOMID, String Branchs)
        {
            try
            {
                String Branch = "";
                if (model.Unit != null)
                {
                    Branch = model.Unit;
                }
                else
                {
                    Branch = Branchs;
                }               
                List<ParentBOMList> modelParentBOM = new List<ParentBOMList>();
                DataTable ParentBOMdt = objModel.GetParentBOM(Branch);
                if (ParentBOMdt.Rows.Count > 0)
                {
                    modelParentBOM = APIHelperMethods.ToModelList<ParentBOMList>(ParentBOMdt);
                    ViewBag.ParentBOMID = ParentBOMID;
                }

                return PartialView("~/Views/BOMRelationship/_PartialParentBOM.cshtml", modelParentBOM);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult GetChildBOM(BomRelationshipViewModel model, string ParentBOMID,string ChildBOMID, String Branchs)
        {
            try
            {
                String Branch = "";
                if (model.Unit != null)
                {
                    Branch = model.Unit;
                }
                else
                {
                    Branch = Branchs;
                }
               
                string Parent_BOMID="";
                if (model.ParentBOMID != null)
                {
                    string _ParentBOMID = model.ParentBOMID;
                    Parent_BOMID = _ParentBOMID.Split('~')[0];
                }
                else
                {
                    Parent_BOMID = ParentBOMID;
                }


                if (ChildBOMID == "")
                {
                    ChildBOMID = model.ChildBOMID;
                }
                

              
                List<ParentBOMList> modelParentBOM = new List<ParentBOMList>();
                DataTable ParentBOMdt = objModel.GetChildBOM(Branch, Parent_BOMID);
                if (ParentBOMdt.Rows.Count > 0)
                {
                    modelParentBOM = APIHelperMethods.ToModelList<ParentBOMList>(ParentBOMdt);
                    ViewBag.ChildBOMID = ChildBOMID;
                }

                return PartialView("~/Views/BOMRelationship/_PartialChildBOM.cshtml", modelParentBOM);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        //public JsonResult ChildBOM(string ParentBOMID)
        //{
        //    List<ParentBOMList> list = new List<ParentBOMList>();
        ////    DataTable ParentBOMdt = objModel.GetChildBOM(ParentBOMID);
        //    //if (ParentBOMdt.Rows.Count > 0)
        //    //{
        //    //    ParentBOMList obj = new ParentBOMList();
        //    //    foreach (DataRow item in ParentBOMdt.Rows)
        //    //    {
        //    //        obj = new ParentBOMList();
        //    //        obj.ID = Convert.ToString(item["ID"]);
        //    //        obj.BOM_No = Convert.ToString(item["BOM_No"]);
        //    //        list.Add(obj);
        //    //    }
        //    //}

        //    return Json(list);
        //}

        [WebMethod]
        public JsonResult EditData(String HiddenID)
        {
            BOMChildList ret = new BOMChildList();

            DataTable dt = (DataTable)TempData["BOMChildDetails"];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {                      

                        ret.SlNO = item["SlNO"].ToString();
                        ret.BOMChildID = item["BOMChildID"].ToString();
                        ret.ChildBOM = item["ChildBOM"].ToString();
                        ret.Guids = item["HIddenID"].ToString();
                        ret.ChildBOMFG = item["ChildBOMFG"].ToString();
                        ret.ChildBOMREV = item["ChildBOMREV"].ToString();
                      //  ViewBag.ParentBOMID = Convert.ToString(item["ParentBOM_ID"]);
                        ViewBag.ChildBOMID = Convert.ToString(item["BOMChildID"]);
                        break;
                    }
                }
            }
            TempData["BOMChildDetails"] = dt;
            TempData.Keep();
           
            return Json(ret);
        }
        [WebMethod]
        public JsonResult UniqueBomRelationshipNo(string RelationshipNo)
        {
            DataTable dt = new DataTable();
           
            string IsPresent = "";
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            dt = oGeneric.GetDataTable("SELECT COUNT(BOMRelation_No) AS BOMRelation_No FROM BOMRelationship_Header WHERE BOMRelation_No = '" + RelationshipNo + "'");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["BOMRelation_No"]) > 0)
                {
                    IsPresent = "1";
                }
            }
            //return IsPresent;
            String retuenMsg = IsPresent;
            return Json(retuenMsg);
        }
        [WebMethod]
        public JsonResult DeleteData(string HiddenID)
        {
            DataTable dt = (DataTable)TempData["BOMChildDetails"];
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

            if (dt != null && dt.Rows.Count > 0)
            {
                int conut = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["SlNO"] = conut;
                    conut++;
                }
            }

            TempData["BOMChildDetails"] = dt;
            TempData.Keep();
            return Json("Device Remove Successfully.");
        }

        [WebMethod]
        public JsonResult SaveBOMRelaton(BomRelationshipViewModel Details)
        {
           
            String Message = "";
            bool Success = false;

            DataTable dtChildBOM = new DataTable();
            dtChildBOM.Columns.Add("ChildBOM_ID");
            dtChildBOM.Columns.Add("ChildBOM_FG");
            dtChildBOM.Columns.Add("ChildBOM_REV");

            DataTable dt_ChildBOM = (DataTable)TempData["BOMChildDetails"];

            List<udtChildBOM> udt = new List<udtChildBOM>();
            if (dt_ChildBOM != null)
            {
                foreach (DataRow item in dt_ChildBOM.Rows)
                {
                    udtChildBOM obj1 = new udtChildBOM();
                    obj1.ChildBOM_ID = Convert.ToInt64(item["ChildBOM_ID"]);
                    obj1.ChildBOM_FG = Convert.ToString(item["ChildBOMFG"]);
                    obj1.ChildBOM_REV = (item["ChildBOMREV"].ToString());

                    udt.Add(obj1);
                }
            }


            DataTable dtEstimate_PRODUCTS = new DataTable();
            dtEstimate_PRODUCTS = ToDataTable(udt);

            string validate = "";
            var duplicateRecords = dtEstimate_PRODUCTS.AsEnumerable()
               .GroupBy(r => r["ChildBOM_ID"]) //coloumn name which has the duplicate values
               .Where(gr => gr.Count() > 1)
               .Select(g => g.Key);

            foreach (var d in duplicateRecords)
            {
                validate = "duplicateBOM";
            }
            DataSet dt = new DataSet();
            if (validate != "duplicateBOM")
            {


               
                if (Convert.ToInt64(Details.BOMRelation_ID) > 0)
                {

                    dt = objModel.BOMRelationEntryInsertUpdate("UPDATEMAINPRODUCT", Convert.ToString(Details.BomRelationshipNo), Convert.ToString(Details.BomRelationshipName), Convert.ToInt32(Details.Unit),
                        dtEstimate_PRODUCTS, Convert.ToInt32(Details.ParentBOMID)
                       , Details.ParentBOMFG, Details.ParentBOMREV,
                        Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToInt64(Session["userid"]), Convert.ToInt64(Details.BOMRelation_ID)
                         );
                }
                else
                {
                    // if (NumberScheme == "ok")
                    //  {   
                    dt = objModel.BOMRelationEntryInsertUpdate("INSERTMAINPRODUCT", Convert.ToString(Details.BomRelationshipNo), Convert.ToString(Details.BomRelationshipName), Convert.ToInt32(Details.Unit),
                    dtEstimate_PRODUCTS, Convert.ToInt32(Details.ParentBOMID)
                   , Details.ParentBOMFG, Details.ParentBOMREV,
                    Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToInt64(Session["userid"]), Convert.ToInt64(Details.BOMRelation_ID)
                     );
                    //}
                    //else
                    //{
                    //    Message = NumberScheme;
                    //}
                }

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        Success = Convert.ToBoolean(row["Success"]);
                        if(Success== false)
                        {
                            TempData["BOMChildDetails"] = dt_ChildBOM;
                            BOMRelationID = Convert.ToInt32("-15");
                        }
                        else
                        {
                            BOMRelationID = Convert.ToInt32(row["DetailsID"]);
                        }
                       
                       
                    }
                }
            }            
            else
            {
                TempData["BOMChildDetails"] = dt_ChildBOM;
                Success =  false;
                BOMRelationID = Convert.ToInt32("-10");
            }

            String retuenMsg = Success + "~" + BOMRelationID ;
            return Json(retuenMsg);
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

        public JsonResult RemoveEstimateDataByID(Int32 detailsid)
        {
            ReturnData obj = new ReturnData();
          
            try
            {
                var datasetobj = objModel.DeleteForBOMRelation("RemoveBOMRelationData", detailsid);
                if (datasetobj.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow item in datasetobj.Tables[0].Rows)
                    {
                        obj.Success = Convert.ToBoolean(item["Success"]);
                        obj.Message = Convert.ToString(item["Message"]);
                    }
                }
            }
            catch { }
            return Json(obj);
        }

        public JsonResult SetEstimateDateFilter(Int64 unitid, string FromDate, string ToDate)
        {
            Boolean Success = false;
            try
            {
                TempData["BranchID"] = unitid;
                TempData["FromDate"] = FromDate;
                TempData["ToDate"] = ToDate;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }
        public ActionResult ExportMPSGridList(int type)
        {
            ViewData["EstimateDetailsListDataTable"] = TempData["EstimateDetailsListDataTable"];

            TempData.Keep();
            DataTable dt = (DataTable)TempData["EstimateDetailsListDataTable"];
            if (ViewData["EstimateDetailsListDataTable"] != null && dt.Rows.Count > 0)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetEstimateGridView(ViewData["EstimateDetailsListDataTable"]), ViewData["EstimateDetailsListDataTable"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetEstimateGridView(ViewData["EstimateDetailsListDataTable"]), ViewData["EstimateDetailsListDataTable"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetEstimateGridView(ViewData["EstimateDetailsListDataTable"]), ViewData["EstimateDetailsListDataTable"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetEstimateGridView(ViewData["EstimateDetailsListDataTable"]), ViewData["EstimateDetailsListDataTable"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetEstimateGridView(ViewData["EstimateDetailsListDataTable"]), ViewData["EstimateDetailsListDataTable"]);
                    default:
                        break;
                }
                return null;
            }
            else
            {
                return this.RedirectToAction("GetBomCenterList", "BOMRelationship");
            }
        }

        private GridViewSettings GetEstimateGridView(object datatable)
        {
           
            var settings = new GridViewSettings();
            settings.Name = "BOMRelation";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "BOMRelation";            
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "BOMRelation_No"
                    || datacolumn.ColumnName == "BOMRelation_Name"
                    || datacolumn.ColumnName == "BranchDescription" || datacolumn.ColumnName == "BOM_No" || datacolumn.ColumnName == "ParentBOM_FG"
                    || datacolumn.ColumnName == "ParentBOM_REV" || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "ModifyDate")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "BOMRelation_No")
                        {
                            column.Caption = "OM Relation No";
                            column.VisibleIndex = 0;
                        }

                        else if (datacolumn.ColumnName == "BOMRelation_Name")
                        {
                            column.Caption = "BOM Relation Name";
                            column.VisibleIndex = 2;

                        }                       
                        else if (datacolumn.ColumnName == "BranchDescription")
                        {
                            column.Caption = "Unit";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "BOM_No")
                        {
                            column.Caption = "Parent BOM No.";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "ParentBOM_FG")
                        {
                            column.Caption = "Parent BOM FG";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "ParentBOM_REV")
                        {
                            column.Caption = "Parent BOM REV";
                            column.VisibleIndex = 6;
                        }


                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 10;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 11;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 12;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 13;
                        }                        
                        column.FieldName = datacolumn.ColumnName;
                        if (datacolumn.DataType.FullName == "System.Decimal")
                        {
                            column.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                        if (datacolumn.DataType.FullName == "System.DateTime")
                        {
                            if (datacolumn.ColumnName == "ModifyDate")
                            {
                                column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy HH:mm:ss";
                            }
                            else
                            {
                                column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                            }
                        }
                    });
                }

            }

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

	}
}