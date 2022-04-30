using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationBot.DataAccess.Migrations
{
    public partial class FavoriteCryptoAssetsTableRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCryptoAsset_CryptoAssets_CryptoAssetId",
                table: "FavoriteCryptoAsset");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCryptoAsset_Users_UserId",
                table: "FavoriteCryptoAsset");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteCryptoAsset",
                table: "FavoriteCryptoAsset");

            migrationBuilder.RenameTable(
                name: "FavoriteCryptoAsset",
                newName: "FavoriteCryptoAssets");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteCryptoAsset_CryptoAssetId",
                table: "FavoriteCryptoAssets",
                newName: "IX_FavoriteCryptoAssets_CryptoAssetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteCryptoAssets",
                table: "FavoriteCryptoAssets",
                columns: new[] { "UserId", "CryptoAssetId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCryptoAssets_CryptoAssets_CryptoAssetId",
                table: "FavoriteCryptoAssets",
                column: "CryptoAssetId",
                principalTable: "CryptoAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCryptoAssets_Users_UserId",
                table: "FavoriteCryptoAssets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCryptoAssets_CryptoAssets_CryptoAssetId",
                table: "FavoriteCryptoAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCryptoAssets_Users_UserId",
                table: "FavoriteCryptoAssets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteCryptoAssets",
                table: "FavoriteCryptoAssets");

            migrationBuilder.RenameTable(
                name: "FavoriteCryptoAssets",
                newName: "FavoriteCryptoAsset");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteCryptoAssets_CryptoAssetId",
                table: "FavoriteCryptoAsset",
                newName: "IX_FavoriteCryptoAsset_CryptoAssetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteCryptoAsset",
                table: "FavoriteCryptoAsset",
                columns: new[] { "UserId", "CryptoAssetId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCryptoAsset_CryptoAssets_CryptoAssetId",
                table: "FavoriteCryptoAsset",
                column: "CryptoAssetId",
                principalTable: "CryptoAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCryptoAsset_Users_UserId",
                table: "FavoriteCryptoAsset",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
