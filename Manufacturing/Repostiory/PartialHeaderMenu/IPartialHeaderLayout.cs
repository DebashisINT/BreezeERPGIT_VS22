using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Manufacturing.Repostiory.PartialHeaderMenu
{
    public interface IPartialHeaderLayout
    {
        DataSet ViewLayoutHeader(ref string retmsg,string company_id, string branch_id, string user_id);
    }
}