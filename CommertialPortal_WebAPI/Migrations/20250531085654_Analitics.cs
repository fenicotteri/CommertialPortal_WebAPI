using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommertialPortal_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Analitics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostAnalitics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    GuestLikes = table.Column<int>(type: "integer", nullable: false),
                    SubscriberLikes = table.Column<int>(type: "integer", nullable: false),
                    GuestViews = table.Column<int>(type: "integer", nullable: false),
                    SubscriberViews = table.Column<int>(type: "integer", nullable: false),
                    PromosCopied = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostAnalitics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostAnalitics_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostAnalitics_PostId",
                table: "PostAnalitics",
                column: "PostId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostAnalitics");
        }
    }
}
