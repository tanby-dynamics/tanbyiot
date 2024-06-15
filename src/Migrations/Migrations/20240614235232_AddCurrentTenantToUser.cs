using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentTenantToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentTenantId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentTenantId",
                table: "Users",
                column: "CurrentTenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_CurrentTenantId",
                table: "Users",
                column: "CurrentTenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_CurrentTenantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentTenantId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentTenantId",
                table: "Users");
        }
    }
}
