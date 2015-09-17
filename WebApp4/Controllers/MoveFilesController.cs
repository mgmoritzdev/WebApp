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
    public class MoveFilesController : Controller
    {

        PreferenceContext prefs = new PreferenceContext();
        MyFileDbContext db = new MyFileDbContext();

        enum PreferencesIndexes
        {
            ORIGIN_INDEX = 1,
            TARGET_INDEX = 2
        };

        // GET: MoveFiles
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

        public ActionResult SelectOriginFolder()
        {
            Preference p = getCurrentDirectory(PreferencesIndexes.ORIGIN_INDEX);
            p.selectedFolder = Helpers.FileHelpers.selectDirectory(p.selectedFolder);

            prefs.Entry(p).State = System.Data.Entity.EntityState.Modified;
            prefs.SaveChanges();

            Debug.WriteLine("Selected Path: " + p.selectedFolder);
            return RedirectToAction("Index");
        }

        public ActionResult SelectTargetFolder()
        {
            Preference p = getCurrentDirectory(PreferencesIndexes.TARGET_INDEX);
            p.selectedFolder = Helpers.FileHelpers.selectDirectory(p.selectedFolder);

            prefs.Entry(p).State = System.Data.Entity.EntityState.Modified;
            prefs.SaveChanges();

            Debug.WriteLine("Selected Path: " + p.selectedFolder);
            return RedirectToAction("Index");
        }

        public ActionResult MoveFiles()
        {
            ViewBag.Success = "";
            ViewBag.Error = "";

            string originDir = getCurrentDirectory(PreferencesIndexes.ORIGIN_INDEX).selectedFolder;
            string targetDir = getCurrentDirectory(PreferencesIndexes.TARGET_INDEX).selectedFolder;

            try 
            {
                foreach (string file in Directory.GetFiles(originDir))
                {
                    string newFilename = targetDir + Path.DirectorySeparatorChar + Path.GetFileName(file);
                    if (!Directory.Exists(targetDir))
                    {
                        Console.WriteLine("The directory didn't exist and will be created.");
                        Directory.CreateDirectory(targetDir);
                    }
                    System.IO.File.Move(file, newFilename);
                }
            }
            catch (IOException e)
            {
                Debug.WriteLine("IOException trying to move files." + e.Data);
                ViewBag.Error = "Erro tentando mover os arquivos.";
            }
            finally
            {
                ViewBag.Success = "Arquivos movidos com sucesso.";
            }

            
            return View();
        }

        public ActionResult CheckMove()
        {
            string originDir = getCurrentDirectory(PreferencesIndexes.ORIGIN_INDEX).selectedFolder;
            string targetDir = getCurrentDirectory(PreferencesIndexes.TARGET_INDEX).selectedFolder;

            string[] fileListOrigin = null;
            string[] fullNamesTarget = null;
            string[] filenamesOnly = null;

            ViewBag.targetIsUpdated = true;



            // should be empty
            if (originDir != null && originDir != "")
            {
                fileListOrigin = Directory.GetFiles(originDir);
            }

            // should have all files on database
            if (targetDir != null && targetDir != "")
            {
                fullNamesTarget = Directory.GetFiles(targetDir);
            }

            // extract filenames w/o path from full names
            filenamesOnly = new string[fullNamesTarget.Length];
            for (int i = 0; i < fullNamesTarget.Length; i++)
            {
                filenamesOnly[i] = Path.GetFileName(fullNamesTarget[i]);
            }

            List<MyFile> filesOnDb = db.MyFiles.ToList();
            foreach (MyFile file in filesOnDb)
            {
                int pos = Array.IndexOf(filenamesOnly, file.name);
                if (!(pos < 0))
                {
                    ConfirmMove cm = new ConfirmMove();
                    cm.confirmed = true;
                    cm.myFileId = file.id;
                    db.ConfirmMoves.Add(cm);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.targetIsUpdated = false;
                }
            }

            ViewBag.originIsEmpty = (fileListOrigin.Length == 0);         

            return View();
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
    }
}