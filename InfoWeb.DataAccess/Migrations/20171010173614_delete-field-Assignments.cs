using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoWeb.DataAccess.Migrations
{
    public partial class deletefieldAssignments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssigmatorId",
                table: "Assignments");

            migrationBuilder.RenameIndex(
                name: "IX_HOUrTYPE_NAME",
                table: "HourTypes",
                newName: "IX_HOURTYPE_NAME");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_HOURTYPE_NAME",
                table: "HourTypes",
                newName: "IX_HOUrTYPE_NAME");

            migrationBuilder.AddColumn<int>(
                name: "AssigmatorId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);
        }
    }
}
