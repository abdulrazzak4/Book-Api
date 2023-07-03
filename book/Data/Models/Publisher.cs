using book.Controllers;

namespace book.Data.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //relations
        public List<Book> Books { get; set; }
    }
}
