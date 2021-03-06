﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SubjectSelection.Models;
using System;

namespace SubjectSelection.Migrations
{
    [DbContext(typeof(SubjectSelectionDbContext))]
    [Migration("20171214174320_authenthication")]
    partial class authenthication
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SubjectSelection.Models.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("SubjectSelection.Models.ExclusiveSubjectLists", b =>
                {
                    b.Property<int>("ExclusiveSubjectListsId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SubjectListAId");

                    b.Property<int>("SubjectListBId");

                    b.HasKey("ExclusiveSubjectListsId");

                    b.HasIndex("SubjectListAId");

                    b.HasIndex("SubjectListBId");

                    b.ToTable("ExclusiveSubjectLists");
                });

            modelBuilder.Entity("SubjectSelection.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Capacity");

                    b.Property<string>("ClassDate");

                    b.Property<string>("Name");

                    b.Property<int>("SubjectId");

                    b.HasKey("GroupId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SubjectSelection.Models.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("ParentListId");

                    b.HasKey("SubjectId");

                    b.HasIndex("ParentListId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("SubjectSelection.Models.SubjectList", b =>
                {
                    b.Property<int>("SubjectListId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OwnerId");

                    b.HasKey("SubjectListId");

                    b.HasIndex("OwnerId");

                    b.ToTable("SubjectLists");
                });

            modelBuilder.Entity("SubjectSelection.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("StudentCardId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("SubjectSelection.Models.UserEditableLists", b =>
                {
                    b.Property<int>("UserEditableListsId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SubjectListId");

                    b.Property<int>("UserId");

                    b.HasKey("UserEditableListsId");

                    b.HasIndex("SubjectListId");

                    b.HasIndex("UserId");

                    b.ToTable("UserEditableLists");
                });

            modelBuilder.Entity("SubjectSelection.Models.UserEditableSubjects", b =>
                {
                    b.Property<int>("UserEditableSubjectsId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SubjectId");

                    b.Property<int>("UserId");

                    b.HasKey("UserEditableSubjectsId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("UserId");

                    b.ToTable("UserEditableSubjects");
                });

            modelBuilder.Entity("SubjectSelection.Models.UserGroups", b =>
                {
                    b.Property<int>("UserGroupsId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GroupId");

                    b.Property<int>("UserId");

                    b.HasKey("UserGroupsId");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("SubjectSelection.Models.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("SubjectSelection.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("SubjectSelection.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("SubjectSelection.Models.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SubjectSelection.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("SubjectSelection.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SubjectSelection.Models.ExclusiveSubjectLists", b =>
                {
                    b.HasOne("SubjectSelection.Models.SubjectList", "SubjectListA")
                        .WithMany("ExclusiveSubjectListsA")
                        .HasForeignKey("SubjectListAId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SubjectSelection.Models.SubjectList", "SubjectListB")
                        .WithMany("ExclusiveSubjectListsB")
                        .HasForeignKey("SubjectListBId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SubjectSelection.Models.Group", b =>
                {
                    b.HasOne("SubjectSelection.Models.Subject", "Subject")
                        .WithMany("Groups")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SubjectSelection.Models.Subject", b =>
                {
                    b.HasOne("SubjectSelection.Models.SubjectList", "ParentList")
                        .WithMany("Subjects")
                        .HasForeignKey("ParentListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SubjectSelection.Models.SubjectList", b =>
                {
                    b.HasOne("SubjectSelection.Models.User", "Owner")
                        .WithMany("UserOwnedSubjectLists")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SubjectSelection.Models.UserEditableLists", b =>
                {
                    b.HasOne("SubjectSelection.Models.SubjectList", "SubjectList")
                        .WithMany("UsersWhoCanEdit")
                        .HasForeignKey("SubjectListId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SubjectSelection.Models.User", "User")
                        .WithMany("ListsEditableByUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SubjectSelection.Models.UserEditableSubjects", b =>
                {
                    b.HasOne("SubjectSelection.Models.Subject", "Subject")
                        .WithMany("UsersWhoCanEdit")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SubjectSelection.Models.User", "User")
                        .WithMany("SubjectsEditableByUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SubjectSelection.Models.UserGroups", b =>
                {
                    b.HasOne("SubjectSelection.Models.Group", "Group")
                        .WithMany("UsersInGroup")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SubjectSelection.Models.User", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
