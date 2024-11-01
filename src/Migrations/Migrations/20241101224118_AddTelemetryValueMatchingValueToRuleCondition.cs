using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTelemetryValueMatchingValueToRuleCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StateKey",
                table: "RuleConditions",
                newName: "TelemetryValueMatchingValue");

            migrationBuilder.RenameColumn(
                name: "PayloadPath",
                table: "RuleConditions",
                newName: "TelemetryValueMatchingPayloadPath");

            migrationBuilder.RenameColumn(
                name: "Conversion",
                table: "RuleConditions",
                newName: "TelemetryValueMatchingType");

            migrationBuilder.RenameColumn(
                name: "ComparisonValue",
                table: "RuleConditions",
                newName: "TelemetryTypeMatchingSpecifiedTypes");

            migrationBuilder.RenameColumn(
                name: "ComparisonOperation",
                table: "RuleConditions",
                newName: "TelemetryValueMatchingComparisonOperationType");

            migrationBuilder.AlterColumn<string>(
                name: "TelemetryTypeMatchingType",
                table: "RuleConditions",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationStateComparisonOperationType",
                table: "RuleConditions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationStateMatchingKey",
                table: "RuleConditions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationStateMatchingPayloadPath",
                table: "RuleConditions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationStateMatchingType",
                table: "RuleConditions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationStateMatchingValue",
                table: "RuleConditions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RuleConditions",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceMatchingGroups",
                table: "RuleConditions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceMatchingId",
                table: "RuleConditions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceMatchingType",
                table: "RuleConditions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationStateComparisonOperationType",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "ApplicationStateMatchingKey",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "ApplicationStateMatchingPayloadPath",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "ApplicationStateMatchingType",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "ApplicationStateMatchingValue",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "DeviceMatchingGroups",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "DeviceMatchingId",
                table: "RuleConditions");

            migrationBuilder.DropColumn(
                name: "DeviceMatchingType",
                table: "RuleConditions");

            migrationBuilder.RenameColumn(
                name: "TelemetryValueMatchingValue",
                table: "RuleConditions",
                newName: "StateKey");

            migrationBuilder.RenameColumn(
                name: "TelemetryValueMatchingType",
                table: "RuleConditions",
                newName: "Conversion");

            migrationBuilder.RenameColumn(
                name: "TelemetryValueMatchingPayloadPath",
                table: "RuleConditions",
                newName: "PayloadPath");

            migrationBuilder.RenameColumn(
                name: "TelemetryValueMatchingComparisonOperationType",
                table: "RuleConditions",
                newName: "ComparisonOperation");

            migrationBuilder.RenameColumn(
                name: "TelemetryTypeMatchingSpecifiedTypes",
                table: "RuleConditions",
                newName: "ComparisonValue");

            migrationBuilder.AlterColumn<int>(
                name: "TelemetryTypeMatchingType",
                table: "RuleConditions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
