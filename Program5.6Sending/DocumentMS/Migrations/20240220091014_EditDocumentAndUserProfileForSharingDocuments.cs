using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentMS.Migrations
{
    public partial class EditDocumentAndUserProfileForSharingDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentUserProfile",
                columns: table => new
                {
                    SharedDocumentsId = table.Column<long>(type: "bigint", nullable: false),
                    SharedUsersUserProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentUserProfile", x => new { x.SharedDocumentsId, x.SharedUsersUserProfileId });
                    table.ForeignKey(
                        name: "FK_DocumentUserProfile_Document_SharedDocumentsId",
                        column: x => x.SharedDocumentsId,
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentUserProfile_UserProfile_SharedUsersUserProfileId",
                        column: x => x.SharedUsersUserProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "UserProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUserProfile_SharedUsersUserProfileId",
                table: "DocumentUserProfile",
                column: "SharedUsersUserProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentUserProfile");
        }
    }
}
