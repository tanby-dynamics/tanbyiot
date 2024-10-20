using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class ReimplementTelemetryTypeCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "RuleConditions",
                newName: "StateKey");

            migrationBuilder.AddColumn<int>(
                name: "TelemetryTypeType",
                table: "RuleConditions",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelemetryTypeType",
                table: "RuleConditions");

            migrationBuilder.RenameColumn(
                name: "StateKey",
                table: "RuleConditions",
                newName: "Key");
        }
    }
}
