using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.Models
{
    public class SubjectSelectionDbContext : IdentityDbContext<User, ApplicationRole, int>//DbContext
    {
        public SubjectSelectionDbContext(DbContextOptions<SubjectSelectionDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExclusiveSubjectLists>()
                .HasOne(s => s.SubjectListA)
                .WithMany(sl => sl.ExclusiveSubjectListsA)
                .HasForeignKey(s => s.SubjectListAId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExclusiveSubjectLists>()
                .HasOne(s => s.SubjectListB)
                .WithMany(sl => sl.ExclusiveSubjectListsB)
                .HasForeignKey(s => s.SubjectListBId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserEditableSubjects>()
                .HasOne(ues => ues.User)
                .WithMany(u => u.SubjectsEditableByUser)
                .HasForeignKey(ues => ues.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserEditableSubjects>()
                .HasOne(ues => ues.Subject)
                .WithMany(s => s.UsersWhoCanEdit)
                .HasForeignKey(ues => ues.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserEditableLists>()
                .HasOne(uel => uel.User)
                .WithMany(u => u.ListsEditableByUser)
                .HasForeignKey(uel => uel.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserEditableLists>()
                .HasOne(uel => uel.SubjectList)
                .WithMany(u => u.UsersWhoCanEdit)
                .HasForeignKey(uel => uel.SubjectListId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroups>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroups>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UsersInGroup)
                .HasForeignKey(ug => ug.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            //base.OnModelCreating(modelBuilder);

        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectList> SubjectLists { get; set; }
        //public DbSet<User> Users { get; set; }


        public DbSet<UserGroups> UserGroups { get; set; }
        public DbSet<UserEditableSubjects> UserEditableSubjects { get; set; }
        public DbSet<UserEditableLists> UserEditableLists { get; set; }
        public DbSet<ExclusiveSubjectLists> ExclusiveSubjectLists { get; set; }
    }
}
