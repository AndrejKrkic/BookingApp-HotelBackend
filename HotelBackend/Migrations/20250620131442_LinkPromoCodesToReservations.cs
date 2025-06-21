using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBackend.Migrations
{
    /// <inheritdoc />
    public partial class LinkPromoCodesToReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneratedPromoCode",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsedPromoCodeId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GeneratedByReservationId",
                table: "PromoCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsedByReservationId",
                table: "PromoCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "GeneratedByReservationId", "UsedByReservationId" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "GeneratedByReservationId", "UsedByReservationId" },
                values: new object[] { 0, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratedPromoCode",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "UsedPromoCodeId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "GeneratedByReservationId",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "UsedByReservationId",
                table: "PromoCodes");
        }
    }
}
