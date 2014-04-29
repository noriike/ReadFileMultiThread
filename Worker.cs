using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadFileMultiThread
{
    public class Worker
    {
        //処理対象データ
        public List<string> datas { get; set; }

        public Worker(List<string> d)
        {
            datas = d;
        }

        public List<ymdrow> Dowork()
        {
            List<ymdrow> lr = new List<ymdrow>();
            lr.Add(new ymdrow(datas.Count.ToString(), datas.Count.ToString(), datas.Count.ToString()));

            return lr;
        }
    }
}