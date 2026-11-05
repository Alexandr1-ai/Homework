using System;
using System.Collections.Generic;
using System.Text;

namespace NotesApp.Data.Models
{
    public class Notes
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public List<NotesTags> NotesTagsList { get; set; } = new();


        public override string ToString()
        {
            return $"{Title}:\n{Text}";
        }

    }
}
