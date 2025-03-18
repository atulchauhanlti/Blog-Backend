using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Tag> GetAllTags()
    {
        return _context.Tags.ToList();
    }

    public Tag GetTagById(int id)
    {
        return _context.Tags.AsNoTracking().FirstOrDefault(c => c.Id == id);
    }

    public void AddTag(Tag tag)
    {
        _context.Tags.Add(tag);
        _context.SaveChanges();
    }

    public void UpdateTag(Tag tag)
    {
        _context.Tags.Update(tag);
        _context.SaveChanges();
    }

    public void DeleteTag(int id)
    {
        var tag = _context.Tags.Find(id);
        if (tag != null)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();
        }
    }
}
