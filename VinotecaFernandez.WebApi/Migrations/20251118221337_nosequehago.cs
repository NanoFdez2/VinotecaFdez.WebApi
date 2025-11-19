using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VinotecaFernandez.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class nosequehago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "Roles", newName: "AspNetRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
