using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EntityLayer
{
    public class Menu
    {
        public int MenuId { get; set; }
        public int SubMenuId { get; set; }
      
        public List<string> MenuList { get; set; }
    }
}
