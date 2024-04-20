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
                    TotalPartyVotes = table.Column<int>(type: "int", nullable: true),
                    BlankVotes = table.Column<int>(type: "int", nullable: false),
                    NullVotes = table.Column<int>(type: "int", nullable: false),
                    ObservedVotes = table.Column<int>(type: "int", nullable: false),
                    RecurredVotes = table.Column<int>(type: "int", nullable: false),
                    Step1completed = table.Column<bool>(type: "bit", nullable: false),
                    Step2completed = table.Column<bool>(type: "bit", nullable: false),
                    Step3completed = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdateDelegadoId = table.Column<int>(type: "int", nullable: true)
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
                    TotalSlateVotes = table.Column<int>(type: "int", nullable: true)
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
                    CircuitId1 = table.Column<int>(type: "int", nullable: true),
                    CircuitPartyId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_Photo_CircuitParty_CircuitId1_CircuitPartyId",
                        columns: x => new { x.CircuitId1, x.CircuitPartyId },
                        principalTable: "CircuitParty",
                        principalColumns: new[] { "CircuitId", "PartyId" });
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
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", null, new DateTime(2024, 4, 19, 22, 48, 40, 853, DateTimeKind.Local).AddTicks(1655), "DatalexionRole", "Admin", "ADMIN", new DateTime(2024, 4, 19, 22, 48, 40, 853, DateTimeKind.Local).AddTicks(1656) },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", null, new DateTime(2024, 4, 19, 22, 48, 40, 853, DateTimeKind.Local).AddTicks(1679), "DatalexionRole", "Analyst", "ANALYST", new DateTime(2024, 4, 19, 22, 48, 40, 853, DateTimeKind.Local).AddTicks(1682) }
                });

            migrationBuilder.InsertData(
                table: "Photo",
                columns: new[] { "Id", "CandidateId", "CandidateId1", "CircuitId", "CircuitId1", "CircuitPartyId", "Creation", "PartyLongId", "PartyShortId", "SlateId", "SlateId1", "URL", "Update", "WingId", "WingId1" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1529), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1536), null, null },
                    { 2, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1546), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1547), null, null },
                    { 3, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1550), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo3.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1550), null, null },
                    { 4, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1553), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo4.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1553), null, null },
                    { 5, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1556), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo5.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1556), null, null },
                    { 6, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1567), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo6.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1568), null, null },
                    { 7, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1570), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo7.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1571), null, null },
                    { 8, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1573), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo8.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1574), null, null },
                    { 9, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1576), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo9.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1577), null, null },
                    { 10, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1800), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo10.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1813), null, null },
                    { 11, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1829), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo11.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1830), null, null },
                    { 12, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1832), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo12.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1833), null, null },
                    { 13, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1835), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo13.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1836), null, null },
                    { 14, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1838), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo14.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1839), null, null },
                    { 15, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1841), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo15.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1842), null, null },
                    { 16, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1844), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo16.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1844), null, null },
                    { 101, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2301), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo101.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2302), null, null },
                    { 102, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2308), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo102.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2309), null, null },
                    { 103, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2312), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo103.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2313), null, null },
                    { 104, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2315), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo104.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2316), null, null },
                    { 105, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2318), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo105.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2319), null, null },
                    { 106, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2322), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo106.jpg", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2323), null, null },
                    { 111, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2325), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo111.png", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2326), null, null },
                    { 112, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2328), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo112.png", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2328), null, null },
                    { 113, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2331), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo113.png", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2331), null, null },
                    { 114, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2334), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo114.png", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2335), null, null },
                    { 115, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2337), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo115.png", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2338), null, null },
                    { 116, null, null, null, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2340), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo116.png", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2341), null, null }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "Id", "Center", "Comments", "Creation", "Name", "Update", "Zoom" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2747), "Montevideo", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2749), null },
                    { 2, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2752), "Canelones", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2753), null },
                    { 3, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2755), "Maldonado", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2755), null },
                    { 4, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2757), "Rocha", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2758), null },
                    { 5, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2759), "Colonia", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2760), null },
                    { 6, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2761), "Artigas", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2762), null },
                    { 7, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2763), "Salto", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2764), null },
                    { 8, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2766), "Paysandú", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2766), null },
                    { 9, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2768), "Tacuarembó", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2768), null },
                    { 10, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2770), "Rivera", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2770), null },
                    { 11, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2772), "San José", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2773), null },
                    { 12, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2774), "Durazno", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2775), null },
                    { 13, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2776), "Treinta y Tres", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2777), null },
                    { 14, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2778), "Cerro Largo", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2779), null },
                    { 15, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2780), "Rivera", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2781), null },
                    { 16, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2782), "Flores", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2783), null },
                    { 17, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2785), "Florida", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2785), null },
                    { 18, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2787), "Lavalleja", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2787), null },
                    { 19, null, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2789), "Soriano", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2790), null }
                });

            migrationBuilder.InsertData(
                table: "Candidate",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1940), "Álvaro Delgado", 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1941) },
                    { 2, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1945), "Laura Raffo", 2, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1946) },
                    { 3, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1948), "Jorge Gandini", 3, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1949) },
                    { 4, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1951), "Juan Sartori", 4, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1952) },
                    { 5, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1953), "Yamandú Orsi", 5, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1954) },
                    { 6, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1956), "Carolina Cosse", 6, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1957) },
                    { 7, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1959), "Mario Bergara", 7, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1959) },
                    { 8, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1961), "Pablo Mieres", 8, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1962) },
                    { 9, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1964), "Edgardo Novick", 9, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1964) },
                    { 10, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1966), "Andrés Lima", 10, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1967) },
                    { 11, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1969), "Gabriel Gurméndez", 11, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1970) },
                    { 12, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1972), "Robert Silva", 12, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1973) },
                    { 13, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1975), "Andrés Ojeda", 13, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1975) },
                    { 14, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1981), "Gustavo Zubía", 14, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1982) },
                    { 15, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1984), "Guzmán Acosta y Lara", 15, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1984) },
                    { 16, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1986), "Tabaré Viera", 16, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(1987) }
                });

            migrationBuilder.InsertData(
                table: "Party",
                columns: new[] { "Id", "Color", "Comments", "Creation", "Name", "PhotoLongId", "PhotoShortId", "ShortName", "Update", "Votes" },
                values: new object[,]
                {
                    { 1, "#3153dd", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2390), "Frente Amplio", 101, 111, "FA", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2391), null },
                    { 2, "#3153dd", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2401), "Partido Nacional", 102, 112, "PN", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2401), null },
                    { 3, "#d62929", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2406), "Partido Colorado", 103, 113, "PC", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2407), null },
                    { 4, "#b929d6", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2410), "Partido Independiente", 104, 114, "PI", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2410), null },
                    { 5, "#f9bb28", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2413), "Cabildo Abierto", 105, 115, "CA", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2414), null },
                    { 6, "#009001", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2416), "PERI", 106, 116, "PERI", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2417), null }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "Update" },
                values: new object[,]
                {
                    { 1, "Frente Amplio", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2480), "Frente Amplio", 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2481) },
                    { 2, "Partido Nacional", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2486), "Partido Nacional", 2, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2487) },
                    { 3, "Partido Colorado", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2492), "Partido Colorado", 3, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2493) }
                });

            migrationBuilder.InsertData(
                table: "Wing",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2669), "FA", 1, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2671) },
                    { 2, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2676), "PN", 2, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2677) },
                    { 3, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2679), "PC", 3, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2680) },
                    { 4, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2682), "PI", 4, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2682) },
                    { 5, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2684), "CA", 5, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2685) },
                    { 6, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2687), "PERI", 6, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2688) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Creation", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Update", "UserName" },
                values: new object[,]
                {
                    { "2a765d8b-9204-4e0f-b4ce-453f6e1bb592", 0, 1, "af53680f-06e7-4e83-b16a-a4b29cc65e79", new DateTime(2024, 4, 19, 22, 48, 40, 853, DateTimeKind.Local).AddTicks(2496), "DatalexionUser", "adminfa@datalexion.lat", false, false, null, "Admin FA", "ADMINFA@DATALEXION.LAT", "ADMINFA", "AQAAAAIAAYagAAAAELkuRnqa5zmLV+x7jO2ZNVkFOX2aH8RR1sbITBiT4sjS+v7mdPsQhQ1gcrnPuzbh/A==", null, false, "bccf6c0c-d3bc-4395-a246-edafebc0bb23", false, new DateTime(2024, 4, 19, 22, 48, 40, 853, DateTimeKind.Local).AddTicks(2500), "adminfa" },
                    { "6c762a89-a7b6-4ee3-96d0-105b219dcaa6", 0, 3, "237dfcd5-618a-4ffe-a8a9-24417bf6e9b2", new DateTime(2024, 4, 19, 22, 48, 41, 271, DateTimeKind.Local).AddTicks(9150), "DatalexionUser", "adminpc@datalexion.lat", false, false, null, "Admin PC", "ADMINPC@DATALEXION.LAT", "ADMINPC", "AQAAAAIAAYagAAAAEAo9Bu4X3DUbe+jAxrEVHvmsyjfbtldVjhrmRvLb4xsJY96/PI0kqwm/7Qk4jybN1Q==", null, false, "fd5e142d-b576-4f0b-99c6-5773bda5c6b0", false, new DateTime(2024, 4, 19, 22, 48, 41, 271, DateTimeKind.Local).AddTicks(9155), "adminpc" },
                    { "8498a3ff-ca69-4b93-9a37-49a73c8dec77", 0, 2, "eac628cb-ee19-4a05-ac24-323cfd248f3f", new DateTime(2024, 4, 19, 22, 48, 41, 68, DateTimeKind.Local).AddTicks(3431), "DatalexionUser", "adminpn@datalexion.lat", false, false, null, "Admin PN", "ADMINPN@DATALEXION.LAT", "ADMINPN", "AQAAAAIAAYagAAAAEH5AB/0oA5L8Qv9RZ7sDhiajTYLmu6bB/wQMbYWqWqJrOgSWFWKwq+qHxAAPq/KZaQ==", null, false, "9956a5fa-eaff-48f2-a93e-4aac2f946309", false, new DateTime(2024, 4, 19, 22, 48, 41, 68, DateTimeKind.Local).AddTicks(3438), "adminpn" },
                    { "b5172b14-f9e4-48f6-9634-2241c87f1719", 0, 3, "25444be4-e840-44da-89a3-a19c9abfe26b", new DateTime(2024, 4, 19, 22, 48, 41, 948, DateTimeKind.Local).AddTicks(2882), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista PC", "NORMAL@DATALEXION.LAT", "ANALYSTPC", "AQAAAAIAAYagAAAAEEbKLcwX+GP8O7pRXtMwftqV4HgEIAje95zqzyzPdeROLn0VesfQl1rltPQwbiLdPg==", null, false, "f56c7913-3c05-49a9-a82a-e96e3d6d5c13", false, new DateTime(2024, 4, 19, 22, 48, 41, 948, DateTimeKind.Local).AddTicks(2887), "analystpc" },
                    { "ddc18aa2-c5c7-40c9-9db3-246d2a05a06c", 0, 2, "6eca28be-0799-48ce-983e-6e5fbdbe7efb", new DateTime(2024, 4, 19, 22, 48, 41, 726, DateTimeKind.Local).AddTicks(145), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista PN", "NORMAL@DATALEXION.LAT", "ANALYSTPN", "AQAAAAIAAYagAAAAEOggU7AYh3kHIO9G9MP0kCH2QkMNCBkBF8g1/K5mKNq3zNTqv8OopvWKsKg6FK/q3Q==", null, false, "1adbca97-0ce4-4f7c-9ca2-2738b2716367", false, new DateTime(2024, 4, 19, 22, 48, 41, 726, DateTimeKind.Local).AddTicks(152), "analystpn" },
                    { "e15e9299-d3b5-42fc-b101-44da6ad799de", 0, 1, "a505b991-1c2f-4f85-ac56-49d43cbc8239", new DateTime(2024, 4, 19, 22, 48, 41, 500, DateTimeKind.Local).AddTicks(8439), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista FA", "NORMAL@DATALEXION.LAT", "ANALYSTFA", "AQAAAAIAAYagAAAAEIbi5JmzFjOw/pE5iggCgNy23U2dVdPeyxTvVZaE88asgXbVjDel6u4h5kN5wpnX0w==", null, false, "1151da94-7f2a-4424-b7c6-7a99947ddc6f", false, new DateTime(2024, 4, 19, 22, 48, 41, 500, DateTimeKind.Local).AddTicks(8446), "analystfa" }
                });

            migrationBuilder.InsertData(
                table: "Delegado",
                columns: new[] { "Id", "CI", "ClientId", "Comments", "Creation", "Email", "Name", "Phone", "Update" },
                values: new object[,]
                {
                    { 1, "11111111", 2, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2537), "gonzalo.delegado@datalexion.lat", "Gonzalo", "099415831", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2538) },
                    { 2, "12345678", 2, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2545), "diego.delegado@datalexion.lat", "Diego", "099589896", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2546) },
                    { 3, "22222222", 1, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2549), "pablo.delegado@datalexion.lat", "Pablo", "099415831", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2549) },
                    { 4, "33333333", 3, null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2552), "gabriel.delegado@datalexion.lat", "Gabriel", "099415831", new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2553) }
                });

            migrationBuilder.InsertData(
                table: "Slate",
                columns: new[] { "Id", "CandidateId", "Color", "Comments", "Creation", "Name", "PhotoId", "ProvinceId", "Update", "Votes", "WingId" },
                values: new object[,]
                {
                    { 1, 6, "#3153dd", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2063), "5005", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2063), null, 1 },
                    { 2, 5, "#3153dd", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2072), "711", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2073), null, 1 },
                    { 3, 6, "#d62929", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2075), "90", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2076), null, 1 },
                    { 4, 5, "#b929d6", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2079), "609", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2079), null, 1 },
                    { 5, 1, "#bec11a", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2082), "71", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2082), null, 2 },
                    { 6, 1, "#3153dd", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2085), "404", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2085), null, 2 },
                    { 7, 2, "#ff0000", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2088), "40", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2088), null, 2 },
                    { 8, 2, "#00ff00", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2091), "250", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2092), null, 2 },
                    { 9, 2, "#0000ff", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2094), "880", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2095), null, 2 },
                    { 10, 11, "#ff00ff", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2098), "15", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2098), null, 3 },
                    { 11, 12, "#987654", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2101), "85", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2102), null, 3 },
                    { 12, 12, "#abcdef", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2104), "1", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2105), null, 3 },
                    { 13, 12, "#fedcba", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2107), "2000", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2108), null, 3 },
                    { 14, 11, "#012345", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2110), "1515", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2111), null, 3 },
                    { 15, 11, "#012345", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2114), "600", null, 1, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2114), null, 3 },
                    { 16, 5, "#abcdef", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2118), "500", null, 2, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2118), null, 1 },
                    { 17, 5, "#fedcba", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2121), "123", null, 2, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2121), null, 1 },
                    { 18, 5, "#012345", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2124), "999", null, 2, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2124), null, 1 },
                    { 19, 5, "#abcdef", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2127), "777", null, 2, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2127), null, 1 },
                    { 20, 1, "#fedcba", null, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2129), "333", null, 2, new DateTime(2024, 4, 19, 22, 48, 42, 193, DateTimeKind.Local).AddTicks(2130), null, 2 }
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
                name: "IX_Photo_CircuitId1_CircuitPartyId",
                table: "Photo",
                columns: new[] { "CircuitId1", "CircuitPartyId" });

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
                name: "CircuitParty");

            migrationBuilder.DropTable(
                name: "Slate");

            migrationBuilder.DropTable(
                name: "Circuit");

            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropTable(
                name: "Wing");

            migrationBuilder.DropTable(
                name: "Municipality");

            migrationBuilder.DropTable(
                name: "Party");

            migrationBuilder.DropTable(
                name: "Delegado");

            migrationBuilder.DropTable(
                name: "Province");
        }
    }
}
