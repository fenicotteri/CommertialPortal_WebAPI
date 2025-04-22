using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommertialPortal_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class add_Subscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientProfileId = table.Column<int>(type: "integer", nullable: false),
                    BusinessProfileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientSubscriptions_BusinessProfiles_BusinessProfileId",
                        column: x => x.BusinessProfileId,
                        principalTable: "BusinessProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientSubscriptions_ClientProfiles_ClientProfileId",
                        column: x => x.ClientProfileId,
                        principalTable: "ClientProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientSubscriptions_BusinessProfileId",
                table: "ClientSubscriptions",
                column: "BusinessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSubscriptions_ClientProfileId_BusinessProfileId",
                table: "ClientSubscriptions",
                columns: new[] { "ClientProfileId", "BusinessProfileId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientSubscriptions");
        }
    }
}
