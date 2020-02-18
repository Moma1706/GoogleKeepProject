using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ToDoApp.Models;

namespace ToDoApp.DBConection
{
    public class ToDoListContext : DbContext
    {
        

        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<Note> Notes { get; set; }
        
        public ToDoListContext() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoList>().HasKey(m => m.Id);
            modelBuilder.Entity<ToDoList>().Property(m => m.Title).IsRequired();
            modelBuilder.Entity<ToDoList>().HasMany(m => m.Notes).WithRequired().HasForeignKey(x => x.ToDoListId);
            modelBuilder.Entity<Note>().HasKey(m => m.Id);
            modelBuilder.Entity<Note>().Property(z => z.Text).IsRequired();
        }
    }
}