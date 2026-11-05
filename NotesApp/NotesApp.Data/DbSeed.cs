using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data.Models;
using System.Linq;

namespace NotesApp.Data
{
    public class DbSeed
    {
        public static void Seed(AppDbContext db)
        {
            if (db.NotesTable.Any())
            {
                Console.WriteLine("База уже заполнена");
                return;
            }

            var notesList = new List<Notes>
            {
                new()
                {
                    Title = "Как выбрать ноутбук для учебы",
                    Text = "Критерии: автономность, вес, экран. Топ-5 моделей и на чем сэкономить",
                    CreatedAt = new DateTime(2026, 1, 12, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 11, 3, 12, 0, 0, DateTimeKind.Utc)
                },
                new ()
                {
                    Title = "5 привычек, крадущих энергию",
                    Text = "Проверка почты, поздние перекусы, отложенные дела — что менять уже сегодня",
                    CreatedAt=new DateTime(2026, 2, 28, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt=new DateTime(2026, 10, 15, 15, 0, 0, DateTimeKind.Utc)
                },
                new()
                {
                    Title = "Зимний уход за кожей: что работает",
                    Text = "Какие компоненты искать в кремах и как не пересушить кожу",
                    CreatedAt = new DateTime(2025, 11, 5, 12, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 11, 1, 17, 0, 0, DateTimeKind.Utc)
                },
                new()
                {
                    Title = "Пустой заголовок",
                    Text = "Как удаленка меняет карту расселения и что нужно городам",
                    CreatedAt = new DateTime(2026, 3, 17, 14, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = null
                },
                new()
                {
                    Title = "Хлеб на закваске для новичков",
                    Text = "Текст заметки",
                    CreatedAt = new DateTime(2026, 4, 9, 16, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = null
                }
            };

            db.NotesTable.AddRange(notesList);
            db.SaveChanges();

            var tagsList = new List<Tags>
            {
                new()
                {
                    Name = "техника",
                    Color = "синий",
                    IsSystem = true
                },
                new()
                {
                    Name = "здоровье",
                    Color = "зеленый",
                    IsSystem = true
                },
                new()
                {
                    Name = "красота",
                    Color = "розовый",
                    IsSystem = false
                },
                new()
                {
                    Name = "общество",
                    Color = "оранжевый",
                    IsSystem = true
                },
                new()
                {
                    Name = "еда",
                    Color = "бордовый",
                    IsSystem = false
                }
            };

            db.TagsTable.AddRange(tagsList);
            db.SaveChanges();

            var ntList = new List<NotesTags>
            {
                new()
                {
                    Note = notesList[0],
                    Tag = tagsList[0],
                    AddedAt = new DateTime(2026, 12, 3, 15, 0, 0, DateTimeKind.Utc),
                    IsPinned = true
                },
                new()
                {
                    Note = notesList[1],
                    Tag = tagsList[1],
                    AddedAt = new DateTime(2026, 10, 15, 12, 0, 0, DateTimeKind.Utc),
                    IsPinned = true
                },
                new()
                {
                    Note = notesList[4],
                    Tag = tagsList[4],
                    AddedAt = new DateTime(2026, 11, 5, 18, 0, 0, DateTimeKind.Utc),
                    IsPinned = false
                }
            };

            db.NotesTagsTable.AddRange(ntList);
            db.SaveChanges();

            Console.WriteLine("Тестовые данные добавлены");
        }
    }
}
