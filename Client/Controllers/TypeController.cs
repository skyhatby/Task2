using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DbFirstModel;

namespace Client.Controllers
{
    [Authorize]
    public class TypeController : Controller
    {
        private readonly Test _db = new Test();

        // GET: /Type/
        [AllowAnonymous]
        [OutputCache(Duration = 1000)]
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult TypesPartial()
        {
            var types = _db.Types.ToList();
            if (HttpContext.Cache["TypesPartial"] == null)
            {
                UpdateCache();
                return PartialView(types);
            }
            return PartialView(HttpContext.Cache["TypesPartial"]);
        }

        // GET: /Type/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var type = _db.Types.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        // GET: /Type/Create
        [OutputCache(Duration = 1000)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Type/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Type1")] Type type)
        {
            if (!ModelState.IsValid) return View(type);
            _db.Types.Add(type);
            _db.SaveChanges();
            UpdateCache();
            return RedirectToAction("Index");
        }

        // GET: /Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var type = _db.Types.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        // POST: /Type/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Type1")] Type type)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(type).State = EntityState.Modified;
                _db.SaveChanges();
                UpdateCache();
                return RedirectToAction("Index");
            }
            return View(type);
        }

        // GET: /Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var type = _db.Types.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        // POST: /Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var type = _db.Types.Find(id);
            foreach (var houses in _db.Houses.Where(x=>x.TypeId==id))
            {
                _db.Comments.RemoveRange(houses.Comments);
            }
            _db.Houses.RemoveRange(type.Houses);
            _db.Types.Remove(type);
            _db.SaveChanges();
            UpdateCache();
            return RedirectToAction("Index");
        }

        private void UpdateCache()
        {
            var houses = _db.Houses.Include(h => h.Type).Include(x => x.Comments).ToList();
            HttpContext.Cache["TypesPartial"] = houses;
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
