namespace Mug.Subscription
{
    public class Book
    {
        public string? Name { get; set; }
    }
    public partial class Subscription
    {
        [Subscribe]
        public Book BookAdded([EventMessage] Book book) => book;
    }
}
