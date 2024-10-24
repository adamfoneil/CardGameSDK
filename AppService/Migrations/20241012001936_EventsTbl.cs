using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Migrations
{
	/// <inheritdoc />
	public partial class EventsTbl : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "EventMessage",
				columns: table => new
				{
					Id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
					GameInstanceId = table.Column<int>(type: "int", nullable: false),
					FromUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_EventMessage", x => x.Id);
					table.ForeignKey(
						name: "FK_EventMessage_GameInstances_GameInstanceId",
						column: x => x.GameInstanceId,
						principalTable: "GameInstances",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_EventMessage_GameInstanceId",
				table: "EventMessage",
				column: "GameInstanceId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "EventMessage");
		}
	}
}
