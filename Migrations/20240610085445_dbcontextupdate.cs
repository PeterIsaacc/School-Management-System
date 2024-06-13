using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Migrations
{
    /// <inheritdoc />
    public partial class dbcontextupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId_RoleId",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true,
                filter: "[SchoolId] IS NULL AND [ActivityId] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId_RoleId_SchoolId_ActivityId",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId", "SchoolId", "ActivityId" },
                unique: true,
                filter: "[SchoolId] IS NOT NULL AND [ActivityId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId_RoleId_SchoolId_ActivityId",
                table: "AspNetUserRoles");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");
        }
    }
}
