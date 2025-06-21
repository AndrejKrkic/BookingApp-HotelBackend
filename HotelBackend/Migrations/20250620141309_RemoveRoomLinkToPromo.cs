using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoomLinkToPromo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "PromoCodes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "PromoCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Code", "DiscountPercentage", "GeneratedByReservationId", "IsUsed", "RoomId", "UsedByReservationId" },
                values: new object[,]
                {
                    { 1, "DISCOUNT10", 10m, 0, false, 1, 0 },
                    { 2, "SUMMER20", 20m, 0, false, 2, 0 }
                });
        }
    }
}
