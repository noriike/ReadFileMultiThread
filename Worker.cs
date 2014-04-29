using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
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

            foreach (var d in datas)
            {
                StreamReader reader = new StreamReader(d);
                FileInfo finfo = new FileInfo(d);
                string f = Path.GetFileNameWithoutExtension(finfo.Name);
                int v1 = 0;
                int v2 = 0;
                while (!reader.EndOfStream)
                {
                    String[] l=reader.ReadLine().Split(',');

                    v1 =v1 + int.Parse(l[0]);
                    v2 =v2 + int.Parse(l[1]);
                }
                reader.Close();
                
                lr.Add(new ymdrow(f, v1.ToString(), v2.ToString()));
            }

            return lr;
        }
    }
}