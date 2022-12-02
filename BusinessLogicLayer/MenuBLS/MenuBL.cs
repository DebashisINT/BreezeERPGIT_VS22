using DataAccessLayer;
using EntityLayer.CommonELS;
using EntityLayer.MenuHelperELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer.MenuBLS
{
    public class UserMenuHelperProcedures
    {
        public const string Proc_MenuHelper = "[dbo].[Proc_MenuHelper]";
    }

    public class MenuBL
    {
        public List<MenuEL> GetAllMenus(int SegmentId)
        {
            ProcedureExecute Proc = new ProcedureExecute(UserMenuHelperProcedures.Proc_MenuHelper);
            Proc.AddPara("@mnu_segmentId", SegmentId);
            Proc.AddPara("@mode", Proc_MenuHelper_Mode.GetAllMenu.ToString());
            DataTable dt = Proc.GetTable();
            return DbHelpers.ToModelList<MenuEL>(dt);
        }

        public List<RightEL> GetAllRights()
        {
            ProcedureExecute Proc = new ProcedureExecute(UserMenuHelperProcedures.Proc_MenuHelper);

            Proc.AddPara("@mode", "GetAllRights");
            DataTable dt = Proc.GetTable();
            return DbHelpers.ToModelList<RightEL>(dt);
        }
        public List<MenuListModel> GetUserMenuListByGroup()
        {
            if (HttpContext.Current.Session["usergoup"] != null)
            {
                try
                {
                    MasterSettings masterbl = new MasterSettings();
                    string mastersettings = masterbl.GetSettings("isServiceManagementRequred");
                    int UserGroupId = Convert.ToInt32(HttpContext.Current.Session["usergoup"]);
                    ProcedureExecute Proc = new ProcedureExecute(UserMenuHelperProcedures.Proc_MenuHelper);
                    Proc.AddPara("@groupid", UserGroupId);
                    Proc.AddPara("@mode", Proc_MenuHelper_Mode.GetUserMenuList.ToString());
                    Proc.AddPara("@showService", mastersettings); DataTable dt = Proc.GetTable();
                    List<UserMenuListModel> AllMenus = DbHelpers.ToModelList<UserMenuListModel>(dt);

                    if (AllMenus != null && AllMenus.Count() > 0)
                    {
                        List<MenuListModel> MenuModelList = new List<MenuListModel>();


                        List<UserMenuListModel> ParentMenus = AllMenus.Where(t => t.ParentId == 0).ToList();

                        foreach (UserMenuListModel parent in ParentMenus)
                        {
                            bool ParentInclude = false;

                            MenuListModel ParentMenu = new MenuListModel();

                            ParentMenu.IconClass = parent.IconClass;
                            ParentMenu.Id = parent.Id;
                            ParentMenu.Link = parent.Link;
                            ParentMenu.Name = parent.Name;
                            ParentMenu.ParentId = 0;
                            ParentMenu.Menu_Prefix = parent.mnu_menuPrefix;
                            ParentMenu.newMenuClass = parent.newMenuClass;

                            List<UserMenuListModel> Level1Menus = AllMenus.Where(t => t.ParentId == parent.Id).ToList();

                            if (Level1Menus != null && Level1Menus.Count() > 0)
                            {
                                foreach (UserMenuListModel Level1Menu in Level1Menus)
                                {
                                    bool Lavel1Include = false;

                                    Lavel1Menu tempLavel1Menu = new Lavel1Menu();

                                    tempLavel1Menu.IconClass = Level1Menu.IconClass;
                                    tempLavel1Menu.Id = Level1Menu.Id;
                                    tempLavel1Menu.Link = Level1Menu.Link;
                                    tempLavel1Menu.Name = Level1Menu.Name;
                                    tempLavel1Menu.ParentId = Level1Menu.ParentId;
                                    tempLavel1Menu.Menu_Prefix = Level1Menu.mnu_menuPrefix;
                                    tempLavel1Menu.newMenuClass = Level1Menu.newMenuClass;
                                    tempLavel1Menu.isNewTab = Level1Menu.isNewTab; 

                                    List<UserMenuListModel> Level2Menus = AllMenus.Where(t => t.ParentId == Level1Menu.Id && t.HaveRights == true).ToList();

                                    if (Level2Menus != null && Level2Menus.Count() > 0)
                                    {
                                        ParentInclude = true;
                                        Lavel1Include = true;

                                        foreach (UserMenuListModel Level2Menu in Level2Menus)
                                        {
                                            Lavel2Menu tempLavel2Menu = new Lavel2Menu()
                                            {
                                                IconClass = Level2Menu.IconClass,
                                                Id = Level2Menu.Id,
                                                Link = Level2Menu.Link,
                                                Name = Level2Menu.Name,
                                                ParentId = Level2Menu.ParentId,
                                                Menu_Prefix = Level2Menu.mnu_menuPrefix,
                                                newMenuClass = Level2Menu.newMenuClass

                                            };

                                            tempLavel1Menu.Lavel2Menus.Add(tempLavel2Menu);
                                        }
                                    }
                                    else
                                    {
                                        if (Level1Menu.HaveRights)
                                        {
                                            ParentInclude = true;
                                            Lavel1Include = true;
                                        }
                                    }

                                    if (Lavel1Include)
                                    {
                                        ParentMenu.Lavel1Menu.Add(tempLavel1Menu);
                                    }
                                }
                            }

                            if (ParentInclude)
                            {
                                MenuModelList.Add(ParentMenu);
                            }
                        }

                        return MenuModelList;
                    }
                }
                catch
                {
                    return new List<MenuListModel>();
                }
            }
            return new List<MenuListModel>();
        }


        public List<RightEL> GetRights(string AllowedRights, List<RightEL> AllRights)
        {

            if (string.IsNullOrWhiteSpace(AllowedRights))
            {
                return new List<RightEL>();
            }

            try
            {
                string[] rghtsArr = AllowedRights.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (rghtsArr != null && rghtsArr.Length > 0)
                {
                    List<int> RightIds = new List<int>();
                    foreach (var item in rghtsArr)
                    {
                        int tempId = 0;
                        if (!string.IsNullOrWhiteSpace(item) && int.TryParse(item.Trim(), out tempId))
                        {
                            RightIds.Add(tempId);
                        }
                    }

                    if (RightIds != null && RightIds.Count() > 0)
                    {
                        if (AllRights.Where(t => RightIds.Contains(t.Id)).Count() > 0)
                        {
                            return AllRights.Where(t => RightIds.Contains(t.Id)).ToList();
                        }
                    }
                }

                return new List<RightEL>();
            }
            catch (Exception ex)
            {

                return new List<RightEL>();
            }
        }
    }

    public enum Proc_MenuHelper_Mode
    {
        GetAllMenu = 1,
        GetUserMenuList = 2
    }
}
