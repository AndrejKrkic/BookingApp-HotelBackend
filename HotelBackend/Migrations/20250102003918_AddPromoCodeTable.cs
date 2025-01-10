using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddPromoCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Code", "DiscountPercentage", "IsUsed", "RoomId" },
                values: new object[,]
                {
                    { 1, "DISCOUNT10", 10m, false, 1 },
                    { 2, "SUMMER20", 20m, false, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromoCodes");

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Capacity", "IsAvailable", "Name", "PricePerNight" },
                values: new object[,]
                {
                    { 1, 1, true, "Single Room", 50m },
                    { 2, 2, true, "Double Room", 90m }
                });
        }
    }
}
