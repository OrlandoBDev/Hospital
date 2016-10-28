﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspNetGroupBasedPermissions.Model;
using AspNetGroupBasedPermissions.Model.ApplicationUSerGroup;
using AspNetGroupBasedPermissions.Repository;
using AspNetGroupBasedPermissions.Repository.DBContext;

namespace AspNetGroupBasedPermissions.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Patients
        public ActionResult Index()
        {
            return View(GetAllPatiens());
        }
        private List<ApplicationUser> GetAllPatiens()
        {
            var patient = new List<ApplicationUser>();
            var users = _db.Users.ToList();
            foreach (var user in users)
            {
                foreach (var group in user.Groups)
                {
                    if (group.Group.Name == "Patients")
                    {
                        patient.Add(user);
                    }
                }
            }
            return patient;
        }
        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           var patient = _db.Users.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( ApplicationUser patient)
        {
            if (ModelState.IsValid)
            {
                patient.UserName = patient.FirstName + "." + patient.LastName;
                var pat = _db.Users.Add(patient);
                try
                {
                    _db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry

                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Display or log error messages

                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            Console.WriteLine(message);
                        }
                    }
                }
                AddToPatientGroup(pat.Id);
                return RedirectToAction("Index");
            }
           


            return View(patient);
        }

        private void AddToPatientGroup(string userId)
        {
           
                var idManager = new IdentityManager();              
                idManager.ClearUserGroups(userId);
                var groupId = _db.Groups.FirstOrDefault(p => p.Name == "Patients");
            if (groupId != null) idManager.AddUserToGroup(userId, groupId.Id);
        }
           
        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patient = _db.Users.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,PatientNumber,Phone,BloodGroup,Addresss,DateAddded,ModifiedBy")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(patient).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patient = _db.Users.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var patient = _db.Users.Find(id);
            _db.Users.Remove(patient);
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
