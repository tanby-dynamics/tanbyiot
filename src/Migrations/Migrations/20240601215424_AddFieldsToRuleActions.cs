using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToRuleActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SendInstructionDeviceGroups",
                table: "RuleActions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SendInstructionDeviceId",
                table: "RuleActions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendInstructionPayload",
                table: "RuleActions",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendInstructionType",
                table: "RuleActions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendInstructionValue",
                table: "RuleActions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RuleActions",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendInstructionDeviceGroups",
                table: "RuleActions");

            migrationBuilder.DropColumn(
                name: "SendInstructionDeviceId",
                table: "RuleActions");

            migrationBuilder.DropColumn(
                name: "SendInstructionPayload",
                table: "RuleActions");

            migrationBuilder.DropColumn(
                name: "SendInstructionType",
                table: "RuleActions");

            migrationBuilder.DropColumn(
                name: "SendInstructionValue",
                table: "RuleActions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RuleActions");
        }
    }
}
