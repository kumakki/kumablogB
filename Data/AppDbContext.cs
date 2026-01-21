using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using kumablogB.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // 例：ブログ記事テーブル
    public DbSet<Users> Users { get; set; }
    public DbSet<Blogs> Blogs { get; set; }
    public DbSet<Sessions> Sessions { get; set; }
}

