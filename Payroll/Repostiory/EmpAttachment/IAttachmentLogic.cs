using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.EmpAttachment
{
    public interface IAttachmentLogic
    {
        void AttachmentModify(p_EmpAttactchmentEngine model,DataTable dt, ref int strIsComplete, ref string strMessage);
    }
}