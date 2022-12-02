using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class KeyValues
    {
        public string Key { get; set; }
        public string value { get; set; }
        public KeyValues(string key, string value)
        {
            this.Key = key;
            this.value = value;
        }
    }
}
