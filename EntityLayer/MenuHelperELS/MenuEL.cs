using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.MenuHelperELS
{
    public class MenuEL
    {
        public int mnu_id { get; set; }

        public string mnu_menuName { get; set; }

        public string mnu_menuLink { get; set; }

        public int mun_parentId { get; set; }

        public int mnu_segmentId { get; set; }
        public string RightsToCheck { get; set; }
    }

    public class UserMenuListModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public string IconClass { get; set; }

        public string Link { get; set; }

        public bool HaveRights { get; set; }
        public string mnu_menuPrefix { get; set; }
        public string newMenuClass { get; set; }

        public bool isNewTab { get; set; } 
    }
}
