namespace DevincziPDF;

using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public string DbPath { get; }

    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer
        ("Data Source=.;Initial Catalog=DBModelPdf;Integrated Security=True;Encrypt=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>().HasData(
            new Blog { BlogId = 1, Url = "https://example.com", BlogName = "Example Blog" },
            new Blog { BlogId = 2, Url = "https://anotherexample.com", BlogName = "Another Example Blog" }
        );

        modelBuilder.Entity<Post>().HasData(
            new Post { PostId = 1, Title = "First Post", Content = "This is the content from database.", BlogId = 1 },
            new Post { PostId = 2, Title = "Second Post", Content = "This is the content from database second.", BlogId = 1 },
            new Post { PostId = 3, Title = "Third Post", Content = "This is the content from database third.", BlogId = 2 }
        );
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
    public string BlogName { get; set; }
    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
