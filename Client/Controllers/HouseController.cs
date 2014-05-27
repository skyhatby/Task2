using System;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Client.Models.ViewModels;
using DbFirstModel;

namespace Client.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly Test _db = new Test();

        // GET: /House/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var houses = _db.Houses.Include(h => h.Type);
            return View(houses.ToList());
        }

        // GET: /House/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var house = _db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }
        
        public ActionResult Create()
        {
            ViewBag.TypeId = new SelectList(_db.Types, "Id", "Type1");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoomCount,TypeId,Price,AvailabilityDate,Owner,Photo")] HouseViewModel house)
        {
            var hou = new House
            {
                Id = house.Id,
                Owner = house.Owner,
                Price = house.Price,
                RoomCount = house.RoomCount,
                TypeId = house.TypeId,
                Type = house.Type,
                AvailabilityDate = DateTime.Now
            };

            if (house.Photo != null && Path.GetFileName(house.Photo.FileName) != String.Empty)
            {
                var contentLength = house.Photo.ContentLength;
                var inputStream = house.Photo.InputStream;
                hou.Photo = new byte[contentLength];
                inputStream.Read(hou.Photo, 0, contentLength);
            }


            if (ModelState.IsValid)
            {
                _db.Houses.Add(hou);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeId = new SelectList(_db.Types, "Id", "Type1", house.TypeId);
            return View(house);
        }

        public ActionResult GetImg(int id)
        {
            var photo = _db.Houses.Where(x=>x.Id==id).Select(x => x.Photo).First();
            using (var streak = new MemoryStream(photo))
            {
                var srcImage = Image.FromStream(streak);
                var myimg = new Bitmap(srcImage);
                myimg.Save(streak, ImageFormat.Jpeg);
                return File(streak.ToArray(), "image/jpeg");
            }
        }
        // GET: /House/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var house = _db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeId = new SelectList(_db.Types, "Id", "Type1", house.TypeId);
            return View(house);
        }

        // POST: /House/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,RoomCount,TypeId,Price,AvailabilityDate,Owner,Photo")] House house)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(house).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeId = new SelectList(_db.Types, "Id", "Type1", house.TypeId);
            return View(house);
        }

        // GET: /House/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var house = _db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // POST: /House/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var house = _db.Houses.Find(id);
            _db.Houses.Remove(house);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
