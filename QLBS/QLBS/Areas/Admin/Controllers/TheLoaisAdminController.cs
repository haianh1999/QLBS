using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLBS.Models;

namespace QLBS.Areas.Admin.Controllers
{
    public class TheLoaisAdminController : Controller
    {
        private LTQLDBContext db = new LTQLDBContext();
        ReadDataFromExcelFile excelPro = new ReadDataFromExcelFile();
        // GET: Admin/TheLoaisAdmin
        public ActionResult Index()
        {
            return View(db.TheLoais.ToList());
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            DataTable dt = CopyDataFromExcelFile(file);
            OverwriteFastData(dt);
            return RedirectToAction("Index", "TheLoaisAdmin");
        }

        public DataTable CopyDataFromExcelFile(HttpPostedFileBase file)
        {
            string fileExtention = file.FileName.Substring(file.FileName.IndexOf("."));
            string _FileName = "TheLoai" + fileExtention; //Tên file Excel
            string _path = Path.Combine(Server.MapPath("~/Uploads/Excels"), _FileName);
            file.SaveAs(_path);
            DataTable dt = excelPro.ReadDataFromExcelFiles(_path, true);
            return dt;
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LTQLDBContext"].ConnectionString);
        private void OverwriteFastData(DataTable dt)
        {
            SqlBulkCopy bulkCopy = new SqlBulkCopy(con);
            bulkCopy.DestinationTableName = "TheLoais";
            bulkCopy.ColumnMappings.Add(0, "TenTheLoai");
            con.Open();
            bulkCopy.WriteToServer(dt);
            con.Close();
        }
        // GET: Admin/TheLoaisAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheLoai theLoai = db.TheLoais.Find(id);
            if (theLoai == null)
            {
                return HttpNotFound();
            }
            return View(theLoai);
        }

        // GET: Admin/TheLoaisAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TheLoaisAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTheLoai,TenTheLoai")] TheLoai theLoai)
        {
            if (ModelState.IsValid)
            {
                db.TheLoais.Add(theLoai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(theLoai);
        }

        // GET: Admin/TheLoaisAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheLoai theLoai = db.TheLoais.Find(id);
            if (theLoai == null)
            {
                return HttpNotFound();
            }
            return View(theLoai);
        }

        // POST: Admin/TheLoaisAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTheLoai,TenTheLoai")] TheLoai theLoai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(theLoai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(theLoai);
        }

        // GET: Admin/TheLoaisAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheLoai theLoai = db.TheLoais.Find(id);
            if (theLoai == null)
            {
                return HttpNotFound();
            }
            return View(theLoai);
        }

        // POST: Admin/TheLoaisAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TheLoai theLoai = db.TheLoais.Find(id);
            db.TheLoais.Remove(theLoai);
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
