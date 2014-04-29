using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace ReadFileMultiThread
{
    public class logic
    {
        const int MAX_THREAD = 4;

        delegate List<ymdrow> delegateworker();
        List<ymdrow> result = new List<ymdrow>();

        public void ExecuteAsync()
        {
            List<string> allfiles = new List<string>();
            String [] directories=Directory.GetDirectories(@"C:\ssi2\log");

            foreach (var d in directories)
            {
                String[] f = Directory.GetFiles(d);

                allfiles.AddRange(f);
            }


            //１スレッドに処理させるファイル数
            int chunksize = allfiles.Count / MAX_THREAD;

            //ファイル数をスレッド数で均等割りにできない分 最後のスレッドに割り当て
            int remain = allfiles.Count % MAX_THREAD;

            //スレッド数分、分配したファイルを格納する
            List<delegateworker> wl = new List<delegateworker>();
            for (int i = 1; i <= MAX_THREAD; i++)
            {
                List<string> d = new List<string>();

                int fileindex = (i - 1) * chunksize;

                if (i == MAX_THREAD)
                {
                    d.AddRange(allfiles.GetRange(fileindex, chunksize + remain));
                }
                else
                {
                    d.AddRange(allfiles.GetRange(fileindex, chunksize));
                }

                Worker w = new Worker(d);
                wl.Add(new delegateworker(w.Dowork));
            }

            //並列処理開始
            List<IAsyncResult> arl = new List<IAsyncResult>();
            foreach (var w in wl)
            {
                arl.Add(w.BeginInvoke(new AsyncCallback(WorkComplete), null));
            }

            //並列処理がすべて終わるまで待機
            while (arl.Where(a =>!a.IsCompleted).Any()) ;

            var aaa = "aa";

        }

        public void WorkComplete(IAsyncResult ar)
        {
            delegateworker dw =(delegateworker)((AsyncResult)ar).AsyncDelegate;

            //ロックが必要？？
            result.AddRange(dw.EndInvoke(ar));
        }
    }
}