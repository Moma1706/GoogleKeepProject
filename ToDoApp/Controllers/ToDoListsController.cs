using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ToDoApp.DBConection;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [Route("api/to-do-lists")]
    public class ToDoListsController : ApiController
    {
        private ToDoListContext db = new ToDoListContext();

        // GET: api/ToDoLists

        [HttpGet]
        public IQueryable<ToDoList> GetToDoLists()
        {
            return db.ToDoLists;
        }

        // GET: api/ToDoLists/5
        [ResponseType(typeof(ToDoList))]
        [HttpGet]
        public IHttpActionResult GetToDoList([FromUri]Guid id)
        {
            ToDoList toDoList = db.ToDoLists.Find(id);
            if (toDoList == null)
            {
                return NotFound();
            }

            return Ok(toDoList);
        }

        // PUT: api/ToDoLists/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutToDoList(Guid id, ToDoList toDoList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != toDoList.Id)
            {
                return BadRequest();
            }

            db.Entry(toDoList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ToDoLists
        [ResponseType(typeof(ToDoList))]
        [HttpPost]
        public IHttpActionResult PostToDoList(ToDoList toDoList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ToDoLists.Add(toDoList);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ToDoListExists(toDoList.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = toDoList.Id }, toDoList);
        }

        // DELETE: api/ToDoLists/5
        [ResponseType(typeof(ToDoList))]
        [HttpDelete]
        public IHttpActionResult DeleteToDoList(Guid id)
        {
            ToDoList toDoList = db.ToDoLists.Find(id);
            if (toDoList == null)
            {
                return NotFound();
            }

            db.ToDoLists.Remove(toDoList);
            db.SaveChanges();

            return Ok(toDoList);
        }
        
        [HttpPut]
        public IHttpActionResult UpdatePosition(int newPosition, Guid id)
        {
            List<ToDoList> list = db.ToDoLists.ToList();

            ToDoList obj = db.ToDoLists.Find(id);
            if(obj == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            int oldPosition = obj.Position;


            if (oldPosition < newPosition)
            {
                ToDoList newItem = list[newPosition];

                for (int i = newPosition; i > oldPosition; i--)
                {
                    list[i] = list[i - 1];
                }
                list[oldPosition] = newItem;               
            }
            else if (oldPosition > newPosition)
            {
                ToDoList newItem = list[oldPosition];

                for (int i = oldPosition; i > newPosition; i--)
                {
                    list[i] = list[i - 1];
                }

                list[newPosition] = newItem;             
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            

            return StatusCode(HttpStatusCode.NoContent);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ToDoListExists(Guid id)
        {
            return db.ToDoLists.Count(e => e.Id == id) > 0;
        }



        private  bool ChangePosition(List<ToDoList> list, int oldPosition, int newPosition)
        {
            if (oldPosition < newPosition)
            {
                ToDoList newItem = list[newPosition];

                for (int i = newPosition; i > oldPosition; i--)
                {
                    list[i] = list[i - 1];
                }
                list[oldPosition] = newItem;
                return true;
            }
            else if (oldPosition > newPosition)
            {
                ToDoList newItem = list[oldPosition];

                for (int i = oldPosition; i > newPosition; i--)
                {
                    list[i] = list[i - 1];
                }

                list[newPosition] = newItem;
                return true;

            }

            return false;
        }
    }
}