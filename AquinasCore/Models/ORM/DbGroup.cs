using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using AquinasCore.Models;
namespace AquinasCore.Models.ORM
{
    public partial class DbGroup : DbContext
    {
        public DbGroup()
            : base("name=DbModel")
        {
        }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Payment_History> Payment_History { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Groups>().ToTable("Groups");
            modelBuilder.Entity<Payment_History>().ToTable("Payment_History");
            modelBuilder.Entity<Contacts>().ToTable("Contacts");
            // --------------------------------------------------------------- // 
            modelBuilder.Entity<Roles>().ToTable("Roles");
            modelBuilder.Entity<Tags>().ToTable("Tags");
            modelBuilder.Entity<Tasks>().ToTable("Tasks");
            modelBuilder.Entity<Comments>().ToTable("Comments");
            modelBuilder.Entity<Groups>()
                .Property(e => e.current_price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Groups>()
                .HasMany(e => e.Payment_History)
                .WithRequired(e => e.Groups)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Groups>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Groups)
                .Map(m => m.ToTable("User_Groups").MapLeftKey("group_id").MapRightKey("user_id"));

            modelBuilder.Entity<Payment_History>()
                .Property(e => e.payment_amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Tasks)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("Task_Roles").MapLeftKey("role_id").MapRightKey("task_id"));

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("User_Role").MapLeftKey("role_id").MapRightKey("inner_id"));

            modelBuilder.Entity<Tags>()
                .HasMany(e => e.Tasks)
                .WithMany(e => e.Tags)
                .Map(m => m.ToTable("Task_Tags").MapLeftKey("tag_id").MapRightKey("task_id"));

            modelBuilder.Entity<Tasks>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Tasks)
                .Map(m => m.ToTable("Task_Users").MapLeftKey("task_id").MapRightKey("user_id"));
        }
    }
}
