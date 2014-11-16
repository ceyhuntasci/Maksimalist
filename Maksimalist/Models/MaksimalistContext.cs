using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Maksimalist.Models
{
    public class MaksimalistContext : DbContext
    {
        public DbSet<Author> Author { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<Matter> Matter { get; set; }
        public DbSet<Advert> Advert { get; set; }





        public MaksimalistContext()
            : base("MaksimalistContext")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Post>()
               .HasRequired(t => t.Category)
               .WithMany(t => t.Posts)
               .HasForeignKey(d => d.CategoryId)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<Post>()
               .HasRequired(t => t.SubCategory)
               .WithMany(t => t.Posts)
               .HasForeignKey(d => d.SubCategoryId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
               .HasOptional(t => t.Gallery)
               .WithMany(t => t.Posts)
               .HasForeignKey(d => d.GalleryId)
               .WillCascadeOnDelete(false);
        }

    }
}