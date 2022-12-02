using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
   public class FileUploadBl
    {
       public void CompanyLogoUpload( string bigLogoPath,string smallLogoPath , string CompanyInternalId)
       {
           ProcedureExecute proc;
           try
           {
              using (proc = new ProcedureExecute("prc_logoUpload"))
               {
                   proc.AddVarcharPara("@cmp_bigLogo", 200, bigLogoPath);
                   proc.AddVarcharPara("@cmp_smallLogo", 200, smallLogoPath);
                   proc.AddVarcharPara("@cmp_internalid", 50, CompanyInternalId);
                   int i = proc.RunActionQuery();
               }
           }

           catch (Exception ex)
           {
               throw ex;
           }

           finally
           {
               proc = null;
           }
       }
    }
}
