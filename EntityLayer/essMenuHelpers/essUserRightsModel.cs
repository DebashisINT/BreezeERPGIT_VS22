using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.essMenuHelpers
{
    public class UserGroupSaveModelEss
    {
        public int grp_id { get; set; }

        public int grp_segmentId { get; set; }

        public string grp_name { get; set; }

        public int? CreateUser { get; set; }

        public int? LastModifyUser { get; set; }

        public string UserGroupRights { get; set; }

        public string mode { get; set; }
    }

    public class MenuELEss  
    {
        public int mnu_id { get; set; }

        public string mnu_menuName { get; set; }

        public string mnu_menuLink { get; set; }

        public int mun_parentId { get; set; }

        public int mnu_segmentId { get; set; }
        public string RightsToCheck { get; set; }

        
    }

    public class RightELESS
    {
        public RightELESS()
        {
            //this.Map_UserGroup_Rights = new HashSet<Map_UserGroup_RightsEL>();
        }

        public int Id { get; set; }
        public string Rights { get; set; }
        public Nullable<bool> IsActive { get; set; }

    }
}
