namespace LabsTRVD.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid UserId { get; set; }
    }
}
