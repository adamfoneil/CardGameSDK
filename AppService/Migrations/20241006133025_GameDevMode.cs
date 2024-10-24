using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Migrations
{
	/// <inheritdoc />
	public partial class GameDevMode : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "IsDevMode",
				table: "GameInstances",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "IsDevMode",
				table: "GameInstances");
		}
	}
}
