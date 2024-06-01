using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToRuleConditions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_Devices_DeviceId",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_Telemetries_Devices_DeviceId",
                table: "Telemetries");

            migrationBuilder.AddColumn<string>(
                name: "ComparisonOperation",
                table: "RuleConditions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComparisonValue",
                table: "RuleConditions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Conversion",
                table: "RuleConditions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayloadPath",
                table: "RuleConditions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RuleConditions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_Devices_DeviceId",
                table: "Instructions",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Telemetries_Devices_DeviceId",
                table: "Telemetries",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_Devices_DeviceId",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_Telemetries_Devices_DeviceId",
                table: "Telemetries");

            migrationBuilder.DropColumn(
                name: "ComparisonOperation",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "ComparisonValue",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "Conversion",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "PayloadPath",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RuleConditions");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_Devices_DeviceId",
                table: "Instructions",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Telemetries_Devices_DeviceId",
                table: "Telemetries",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
