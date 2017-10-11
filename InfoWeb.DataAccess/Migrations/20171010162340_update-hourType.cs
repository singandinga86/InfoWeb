using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoWeb.DataAccess.Migrations
{
    public partial class updatehourType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HOURTYPE_NAME",
                table: "HourTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "HourTypes",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_HOURTYPE_NAME",
                table: "HourTypes",
                column: "Name", 
                unique:true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "HourTypes",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
