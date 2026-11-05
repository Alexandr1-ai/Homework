using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotesApp.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Путь к файлу: {AppDbContext.DbPath}");

            using var db = new AppDbContext();
            db.Database.EnsureCreated();

            DbSeed.Seed(db);

            var notesTable = db.NotesTable.ToList();
            
            var tagsTable = db.TagsTable.ToList();

            var ntTable = db.NotesTagsTable
                .Include(nt => nt.Note)
                .Include(nt => nt.Tag)
                .ToList();

            Console.WriteLine("\n\tЗАМЕТКИ");
            foreach (var n in notesTable)
            {
                Console.WriteLine(n);
                Console.WriteLine('\n');
            }

            Console.WriteLine("\n\tТЕГИ");
            foreach (var t in tagsTable)
            {
                Console.WriteLine(t);
            }

            Console.WriteLine("\n\tСВЯЗИ");
            foreach (var nt in ntTable)
            {
                Console.WriteLine(nt);
            }
        }
    }
}
