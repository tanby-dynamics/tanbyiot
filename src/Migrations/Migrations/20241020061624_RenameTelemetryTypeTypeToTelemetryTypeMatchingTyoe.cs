using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RenameTelemetryTypeTypeToTelemetryTypeMatchingTyoe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TelemetryTypeType",
                table: "RuleConditions",
                newName: "TelemetryTypeMatchingType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TelemetryTypeMatchingType",
                table: "RuleConditions",
                newName: "TelemetryTypeType");
        }
    }
}
