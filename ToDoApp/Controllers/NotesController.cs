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
    public class NotesController : ApiController
    {
        private ToDoListContext db = new ToDoListContext();

        // GET: api/Notes
        [HttpGet]
        public IQueryable<Note> GetNotes([FromUri]Guid id)
        {
            ToDoList toDoList = db.ToDoLists.Find(id);

            if(toDoList == null)
            {
                return null;
            }

            return (IQueryable<Note>)toDoList.Notes;

           
        }

        // GET: api/Notes/5
        [ResponseType(typeof(Note))]
        [HttpGet]
        public IHttpActionResult GetNote([FromUri]Guid id,Guid noteId)
        {
            ToDoList toDoList = db.ToDoLists.Find(id);

            if (toDoList == null)
            {
                return null;
            }




            Note note = toDoList.Notes.FirstOrDefault(x => x.Id == noteId);
            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // PUT: api/Notes/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutNote(Guid toDoListId, Guid noteId,[FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (noteId != note.Id)
            {
                return BadRequest();
            }

            ToDoList toDoList = db.ToDoLists.Find(toDoListId);
            if (toDoList == null)
            {
                return NotFound();
            }

            Note updateNote = toDoList.Notes.FirstOrDefault(x => x.Id == noteId);
            if(updateNote == null)
            {
                return NotFound();
            }

            updateNote.Position = note.Position;
            updateNote.Text = note.Text;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(noteId))
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

        // POST: api/Notes

        [ResponseType(typeof(Note))]
        [HttpPost]
        public IHttpActionResult PostNote(Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notes.Add(note);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (NoteExists(note.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = note.Id }, note);
        }

        // DELETE: api/Notes/5
        [ResponseType(typeof(Note))]
        [HttpDelete]
        public IHttpActionResult DeleteNote(Guid id)
        {
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            db.Notes.Remove(note);
            db.SaveChanges();

            return Ok(note);
        }


        [HttpPut]
        public IHttpActionResult UpdatePosition(int newPosition, Guid id)
        {
            List<Note> list = db.Notes.ToList();

            Note obj = db.Notes.Find(id);
            if (obj == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            int oldPosition = obj.Position;


            if (oldPosition < newPosition)
            {
                Note newItem = list[newPosition];

                for (int i = newPosition; i > oldPosition; i--)
                {
                    list[i] = list[i - 1];
                }
                list[oldPosition] = newItem;
            }
            else if (oldPosition > newPosition)
            {
                Note newItem = list[oldPosition];

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

        private bool NoteExists(Guid id)
        {
            return db.Notes.Count(e => e.Id == id) > 0;
        }
    }
}