using System;
using System.Collections.Generic;
using System.Text;

namespace NotesApp.Data.Models
{
    public class Tags
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Color { get; set; }
        public bool IsSystem { get; set; }
      
        public List<NotesTags> NotesTagsList { get; set; } = new();

        public override string ToString()
        {
            return Name;
        }
    }
}
