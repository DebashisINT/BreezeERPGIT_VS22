using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.CommonELS
{
    public class CommonResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object AddonData { get; set; }
    }
}
