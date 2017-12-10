using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SubjectSelection.Migrations
{
    public partial class migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectLists_SubjectLists_SubjectListId1",
                table: "SubjectLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Users_UserId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_UserId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_SubjectLists_SubjectListId1",
                table: "SubjectLists");

            migrationBuilder.DropIndex(
                name: "IX_Groups_UserId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SubjectListId1",
                table: "SubjectLists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Groups");

            migrationBuilder.CreateTable(
                name: "ExclusiveSubjectLists",
                columns: table => new
                {
                    ExclusiveSubjectListsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubjectListAId = table.Column<int>(nullable: false),
                    SubjectListBId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExclusiveSubjectLists", x => x.ExclusiveSubjectListsId);
                    table.ForeignKey(
                        name: "FK_ExclusiveSubjectLists_SubjectLists_SubjectListAId",
                        column: x => x.SubjectListAId,
                        principalTable: "SubjectLists",
                        principalColumn: "SubjectListId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExclusiveSubjectLists_SubjectLists_SubjectListBId",
                        column: x => x.SubjectListBId,
                        principalTable: "SubjectLists",
                        principalColumn: "SubjectListId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserEditableLists",
                columns: table => new
                {
                    UserEditableListsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubjectListId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEditableLists", x => x.UserEditableListsId);
                    table.ForeignKey(
                        name: "FK_UserEditableLists_SubjectLists_SubjectListId",
                        column: x => x.SubjectListId,
                        principalTable: "SubjectLists",
                        principalColumn: "SubjectListId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserEditableLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserEditableSubjects",
                columns: table => new
                {
                    UserEditableSubjectsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEditableSubjects", x => x.UserEditableSubjectsId);
                    table.ForeignKey(
                        name: "FK_UserEditableSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserEditableSubjects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    UserGroupsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.UserGroupsId);
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExclusiveSubjectLists_SubjectListAId",
                table: "ExclusiveSubjectLists",
                column: "SubjectListAId");

            migrationBuilder.CreateIndex(
                name: "IX_ExclusiveSubjectLists_SubjectListBId",
                table: "ExclusiveSubjectLists",
                column: "SubjectListBId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEditableLists_SubjectListId",
                table: "UserEditableLists",
                column: "SubjectListId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEditableLists_UserId",
                table: "UserEditableLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEditableSubjects_SubjectId",
                table: "UserEditableSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEditableSubjects_UserId",
                table: "UserEditableSubjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupId",
                table: "UserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserId",
                table: "UserGroups",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExclusiveSubjectLists");

            migrationBuilder.DropTable(
                name: "UserEditableLists");

            migrationBuilder.DropTable(
                name: "UserEditableSubjects");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectListId1",
                table: "SubjectLists",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Groups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_UserId",
                table: "Subjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectLists_SubjectListId1",
                table: "SubjectLists",
                column: "SubjectListId1");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectLists_SubjectLists_SubjectListId1",
                table: "SubjectLists",
                column: "SubjectListId1",
                principalTable: "SubjectLists",
                principalColumn: "SubjectListId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Users_UserId",
                table: "Subjects",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
