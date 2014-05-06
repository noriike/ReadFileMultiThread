using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;
namespace ReadFileMultiThread
{
    public class logic
    {
        private int MAX_THREAD = 4;

        delegate List<ymdrow> delegateworker();
        delegateworker dw;
        List<ymdrow> result = new List<ymdrow>();

        public void ExecuteAsync(String path)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            

            result=new List<ymdrow>();

            List<string> allfiles = new List<string>();
            String [] directories=Directory.GetDirectories(path);

            foreach (var d in directories)
            {
                String[] f = Directory.GetFiles(d);

                allfiles.AddRange(f);
            }

            //スレッド数よりファイル数が少なかった場合、ファイル数をスレッド数にする
            MAX_THREAD = allfiles.Count < MAX_THREAD ? allfiles.Count : MAX_THREAD;

            //１スレッドに処理させるファイル数
            int chunksize = allfiles.Count / MAX_THREAD;

            //ファイル数をスレッド数で均等割りにできない分 最後のスレッドに追加で割り当て
            int remain = allfiles.Count % MAX_THREAD;

            //スレッド数分、分配したファイルを格納する
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
                dw += w.Dowork;
            }

            //並列処理開始
            var arl = dw.GetInvocationList().Select(d => 
                                                       (d as delegateworker).BeginInvoke(new AsyncCallback(WorkComplete), null))
                                            .ToArray();

            //並列処理がすべて終わるまで待機
            while (arl.Where(a =>!a.IsCompleted).Any()) ;

            //以下、処理結果の結合処理
            var gr = result.Select(f => f)
                           .GroupBy(f => new { f.ymd })
                           .Select(fg =>
                                    new ymdrow(fg.Key.ToString(),
                                               fg.Sum(w => int.Parse(w.value1)).ToString(), 
                                               fg.Sum(w => int.Parse(w.value2)).ToString()
                                               )
                                   )
                           .OrderBy(f => f.ymd);

            sw.Stop();
            Debug.WriteLine(sw.ElapsedMilliseconds);
        }

        public void WorkComplete(IAsyncResult ar)
        {
            delegateworker dw =(delegateworker)((AsyncResult)ar).AsyncDelegate;
            Object lockObj = new object();

            //ロックが必要？？
            lock (lockObj)
            {
                //返り値を格納する
                result.AddRange(dw.EndInvoke(ar));
            }
        }
    }
}