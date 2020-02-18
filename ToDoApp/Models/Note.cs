using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class Note
    {
        public Note()
        {
        }

        public Guid Id { get; set; }
        public String Text { get; set; }
        public int Position { get; set; }
        public Guid ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; }
    }
}