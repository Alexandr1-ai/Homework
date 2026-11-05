using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data.Models;
using System.IO;

namespace NotesApp.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Notes> NotesTable { get; set; }
        public DbSet<Tags> TagsTable { get; set; }
        public DbSet<NotesTags> NotesTagsTable { get; set; }

        public static string DbPath
        {
            get
            {
                var folder =
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                Directory.CreateDirectory(folder);
                return Path.Combine(folder, "notesapp.db");
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }
}
