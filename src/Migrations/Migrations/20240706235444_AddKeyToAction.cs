using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddKeyToAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SendInstructionPayload",
                table: "RuleActions",
                newName: "Payload");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "RuleActions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "RuleActions");

            migrationBuilder.RenameColumn(
                name: "Payload",
                table: "RuleActions",
                newName: "SendInstructionPayload");
        }
    }
}
