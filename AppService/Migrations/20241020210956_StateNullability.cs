using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Migrations
{
	/// <inheritdoc />
	public partial class StateNullability : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "State",
				table: "GameInstances",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AddColumn<string>(
				name: "Winner",
				table: "GameInstances",
				type: "nvarchar(50)",
				maxLength: 50,
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Winner",
				table: "GameInstances");

			migrationBuilder.AlterColumn<string>(
				name: "State",
				table: "GameInstances",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);
		}
	}
}
