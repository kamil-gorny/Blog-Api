using Blog_Api.DataModel.Entities;

namespace Blog_Api.Services.Interfaces;

public interface ITagService
{
    Task AddTag(Tag tag);
    Task<Tag?> ReadTag(Guid id);
    Task DeleteTag(Guid id);
    Task<List<Tag>> ReadAllTags();
}