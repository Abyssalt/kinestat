using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace KineStat.Controllers
{
    public class AssessmentsController : Controller
    {
        private readonly KineDbContext _context;

        public AssessmentsController(KineDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kineDbContext = _context.Assessments.Include(a => a.Patient).Include(a => a.Physio);
            return View(await kineDbContext.ToListAsync());
        }

        [Route("Assessment/{id}/Details")]
        public async Task<IActionResult> AssessmentDetails(int id)
        {
            var assessment = await _context.Assessments
                .Include(a => a.Patient)
                .Include(a => a.Physio)
                .Include(a => a.Dossier)
                .Include(a => a.RedFlagsDetected)
                .Include(a => a.Questions)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assessment == null)
                return NotFound();

            var socrate = await _context.Set<Socrate>()
                .FirstOrDefaultAsync(s => s.AssessmentId == id);

            ViewBag.Socrate = socrate;
            return View(assessment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Email", assessment.PatientId);
            ViewData["PhysioId"] = new SelectList(_context.Physios, "Id", "Email", assessment.PhysioId);
            return View(assessment);
        }

        // POST: Assessments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,PatientId,PhysioId")] Assessment assessment)
        {
            if (id != assessment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assessment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessmentExists(assessment.Id))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Email", assessment.PatientId);
            ViewData["PhysioId"] = new SelectList(_context.Physios, "Id", "Email", assessment.PhysioId);
            return View(assessment);
        }

        // POST: Assessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int dossierId)
        {
            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment != null)
            {
                _context.Assessments.Remove(assessment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(
                "DossierDetails", "Folder",new { id = dossierId }
            );
        }

        private bool AssessmentExists(int id)
        {
            return _context.Assessments.Any(e => e.Id == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartAssessment(int PatientId, int DossierId, int PhysioId, int MedicalContextId)
        {
            var patient = await _context.Patients.FindAsync(PatientId);
            if (patient == null)
                return NotFound("Patient introuvable");

            var dossier = await _context.Dossiers.FindAsync(DossierId);
            if (dossier == null)
                return NotFound("Dossier introuvable");

            if (!await _context.MedicalContexts.AnyAsync(mc => mc.Id == MedicalContextId))
                return BadRequest("Contexte médical invalide");

            var assessment = new Assessment
            {
                PatientId = PatientId,
                PhysioId = PhysioId,
                DossierId = DossierId,
                MedicalContextId = MedicalContextId,
                Date = DateTime.Now
            };

            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Socrate", "Patient", new { id = PatientId, assessmentId = assessment.Id });
        }

        [Route("Dossier/{dossierId}/CreateAssessment")]
        public async Task<IActionResult> CreateAssessment(int dossierId)
        {
            var dossier = await _context.Dossiers
                .Include(d => d.Patient)
                .ThenInclude(p => p.Physio)
                .FirstOrDefaultAsync(d => d.Id == dossierId);

            if (dossier == null)
                return NotFound("Dossier introuvable");

            if (dossier.Patient == null)
                return NotFound("Patient introuvable");

            ViewBag.MedicalContexts = await _context.MedicalContexts.ToListAsync();

            return View(new Assessment
            {
                DossierId = dossierId,
                PatientId = dossier.Patient.Id,
                PhysioId = dossier.Patient.PhysioId,
                Date = DateTime.Today,
                RedFlagsPercentage = 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRedFlagsPercentage(int assessmentId, string redFlagsPercentage)
        {
            var assessment = await _context.Assessments.FindAsync(assessmentId);

            if (assessment == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(redFlagsPercentage))
                redFlagsPercentage = "0";

            redFlagsPercentage = redFlagsPercentage.Replace(',', '.');

            if (!double.TryParse(
                    redFlagsPercentage,
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out double value))
            {
                value = 0;
            }


            assessment.RedFlagsPercentage = value;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "AssessmentDetails",
                "Assessments",
                new { id = assessment.Id }
            );
        }


    }
}