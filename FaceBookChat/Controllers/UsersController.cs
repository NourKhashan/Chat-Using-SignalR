using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FaceBookChat.Models;
using FaceBookChat.ViewModels;
using FaceBookChat.CommonBL;

namespace FaceBookChat.Controllers
{
    public class UsersController : Controller
    {
        private ChatModelContext db = new ChatModelContext();

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult GetId(string messageFrom)
        {
            return Json(db.Users.SingleOrDefault(m=>m.Name == messageFrom).Id, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Login(String Name)
        {
            User UserFounded = db.Users.SingleOrDefault(user => user.Name == Name);
            
            
            if (UserFounded == null)
            {
                ModelState.AddModelError("", "Login Failed");

            }
            if (ModelState.IsValid && UserFounded != null)
            {
                MessagesWithUsers messagesWithUsers = new MessagesWithUsers();
                messagesWithUsers.Users = db.Users.ToList<User>();
                messagesWithUsers.Groups = db.Groups.ToList<Group>();
                messagesWithUsers.messageUser = db.MessageUsers.OrderByDescending(d => d.Time).FirstOrDefault();
                messagesWithUsers.groupUsers = db.GroupUsers.OrderByDescending(d => d.Time).FirstOrDefault();
                Users.GetUserName = Name;
                Session["UserName"] = Name;
                return View("../ChatMessenger/Chat", messagesWithUsers);
            }
            
            return View();
                
        }   

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
