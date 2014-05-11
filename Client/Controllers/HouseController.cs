using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Client.Models;

namespace Client.Controllers
{
    public class HouseController : Controller
    {
        private readonly DbEntities _db = new DbEntities();

        // GET: /House/
        public ActionResult Index()
        {
            var houses = _db.Houses.Include(h => h.Type);
            return View(houses.ToList());
        }

        // GET: /House/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = _db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // GET: /House/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.TypeId = new SelectList(_db.Types, "Id", "Type1");
            return View();
        }

        // POST: /House/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include="Id,RoomCount,TypeId,Price,AvailabilityDate,Owner,Photo")] House house)
        {
            if (ModelState.IsValid)
            {
                _db.Houses.Add(house);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeId = new SelectList(_db.Types, "Id", "Type1", house.TypeId);
            return View(house);
        }

        // GET: /House/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = _db.Houses.Find(id);
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
        [Authorize]
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
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = _db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // POST: /House/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            House house = _db.Houses.Find(id);
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
