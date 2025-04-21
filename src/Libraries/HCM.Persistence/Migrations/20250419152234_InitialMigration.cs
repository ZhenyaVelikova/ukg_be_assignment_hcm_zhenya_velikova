using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HCM.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Positions_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonType = table.Column<byte>(type: "tinyint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ReportsToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PositionEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Departments_DepartmentEntityId",
                        column: x => x.DepartmentEntityId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_People_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_People_ReportsToId",
                        column: x => x.ReportsToId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Positions_PositionEntityId",
                        column: x => x.PositionEntityId,
                        principalTable: "Positions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_People_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DisplayName", "ModifiedAt", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { new Guid("3b9a2100-5e8e-4e57-a171-5af606db8b9b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "Manager", null, null, "MANAGER" },
                    { new Guid("8387a601-6341-4787-b779-f4efc2c8f33f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "System Administrator", null, null, "SYSTEM_ADMIN" },
                    { new Guid("8cabb666-6f24-44ba-89ce-abd55e8782ed"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "Employee", null, null, "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "ModifiedAt", "ModifiedById", "Password", "UserName" },
                values: new object[] { new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), true, null, null, "AFYNtbZhukrYLlPRPf/AZDgPfDwQhqOd1MKeISmNkSP5K2UaHLOUErSl/4iaoWZO4Q==", "admin" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "ModifiedAt", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { new Guid("18220425-685f-4105-ab7b-7aa8ec50c5df"), new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), true, null, null, "Management" },
                    { new Guid("aa6228b7-3507-42ce-a479-7335d9bb396b"), new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), true, null, null, "Software" },
                    { new Guid("efc48980-042d-4e52-98d9-daa6c1deeaa7"), new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), true, null, null, "HR" }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "ModifiedAt", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { new Guid("44461ae7-0441-495d-bd78-1e67b5c9cc43"), new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), true, null, null, "Software Engineer" },
                    { new Guid("adb704cc-ee89-4f34-9b69-8f7a54d80e0d"), new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), true, null, null, "CEO" },
                    { new Guid("ba3ca0a8-1560-4734-bc7b-7e6c3103d07d"), new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), true, null, null, "HR Manager" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("8387a601-6341-4787-b779-f4efc2c8f33f"), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "ModifiedAt", "ModifiedById", "Password", "UserName" },
                values: new object[,]
                {
                    { new Guid("4bd3c8c1-b7fd-4f91-a629-71e7182ac88a"), new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), true, null, null, "AFYNtbZhukrYLlPRPf/AZDgPfDwQhqOd1MKeISmNkSP5K2UaHLOUErSl/4iaoWZO4Q==", "taylor.jones" },
                    { new Guid("a97a0432-6a4f-4f88-8997-af7cc7f806c2"), new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), true, null, null, "AFYNtbZhukrYLlPRPf/AZDgPfDwQhqOd1MKeISmNkSP5K2UaHLOUErSl/4iaoWZO4Q==", "stuart.brooks" },
                    { new Guid("ef81dce2-ff4d-4abf-88a7-6ed24d13e4b6"), new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), true, null, null, "AFYNtbZhukrYLlPRPf/AZDgPfDwQhqOd1MKeISmNkSP5K2UaHLOUErSl/4iaoWZO4Q==", "john.smith" }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DepartmentEntityId", "DepartmentId", "Email", "FirstName", "IsActive", "LastName", "ModifiedAt", "ModifiedById", "PersonType", "PositionEntityId", "PositionId", "ReportsToId", "StartDate", "TerminationDate", "UserId" },
                values: new object[] { new Guid("79b22586-b339-445e-8c2b-9aa4381f0e01"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), null, new Guid("18220425-685f-4105-ab7b-7aa8ec50c5df"), "john.smith@domain.com", "John", true, "Smith", null, null, (byte)0, null, new Guid("adb704cc-ee89-4f34-9b69-8f7a54d80e0d"), null, new DateTime(2023, 5, 18, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), null, new Guid("ef81dce2-ff4d-4abf-88a7-6ed24d13e4b6") });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("3b9a2100-5e8e-4e57-a171-5af606db8b9b"), new Guid("4bd3c8c1-b7fd-4f91-a629-71e7182ac88a") },
                    { new Guid("8cabb666-6f24-44ba-89ce-abd55e8782ed"), new Guid("a97a0432-6a4f-4f88-8997-af7cc7f806c2") },
                    { new Guid("8387a601-6341-4787-b779-f4efc2c8f33f"), new Guid("ef81dce2-ff4d-4abf-88a7-6ed24d13e4b6") }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DepartmentEntityId", "DepartmentId", "Email", "FirstName", "IsActive", "LastName", "ModifiedAt", "ModifiedById", "PersonType", "PositionEntityId", "PositionId", "ReportsToId", "StartDate", "TerminationDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("9ee79e7b-7c87-4f84-af10-85f504ad348c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), null, new Guid("efc48980-042d-4e52-98d9-daa6c1deeaa7"), "taylor.jones@domain.com", "Taylor", true, "Jones", null, null, (byte)0, null, new Guid("ba3ca0a8-1560-4734-bc7b-7e6c3103d07d"), new Guid("79b22586-b339-445e-8c2b-9aa4381f0e01"), new DateTime(2023, 6, 18, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), null, new Guid("4bd3c8c1-b7fd-4f91-a629-71e7182ac88a") },
                    { new Guid("fe337be1-1e81-4e54-9d0b-a45cd6b09527"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"), null, new Guid("aa6228b7-3507-42ce-a479-7335d9bb396b"), "stuart.brooks@domain.com", "Stuart", true, "Brooks", null, null, (byte)0, null, new Guid("44461ae7-0441-495d-bd78-1e67b5c9cc43"), new Guid("79b22586-b339-445e-8c2b-9aa4381f0e01"), new DateTime(2023, 7, 18, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505), null, new Guid("a97a0432-6a4f-4f88-8997-af7cc7f806c2") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedById",
                table: "Departments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ModifiedById",
                table: "Departments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_CreatedById",
                table: "People",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_People_DepartmentEntityId",
                table: "People",
                column: "DepartmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_People_DepartmentId",
                table: "People",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_People_Email",
                table: "People",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_ModifiedById",
                table: "People",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_People_PositionEntityId",
                table: "People",
                column: "PositionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_People_PositionId",
                table: "People",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_People_ReportsToId",
                table: "People",
                column: "ReportsToId");

            migrationBuilder.CreateIndex(
                name: "IX_People_UserId",
                table: "People",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CreatedById",
                table: "Positions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_ModifiedById",
                table: "Positions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Name",
                table: "Positions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifiedById",
                table: "Users",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
