using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksApi.Migrations
{
    /// <inheritdoc />
    public partial class MigrationBorrow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowModel_Books_BookId",
                table: "BorrowModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowModel_Users_UserId",
                table: "BorrowModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowModel",
                table: "BorrowModel");

            migrationBuilder.RenameTable(
                name: "BorrowModel",
                newName: "Borrow");

            migrationBuilder.RenameIndex(
                name: "IX_BorrowModel_UserId",
                table: "Borrow",
                newName: "IX_Borrow_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BorrowModel_BookId",
                table: "Borrow",
                newName: "IX_Borrow_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Borrow",
                table: "Borrow",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrow_Books_BookId",
                table: "Borrow",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrow_Users_UserId",
                table: "Borrow",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrow_Books_BookId",
                table: "Borrow");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrow_Users_UserId",
                table: "Borrow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Borrow",
                table: "Borrow");

            migrationBuilder.RenameTable(
                name: "Borrow",
                newName: "BorrowModel");

            migrationBuilder.RenameIndex(
                name: "IX_Borrow_UserId",
                table: "BorrowModel",
                newName: "IX_BorrowModel_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrow_BookId",
                table: "BorrowModel",
                newName: "IX_BorrowModel_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowModel",
                table: "BorrowModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowModel_Books_BookId",
                table: "BorrowModel",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowModel_Users_UserId",
                table: "BorrowModel",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
