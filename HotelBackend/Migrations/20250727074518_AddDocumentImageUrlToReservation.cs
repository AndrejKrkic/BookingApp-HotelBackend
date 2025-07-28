using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentImageUrlToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentImageUrl",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentImageUrl",
                table: "Reservations");
        }
    }
}
