using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FaceBookChat.Models;
using Newtonsoft.Json;

namespace FaceBookChat.Controllers
{
    public class MessageUsersController : Controller
    {
        private ChatModelContext db = new ChatModelContext();

        // GET: MessageUsers
        public ActionResult Index()
        {
            var messageUsers = db.MessageUsers.Include(m => m.UserReciver).Include(m => m.UserSender);
            return View(messageUsers.ToList());
        }

        //Post Group



        public ActionResult GetMessages(string messageFrom , string messageTo)
        {
            db.Configuration.ProxyCreationEnabled = false;

            // Sender Id
            var IdFrom = db.Users.SingleOrDefault(user=> user.Name == messageFrom).Id;
            var IdTo = db.Users.SingleOrDefault(user => user.Name == messageTo).Id;


            List<MessageUser> messageUsers = db.MessageUsers.Include(m => m.UserReciver).Include(m => m.UserSender).Where(user=> user.SenderId == IdFrom).Where(user=>user.ReciverId == IdTo).ToList<MessageUser>();
            // Reciver Id
             messageUsers.AddRange ( db.MessageUsers.Include(m => m.UserReciver).Include(m => m.UserSender).Where(user => user.SenderId == IdTo).Where(user=>user.ReciverId == IdFrom));

            string json = JsonConvert.SerializeObject(messageUsers.OrderBy(user => user.Time).ToList<MessageUser>(), Formatting.None);

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        // GET: MessageUsers/Details/5



        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageUser messageUser = db.MessageUsers.Find(id);
            if (messageUser == null)
            {
                return HttpNotFound();
            }
            return View(messageUser);
        }

        // GET: MessageUsers/Create
        public ActionResult Create()
        {
            ViewBag.ReciverId = new SelectList(db.Users, "Id", "Name");
            ViewBag.SenderId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: MessageUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Message,Time,SeenStatus,SenderId,ReciverId")] MessageUser messageUser)
        {
            if (ModelState.IsValid)
            {
                db.MessageUsers.Add(messageUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReciverId = new SelectList(db.Users, "Id", "Name", messageUser.ReciverId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "Name", messageUser.SenderId);
            return View(messageUser);
        }

        // GET: MessageUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageUser messageUser = db.MessageUsers.Find(id);
            if (messageUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReciverId = new SelectList(db.Users, "Id", "Name", messageUser.ReciverId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "Name", messageUser.SenderId);
            return View(messageUser);
        }

        // POST: MessageUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Message,Time,SeenStatus,SenderId,ReciverId")] MessageUser messageUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messageUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReciverId = new SelectList(db.Users, "Id", "Name", messageUser.ReciverId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "Name", messageUser.SenderId);
            return View(messageUser);
        }

        // GET: MessageUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageUser messageUser = db.MessageUsers.Find(id);
            if (messageUser == null)
            {
                return HttpNotFound();
            }
            return View(messageUser);
        }

        // POST: MessageUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MessageUser messageUser = db.MessageUsers.Find(id);
            db.MessageUsers.Remove(messageUser);
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
