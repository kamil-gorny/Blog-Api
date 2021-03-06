using Blog_Api.DataModel.Entities;

namespace Blog_Api.DataModel.Responses;

public class ReadPostResponseModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreationDate { get; set; }
    public Guid UserId { get; set; }
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}