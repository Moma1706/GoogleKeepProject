using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class ToDoList
    {
        public ToDoList()
        {
            Notes = new List<Note>();
        }

        public Guid Id { get; set; }
        public String Title { get; set; }
        public int Position { get; set; }
        public List<Note> Notes { get; set; }
        public DateTime DateTime { get; set; }
    }
}