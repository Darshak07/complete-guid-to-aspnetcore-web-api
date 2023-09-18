namespace my_books.Data.ViewModels
{
    public class AuthorVM
    {
        public string? FullName { get; set; }
    }

    public class AuthorWithBooksVM
    {
        public string? FullName { get; set; }
        public List<string>? BooksTitles { get; set; }
    }
}
