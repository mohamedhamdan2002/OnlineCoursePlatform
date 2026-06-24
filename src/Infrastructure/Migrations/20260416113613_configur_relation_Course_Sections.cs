using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class configur_relation_Course_Sections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseId1",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CourseId1",
                table: "Sections",
                column: "CourseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Courses_CourseId1",
                table: "Sections",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Courses_CourseId1",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_CourseId1",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "Sections");
        }
    }
}
