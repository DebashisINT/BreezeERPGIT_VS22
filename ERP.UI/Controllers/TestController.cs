using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        public ActionResult ProjectPagePathList()
        {
            //DirectoryInfo d = new DirectoryInfo(@"D:\Workspace\Projects\ERP\ERP.UI");//Assuming Test is your Folder
            //FileInfo[] Files = d.GetFiles("*"); //Getting Text files
            //string str = "";
            //foreach (FileInfo file in Files)
            //{
            //    str = str + ", " + file.Name;
            //}

            string str = "";
            foreach (string file in GetFiles(@"D:\Workspace\Projects\ERP\ERP.UI\OMS"))
            {
                str = str + ", " + file;
            }

            ViewBag.PageList = str;
            return View();
        }


        static IEnumerable<string> GetFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
        }

        public PartialViewResult _PartialWebFormToMvcTest()
        {
            return PartialView();
        }
	}
}