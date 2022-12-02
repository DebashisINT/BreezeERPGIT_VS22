using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportModuleBusinessLayer
{
   public  class Documentattachment
    {
       public DataTable DocumentfetchbyType(string doctype)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Sp_Import_PopulateDocument");
           proc.AddPara("@doctype", doctype);
           dt = proc.GetTable();
           return dt;

       }
       public DataTable GetEditedData(string docid)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Sp_Import_PopulateDocument");
           proc.AddPara("@Action", "GetDetails");
           proc.AddPara("@DocID", docid);
           dt = proc.GetTable();
           return dt;

       }

       public DataTable DocumentMultipleImage(DataTable dtdocumentdetails, string User, string DocId,string  DocNumber,string DocName,string Doctype)
       {

           ProcedureExecute proc = new ProcedureExecute("Sp_Import_DocumentSave");
           proc.AddPara("@Createduser", User);
           proc.AddPara("@tbl_ImportDocument", dtdocumentdetails);
           proc.AddPara("@Action", "Save");
           proc.AddPara("@DocId", DocId);
           proc.AddPara("@DocNummber", DocNumber);
           proc.AddPara("@Doctype", Doctype);
           DataTable dtproduct = proc.GetTable();
           return dtproduct;

       }


       //public  DataTable DocumentAttachmentfetch()
       //{

       //    DataTable dt = new DataTable();
       //    ProcedureExecute proc = new ProcedureExecute("Proc_Import_DocumentAttachmentBind");
       //     dt = proc.GetTable();
       //    return dt;

       //}


       public DataTable DocumentAttachmentfetch()
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Proc_Import_DocumentAttachmentBind");
           proc.AddPara("@Action", "Bind");
           dt = proc.GetTable();
           return dt;

       }
       public static DataTable DocumentAttachmentfetchByID(string Id)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Proc_Import_DocumentAttachmentBind");
           proc.AddPara("@Action", "BindByID");
           proc.AddPara("@ID", Id);
           dt = proc.GetTable();
           return dt;

       }

       public DataTable DelteAttachment(string DociD)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Sp_Import_DocumentSave");
           proc.AddPara("@Action", "DeleteAttachment");
           proc.AddPara("@DocId", DociD);
           dt = proc.GetTable();
           return dt;

       }
    }
}
