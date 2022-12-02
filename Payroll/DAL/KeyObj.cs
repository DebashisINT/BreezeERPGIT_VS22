using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class KeyObj
    {
        public string key { get; set; }
        public object Obj { get; set; }

        public bool isReturn { get; set; }

        public KeyObj(string key, object obj)
        {
            this.key = key;
            this.Obj = obj;
        }
        public KeyObj(string key, object obj,bool returnType)
        {
            this.key = key;
            this.Obj = obj;
            this.isReturn = returnType;
        }
    }
}
