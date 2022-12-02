using BusinessLogicLayer.UserGroupsBLS;
using EntityLayer.UserGroupsEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class AjaxController : Controller
    {
        //
        // GET: /Ajax/

        public PartialViewResult _PartialGroupUserListForShow(int GroupId)
        {
            ViewBag.GroupId = GroupId;
            List<GroupUserListModel> model = new UserGroupBL().GetUsersByGroupIdKeyValue(GroupId);
            return PartialView(model);
        }
        public PartialViewResult _PartialContactPersonListForShow(string agentInternalId)
        {
            ViewBag.agentInternalId = agentInternalId;
            List<GetContactPersListModel> model = new UserGroupBL().GetContactlistByIdKeyValue(agentInternalId);
            return PartialView(model);
        }
	}
}