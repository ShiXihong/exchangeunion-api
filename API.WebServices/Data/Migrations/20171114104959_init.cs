using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace API.WebServices.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Subscribe");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Subscribe");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Subscribe");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Subscribe");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Press");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Press");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Press");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Contact");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Subscribe",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Subscribe");

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Subscribe",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Subscribe",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Subscribe",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Subscribe",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Press",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Press",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Press",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Contact",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
