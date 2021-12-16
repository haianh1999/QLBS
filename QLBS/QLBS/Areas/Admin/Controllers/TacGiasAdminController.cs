using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLBS.Models;

namespace QLBS.Areas.Admin.Controllers
{
    public class TacGiasAdminController : Controller
    {
        private LTQLDBContext db = new LTQLDBContext();
        ReadDataFromExcelFile excelPro = new ReadDataFromExcelFile();
        // GET: Admin/TacGiasAdmin
        public ActionResult Index()
        {
            return View(db.TacGias.ToList());
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            DataTable dt = CopyDataFromExcelFile(file);
            OverwriteFastData(dt);
            return RedirectToAction("Index", "TacGiasAdmin");
        }

        public DataTable CopyDataFromExcelFile(HttpPostedFileBase file)
        {
            string fileExtention = file.FileName.Substring(file.FileName.IndexOf("."));
            string _FileName = "Tacgias" + fileExtention; //Tên file Excel
            string _path = Path.Combine(Server.MapPath("~/Uploads/Excels"), _FileName);
            file.SaveAs(_path);
            DataTable dt = excelPro.ReadDataFromExcelFiles(_path, true);
            return dt;
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LTQLDBContext"].ConnectionString);
        private void OverwriteFastData(DataTable dt)
        {
            SqlBulkCopy bulkCopy = new SqlBulkCopy(con);
            bulkCopy.DestinationTableName = "TacGias";
            bulkCopy.ColumnMappings.Add(0, "TenTacGia");            
            con.Open();
            bulkCopy.WriteToServer(dt);
            con.Close();
        }

        // GET: Admin/TacGiasAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TacGia tacGia = db.TacGias.Find(id);
            if (tacGia == null)
            {
                return HttpNotFound();
            }
            return View(tacGia);
        }

        // GET: Admin/TacGiasAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TacGiasAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTacGia,TenTacGia")] TacGia tacGia)
        {
            if (ModelState.IsValid)
            {
                db.TacGias.Add(tacGia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tacGia);
        }

        // GET: Admin/TacGiasAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TacGia tacGia = db.TacGias.Find(id);
            if (tacGia == null)
            {
                return HttpNotFound();
            }
            return View(tacGia);
        }

        // POST: Admin/TacGiasAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTacGia,TenTacGia")] TacGia tacGia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tacGia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tacGia);
        }

        // GET: Admin/TacGiasAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TacGia tacGia = db.TacGias.Find(id);
            if (tacGia == null)
            {
                return HttpNotFound();
            }
            return View(tacGia);
        }

        // POST: Admin/TacGiasAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TacGia tacGia = db.TacGias.Find(id);
            db.TacGias.Remove(tacGia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
