using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LeshBrain.Models;

namespace LeshBrain.Models
{
    public class ContextDB:IdentityDbContext<UserEntity,IdentityRole<int>,int>
    {
        public ContextDB(DbContextOptions<ContextDB> options):base(options)
        {
            //Database.EnsureCreated();
        }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestHistory> TestHistories { get; set; }
        public DbSet<Topic> Topics { get; set; }
    }
}
