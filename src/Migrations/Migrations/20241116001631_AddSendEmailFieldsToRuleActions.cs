using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddSendEmailFieldsToRuleActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SendEmailBody",
                table: "RuleActions",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendEmailSenderEmail",
                table: "RuleActions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendEmailSenderName",
                table: "RuleActions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendEmailSubject",
                table: "RuleActions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendEmailToEmail",
                table: "RuleActions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendEmailBody",
                table: "RuleActions");

            migrationBuilder.DropColumn(
                name: "SendEmailSenderEmail",
                table: "RuleActions");

            migrationBuilder.DropColumn(
                name: "SendEmailSenderName",
                table: "RuleActions");

            migrationBuilder.DropColumn(
                name: "SendEmailSubject",
                table: "RuleActions");

            migrationBuilder.DropColumn(
                name: "SendEmailToEmail",
                table: "RuleActions");
        }
    }
}
