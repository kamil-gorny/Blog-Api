namespace Blog_Api.DataModel.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreationDate { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
}