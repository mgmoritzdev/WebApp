using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp4.Models;

namespace WebApp4.Controllers
{
    public class GetDataFromFileSystemController : Controller
    {
        PreferenceContext prefs = new PreferenceContext();
        MyFileDbContext db = new MyFileDbContext();

        enum PreferencesIndexes
        {
            ORIGIN_INDEX = 1,
            TARGET_INDEX = 2
        };

        // GET: GetDataFromFileSystem
        public ActionResult Index()
        {
            ViewBag.Preferences1 = getCurrentDirectory(PreferencesIndexes.ORIGIN_INDEX);
            ViewBag.Preferences2 = getCurrentDirectory(PreferencesIndexes.TARGET_INDEX);
            while (ViewBag.Preferences1 == null || ViewBag.Preferences2 == null)
            {
                initializePreferences();
            }

            return View();
        }

        public ActionResult GetFileList()
        {
            ViewBag.Success = "";
            ViewBag.Error = "";

            // clear all entries
            List<MyFile> files = db.MyFiles.ToList<MyFile>();
            foreach (MyFile file in files)
            {
                db.MyFiles.Remove(file);
                db.SaveChanges();
            }

            // get the list of new entries
            string directoryName = getCurrentDirectory(PreferencesIndexes.ORIGIN_INDEX).selectedFolder;
            string[] fileList = null;
            if (directoryName != null && directoryName != "")
            {
                fileList = Directory.GetFiles(directoryName);
            }
            else
            {
                ViewBag.Error = "Diretório inválido.";
            }

            if (fileList != null && fileList.Length > 0)
            {

                foreach (string file in fileList)
                {
                    Debug.WriteLine("file: " + file);
                    try
                    {
                        MyFile f = null;
                        if (ModelState.IsValid)
                        {
                            FileInfo fi = new FileInfo(file);
                            f = saveFileInfoIntoDatabase(fi);
                        }
                    }
                    catch
                    {
                        ViewBag.Error("Falha ao acessar o banco de dados!");
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.Error("Nenhum arquivo carregado!");
                return View();
            }

            ViewBag.Success = "Lista de arquivos armazenadas com sucesso.";
            return View();
        }

        public ActionResult SelectFolder()
        {
            Preference p = getCurrentDirectory(PreferencesIndexes.ORIGIN_INDEX);
            p.selectedFolder = Helpers.FileHelpers.selectDirectory(p.selectedFolder);

            prefs.Entry(p).State = System.Data.Entity.EntityState.Modified;
            prefs.SaveChanges();

            Debug.WriteLine("Selected Path: " + p.selectedFolder);
            return RedirectToAction("Index");
        }

        private void initializePreferences()
        {
            Preference p1 = new Preference();
            Preference p2 = new Preference();

            p1.selectedFolder = (Directory.GetCurrentDirectory());
            p2.selectedFolder = (Directory.GetCurrentDirectory());

            prefs.Preferences.Add(p1);
            prefs.Preferences.Add(p2);

            prefs.SaveChanges();

            ViewBag.Preferences1 = getCurrentDirectory(PreferencesIndexes.ORIGIN_INDEX);
            ViewBag.Preferences2 = getCurrentDirectory(PreferencesIndexes.TARGET_INDEX);
        }
        
        private Preference getCurrentDirectory(PreferencesIndexes index)
        {
            return prefs.Preferences.Find((int)index);
        }

        private MyFile saveFileInfoIntoDatabase(FileInfo fi)
        {
            MyFile f = new MyFile();
            f.dir = fi.Directory.FullName;
            f.name = fi.Name;
            f.size = (int)fi.Length;
            db.MyFiles.Add(f);
            db.SaveChanges();

            return f;
        }
    }
}
