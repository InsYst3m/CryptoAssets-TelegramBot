using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationBot.DataAccess.Migrations
{
    public partial class FavoriteCryptoAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteCryptoAsset",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CryptoAssetId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteCryptoAsset", x => new { x.UserId, x.CryptoAssetId });
                    table.ForeignKey(
                        name: "FK_FavoriteCryptoAsset_CryptoAssets_CryptoAssetId",
                        column: x => x.CryptoAssetId,
                        principalTable: "CryptoAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteCryptoAsset_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCryptoAsset_CryptoAssetId",
                table: "FavoriteCryptoAsset",
                column: "CryptoAssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteCryptoAsset");

            migrationBuilder.DropTable(
                name: "CryptoAssets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
