using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer.EmailTemplate
{
    public class Emailtemplate
    {


        public static DataTable GetEmailTags(string Type)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_Email_Template_Helper");
            proc.AddPara("@Typeid", Type);
            ds = proc.GetTable();
            return ds;
        }

        // Rev Sanchita
        //public static DataTable GetHeaderFooterTags(string Type)
        public static DataTable GetHeaderFooterTags()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_HeaderFooter_Template_Helper");
            //proc.AddPara("@Typeid", Type);
            ds = proc.GetTable();
            return ds;
        }
        // End of Rev Sanchita
     
    }
    public class Emailtags
    {
        public string EmailTags { get; set; }

        public int Id { get; set; }
        public int StageId { get; set; }
    }

    // Rev Sanchita
    public class HFtags
    {
        public string HFTags { get; set; }

        public int Id { get; set; }
        public int StageId { get; set; }
    }
    // End of Rev Sanchita

    public class CommonResult
    {

        public object AddonData { get; set; }
    }

}
