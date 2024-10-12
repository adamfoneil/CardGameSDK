using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Migrations
{
    /// <inheritdoc />
    public partial class EventGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventMessage_GameInstances_GameInstanceId",
                table: "EventMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventMessage",
                table: "EventMessage");

            migrationBuilder.RenameTable(
                name: "EventMessage",
                newName: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_EventMessage_GameInstanceId",
                table: "Events",
                newName: "IX_Events_GameInstanceId");

            migrationBuilder.AlterColumn<int>(
                name: "GameInstanceId",
                table: "Events",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Game",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_GameInstances_GameInstanceId",
                table: "Events",
                column: "GameInstanceId",
                principalTable: "GameInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_GameInstances_GameInstanceId",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Game",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "EventMessage");

            migrationBuilder.RenameIndex(
                name: "IX_Events_GameInstanceId",
                table: "EventMessage",
                newName: "IX_EventMessage_GameInstanceId");

            migrationBuilder.AlterColumn<int>(
                name: "GameInstanceId",
                table: "EventMessage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventMessage",
                table: "EventMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventMessage_GameInstances_GameInstanceId",
                table: "EventMessage",
                column: "GameInstanceId",
                principalTable: "GameInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
