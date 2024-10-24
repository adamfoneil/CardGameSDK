using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Migrations
{
	/// <inheritdoc />
	public partial class GameInstanceUrl : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "Url",
				table: "GameInstances",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				defaultValue: "");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Url",
				table: "GameInstances");
		}
	}
}
