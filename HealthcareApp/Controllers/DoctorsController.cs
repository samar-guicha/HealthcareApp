using HealthcareApp.Models.Health;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Controllers
{
    public class DoctorsController : Controller
    {
        HealthDBContext ctx;
        public DoctorsController(HealthDBContext context)
        {
            ctx = context;
        }

        // GET: HealthController
        public ActionResult Index()
        {
            return View(ctx.Doctors.ToList());
        }
        //GET: HealthController/DoctorsAndTheirAppointments
        public ActionResult DoctorsAndTheirAppointments()
        {
            var Appoitments = ctx.Appointments.ToList();
            return View(ctx.Doctors.ToList());
        }
        //GET: HealthController/SearchBy2
        public IActionResult SearchBy2()
        {
            var doctors = ctx.Doctors.ToList();
            ViewBag.Specialization = doctors.Select(m => m.Specialization).Distinct().OrderBy(g => g).ToList();
            return View(doctors);

        }
        // POST: HealthController/SearchBy2
        [HttpPost]
        public IActionResult SearchBy2( string specialization, string name)
        {

            var doctors = ctx.Doctors.ToList();
            ViewBag.Specialization = doctors.Select(m => m.Specialization).ToList();
            ViewBag.Name = name;

            if (!string.IsNullOrEmpty(specialization) && specialization != "All")
            {
                doctors = doctors.Where(m => m.Specialization == specialization).ToList();
            }

            if (!string.IsNullOrEmpty(name))
            {
                doctors = doctors.Where(m => m.Name.Contains(name)).ToList();
            }
            if (specialization == "ALL")
            {
                doctors = ctx.Doctors.ToList();
            }
            if (specialization == "ALL" && !string.IsNullOrEmpty(name))
            {
                doctors = doctors.Where(m => m.Name.Contains(name)).ToList();
            }
            return View("SearchBy2", doctors);
        }
        public IActionResult MyAppoitments(int id)
        {
            var docs = ctx.Doctors.ToList();
            var appoitments=ctx.Appointments.ToList();
            var res = from m in appoitments where m.IdDoctor == id select m;
            var res2 = ctx.Appointments.Where(m => m.IdDoctor == id);
            return View(res2.ToList());
        }

        // GET: HealthController/Details/5
        public ActionResult Details(int id)
        {
            Doctor d = ctx.Doctors.Find(id);

            return View(d);
        }

        // GET: HealthController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HealthController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Doctor doc)
        {
            try
            {
                ctx.Doctors.Add(doc);
                ctx.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HealthController/Edit/5
        public ActionResult Edit(int id)
        {
            Doctor d = ctx.Doctors.Find(id);
            return View(d);
        }

        // POST: HealthController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, Doctor doc)
        {
            try
            {
                ctx.Doctors.Update(doc);
                ctx.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HealthController/Delete/5
        public ActionResult Delete(int id)
        {
            Doctor d = ctx.Doctors.Find(id);

            return View(d);
        }

        // POST: HealthController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection, Doctor doc)
        {
            try
            {
                ctx.Doctors.Remove(doc);
                ctx.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
