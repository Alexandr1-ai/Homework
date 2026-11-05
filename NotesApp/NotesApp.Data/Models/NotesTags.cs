using System;
using System.Collections.Generic;
using System.Text;

namespace NotesApp.Data.Models
{
    public class NotesTags
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public Notes Note { get; set; } = null!;
        public int TagId { get; set; }
        public Tags Tag { get; set; } = null!;
        public DateTime AddedAt { get; set; }
        public bool IsPinned { get; set; }

        public override string ToString()
        {
            string tagName = Tag?.Name ?? "Тег не прикреплен";
            string noteTitle = Note?.Title ?? "Заголовок";
            return $"{tagName}: {noteTitle} (Id: {NoteId})";
        }
    }
}
