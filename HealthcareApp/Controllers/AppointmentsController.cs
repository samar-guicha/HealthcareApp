using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using HealthcareApp.Models.Health;
using HealthcareApp.Models.ViewModels;

namespace HealthcareApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly HealthDBContext _context;

        public AppointmentsController(HealthDBContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var healthDBContext = _context.Appointments.Include(a => a.IdDoctorNavigation);
            return View(await healthDBContext.ToListAsync());
        }
        public async Task<IActionResult> AppointmentsAndTheirDoctors()
        {
            var healthDBContext = _context.Appointments.Include(a => a.IdDoctorNavigation);
            return View(await healthDBContext.ToListAsync());
        }

        public IActionResult AppointmentsAndTheirDoctors_UsingModel()
        {
            var appointments = _context.Appointments.ToList();
            var docs = _context.Doctors.ToList();
            var querry_res = from a in appointments
                             join d in docs on a.IdDoctor equals d.Id
                             select new DocAppointment
                             {
                                 aLocation = a.Location,
                                 aDescription = a.Description,
                                 dName = d.Name,
                                 dSpecialization = d.Specialization,
                                 dLicenseNumber = d.LicenseNumber
                             };
            return View(querry_res);
        }
        public IActionResult SearchByLocation(string s)
        {
            var appointments = _context.Appointments.ToList();
            var doctors = _context.Doctors.ToList();

            if (string.IsNullOrEmpty(s))
            {
                return View(appointments);
            }

            var filteredAppointments = _context.Appointments.Where(a => a.Location.Contains(s)).ToList();
            return View(filteredAppointments);
        }
        public IActionResult SearchByDescription(string s)
        {
            var appointments = _context.Appointments.ToList();
            var doctors = _context.Doctors.ToList();

            if (string.IsNullOrEmpty(s))
            {
                return View(appointments);
            }

            var filteredAppointments = _context.Appointments.Where(a => a.Description.Contains(s)).ToList();
            return View(filteredAppointments);
        }
       

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.IdDoctorNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["IdDoctor"] = new SelectList(_context.Doctors, "Id", "Name");
            return View();
        }

        // POST: Appointments/Create
// To protect from overposting attacks, enable the specific properties you want to bind to.
// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Id,Location,Description,IdDoctor")] Appointment appointment)
{
    if (ModelState.IsValid)
    {
        _context.Add(appointment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    ViewData["IdDoctor"] = new SelectList(_context.Doctors, "Id", "Name", appointment.IdDoctor);
    return View(appointment);
}


        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["IdDoctor"] = new SelectList(_context.Doctors, "Id", "Id", appointment.IdDoctor);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Location,Description,IdDoctor")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDoctor"] = new SelectList(_context.Doctors, "Id", "Id", appointment.IdDoctor);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.IdDoctorNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointments == null)
            {
                return Problem("Entity set 'HealthDBContext.Appointments'  is null.");
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
          return (_context.Appointments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
