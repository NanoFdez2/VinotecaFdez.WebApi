using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VinotecaFernandez.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class _18112025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "AspNetRoles", newName: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "Roles", newName: "AspNetRoles");
        }
    }
}
