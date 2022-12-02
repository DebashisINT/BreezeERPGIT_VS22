using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace ImportModuleBusinessLayer
{
    public class Documenttemplate
    {

        public DataTable DocumenttemplateManage(string data, string templatename, string bodyhtml, string remrks, string type, Boolean Defaultcheck, string Id = null)
        {

            DataTable ds = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("Sp_DocumenttemplateSave");
            proc.AddPara("@data", data);
            proc.AddPara("@templatename", templatename);
            proc.AddPara("@bodyhtml", bodyhtml);
            proc.AddPara("@remrks", remrks);
            proc.AddPara("@type", type);
            proc.AddPara("@Defaultcheck", Defaultcheck);
            proc.AddPara("@Id", Id);

            ds = proc.GetTable();
            return ds;
        }


        public static DataTable GetEmailTags(string Type)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_ImportDocument_Template_Helper");
            proc.AddPara("@Typeid", Type);
            proc.AddPara("@Action", "Tags");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetImportDetails()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_ImportDocument_Template_Helper");
            proc.AddPara("@Action", "ImportTagsdetails");

            ds = proc.GetTable();
            return ds;
        }

        public static DataTable GetImportDetailsByID(string ID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_ImportDocument_Template_Helper");
            proc.AddPara("@Action", "ImportTagsdetailsModify");
            proc.AddPara("@ImportTagId", ID);
            ds = proc.GetTable();
            return ds;
        }

        public static DataTable DeleteTemplate(string ID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Sp_DocumenttemplateSave");
            proc.AddPara("@data", "Delete");
            proc.AddPara("@Id", ID);
            ds = proc.GetTable();
            return ds;
        }

    }


    public class ImportTags
    {
        public string ImportTag { get; set; }
        public int Id { get; set; }
        public int StageId { get; set; }
    }


    public class CommonResult
    {
        public object AddonData { get; set; }

    }


    public class ImportTemplate
    {
        public string ID { get; set; }
        public string Template { get; set; }
        public string DocType { get; set; }
        public bool IsDefault { get; set; }
        public string Body { get; set; }
        public string Remarks { get; set; }
    }

}
