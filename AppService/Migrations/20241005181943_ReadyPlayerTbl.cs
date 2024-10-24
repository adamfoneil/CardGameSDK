using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Migrations
{
	/// <inheritdoc />
	public partial class ReadyPlayerTbl : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Players_AspNetUsers_UserId",
				table: "Players");

			migrationBuilder.DropForeignKey(
				name: "FK_Players_GameInstances_GameInstanceId",
				table: "Players");

			migrationBuilder.DropForeignKey(
				name: "FK_ReadyPlayer_AspNetUsers_UserId",
				table: "ReadyPlayer");

			migrationBuilder.DropUniqueConstraint(
				name: "AK_ReadyPlayer_Game_UserId",
				table: "ReadyPlayer");

			migrationBuilder.DropPrimaryKey(
				name: "PK_ReadyPlayer",
				table: "ReadyPlayer");

			migrationBuilder.DropUniqueConstraint(
				name: "AK_Players_GameInstanceId_UserId",
				table: "Players");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Players",
				table: "Players");

			migrationBuilder.RenameTable(
				name: "ReadyPlayer",
				newName: "ReadyPlayers");

			migrationBuilder.RenameTable(
				name: "Players",
				newName: "ActivePlayers");

			migrationBuilder.RenameIndex(
				name: "IX_ReadyPlayer_UserId",
				table: "ReadyPlayers",
				newName: "IX_ReadyPlayers_UserId");

			migrationBuilder.RenameIndex(
				name: "IX_Players_UserId",
				table: "ActivePlayers",
				newName: "IX_ActivePlayers_UserId");

			migrationBuilder.AddUniqueConstraint(
				name: "AK_ReadyPlayers_Game_UserId",
				table: "ReadyPlayers",
				columns: new[] { "Game", "UserId" });

			migrationBuilder.AddPrimaryKey(
				name: "PK_ReadyPlayers",
				table: "ReadyPlayers",
				column: "Id");

			migrationBuilder.AddUniqueConstraint(
				name: "AK_ActivePlayers_GameInstanceId_UserId",
				table: "ActivePlayers",
				columns: new[] { "GameInstanceId", "UserId" });

			migrationBuilder.AddPrimaryKey(
				name: "PK_ActivePlayers",
				table: "ActivePlayers",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_ActivePlayers_AspNetUsers_UserId",
				table: "ActivePlayers",
				column: "UserId",
				principalTable: "AspNetUsers",
				principalColumn: "UserId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_ActivePlayers_GameInstances_GameInstanceId",
				table: "ActivePlayers",
				column: "GameInstanceId",
				principalTable: "GameInstances",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_ReadyPlayers_AspNetUsers_UserId",
				table: "ReadyPlayers",
				column: "UserId",
				principalTable: "AspNetUsers",
				principalColumn: "UserId",
				onDelete: ReferentialAction.Restrict);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ActivePlayers_AspNetUsers_UserId",
				table: "ActivePlayers");

			migrationBuilder.DropForeignKey(
				name: "FK_ActivePlayers_GameInstances_GameInstanceId",
				table: "ActivePlayers");

			migrationBuilder.DropForeignKey(
				name: "FK_ReadyPlayers_AspNetUsers_UserId",
				table: "ReadyPlayers");

			migrationBuilder.DropUniqueConstraint(
				name: "AK_ReadyPlayers_Game_UserId",
				table: "ReadyPlayers");

			migrationBuilder.DropPrimaryKey(
				name: "PK_ReadyPlayers",
				table: "ReadyPlayers");

			migrationBuilder.DropUniqueConstraint(
				name: "AK_ActivePlayers_GameInstanceId_UserId",
				table: "ActivePlayers");

			migrationBuilder.DropPrimaryKey(
				name: "PK_ActivePlayers",
				table: "ActivePlayers");

			migrationBuilder.RenameTable(
				name: "ReadyPlayers",
				newName: "ReadyPlayer");

			migrationBuilder.RenameTable(
				name: "ActivePlayers",
				newName: "Players");

			migrationBuilder.RenameIndex(
				name: "IX_ReadyPlayers_UserId",
				table: "ReadyPlayer",
				newName: "IX_ReadyPlayer_UserId");

			migrationBuilder.RenameIndex(
				name: "IX_ActivePlayers_UserId",
				table: "Players",
				newName: "IX_Players_UserId");

			migrationBuilder.AddUniqueConstraint(
				name: "AK_ReadyPlayer_Game_UserId",
				table: "ReadyPlayer",
				columns: new[] { "Game", "UserId" });

			migrationBuilder.AddPrimaryKey(
				name: "PK_ReadyPlayer",
				table: "ReadyPlayer",
				column: "Id");

			migrationBuilder.AddUniqueConstraint(
				name: "AK_Players_GameInstanceId_UserId",
				table: "Players",
				columns: new[] { "GameInstanceId", "UserId" });

			migrationBuilder.AddPrimaryKey(
				name: "PK_Players",
				table: "Players",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Players_AspNetUsers_UserId",
				table: "Players",
				column: "UserId",
				principalTable: "AspNetUsers",
				principalColumn: "UserId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Players_GameInstances_GameInstanceId",
				table: "Players",
				column: "GameInstanceId",
				principalTable: "GameInstances",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_ReadyPlayer_AspNetUsers_UserId",
				table: "ReadyPlayer",
				column: "UserId",
				principalTable: "AspNetUsers",
				principalColumn: "UserId",
				onDelete: ReferentialAction.Restrict);
		}
	}
}
