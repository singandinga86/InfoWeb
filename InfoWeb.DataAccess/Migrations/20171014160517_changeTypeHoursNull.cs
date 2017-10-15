using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoWeb.DataAccess.Migrations
{
    public partial class changeTypeHoursNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_HourTypes_HourTypeId",
                table: "Assignments");

            migrationBuilder.AlterColumn<int>(
                name: "HourTypeId",
                table: "Assignments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_HourTypes_HourTypeId",
                table: "Assignments",
                column: "HourTypeId",
                principalTable: "HourTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_HourTypes_HourTypeId",
                table: "Assignments");

            migrationBuilder.AlterColumn<int>(
                name: "HourTypeId",
                table: "Assignments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_HourTypes_HourTypeId",
                table: "Assignments",
                column: "HourTypeId",
                principalTable: "HourTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
