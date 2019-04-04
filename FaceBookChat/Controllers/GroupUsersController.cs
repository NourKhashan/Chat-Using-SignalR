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
    public class GroupUsersController : Controller
    {
        private ChatModelContext db = new ChatModelContext();

        // GET: GroupUsers
        public ActionResult Index()
        {
            var groupUsers = db.GroupUsers.Include(g => g.Group).Include(g => g.User);
            return View(groupUsers.ToList());
        }


        public ActionResult GetMessages(string messageFrom, string messageTo)
        {
            db.Configuration.ProxyCreationEnabled = false;

            // Groups
            // Sender Id
            var IdFrom = db.Users.SingleOrDefault(user => user.Name == messageFrom).Id;
            var IdTo = db.Groups.SingleOrDefault(group => group.Name == messageTo).Id;// GroupId

            //Check If User in Group OR Not
            var userInGroup = db.GroupUsers.Where(group=>group.GroupId == IdTo).FirstOrDefault(group => group.UserId == IdFrom);
            if(userInGroup == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);

            }
            // Get Messgaes
            var groupUsersMessages = db.GroupUsers.Where(group => group.GroupId == IdTo).ToList<GroupUsers>();
            //List<MessageUser> messageUsers = db.MessageUsers.Include(m => m.UserReciver).Include(m => m.UserSender).Where(user => user.SenderId == IdFrom).Where(user => user.ReciverId == IdTo).ToList<MessageUser>();
            //// Reciver Id
            //messageUsers.AddRange(db.MessageUsers.Include(m => m.UserReciver).Include(m => m.UserSender).Where(user => user.SenderId == IdTo).Where(user => user.ReciverId == IdFrom));

            string json = JsonConvert.SerializeObject(groupUsersMessages.OrderBy(user => user.Time).ToList<GroupUsers>(), new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,

            });
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        // GET: GroupUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupUsers groupUsers = db.GroupUsers.Find(id);
            if (groupUsers == null)
            {
                return HttpNotFound();
            }
            return View(groupUsers);
        }

        // GET: GroupUsers/Create
        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: GroupUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupId,UserId,Message,Time")] GroupUsers groupUsers)
        {
            if (ModelState.IsValid)
            {
                db.GroupUsers.Add(groupUsers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", groupUsers.GroupId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", groupUsers.UserId);
            return View(groupUsers);
        }

        // GET: GroupUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupUsers groupUsers = db.GroupUsers.Find(id);
            if (groupUsers == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", groupUsers.GroupId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", groupUsers.UserId);
            return View(groupUsers);
        }

        // POST: GroupUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,UserId,Message,Time")] GroupUsers groupUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupUsers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", groupUsers.GroupId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", groupUsers.UserId);
            return View(groupUsers);
        }

        // GET: GroupUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupUsers groupUsers = db.GroupUsers.Find(id);
            if (groupUsers == null)
            {
                return HttpNotFound();
            }
            return View(groupUsers);
        }

        // POST: GroupUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GroupUsers groupUsers = db.GroupUsers.Find(id);
            db.GroupUsers.Remove(groupUsers);
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
