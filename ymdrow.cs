using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadFileMultiThread
{
    public class ymdrow
    {
        public String ymd { get; set; }

        public String value1{get;set;}

        public String value2 { get; set; }

        public ymdrow(String y,String v1,String v2)
        {
            this.ymd = y;
            this.value1 = v1;
            this.value2 = v2;
        }
    }
}