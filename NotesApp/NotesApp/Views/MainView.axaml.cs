using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;

namespace NotesApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();
            DbSeed.Seed(db);
        }

        LoadNotes();
        LoadTags();
        LoadNotesTags();
    }

    private void LoadNotes()
    {
        using var db = new AppDbContext();
        NotesList.ItemsSource = db.NotesTable
            .OrderBy(n => n.Title)
            .ToList();
    }

    private void LoadTags()
    {
        using var db = new AppDbContext();
        TagsList.ItemsSource = db.TagsTable
            .OrderBy(t => t.Name)
            .ToList();
    }

    private void LoadNotesTags()
    {
        using var db = new AppDbContext();
        NotesTagsList.ItemsSource = db.NotesTagsTable
            .Include(nt => nt.Note)
            .Include(nt => nt.Tag)
            .Where(nt => nt.IsPinned)
            .OrderBy(nt => nt.Tag.Name)
            .ToList();
    }
}