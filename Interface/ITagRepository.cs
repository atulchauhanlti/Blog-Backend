using System;
using System.Collections.Generic;
using backend.Models;

namespace backend.Interface;

public interface ITagRepository
{
    IEnumerable<Tag> GetAllTags();
    Tag GetTagById(int id);
    void AddTag(Tag tag);
    void UpdateTag(Tag tag);
    void DeleteTag(int id);
}
