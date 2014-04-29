using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadFileMultiThread
{
    public class ymdrow:IComparable<ymdrow>
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

        public int CompareTo(ymdrow other)
        {
            if(other==null) return 1;

            if (other.ymd == ymd)
            {
                return 0;
            }
            else if (int.Parse(other.ymd) > int.Parse(ymd))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}