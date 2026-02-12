namespace LINQPractice
{
    public class Order
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "";

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
