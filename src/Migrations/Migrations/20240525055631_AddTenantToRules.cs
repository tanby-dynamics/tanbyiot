using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantToRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Rules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Rules_TenantId",
                table: "Rules",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Tenants_TenantId",
                table: "Rules",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Tenants_TenantId",
                table: "Rules");

            migrationBuilder.DropIndex(
                name: "IX_Rules_TenantId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Rules");
        }
    }
}
