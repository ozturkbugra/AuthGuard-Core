using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthGuardCore.Migrations
{
    /// <inheritdoc />
    public partial class MessageTableAppuserNavigationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SenderEmail",
                table: "Messages",
                type: "nvarchar(256)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RecieverEmail",
                table: "Messages",
                type: "nvarchar(256)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecieverEmail",
                table: "Messages",
                column: "RecieverEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderEmail",
                table: "Messages",
                column: "SenderEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_RecieverEmail",
                table: "Messages",
                column: "RecieverEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderEmail",
                table: "Messages",
                column: "SenderEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_RecieverEmail",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderEmail",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RecieverEmail",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderEmail",
                table: "Messages");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "SenderEmail",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)");

            migrationBuilder.AlterColumn<string>(
                name: "RecieverEmail",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
