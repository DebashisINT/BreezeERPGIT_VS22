using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manufacturing.Controllers
{
    public class MfbomController : Controller
    {
        // GET: Mfbom
        public ActionResult AddEdit()
        {
            return View();
        }

        [HttpPost]
        public string GetNumberingScheme(string SearchKey)
        {
            ExecProcedure execProc = new ExecProcedure();
            List<KeyObj> paramList = new List<KeyObj>();
            execProc.ProcedureName = "prc_BomDetails";
            paramList.Add(new KeyObj("@action", "GetNumberingScheme"));
            paramList.Add(new KeyObj("@SearchKey", SearchKey));
            paramList.Add(new KeyObj("@branchlist", Convert.ToString(Session["userbranchHierarchy"]))); 
            execProc.param = paramList;
            DataTable dt = execProc.ExecuteProcedureGetTable(); 
            paramList.Clear();

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return  serializer.Serialize(rows);



        }
    }
}