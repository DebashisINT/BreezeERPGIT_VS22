using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.MenuHelperELS
{
    public class MenuListModel
    {
        public MenuListModel()
        {
            Lavel1Menu = new List<Lavel1Menu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string IconClass { get; set; }
        public string Link { get; set; }
        public string Menu_Prefix { get; set; }
        public string newMenuClass { get; set; }
        public bool isNewTab { get; set; }
        public List<Lavel1Menu> Lavel1Menu { get; set; }
    }

    public class Lavel1Menu
    {
        public Lavel1Menu()
        {
            Lavel2Menus = new List<Lavel2Menu>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string IconClass { get; set; }
        public string Link { get; set; }
        public string Menu_Prefix { get; set; }
        public string newMenuClass { get; set; }
        public bool isNewTab { get; set; }
        public List<Lavel2Menu> Lavel2Menus { get; set; }
    }

    public class Lavel2Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string IconClass { get; set; }
        public string Link { get; set; }
        public string newMenuClass { get; set; }
        public bool isNewTab { get; set; }
        public string Menu_Prefix { get; set; }
    }
}
