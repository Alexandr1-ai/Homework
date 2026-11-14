using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Data.Models;

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

        // Данные
        LoadNotes();
        LoadTags();
        LoadNotesTags();
    }

    private void LoadNotes()
    {
        using var db = new AppDbContext();
        var notes = db.NotesTable
            .OrderBy(n => n.Title)
            .ToList();

        NotesList.ItemsSource = notes;

        NoteTitleComboBox.ItemsSource = notes;
    }

    private void LoadTags()
    {
        using var db = new AppDbContext();
        var tags = db.TagsTable
            .OrderBy(t => t.Name)
            .ToList();

        TagsList.ItemsSource = tags;

        TagNameComboBox.ItemsSource = tags;
    }

    private void LoadNotesTags()
    {
        using var db = new AppDbContext();
        NotesTagsList.ItemsSource = db.NotesTagsTable
            .Include(nt => nt.Note)
            .Include(nt => nt.Tag)
            .OrderByDescending(nt => nt.IsPinned)
            .ToList();
    }


    // Кнопки заметок
    private void AddNoteClick(object? sender, RoutedEventArgs e)
    {
        NoteMessage.Text = "";
        NoteTitleMessage.Text = "";
        NoteTextMessage.Text = "";
        NoteDateMessage.Text = "";

        string title = NoteTitleBox.Text;
        string text = NoteTextBox.Text;

        if (string.IsNullOrWhiteSpace(title))
        {
            NoteTitleMessage.Text = "Введите заголовок";
            return;
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            NoteTextMessage.Text = "Введите текст";
            return;
        }

        if (NoteDate.SelectedDate is null)
        {
            NoteDateMessage.Text = "Выберите дату";
            return;
        }

        DateTime createdDate = NoteDate.SelectedDate.Value;
        DateTime? updateDate = NoteUpdateDate.SelectedDate;

        var newNote = new Notes { Title = title, Text = text, CreatedAt = createdDate, UpdatedAt = updateDate };

        try
        {
            using var db = new AppDbContext();
            db.NotesTable.Add(newNote);
            db.SaveChanges();

            LoadNotes();
            LoadNotesTags();
            NoteMessage.Text = "Заметка добавлена";
        }
        catch (Exception ex)
        {
            NoteMessage.Text = "Ошибка сохранения: " + ex.Message;
        }
    }

    private void DeleteNoteClick(object? sender, RoutedEventArgs e)
    {
        DeleteNoteMessage.Text = "";

        if (NotesList.SelectedItem is not Notes note)
        {
            DeleteNoteMessage.Text = "Выберите заметку для удаления";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var deleteNote = db.NotesTable.FirstOrDefault(nt => nt.Id == note.Id);

            if (deleteNote is null)
            {
                DeleteNoteMessage.Text = "Заметка не найдена";
                LoadNotes();
                return;
            }

            db.NotesTable.Remove(deleteNote);
            db.SaveChanges();

            LoadNotes();
            LoadNotesTags();
            DeleteNoteMessage.Text = "Заметка удалена";
        }
        catch (Exception ex)
        {
            DeleteNoteMessage.Text = "Ошибка удаления: " + ex.Message;
        }
    }

    // Кнопки Тегов
    private void AddTagClick(object? sender, RoutedEventArgs e)
    {
        TagMessage.Text = "";
        TagNameMessage.Text = "";

        string name = TagNameBox.Text;
        string color = TagColorBox.Text;

        if (string.IsNullOrWhiteSpace(name))
        {
            TagNameMessage.Text = "Введите название тега";
            return;
        }

        var newTag = new Tags { Name = name, Color = color, IsSystem = TagIsSystemBox.IsChecked == true };

        try
        {
            using var db = new AppDbContext();
            db.TagsTable.Add(newTag);
            db.SaveChanges();

            LoadTags();
            LoadNotesTags();
            TagMessage.Text = "Тег добавлен";
        }
        catch (Exception ex)
        {
            TagMessage.Text = "Ошибка сохранения: " + ex.Message;
        }
    }

    private void DeleteTagClick(object? sender, RoutedEventArgs e)
    {
        DeleteTagMessage.Text = "";
        if (TagsList.SelectedItem is not Tags tag)
        {
            DeleteTagMessage.Text = "Выберите тег для удаления";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var deleteTag = db.TagsTable.FirstOrDefault(t => t.Id == tag.Id);

            if (deleteTag is null)
            {
                DeleteTagMessage.Text = "Тег не найден";
                LoadTags();
                return;
            }

            db.TagsTable.Remove(deleteTag);
            db.SaveChanges();

            LoadTags();
            LoadNotesTags();
            DeleteTagMessage.Text = "Тег удален";
        }
        catch (Exception ex)
        {
            DeleteTagMessage.Text = "Ошибка удаления: " + ex.Message;
        }
    }

    private void UpdateTagClick(object? sender, RoutedEventArgs e)
    {
        UpdateTagMessage.Text = "";
        TagNameMessage.Text = "";

        if (TagsList.SelectedItem is not Tags tag)
        {
            UpdateTagMessage.Text = "Выберите тег для изменения";
            return;
        }

        string name = TagNameBox.Text;

        if (string.IsNullOrWhiteSpace(name))
        {
            TagNameMessage.Text = "Введите название тега";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var updateTag = db.TagsTable.FirstOrDefault(t => t.Id == tag.Id);

            if (updateTag == null)
            {
                UpdateTagMessage.Text = "Ошибка: тег не найден";
                LoadTags();
                return;
            }

            updateTag.Name = name;
            updateTag.Color = string.IsNullOrWhiteSpace(TagColorBox.Text) ? null : TagColorBox.Text.Trim();
            updateTag.IsSystem = TagIsSystemBox.IsChecked == true;


            db.SaveChanges();

            LoadTags();
            LoadNotesTags();
            UpdateTagMessage.Text = "Тег обновлен";
        }
        catch (Exception ex)
        {
            UpdateTagMessage.Text = "Ошибка обновления: " + ex.Message;
        }
    }

    private void TagListSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (TagsList.SelectedItem is not Tags tag)
        {
            TagNameBox.Text = "";
            TagColorBox.Text = "";
            TagIsSystemBox.IsChecked = false;
            return;
        }

        TagNameBox.Text = tag.Name;
        TagColorBox.Text = tag.Color;
        TagIsSystemBox.IsChecked = tag.IsSystem;
    }

    // Кнопки Закрепленных тегов
    private void AddNoteTagClick(object? sender, RoutedEventArgs e)
    {
        NoteTagMessage.Text = "";
        NtTitleMessage.Text = "";
        NtNameMessage.Text= "";
        AddedAtMessage.Text = "";

        if (NoteTitleComboBox.SelectedItem is not Notes note)
        {
            NtTitleMessage.Text = "Выберите заметку";
            return;
        }

        if (TagNameComboBox.SelectedItem is not Tags tag)
        {
            NtNameMessage.Text = "Выберите тег";
            return;
        }

        if (AddedAtDate.SelectedDate is null)
        {
            AddedAtMessage.Text = "Выберите дату";
            return;
        }

        int idNote = note.Id;
        int idTag = tag.Id;
        DateTime addedAt = AddedAtDate.SelectedDate.Value;

        var newNoteTag = new NotesTags { NoteId = idNote, TagId = idTag, AddedAt = addedAt, IsPinned = IsPinnedBox.IsChecked == true };

        try
        {
            using var db = new AppDbContext();

            if (db.NotesTagsTable.Any(nt => nt.NoteId == note.Id && nt.TagId == tag.Id))
            {
                NoteTagMessage.Text = "Этот тег уже прикреплен к заметке";
                return;
            }

            db.NotesTagsTable.Add(newNoteTag);
            db.SaveChanges();

            LoadNotesTags();
            if (IsPinnedBox.IsChecked == true)
            {
                NoteTagMessage.Text = "Тег закреплен";
            }
            else
            {
                NoteTagMessage.Text = "Тег не закреплен";
            }
        }
        catch (Exception ex)
        {
            NoteTagMessage.Text = "Ошибка закрепления тега: " + ex.Message;
        }
    }

    private void DeleteNoteTagClick(object? sender, RoutedEventArgs e)
    {
        DeleteNoteTagMessage.Text = "";
        if (NotesTagsList.SelectedItem is not NotesTags noteTag)
        {
            DeleteNoteTagMessage.Text = "Выберите закрепленный тег для удаления";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var deleteNoteTag = db.NotesTagsTable.FirstOrDefault(nt => nt.Id == noteTag.Id);

            if (deleteNoteTag is null)
            {
                DeleteNoteTagMessage.Text = "Закрепленная заметка не найдена";
                LoadNotesTags();
                return;
            }

            db.NotesTagsTable.Remove(deleteNoteTag);
            db.SaveChanges();

            LoadNotesTags();
            DeleteNoteTagMessage.Text = "Тег откреплен";
        }
        catch (Exception ex)
        {
            DeleteNoteTagMessage.Text = "Ошибка открепления: " + ex.Message;
        }
    }
}