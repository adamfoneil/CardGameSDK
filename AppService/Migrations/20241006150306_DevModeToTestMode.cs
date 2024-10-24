using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Migrations
{
	/// <inheritdoc />
	public partial class DevModeToTestMode : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "IsDevMode",
				table: "GameInstances",
				newName: "IsTestMode");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "IsTestMode",
				table: "GameInstances",
				newName: "IsDevMode");
		}
	}
}
