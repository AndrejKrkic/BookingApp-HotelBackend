using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddGuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GuestName",
                table: "Reservations",
                newName: "Token");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Reservations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReservationGuests",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationGuests", x => new { x.ReservationId, x.GuestId });
                    table.ForeignKey(
                        name: "FK_ReservationGuests_Guests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationGuests_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationGuests_GuestId",
                table: "ReservationGuests",
                column: "GuestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationGuests");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Reservations",
                newName: "GuestName");
        }
    }
}
