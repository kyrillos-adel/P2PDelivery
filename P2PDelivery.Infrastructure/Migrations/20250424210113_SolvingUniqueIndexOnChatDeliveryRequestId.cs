using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SolvingUniqueIndexOnChatDeliveryRequestId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_DeliveryRequests_DeliveryRequestId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_DeliveryRequestId",
                table: "Chats");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryRequestId1",
                table: "Chats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_DeliveryRequestId",
                table: "Chats",
                column: "DeliveryRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_DeliveryRequestId1",
                table: "Chats",
                column: "DeliveryRequestId1",
                unique: true,
                filter: "[DeliveryRequestId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_DeliveryRequests_DeliveryRequestId",
                table: "Chats",
                column: "DeliveryRequestId",
                principalTable: "DeliveryRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_DeliveryRequests_DeliveryRequestId1",
                table: "Chats",
                column: "DeliveryRequestId1",
                principalTable: "DeliveryRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_DeliveryRequests_DeliveryRequestId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_DeliveryRequests_DeliveryRequestId1",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_DeliveryRequestId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_DeliveryRequestId1",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "DeliveryRequestId1",
                table: "Chats");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_DeliveryRequestId",
                table: "Chats",
                column: "DeliveryRequestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_DeliveryRequests_DeliveryRequestId",
                table: "Chats",
                column: "DeliveryRequestId",
                principalTable: "DeliveryRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
