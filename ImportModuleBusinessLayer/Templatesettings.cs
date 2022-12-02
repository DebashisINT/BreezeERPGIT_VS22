using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImportModuleBusinessLayer
{
   public   class Templatesettings
    {

       public DataTable GetTemplates(string TypeId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Import_Templatehelper");
            proc.AddPara("@Action", "Gettemplates");
            proc.AddPara("@ID", TypeId);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetTemplatetags(string Id, string Action)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Import_Templatehelper");
            proc.AddPara("@Action", Action);
            proc.AddPara("@Id", Id);

            dt = proc.GetTable();
            return dt;
        }


        public static string GetFormattedString<T>(Object myObj, string StringToBeFormatted)
        {
            string FormattedString = "";
            Type temp = typeof(T);
            Type objectType = myObj.GetType();
            T obj = Activator.CreateInstance<T>();

            foreach (PropertyInfo pro in temp.GetProperties())
            {
                object value = myObj.GetType().GetProperty(pro.Name).GetValue(myObj, null);
                string ParamName = "@" + pro.Name + "@";
                string ValueToBeInserted = "";
                if (value != null)
                {
                    ValueToBeInserted = value.ToString();
                }
                StringToBeFormatted = StringToBeFormatted.Replace(ParamName, ValueToBeInserted);
            }

            FormattedString = StringToBeFormatted;

            return FormattedString;
        }




    }
}
