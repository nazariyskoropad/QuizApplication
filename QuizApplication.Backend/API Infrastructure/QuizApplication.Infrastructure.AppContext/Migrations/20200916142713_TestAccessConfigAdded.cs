using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizApplication.Infrastructure.AppContext.Migrations
{
    public partial class TestAccessConfigAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RunsNumber",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "CreatedByIp",
                table: "RefreshToken");

            migrationBuilder.CreateTable(
                name: "TestAccessConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(nullable: false),
                    UniqueLink = table.Column<string>(nullable: true),
                    RunsNumber = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAccessConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAccessConfigs_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestAccessConfigs_TestId",
                table: "TestAccessConfigs",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestAccessConfigs");

            migrationBuilder.AddColumn<int>(
                name: "RunsNumber",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Tests",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByIp",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
