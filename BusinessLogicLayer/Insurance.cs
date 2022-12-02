using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class Insurance
    {
        public DataSet GridContactPerson_BL(string name, string Officephone, string Residencephone, string stringMobilephone, string email, 
            string cp_designation, string cp_relationShip, string cp_status, string cp_Pan, string cp_Din, string contactid, string userid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ContactPersonUpdateforInsComp");
            proc.AddVarcharPara("@name", 100, name);
            proc.AddVarcharPara("@Officephone", 100, Officephone);
            proc.AddVarcharPara("@Residencephone", 100, Residencephone);
            proc.AddVarcharPara("@Mobilephone", 100, stringMobilephone);
            proc.AddVarcharPara("@email", 250, email);
            proc.AddNVarcharPara("@cp_designation", 250, cp_designation);
            proc.AddVarcharPara("@cp_relationShip", 100, cp_relationShip);
            proc.AddVarcharPara("@cp_status", 100, cp_status);
            proc.AddVarcharPara("@cp_Pan", 100, cp_Pan);
            proc.AddVarcharPara("@cp_Din", 100, cp_Din);
            proc.AddVarcharPara("@contactid", 100, contactid);
            proc.AddIntegerPara("@userid", Convert.ToInt32(userid));
            ds = proc.GetDataSet();
            return ds;
        }
    }
}
