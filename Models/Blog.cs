using System.Collections.Generic;

namespace BlogsConsole.Models
{
    // Version 0.3b - Need to figure out how to add Post. Asking instructor for assistance.
    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }

        public List<Post> Posts { get; set; }
    }
}
