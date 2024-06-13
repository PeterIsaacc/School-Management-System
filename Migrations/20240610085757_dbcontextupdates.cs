using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Migrations
{
    /// <inheritdoc />
    public partial class dbcontextupdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId_RoleId_SchoolId",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId", "SchoolId" },
                unique: true,
                filter: "[SchoolId] IS NOT NULL AND [ActivityId] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId_RoleId_SchoolId",
                table: "AspNetUserRoles");
        }
    }
}
