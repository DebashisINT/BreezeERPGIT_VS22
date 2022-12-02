using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.PartialHeaderMenu
{
    public interface IPartialHeader
    {
        DataSet ViewLayoutHeader(ref string retmsg, string company_id, string branch_id, string user_id);
    }
}