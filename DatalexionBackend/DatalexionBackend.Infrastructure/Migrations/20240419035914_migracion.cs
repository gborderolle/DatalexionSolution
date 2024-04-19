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
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", null, new DateTime(2024, 4, 19, 0, 59, 10, 924, DateTimeKind.Local).AddTicks(3007), "DatalexionRole", "Admin", "ADMIN", new DateTime(2024, 4, 19, 0, 59, 10, 924, DateTimeKind.Local).AddTicks(3008) },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", null, new DateTime(2024, 4, 19, 0, 59, 10, 924, DateTimeKind.Local).AddTicks(3025), "DatalexionRole", "Analyst", "ANALYST", new DateTime(2024, 4, 19, 0, 59, 10, 924, DateTimeKind.Local).AddTicks(3026) }
                });

            migrationBuilder.InsertData(
                table: "Photo",
                columns: new[] { "Id", "CandidateId", "CandidateId1", "CircuitId", "CircuitId1", "CircuitPartyId", "Creation", "PartyLongId", "PartyShortId", "SlateId", "SlateId1", "URL", "Update", "WingId", "WingId1" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4907), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4917), null, null },
                    { 2, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4929), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4929), null, null },
                    { 3, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4932), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo3.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4933), null, null },
                    { 4, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4935), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo4.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4936), null, null },
                    { 5, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4938), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo5.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4939), null, null },
                    { 6, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4959), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo6.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4960), null, null },
                    { 7, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4962), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo7.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4962), null, null },
                    { 8, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4965), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo8.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4965), null, null },
                    { 9, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4970), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo9.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4970), null, null },
                    { 10, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4974), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo10.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4975), null, null },
                    { 11, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4977), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo11.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4978), null, null },
                    { 12, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4980), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo12.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4980), null, null },
                    { 13, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4982), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo13.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4983), null, null },
                    { 14, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4985), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo14.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4986), null, null },
                    { 15, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4988), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo15.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4988), null, null },
                    { 16, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4990), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo16.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(4991), null, null },
                    { 101, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5414), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo101.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5414), null, null },
                    { 102, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5419), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo102.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5419), null, null },
                    { 103, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5421), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo103.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5422), null, null },
                    { 104, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5424), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo104.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5424), null, null },
                    { 105, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5427), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo105.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5427), null, null },
                    { 106, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5430), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo106.jpg", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5431), null, null },
                    { 111, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5585), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo111.png", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5585), null, null },
                    { 112, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5588), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo112.png", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5588), null, null },
                    { 113, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5590), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo113.png", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5591), null, null },
                    { 114, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5595), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo114.png", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5595), null, null },
                    { 115, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5598), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo115.png", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5598), null, null },
                    { 116, null, null, null, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5600), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo116.png", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5601), null, null }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "Id", "Center", "Comments", "Creation", "Name", "Update", "Zoom" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5983), "Montevideo", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5983), null },
                    { 2, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5987), "Canelones", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5987), null },
                    { 3, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5989), "Maldonado", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5990), null },
                    { 4, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5991), "Rocha", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5992), null },
                    { 5, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5993), "Colonia", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5994), null },
                    { 6, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5995), "Artigas", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5996), null },
                    { 7, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5997), "Salto", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5998), null },
                    { 8, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5999), "Paysandú", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6000), null },
                    { 9, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6001), "Tacuarembó", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6002), null },
                    { 10, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6003), "Rivera", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6004), null },
                    { 11, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6005), "San José", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6006), null },
                    { 12, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6007), "Durazno", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6007), null },
                    { 13, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6009), "Treinta y Tres", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6009), null },
                    { 14, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6011), "Cerro Largo", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6011), null },
                    { 15, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6013), "Rivera", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6013), null },
                    { 16, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6015), "Flores", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6015), null },
                    { 17, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6017), "Florida", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6017), null },
                    { 18, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6018), "Lavalleja", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6019), null },
                    { 19, null, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6111), "Soriano", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(6112), null }
                });

            migrationBuilder.InsertData(
                table: "Candidate",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5063), "Álvaro Delgado", 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5064) },
                    { 2, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5068), "Laura Raffo", 2, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5068) },
                    { 3, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5070), "Jorge Gandini", 3, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5071) },
                    { 4, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5161), "Juan Sartori", 4, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5161) },
                    { 5, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5164), "Yamandú Orsi", 5, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5165) },
                    { 6, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5166), "Carolina Cosse", 6, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5167) },
                    { 7, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5168), "Mario Bergara", 7, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5169) },
                    { 8, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5171), "Pablo Mieres", 8, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5172) },
                    { 9, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5173), "Edgardo Novick", 9, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5174) },
                    { 10, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5176), "Andrés Lima", 10, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5177) },
                    { 11, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5178), "Gabriel Gurméndez", 11, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5179) },
                    { 12, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5181), "Robert Silva", 12, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5181) },
                    { 13, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5183), "Andrés Ojeda", 13, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5184) },
                    { 14, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5185), "Gustavo Zubía", 14, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5186) },
                    { 15, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5188), "Guzmán Acosta y Lara", 15, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5188) },
                    { 16, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5190), "Tabaré Viera", 16, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5191) }
                });

            migrationBuilder.InsertData(
                table: "Party",
                columns: new[] { "Id", "Color", "Comments", "Creation", "Name", "PhotoLongId", "PhotoShortId", "ShortName", "Update", "Votes" },
                values: new object[,]
                {
                    { 1, "#3153dd", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5654), "Frente Amplio", 101, 111, "FA", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5655), null },
                    { 2, "#3153dd", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5662), "Partido Nacional", 102, 112, "PN", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5663), null },
                    { 3, "#d62929", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5666), "Partido Colorado", 103, 113, "PC", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5667), null },
                    { 4, "#b929d6", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5670), "Partido Independiente", 104, 114, "PI", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5671), null },
                    { 5, "#f9bb28", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5673), "Cabildo Abierto", 105, 115, "CA", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5674), null },
                    { 6, "#009001", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5677), "PERI", 106, 116, "PERI", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5677), null }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "Update" },
                values: new object[,]
                {
                    { 1, "Frente Amplio", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5744), "Frente Amplio", 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5745) },
                    { 2, "Partido Nacional", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5751), "Partido Nacional", 2, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5752) },
                    { 3, "Partido Colorado", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5756), "Partido Colorado", 3, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5757) }
                });

            migrationBuilder.InsertData(
                table: "Wing",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5867), "FA", 1, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5868) },
                    { 2, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5874), "PN", 2, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5874) },
                    { 3, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5877), "PC", 3, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5878) },
                    { 4, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5880), "PI", 4, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5880) },
                    { 5, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5882), "CA", 5, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5883) },
                    { 6, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5884), "PERI", 6, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5885) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Creation", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Update", "UserName" },
                values: new object[,]
                {
                    { "2a765d8b-9204-4e0f-b4ce-453f6e1bb592", 0, 1, "d9e07b21-ecd2-4a02-baa0-2234b4629aff", new DateTime(2024, 4, 19, 0, 59, 10, 924, DateTimeKind.Local).AddTicks(3802), "DatalexionUser", "adminfa@datalexion.lat", false, false, null, "Admin FA", "ADMINFA@DATALEXION.LAT", "ADMINFA", "AQAAAAIAAYagAAAAEC18Vqmffdd+hlWo1FURLLuMuS+EUAXDtCnwjWcOX9aihgLACtYutPSx45Az/0fxSQ==", null, false, "e0710836-e253-4f3d-83d1-88234b48c8b1", false, new DateTime(2024, 4, 19, 0, 59, 10, 924, DateTimeKind.Local).AddTicks(3804), "adminfa" },
                    { "6c762a89-a7b6-4ee3-96d0-105b219dcaa6", 0, 3, "ba66decb-c622-424c-96a3-1a25638b60cb", new DateTime(2024, 4, 19, 0, 59, 11, 281, DateTimeKind.Local).AddTicks(8975), "DatalexionUser", "adminpc@datalexion.lat", false, false, null, "Admin PC", "ADMINPC@DATALEXION.LAT", "ADMINPC", "AQAAAAIAAYagAAAAEAIn1bGmvFtcNRwZ4Zxb4+TMElHjio4XbLa1v4vTSB/2Jhj++MyP3PRxqJ4jy7Vr2Q==", null, false, "1b9e59f4-3a47-4ffb-b15f-a8e1d7c58867", false, new DateTime(2024, 4, 19, 0, 59, 11, 281, DateTimeKind.Local).AddTicks(8988), "adminpc" },
                    { "8498a3ff-ca69-4b93-9a37-49a73c8dec77", 0, 2, "c9f8ffb3-c139-4461-af7a-fbb9f8dda473", new DateTime(2024, 4, 19, 0, 59, 11, 108, DateTimeKind.Local).AddTicks(7013), "DatalexionUser", "adminpn@datalexion.lat", false, false, null, "Admin PN", "ADMINPN@DATALEXION.LAT", "ADMINPN", "AQAAAAIAAYagAAAAEHsdxg7vJJzAq7HwsGXXbEO6XX1bP91tgKCzhmiKYmIUbKiXoRnBmDGnNpIfkfuORw==", null, false, "c98918b3-d6c0-44dd-8cde-36d3486f1fac", false, new DateTime(2024, 4, 19, 0, 59, 11, 108, DateTimeKind.Local).AddTicks(7019), "adminpn" },
                    { "b5172b14-f9e4-48f6-9634-2241c87f1719", 0, 3, "1ffa1fcb-aaac-4598-ba5f-66ebbec52afc", new DateTime(2024, 4, 19, 0, 59, 11, 894, DateTimeKind.Local).AddTicks(568), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista PC", "NORMAL@DATALEXION.LAT", "ANALYSTPC", "AQAAAAIAAYagAAAAEAnLq7Uj6Mc3W98Moy8Us+2Ul+cpLIQvX2BErWBCLvVnEorDpbM51K08Of2yU8P+iw==", null, false, "e2bfe13d-a9ca-4ecb-9125-2fd3b4d3eb08", false, new DateTime(2024, 4, 19, 0, 59, 11, 894, DateTimeKind.Local).AddTicks(579), "analystpc" },
                    { "ddc18aa2-c5c7-40c9-9db3-246d2a05a06c", 0, 2, "0a8ea6f8-0955-4751-a0d7-0e29dabaf511", new DateTime(2024, 4, 19, 0, 59, 11, 683, DateTimeKind.Local).AddTicks(9555), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista PN", "NORMAL@DATALEXION.LAT", "ANALYSTPN", "AQAAAAIAAYagAAAAEChKHPUYOnVnaZHbD1stBlvTqLbwlrlfFX3jod7rYWe7lgVApK1zBdie/fdlX20rTw==", null, false, "073ba4db-e092-433f-af16-98749819d581", false, new DateTime(2024, 4, 19, 0, 59, 11, 683, DateTimeKind.Local).AddTicks(9561), "analystpn" },
                    { "e15e9299-d3b5-42fc-b101-44da6ad799de", 0, 1, "96b2f0a4-bf8c-4a2e-908e-2a74c3643a05", new DateTime(2024, 4, 19, 0, 59, 11, 476, DateTimeKind.Local).AddTicks(1023), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista FA", "NORMAL@DATALEXION.LAT", "ANALYSTFA", "AQAAAAIAAYagAAAAEDxoO74oUQK8sQWvreYhILHNoUiz55IXTL0d8vhuce+UARRnxxyGG36uDjEA9X3Wwg==", null, false, "0de85f01-d4ef-4245-878b-21bcbe6a7e74", false, new DateTime(2024, 4, 19, 0, 59, 11, 476, DateTimeKind.Local).AddTicks(1035), "analystfa" }
                });

            migrationBuilder.InsertData(
                table: "Delegado",
                columns: new[] { "Id", "CI", "ClientId", "Comments", "Creation", "Email", "Name", "Phone", "Update" },
                values: new object[,]
                {
                    { 1, "11111111", 2, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5803), "gonzalo.delegado@datalexion.lat", "Gonzalo", "099415831", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5803) },
                    { 2, "12345678", 2, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5810), "diego.delegado@datalexion.lat", "Diego", "099589896", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5810) },
                    { 3, "22222222", 1, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5813), "pablo.delegado@datalexion.lat", "Pablo", "099415831", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5814) },
                    { 4, "33333333", 3, null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5816), "gabriel.delegado@datalexion.lat", "Gabriel", "099415831", new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5817) }
                });

            migrationBuilder.InsertData(
                table: "Slate",
                columns: new[] { "Id", "CandidateId", "Color", "Comments", "Creation", "Name", "PhotoId", "ProvinceId", "Update", "Votes", "WingId" },
                values: new object[,]
                {
                    { 1, 6, "#3153dd", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5269), "5005", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5270), null, 1 },
                    { 2, 5, "#3153dd", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5282), "711", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5283), null, 1 },
                    { 3, 6, "#d62929", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5286), "90", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5286), null, 1 },
                    { 4, 5, "#b929d6", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5289), "609", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5290), null, 1 },
                    { 5, 1, "#bec11a", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5292), "71", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5293), null, 2 },
                    { 6, 1, "#3153dd", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5295), "404", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5296), null, 2 },
                    { 7, 2, "#ff0000", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5299), "40", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5299), null, 2 },
                    { 8, 2, "#00ff00", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5302), "250", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5302), null, 2 },
                    { 9, 2, "#0000ff", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5305), "880", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5305), null, 2 },
                    { 10, 11, "#ff00ff", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5308), "15", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5309), null, 3 },
                    { 11, 12, "#987654", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5311), "85", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5312), null, 3 },
                    { 12, 12, "#abcdef", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5315), "1", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5315), null, 3 },
                    { 13, 12, "#fedcba", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5318), "2000", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5318), null, 3 },
                    { 14, 11, "#012345", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5321), "1515", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5322), null, 3 },
                    { 15, 11, "#012345", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5324), "600", null, 1, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5325), null, 3 },
                    { 16, 5, "#abcdef", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5327), "500", null, 2, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5328), null, 1 },
                    { 17, 5, "#fedcba", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5331), "123", null, 2, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5331), null, 1 },
                    { 18, 5, "#012345", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5334), "999", null, 2, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5334), null, 1 },
                    { 19, 5, "#abcdef", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5337), "777", null, 2, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5338), null, 1 },
                    { 20, 1, "#fedcba", null, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5340), "333", null, 2, new DateTime(2024, 4, 19, 0, 59, 12, 12, DateTimeKind.Local).AddTicks(5341), null, 2 }
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
