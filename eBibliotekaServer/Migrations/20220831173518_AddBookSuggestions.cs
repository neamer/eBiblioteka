using Microsoft.EntityFrameworkCore.Migrations;

namespace eBibliotekaServer.Migrations
{
    public partial class AddBookSuggestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookSuggestions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LibraryID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookSuggestions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BookSuggestions_Accounts_UserID",
                        column: x => x.UserID,
                        principalTable: "Accounts",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_BookSuggestions_Libraries_LibraryID",
                        column: x => x.LibraryID,
                        principalTable: "Libraries",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookSuggestions_LibraryID",
                table: "BookSuggestions",
                column: "LibraryID");

            migrationBuilder.CreateIndex(
                name: "IX_BookSuggestions_UserID",
                table: "BookSuggestions",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookSuggestions");
        }
    }
}
