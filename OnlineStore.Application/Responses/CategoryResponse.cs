namespace OnlineStore.Application.Responses;
public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly CreatedAt { get; set; }
    public int? ParentCategoryId { get; set; }
    public List<CategoryResponse> SubCategories { get; set; } = new List<CategoryResponse>();

}
