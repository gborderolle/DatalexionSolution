using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatalexionBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Entity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zoom = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Circuit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LatLong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlankVotes = table.Column<int>(type: "int", nullable: false),
                    NullVotes = table.Column<int>(type: "int", nullable: false),
                    ObservedVotes = table.Column<int>(type: "int", nullable: false),
                    RecurredVotes = table.Column<int>(type: "int", nullable: false),
                    Step1completed = table.Column<bool>(type: "bit", nullable: false),
                    Step2completed = table.Column<bool>(type: "bit", nullable: false),
                    Step3completed = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdateDelegadoId = table.Column<int>(type: "int", nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    MunicipalityId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Circuit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CircuitDelegado",
                columns: table => new
                {
                    CircuitId = table.Column<int>(type: "int", nullable: false),
                    DelegadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircuitDelegado", x => new { x.CircuitId, x.DelegadoId });
                    table.ForeignKey(
                        name: "FK_CircuitDelegado_Circuit_CircuitId",
                        column: x => x.CircuitId,
                        principalTable: "Circuit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CircuitParty",
                columns: table => new
                {
                    CircuitId = table.Column<int>(type: "int", nullable: false),
                    PartyId = table.Column<int>(type: "int", nullable: false),
                    Votes = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircuitParty", x => new { x.CircuitId, x.PartyId });
                    table.ForeignKey(
                        name: "FK_CircuitParty_Circuit_CircuitId",
                        column: x => x.CircuitId,
                        principalTable: "Circuit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CircuitSlate",
                columns: table => new
                {
                    CircuitId = table.Column<int>(type: "int", nullable: false),
                    SlateId = table.Column<int>(type: "int", nullable: false),
                    Votes = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircuitSlate", x => new { x.CircuitId, x.SlateId });
                    table.ForeignKey(
                        name: "FK_CircuitSlate_Circuit_CircuitId",
                        column: x => x.CircuitId,
                        principalTable: "Circuit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Delegado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delegado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delegado_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Municipality",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false),
                    DelegadoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipality", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipality_Delegado_DelegadoId",
                        column: x => x.DelegadoId,
                        principalTable: "Delegado",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Municipality_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SlateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Party",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Votes = table.Column<int>(type: "int", nullable: true),
                    PhotoLongId = table.Column<int>(type: "int", nullable: true),
                    PhotoShortId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Party", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CircuitId = table.Column<int>(type: "int", nullable: true),
                    CandidateId = table.Column<int>(type: "int", nullable: true),
                    CandidateId1 = table.Column<int>(type: "int", nullable: true),
                    PartyLongId = table.Column<int>(type: "int", nullable: true),
                    PartyShortId = table.Column<int>(type: "int", nullable: true),
                    WingId = table.Column<int>(type: "int", nullable: true),
                    WingId1 = table.Column<int>(type: "int", nullable: true),
                    SlateId = table.Column<int>(type: "int", nullable: true),
                    SlateId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photo_Candidate_CandidateId1",
                        column: x => x.CandidateId1,
                        principalTable: "Candidate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Photo_Circuit_CircuitId",
                        column: x => x.CircuitId,
                        principalTable: "Circuit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Photo_Party_PartyLongId",
                        column: x => x.PartyLongId,
                        principalTable: "Party",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Photo_Party_PartyShortId",
                        column: x => x.PartyShortId,
                        principalTable: "Party",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Wing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: true),
                    PartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wing_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wing_Photo_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Slate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Votes = table.Column<int>(type: "int", nullable: true),
                    CandidateId = table.Column<int>(type: "int", nullable: true),
                    PhotoId = table.Column<int>(type: "int", nullable: true),
                    WingId = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slate_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Slate_Photo_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Slate_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Slate_Wing_WingId",
                        column: x => x.WingId,
                        principalTable: "Wing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Creation", "Discriminator", "Name", "NormalizedName", "Update" },
                values: new object[,]
                {
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", null, new DateTime(2024, 3, 28, 17, 43, 3, 161, DateTimeKind.Local).AddTicks(1402), "DatalexionRole", "Admin", "ADMIN", new DateTime(2024, 3, 28, 17, 43, 3, 161, DateTimeKind.Local).AddTicks(1403) },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", null, new DateTime(2024, 3, 28, 17, 43, 3, 161, DateTimeKind.Local).AddTicks(1417), "DatalexionRole", "Analyst", "ANALYST", new DateTime(2024, 3, 28, 17, 43, 3, 161, DateTimeKind.Local).AddTicks(1418) }
                });

            migrationBuilder.InsertData(
                table: "Photo",
                columns: new[] { "Id", "CandidateId", "CandidateId1", "CircuitId", "Creation", "PartyLongId", "PartyShortId", "SlateId", "SlateId1", "URL", "Update", "WingId", "WingId1" },
                values: new object[,]
                {
                    { 1, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2648), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2664), null, null },
                    { 2, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2684), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2684), null, null },
                    { 3, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2686), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo3.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2687), null, null },
                    { 4, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2693), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo4.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2693), null, null },
                    { 5, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2695), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo5.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2696), null, null },
                    { 6, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2716), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo6.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2716), null, null },
                    { 7, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2723), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo7.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2723), null, null },
                    { 8, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2727), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo8.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2727), null, null },
                    { 9, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2730), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo9.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2730), null, null },
                    { 10, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2734), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo10.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(2734), null, null },
                    { 11, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3173), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo11.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3176), null, null },
                    { 12, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3177), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo12.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3178), null, null },
                    { 13, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3179), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo13.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3179), null, null },
                    { 14, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3181), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo14.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3181), null, null },
                    { 15, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3182), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo15.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3183), null, null },
                    { 16, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3184), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo16.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3184), null, null },
                    { 101, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4008), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo101.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4008), null, null },
                    { 102, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4011), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo102.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4012), null, null },
                    { 103, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4013), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo103.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4013), null, null },
                    { 104, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4019), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo104.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4021), null, null },
                    { 105, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4022), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo105.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4022), null, null },
                    { 106, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4024), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo106.jpg", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4024), null, null },
                    { 111, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4026), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo111.png", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4026), null, null },
                    { 112, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4027), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo112.png", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4028), null, null },
                    { 113, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4029), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo113.png", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4030), null, null },
                    { 114, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4087), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo114.png", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4088), null, null },
                    { 115, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4089), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo115.png", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4089), null, null },
                    { 116, null, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4090), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo116.png", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4092), null, null }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "Id", "Center", "Comments", "Creation", "Name", "Update", "Zoom" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4356), "Montevideo", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4356), null },
                    { 2, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4357), "Canelones", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4358), null },
                    { 3, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4358), "Maldonado", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4359), null },
                    { 4, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4359), "Rocha", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4360), null },
                    { 5, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4360), "Colonia", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4361), null },
                    { 6, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4361), "Artigas", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4362), null },
                    { 7, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4362), "Salto", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4362), null },
                    { 8, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4363), "Paysandú", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4363), null },
                    { 9, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4364), "Tacuarembó", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4365), null },
                    { 10, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4366), "Rivera", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4366), null },
                    { 11, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4367), "San José", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4367), null },
                    { 12, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4368), "Durazno", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4368), null },
                    { 13, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4369), "Treinta y Tres", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4369), null },
                    { 14, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4370), "Cerro Largo", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4370), null },
                    { 15, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4371), "Rivera", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4371), null },
                    { 16, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4371), "Flores", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4372), null },
                    { 17, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4372), "Florida", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4373), null },
                    { 18, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4373), "Lavalleja", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4374), null },
                    { 19, null, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4374), "Soriano", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4375), null }
                });

            migrationBuilder.InsertData(
                table: "Candidate",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3310), "Álvaro Delgado", 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3311) },
                    { 2, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3351), "Laura Raffo", 2, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3352) },
                    { 3, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3353), "Jorge Gandini", 3, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3353) },
                    { 4, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3354), "Juan Sartori", 4, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3354) },
                    { 5, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3356), "Yamandú Orsi", 5, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3357) },
                    { 6, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3358), "Carolina Cosse", 6, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3358) },
                    { 7, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3359), "Mario Bergara", 7, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3359) },
                    { 8, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3360), "Pablo Mieres", 8, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3360) },
                    { 9, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3361), "Edgardo Novick", 9, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3361) },
                    { 10, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3362), "Andrés Lima", 10, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3362) },
                    { 11, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3363), "Gabriel Gurméndez", 11, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3363) },
                    { 12, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3364), "Robert Silva", 12, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3364) },
                    { 13, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3365), "Andrés Ojeda", 13, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3366) },
                    { 14, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3366), "Gustavo Zubía", 14, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3367) },
                    { 15, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3367), "Guzmán Acosta y Lara", 15, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3368) },
                    { 16, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3369), "Tabaré Viera", 16, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3369) }
                });

            migrationBuilder.InsertData(
                table: "Party",
                columns: new[] { "Id", "Color", "Comments", "Creation", "Name", "PhotoLongId", "PhotoShortId", "ShortName", "Update", "Votes" },
                values: new object[,]
                {
                    { 1, "#3153dd", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4121), "Frente Amplio", 101, 111, "FA", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4122), null },
                    { 2, "#3153dd", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4130), "Partido Nacional", 102, 112, "PN", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4131), null },
                    { 3, "#d62929", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4133), "Partido Colorado", 103, 113, "PC", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4134), null },
                    { 4, "#b929d6", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4135), "Partido Independiente", 104, 114, "PI", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4136), null },
                    { 5, "#f9bb28", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4138), "Cabildo Abierto", 105, 115, "CA", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4138), null },
                    { 6, "#009001", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4139), "PERI", 106, 116, "PERI", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4139), null }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "Update" },
                values: new object[,]
                {
                    { 1, "Frente Amplio", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4190), "Frente Amplio", 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4192) },
                    { 2, "Partido Nacional", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4196), "Partido Nacional", 2, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4196) },
                    { 3, "Partido Colorado", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4198), "Partido Colorado", 3, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4198) }
                });

            migrationBuilder.InsertData(
                table: "Wing",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4291), "FA", 1, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4291) },
                    { 2, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4297), "PN", 2, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4297) },
                    { 3, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4298), "PC", 3, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4298) },
                    { 4, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4299), "PI", 4, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4299) },
                    { 5, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4300), "CA", 5, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4300) },
                    { 6, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4302), "PERI", 6, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4302) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Creation", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Update", "UserName" },
                values: new object[,]
                {
                    { "2a765d8b-9204-4e0f-b4ce-453f6e1bb592", 0, 1, "b52143f4-dcab-41b9-9ae8-ec874bae2889", new DateTime(2024, 3, 28, 17, 43, 3, 161, DateTimeKind.Local).AddTicks(1673), "DatalexionUser", "adminfa@datalexion.lat", false, false, null, "Admin FA", "ADMINFA@DATALEXION.LAT", "ADMINFA", "AQAAAAIAAYagAAAAEOSHysGdRtEaJJ7rxOlHR8tY3Ae8timK/7kZgVvSKvFPtgeeBFyP7eycW2y4IiKpng==", null, false, "5674bae7-9d5d-420a-a903-c26b9fe67aff", false, new DateTime(2024, 3, 28, 17, 43, 3, 161, DateTimeKind.Local).AddTicks(1674), "adminfa" },
                    { "6c762a89-a7b6-4ee3-96d0-105b219dcaa6", 0, 3, "5425ae10-440d-4406-8bcc-5ac96b28bb40", new DateTime(2024, 3, 28, 17, 43, 3, 367, DateTimeKind.Local).AddTicks(8369), "DatalexionUser", "adminpc@datalexion.lat", false, false, null, "Admin PC", "ADMINPC@DATALEXION.LAT", "ADMINPC", "AQAAAAIAAYagAAAAEP6kh6rQnBBeP7OUl7c30+u1XmZ8XUYNrdmevGU13ZaBjdMxsRia12+PmxoHwl/Odg==", null, false, "12b33634-79ce-4cd9-99f6-0c8a7dd81d5a", false, new DateTime(2024, 3, 28, 17, 43, 3, 367, DateTimeKind.Local).AddTicks(8377), "adminpc" },
                    { "8498a3ff-ca69-4b93-9a37-49a73c8dec77", 0, 2, "89677d93-87c2-43b4-ba35-18e3086c2b87", new DateTime(2024, 3, 28, 17, 43, 3, 261, DateTimeKind.Local).AddTicks(8084), "DatalexionUser", "adminpn@datalexion.lat", false, false, null, "Admin PN", "ADMINPN@DATALEXION.LAT", "ADMINPN", "AQAAAAIAAYagAAAAEPiv3WIn7G+y74E8ZK4wMcTCMhV6TPFS094YvyIFsUPjicxYxAeqt+WfBRDM+LwZdQ==", null, false, "4d38e36c-c474-40d8-b5a1-7a9b9bddd37e", false, new DateTime(2024, 3, 28, 17, 43, 3, 261, DateTimeKind.Local).AddTicks(8090), "adminpn" },
                    { "b5172b14-f9e4-48f6-9634-2241c87f1719", 0, 3, "643bb06a-7279-4f60-abb0-ef35145d1974", new DateTime(2024, 3, 28, 17, 43, 3, 636, DateTimeKind.Local).AddTicks(6864), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista PC", "NORMAL@DATALEXION.LAT", "ANALYSTPC", "AQAAAAIAAYagAAAAECvM/R/hrZq8ZPxiEJd4B/kE25JJu86DjTNq0BgfySI3TzG1zRK6MSFEuBYWKjH9Eg==", null, false, "7687bfc4-aadd-4aba-a77b-65a9d6fb7b74", false, new DateTime(2024, 3, 28, 17, 43, 3, 636, DateTimeKind.Local).AddTicks(6872), "analystpc" },
                    { "ddc18aa2-c5c7-40c9-9db3-246d2a05a06c", 0, 2, "07ea9ed0-8578-4085-80f1-4acc0841d524", new DateTime(2024, 3, 28, 17, 43, 3, 544, DateTimeKind.Local).AddTicks(4097), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista PN", "NORMAL@DATALEXION.LAT", "ANALYSTPN", "AQAAAAIAAYagAAAAEPv4q3HkQdGMr2WwesvakJs98TMJLqlntsU7F7aQwDtSC01C+zqbsHqyxrNUrvrsEw==", null, false, "7898ed17-e63e-4331-9bb2-7b045de38a73", false, new DateTime(2024, 3, 28, 17, 43, 3, 544, DateTimeKind.Local).AddTicks(4110), "analystpn" },
                    { "e15e9299-d3b5-42fc-b101-44da6ad799de", 0, 1, "c40e3e13-cb05-450c-b552-821422a94376", new DateTime(2024, 3, 28, 17, 43, 3, 458, DateTimeKind.Local).AddTicks(7472), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista FA", "NORMAL@DATALEXION.LAT", "ANALYSTFA", "AQAAAAIAAYagAAAAEIOjiuikAdA+KwGtEFTgMhDt/FZMQx53uUEJhoAy5eOyGlCz9wk1J9/fFOUZcVY7oA==", null, false, "beef589a-aae1-439c-8e72-eb4fe3ab7e2d", false, new DateTime(2024, 3, 28, 17, 43, 3, 458, DateTimeKind.Local).AddTicks(7482), "analystfa" }
                });

            migrationBuilder.InsertData(
                table: "Delegado",
                columns: new[] { "Id", "CI", "ClientId", "Comments", "Creation", "Email", "Name", "Phone", "Update" },
                values: new object[,]
                {
                    { 1, "11111111", 2, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4237), "gonzalo.delegado@datalexion.lat", "Gonzalo", "099415831", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4237) },
                    { 2, "12345678", 2, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4240), "diego.delegado@datalexion.lat", "Diego", "099589896", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4241) },
                    { 3, "22222222", 1, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4242), "pablo.delegado@datalexion.lat", "Pablo", "099415831", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4243) },
                    { 4, "33333333", 3, null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4244), "gabriel.delegado@datalexion.lat", "Gabriel", "099415831", new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(4244) }
                });

            migrationBuilder.InsertData(
                table: "Slate",
                columns: new[] { "Id", "CandidateId", "Color", "Comments", "Creation", "Name", "PhotoId", "ProvinceId", "Update", "Votes", "WingId" },
                values: new object[,]
                {
                    { 1, 6, "#3153dd", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3428), "5005", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3428), null, 1 },
                    { 2, 5, "#3153dd", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3434), "711", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3434), null, 1 },
                    { 3, 6, "#d62929", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3436), "90", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3436), null, 1 },
                    { 4, 5, "#b929d6", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3438), "609", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3438), null, 1 },
                    { 5, 1, "#bec11a", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3439), "71", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3439), null, 2 },
                    { 6, 1, "#3153dd", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3442), "404", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3442), null, 2 },
                    { 7, 2, "#ff0000", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3444), "40", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3444), null, 2 },
                    { 8, 2, "#00ff00", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3445), "250", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3445), null, 2 },
                    { 9, 2, "#0000ff", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3447), "880", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3447), null, 2 },
                    { 10, 11, "#ff00ff", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3453), "15", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3453), null, 3 },
                    { 11, 12, "#987654", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3454), "85", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3455), null, 3 },
                    { 12, 12, "#abcdef", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3456), "1", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3456), null, 3 },
                    { 13, 12, "#fedcba", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3930), "2000", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3931), null, 3 },
                    { 14, 11, "#012345", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3933), "1515", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3933), null, 3 },
                    { 15, 11, "#012345", null, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3934), "600", null, 1, new DateTime(2024, 3, 28, 17, 43, 3, 741, DateTimeKind.Local).AddTicks(3935), null, 3 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "role", "Admin", "2a765d8b-9204-4e0f-b4ce-453f6e1bb592" },
                    { 2, "role", "Admin", "8498a3ff-ca69-4b93-9a37-49a73c8dec77" },
                    { 3, "role", "Admin", "6c762a89-a7b6-4ee3-96d0-105b219dcaa6" },
                    { 4, "role", "Analyst", "e15e9299-d3b5-42fc-b101-44da6ad799de" },
                    { 5, "role", "Analyst", "ddc18aa2-c5c7-40c9-9db3-246d2a05a06c" },
                    { 6, "role", "Analyst", "b5172b14-f9e4-48f6-9634-2241c87f1719" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", "2a765d8b-9204-4e0f-b4ce-453f6e1bb592" },
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", "6c762a89-a7b6-4ee3-96d0-105b219dcaa6" },
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", "8498a3ff-ca69-4b93-9a37-49a73c8dec77" },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", "b5172b14-f9e4-48f6-9634-2241c87f1719" },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", "ddc18aa2-c5c7-40c9-9db3-246d2a05a06c" },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", "e15e9299-d3b5-42fc-b101-44da6ad799de" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClientId",
                table: "AspNetUsers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_PhotoId",
                table: "Candidate",
                column: "PhotoId",
                unique: true,
                filter: "[PhotoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Circuit_MunicipalityId",
                table: "Circuit",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Circuit_MunicipalityId1",
                table: "Circuit",
                column: "MunicipalityId1");

            migrationBuilder.CreateIndex(
                name: "IX_CircuitDelegado_DelegadoId",
                table: "CircuitDelegado",
                column: "DelegadoId");

            migrationBuilder.CreateIndex(
                name: "IX_CircuitParty_PartyId",
                table: "CircuitParty",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_CircuitSlate_SlateId",
                table: "CircuitSlate",
                column: "SlateId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_PartyId",
                table: "Client",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegado_ClientId",
                table: "Delegado",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipality_DelegadoId",
                table: "Municipality",
                column: "DelegadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipality_ProvinceId",
                table: "Municipality",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_SlateId",
                table: "Participant",
                column: "SlateId");

            migrationBuilder.CreateIndex(
                name: "IX_Party_PhotoLongId",
                table: "Party",
                column: "PhotoLongId",
                unique: true,
                filter: "[PhotoLongId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Party_PhotoShortId",
                table: "Party",
                column: "PhotoShortId",
                unique: true,
                filter: "[PhotoShortId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_CandidateId1",
                table: "Photo",
                column: "CandidateId1");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_CircuitId",
                table: "Photo",
                column: "CircuitId");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_PartyLongId",
                table: "Photo",
                column: "PartyLongId");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_PartyShortId",
                table: "Photo",
                column: "PartyShortId");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_SlateId1",
                table: "Photo",
                column: "SlateId1");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_WingId1",
                table: "Photo",
                column: "WingId1");

            migrationBuilder.CreateIndex(
                name: "IX_Slate_CandidateId",
                table: "Slate",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Slate_PhotoId",
                table: "Slate",
                column: "PhotoId",
                unique: true,
                filter: "[PhotoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Slate_ProvinceId",
                table: "Slate",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Slate_WingId",
                table: "Slate",
                column: "WingId");

            migrationBuilder.CreateIndex(
                name: "IX_Wing_PartyId",
                table: "Wing",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Wing_PhotoId",
                table: "Wing",
                column: "PhotoId",
                unique: true,
                filter: "[PhotoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Client_ClientId",
                table: "AspNetUsers",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Photo_PhotoId",
                table: "Candidate",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Circuit_Municipality_MunicipalityId",
                table: "Circuit",
                column: "MunicipalityId",
                principalTable: "Municipality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Circuit_Municipality_MunicipalityId1",
                table: "Circuit",
                column: "MunicipalityId1",
                principalTable: "Municipality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CircuitDelegado_Delegado_DelegadoId",
                table: "CircuitDelegado",
                column: "DelegadoId",
                principalTable: "Delegado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CircuitParty_Party_PartyId",
                table: "CircuitParty",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CircuitSlate_Slate_SlateId",
                table: "CircuitSlate",
                column: "SlateId",
                principalTable: "Slate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Party_PartyId",
                table: "Client",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Slate_SlateId",
                table: "Participant",
                column: "SlateId",
                principalTable: "Slate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Party_Photo_PhotoLongId",
                table: "Party",
                column: "PhotoLongId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Party_Photo_PhotoShortId",
                table: "Party",
                column: "PhotoShortId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Slate_SlateId1",
                table: "Photo",
                column: "SlateId1",
                principalTable: "Slate",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Wing_WingId1",
                table: "Photo",
                column: "WingId1",
                principalTable: "Wing",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delegado_Client_ClientId",
                table: "Delegado");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Photo_PhotoId",
                table: "Candidate");

            migrationBuilder.DropForeignKey(
                name: "FK_Party_Photo_PhotoLongId",
                table: "Party");

            migrationBuilder.DropForeignKey(
                name: "FK_Party_Photo_PhotoShortId",
                table: "Party");

            migrationBuilder.DropForeignKey(
                name: "FK_Slate_Photo_PhotoId",
                table: "Slate");

            migrationBuilder.DropForeignKey(
                name: "FK_Wing_Photo_PhotoId",
                table: "Wing");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CircuitDelegado");

            migrationBuilder.DropTable(
                name: "CircuitParty");

            migrationBuilder.DropTable(
                name: "CircuitSlate");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "Circuit");

            migrationBuilder.DropTable(
                name: "Slate");

            migrationBuilder.DropTable(
                name: "Municipality");

            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropTable(
                name: "Wing");

            migrationBuilder.DropTable(
                name: "Delegado");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "Party");
        }
    }
}
