namespace LINQPractice
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public List<ProductCategory> ProductCategories { get; set; } = new();
    }

    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }

}
