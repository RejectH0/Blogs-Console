namespace BlogsConsole.Models
{
    // Version 0.3b - Need to figure out how to add Post. Asking instructor for assistance.
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
