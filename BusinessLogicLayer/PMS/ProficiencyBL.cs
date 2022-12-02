using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer.PMS
{
    public class ProficiencyBL
    {
        public DataSet DropDownDetailForProficiency()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROFICIENCY_LOAD");
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable InsertProficiency(string ProficiencyID, string ProficiencyNAME, string Ratable_Entity, string Min_Rate, string Max_Rate, string RatingName, string Rating_Value, string IsDefault,
            String RATING1,String RATING2,String RATING3,String RATING4,String RATING5,String RATING6,String RATING7,String RATING8,String RATING9,String RATING10,int DEFAULTVALU)
        {
            DataTable ret = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROFICIENCY_ADDEDIT");
            proc.AddNVarcharPara("@ProficiencyID", 10, ProficiencyID);
            proc.AddNVarcharPara("@ProficiencyNAME", 100, ProficiencyNAME);
            proc.AddNVarcharPara("@Ratable_Entity", 50, Ratable_Entity);
            proc.AddNVarcharPara("@Min_Rate", 10, Min_Rate);
            proc.AddNVarcharPara("@Max_Rate", 10, Max_Rate);
            proc.AddNVarcharPara("@RatingName", 100, RatingName);
            proc.AddNVarcharPara("@Rating_Value", 100, Rating_Value);
            proc.AddNVarcharPara("@IsDefault", 10, IsDefault);
            proc.AddNVarcharPara("@RATING1", 100, RATING1);
            proc.AddNVarcharPara("@RATING2", 100, RATING2);
            proc.AddNVarcharPara("@RATING3", 100, RATING3);
            proc.AddNVarcharPara("@RATING4", 100, RATING4);
            proc.AddNVarcharPara("@RATING5", 100, RATING5);
            proc.AddNVarcharPara("@RATING6", 100, RATING6);
            proc.AddNVarcharPara("@RATING7", 100, RATING7);
            proc.AddNVarcharPara("@RATING8", 100, RATING8);
            proc.AddNVarcharPara("@RATING9", 100, RATING9);
            proc.AddNVarcharPara("@RATING10", 100, RATING10);
            proc.AddIntegerPara("@DEFAULTVALU", DEFAULTVALU);
            proc.AddNVarcharPara("@CREATE_BY", 10, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddNVarcharPara("@UPDATE_BY", 10, Convert.ToString(HttpContext.Current.Session["userid"]));

            ret = proc.GetTable();
            return ret;
        }

        public DataTable GetProficiencyList()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROFICIENCY_LIST");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable ViewProficiency(string ProficiencyID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROFICIENCY_VIEW");
            proc.AddNVarcharPara("@ProficiencyID", 10, ProficiencyID);
            ds = proc.GetTable();
            return ds;
        }

        public int DeleteProficiency(string ProficiencyID)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_PROFICIENCY_DELETE");
            proc.AddNVarcharPara("@ProficiencyID", 10, ProficiencyID);
            ret = proc.RunActionQuery();
            return ret;
        }

    }
}
