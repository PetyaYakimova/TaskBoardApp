using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoardApp.Data.Migrations
{
    public partial class DataSeeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Board identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Board name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                },
                comment: "Board");

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Task identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false, comment: "Task title"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Task description"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Task created on date"),
                    BoardId = table.Column<int>(type: "int", nullable: true, comment: "Task board identifier"),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Task application user identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Tasks");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d3f6cbb4-dca9-4726-adb4-aafec316cc46", 0, "4c3cdf4b-a309-46c6-ae56-d439491be85c", null, false, false, null, null, "TEST@SOFTUNI.BG", "AQAAAAEAACcQAAAAEOVG/qP2jF24X4vlcT21JsjASSms5tT8zgWoGX9O337JtFX/CpwJx+cpqEX/UNWiVA==", null, false, "a33cdc81-693b-423e-8fea-cfec8895792f", false, "test@sontuni.bg" });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Open" },
                    { 2, "In Progress" },
                    { 3, "Done" }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "BoardId", "CreatedOn", "Description", "OwnerId", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 7, 26, 11, 45, 48, 881, DateTimeKind.Local).AddTicks(3633), "Make it look prettier", "d3f6cbb4-dca9-4726-adb4-aafec316cc46", "Improve CSS styles" },
                    { 2, 1, new DateTime(2024, 2, 6, 11, 45, 48, 881, DateTimeKind.Local).AddTicks(3698), "Create Android client app for the TaskBoard RESTful API", "d3f6cbb4-dca9-4726-adb4-aafec316cc46", "Android Client API" },
                    { 3, 2, new DateTime(2024, 1, 11, 11, 45, 48, 881, DateTimeKind.Local).AddTicks(3706), "Create Windows oForm desctop app", "d3f6cbb4-dca9-4726-adb4-aafec316cc46", "Desktop Client App" },
                    { 4, 3, new DateTime(2023, 2, 11, 11, 45, 48, 881, DateTimeKind.Local).AddTicks(3715), "Implement [Create Task] page for adding new tasks", "d3f6cbb4-dca9-4726-adb4-aafec316cc46", "Create Tasks" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_BoardId",
                table: "Tasks",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d3f6cbb4-dca9-4726-adb4-aafec316cc46");
        }
    }
}
