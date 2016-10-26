﻿using AspNetGroupBasedPermissions.Model;
using AspNetGroupBasedPermissions.Repository;
using AspNetGroupBasedPermissions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspNetGroupBasedPermissions.Repository.DBContext;
using AutoMapper;

namespace AspNetGroupBasedPermissions.Controllers
{
    public class AppointmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Appointment
        public ActionResult Index()
        {
            var app = new List<AppointmentViewModel>();
            var appresult = db.Appointments.ToList();
            foreach (var appointment in appresult )
            {
                app =
                    Mapper.Map<Appointment,List<AppointmentViewModel> >(appointment);

                
            }
            return View(app);
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Appointment/Create
        public ActionResult Create()
        {
            ViewBag.PatientViewmodelId = new SelectList(db.Patients.ToList(), "Id", "Firstname");
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(AppointmentViewModel appointment)
        {
            ViewBag.PatientViewmodelId = new SelectList(db.Patients.ToList(), "Id", "Firstname", appointment.PatientViewmodelId);

            if (!ModelState.IsValid)
            {
              
                return View();
            }
            try
            {
                var app =
                   Mapper.Map< AppointmentViewModel,Appointment>(appointment);
                
                app.Date = appointment.Date;
                app.PatientId = appointment.PatientViewmodelId;
                app.DateAdded = DateTime.Now;
               
                app.CreatedBy = User.Identity.Name;
                app.Reason = appointment.Reason;
                app.Description = appointment.Description;
                db.Appointments.Add(app);
                db.SaveChanges();

                return View("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment app = db.Appointments.Find(id);
            if (app == null)
            {
                return HttpNotFound();
            }

            return View();
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here....check if
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Appointment/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment app = db.Appointments.Find(id);
            if (app == null)
            {
                return HttpNotFound();
            }

            return View();
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
               
                Appointment app = db.Appointments.Find(id);
              
                 db.Appointments.Remove(app);
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
