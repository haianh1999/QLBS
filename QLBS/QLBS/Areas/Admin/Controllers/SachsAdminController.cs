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
    public class SachsAdminController : Controller
    {
        private LTQLDBContext db = new LTQLDBContext();
        AutogenKey genkey = new AutogenKey();
        ReadDataFromExcelFile excelPro = new ReadDataFromExcelFile();

        // GET: Admin/SachsAdmin
        public ActionResult Index()
        {
            var sachs = db.Sachs.Include(s => s.TacGias).Include(s => s.TheLoais);
            return View(sachs.ToList());
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            DataTable dt = CopyDataFromExcelFile(file);
            OverwriteFastData(dt);
            return RedirectToAction("Index", "SachsAdmin");
        }

        public DataTable CopyDataFromExcelFile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string fileExtention = file.FileName.Substring(file.FileName.IndexOf("."));
            string _FileName = "Saches" + r.Next() + fileExtention; //Tên file Excel
            string _path = Path.Combine(Server.MapPath("~/Uploads/Excels"), _FileName);
            file.SaveAs(_path);
            DataTable dt = excelPro.ReadDataFromExcelFiles(_path, true);
            return dt;
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LTQLDBContext"].ConnectionString);
        private void OverwriteFastData(DataTable dt)
        {
            SqlBulkCopy bulkCopy = new SqlBulkCopy(con);
            bulkCopy.DestinationTableName = "Saches";
            bulkCopy.ColumnMappings.Add(0, "IDSach");
            bulkCopy.ColumnMappings.Add(1, "TenSach");
            bulkCopy.ColumnMappings.Add(2, "GiaSach");
            bulkCopy.ColumnMappings.Add(3, "MaTheLoai");
            bulkCopy.ColumnMappings.Add(4, "MaTacGia");
            con.Open();
            bulkCopy.WriteToServer(dt);
            con.Close();
        }

        // GET: Admin/SachsAdmin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Sachs.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // GET: Admin/SachsAdmin/Create
        public ActionResult Create()
        {

            if (db.Sachs.OrderByDescending(m => m.IDSach).Count() == 0)
            {
                var newID = "Sach01";
                ViewBag.newproID = newID;
            }
            else
            {
                var PdID = db.Sachs.OrderByDescending(m => m.IDSach).FirstOrDefault().IDSach;
                var newID = genkey.generatekey(PdID, 4);
                ViewBag.newproID = newID;
            }
            ViewBag.MaTacGia = new SelectList(db.TacGias, "MaTacGia", "TenTacGia");
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai");
            return View();
        }

        // POST: Admin/SachsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDSach,TenSach,GiaSach,MaTheLoai,MaTacGia")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Sachs.Add(sach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaTacGia = new SelectList(db.TacGias, "MaTacGia", "TenTacGia", sach.MaTacGia);
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai", sach.MaTheLoai);
            return View(sach);
        }

        // GET: Admin/SachsAdmin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Sachs.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaTacGia = new SelectList(db.TacGias, "MaTacGia", "TenTacGia", sach.MaTacGia);
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai", sach.MaTheLoai);
            return View(sach);
        }

        // POST: Admin/SachsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDSach,TenSach,GiaSach,MaTheLoai,MaTacGia")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaTacGia = new SelectList(db.TacGias, "MaTacGia", "TenTacGia", sach.MaTacGia);
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai", sach.MaTheLoai);
            return View(sach);
        }

        // GET: Admin/SachsAdmin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Sachs.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: Admin/SachsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Sach sach = db.Sachs.Find(id);
            db.Sachs.Remove(sach);
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
