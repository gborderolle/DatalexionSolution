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
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", null, new DateTime(2024, 4, 19, 14, 15, 10, 675, DateTimeKind.Local).AddTicks(5679), "DatalexionRole", "Admin", "ADMIN", new DateTime(2024, 4, 19, 14, 15, 10, 675, DateTimeKind.Local).AddTicks(5680) },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", null, new DateTime(2024, 4, 19, 14, 15, 10, 675, DateTimeKind.Local).AddTicks(5688), "DatalexionRole", "Analyst", "ANALYST", new DateTime(2024, 4, 19, 14, 15, 10, 675, DateTimeKind.Local).AddTicks(5689) }
                });

            migrationBuilder.InsertData(
                table: "Photo",
                columns: new[] { "Id", "CandidateId", "CandidateId1", "CircuitId", "CircuitId1", "CircuitPartyId", "Creation", "PartyLongId", "PartyShortId", "SlateId", "SlateId1", "URL", "Update", "WingId", "WingId1" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2838), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2844), null, null },
                    { 2, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2862), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2862), null, null },
                    { 3, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2864), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo3.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2864), null, null },
                    { 4, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2866), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo4.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2866), null, null },
                    { 5, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2868), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo5.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2880), null, null },
                    { 6, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2895), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo6.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2895), null, null },
                    { 7, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2897), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo7.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2898), null, null },
                    { 8, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2899), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo8.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2899), null, null },
                    { 9, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2902), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo9.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2903), null, null },
                    { 10, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2905), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo10.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2905), null, null },
                    { 11, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2906), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo11.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2907), null, null },
                    { 12, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2909), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo12.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2909), null, null },
                    { 13, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2911), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo13.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2912), null, null },
                    { 14, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2913), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo14.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2914), null, null },
                    { 15, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2915), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo15.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2915), null, null },
                    { 16, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2916), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo16.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(2917), null, null },
                    { 101, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3263), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo101.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3264), null, null },
                    { 102, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3267), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo102.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3268), null, null },
                    { 103, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3269), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo103.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3270), null, null },
                    { 104, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3273), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo104.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3273), null, null },
                    { 105, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3275), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo105.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3275), null, null },
                    { 106, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3278), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo106.jpg", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3279), null, null },
                    { 111, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3280), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo111.png", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3281), null, null },
                    { 112, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3282), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo112.png", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3282), null, null },
                    { 113, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3284), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo113.png", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3285), null, null },
                    { 114, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3287), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo114.png", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3288), null, null },
                    { 115, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3289), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo115.png", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3290), null, null },
                    { 116, null, null, null, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3291), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo116.png", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3291), null, null }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "Id", "Center", "Comments", "Creation", "Name", "Update", "Zoom" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3573), "Montevideo", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3574), null },
                    { 2, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3575), "Canelones", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3575), null },
                    { 3, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3576), "Maldonado", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3576), null },
                    { 4, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3577), "Rocha", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3578), null },
                    { 5, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3578), "Colonia", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3579), null },
                    { 6, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3579), "Artigas", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3580), null },
                    { 7, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3581), "Salto", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3581), null },
                    { 8, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3582), "Paysandú", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3582), null },
                    { 9, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3583), "Tacuarembó", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3583), null },
                    { 10, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3584), "Rivera", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3584), null },
                    { 11, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3585), "San José", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3585), null },
                    { 12, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3586), "Durazno", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3586), null },
                    { 13, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3587), "Treinta y Tres", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3587), null },
                    { 14, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3588), "Cerro Largo", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3589), null },
                    { 15, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3589), "Rivera", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3590), null },
                    { 16, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3590), "Flores", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3591), null },
                    { 17, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3591), "Florida", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3592), null },
                    { 18, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3592), "Lavalleja", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3593), null },
                    { 19, null, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3594), "Soriano", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3594), null }
                });

            migrationBuilder.InsertData(
                table: "Candidate",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3011), "Álvaro Delgado", 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3012) },
                    { 2, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3013), "Laura Raffo", 2, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3014) },
                    { 3, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3015), "Jorge Gandini", 3, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3016) },
                    { 4, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3017), "Juan Sartori", 4, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3017) },
                    { 5, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3018), "Yamandú Orsi", 5, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3018) },
                    { 6, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3019), "Carolina Cosse", 6, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3020) },
                    { 7, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3020), "Mario Bergara", 7, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3021) },
                    { 8, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3022), "Pablo Mieres", 8, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3022) },
                    { 9, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3023), "Edgardo Novick", 9, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3024) },
                    { 10, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3024), "Andrés Lima", 10, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3025) },
                    { 11, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3026), "Gabriel Gurméndez", 11, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3026) },
                    { 12, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3027), "Robert Silva", 12, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3027) },
                    { 13, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3028), "Andrés Ojeda", 13, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3029) },
                    { 14, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3030), "Gustavo Zubía", 14, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3030) },
                    { 15, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3031), "Guzmán Acosta y Lara", 15, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3031) },
                    { 16, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3032), "Tabaré Viera", 16, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3033) }
                });

            migrationBuilder.InsertData(
                table: "Party",
                columns: new[] { "Id", "Color", "Comments", "Creation", "Name", "PhotoLongId", "PhotoShortId", "ShortName", "Update", "Votes" },
                values: new object[,]
                {
                    { 1, "#3153dd", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3355), "Frente Amplio", 101, 111, "FA", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3356), null },
                    { 2, "#3153dd", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3372), "Partido Nacional", 102, 112, "PN", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3372), null },
                    { 3, "#d62929", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3374), "Partido Colorado", 103, 113, "PC", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3375), null },
                    { 4, "#b929d6", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3376), "Partido Independiente", 104, 114, "PI", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3376), null },
                    { 5, "#f9bb28", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3378), "Cabildo Abierto", 105, 115, "CA", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3378), null },
                    { 6, "#009001", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3380), "PERI", 106, 116, "PERI", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3380), null }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "Update" },
                values: new object[,]
                {
                    { 1, "Frente Amplio", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3442), "Frente Amplio", 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3443) },
                    { 2, "Partido Nacional", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3446), "Partido Nacional", 2, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3447) },
                    { 3, "Partido Colorado", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3449), "Partido Colorado", 3, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3450) }
                });

            migrationBuilder.InsertData(
                table: "Wing",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3519), "FA", 1, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3519) },
                    { 2, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3530), "PN", 2, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3530) },
                    { 3, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3531), "PC", 3, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3532) },
                    { 4, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3533), "PI", 4, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3533) },
                    { 5, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3534), "CA", 5, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3535) },
                    { 6, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3535), "PERI", 6, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3536) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Creation", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Update", "UserName" },
                values: new object[,]
                {
                    { "2a765d8b-9204-4e0f-b4ce-453f6e1bb592", 0, 1, "8801d110-4256-43e4-b7ee-7a9e99d0d94b", new DateTime(2024, 4, 19, 14, 15, 10, 675, DateTimeKind.Local).AddTicks(6490), "DatalexionUser", "adminfa@datalexion.lat", false, false, null, "Admin FA", "ADMINFA@DATALEXION.LAT", "ADMINFA", "AQAAAAIAAYagAAAAEFoN/oCwZKJxgSFvBCUiwdaHdXNmWQJNn053P66xKcl12xKA31i6JfnsWCoXnPj08g==", null, false, "f3607b6c-f460-4d52-ab5f-672f8d2f1fa2", false, new DateTime(2024, 4, 19, 14, 15, 10, 675, DateTimeKind.Local).AddTicks(6492), "adminfa" },
                    { "6c762a89-a7b6-4ee3-96d0-105b219dcaa6", 0, 3, "af888504-1e47-49c8-bcb5-bcd0b839d582", new DateTime(2024, 4, 19, 14, 15, 10, 931, DateTimeKind.Local).AddTicks(4772), "DatalexionUser", "adminpc@datalexion.lat", false, false, null, "Admin PC", "ADMINPC@DATALEXION.LAT", "ADMINPC", "AQAAAAIAAYagAAAAEKOtE7P3nwxgUf6Cx19h390hh/oFA1Ts+b3jCHJhDzVB8g8nkN4dbJwiYNpQn96A1A==", null, false, "7a824d24-5878-44d8-a86a-04b85d9e58c5", false, new DateTime(2024, 4, 19, 14, 15, 10, 931, DateTimeKind.Local).AddTicks(4778), "adminpc" },
                    { "8498a3ff-ca69-4b93-9a37-49a73c8dec77", 0, 2, "3e8dce8f-6a63-4078-ace9-c2e6fa09a6da", new DateTime(2024, 4, 19, 14, 15, 10, 823, DateTimeKind.Local).AddTicks(2466), "DatalexionUser", "adminpn@datalexion.lat", false, false, null, "Admin PN", "ADMINPN@DATALEXION.LAT", "ADMINPN", "AQAAAAIAAYagAAAAEMZr37sc/DN+SkW3Asp3mU6UDevPmXScWQ92BKd72XWhK1mVXfUMY6qrp2399xI0WQ==", null, false, "ed6b8b42-66b2-41c1-bf3e-41ece6286b99", false, new DateTime(2024, 4, 19, 14, 15, 10, 823, DateTimeKind.Local).AddTicks(2472), "adminpn" },
                    { "b5172b14-f9e4-48f6-9634-2241c87f1719", 0, 3, "bd62c836-644b-4dc5-9e51-567e5a6a68cf", new DateTime(2024, 4, 19, 14, 15, 11, 247, DateTimeKind.Local).AddTicks(3255), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista PC", "NORMAL@DATALEXION.LAT", "ANALYSTPC", "AQAAAAIAAYagAAAAEPIzJ61e5GFHcZn11yuirw6qjYP8q7Mj92WxzOal65ix75nPlBCzj+Q3WB7lDGqOHw==", null, false, "fdc5802d-8077-4b8d-bb12-c8c2fde59254", false, new DateTime(2024, 4, 19, 14, 15, 11, 247, DateTimeKind.Local).AddTicks(3260), "analystpc" },
                    { "ddc18aa2-c5c7-40c9-9db3-246d2a05a06c", 0, 2, "d694cc7c-66bf-4dd9-96fa-de8996b4ffc4", new DateTime(2024, 4, 19, 14, 15, 11, 158, DateTimeKind.Local).AddTicks(7742), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista PN", "NORMAL@DATALEXION.LAT", "ANALYSTPN", "AQAAAAIAAYagAAAAEHls/Bq1z0Gxpic3AcEwfumWsa4zX/JsaFhQMG/1GGPLVh7c7TkjRQEf0eGFSNQtCA==", null, false, "cb158f51-aabe-4bb4-a173-1f014fe22281", false, new DateTime(2024, 4, 19, 14, 15, 11, 158, DateTimeKind.Local).AddTicks(7747), "analystpn" },
                    { "e15e9299-d3b5-42fc-b101-44da6ad799de", 0, 1, "b99f687d-1be7-4de4-a4bc-9fc8987fe8ca", new DateTime(2024, 4, 19, 14, 15, 11, 56, DateTimeKind.Local).AddTicks(9590), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista FA", "NORMAL@DATALEXION.LAT", "ANALYSTFA", "AQAAAAIAAYagAAAAEKO7GCN5T/YQSuLmKGhPeW6QfrIj5qJh6L4ECCLmXu8Bd5vSZEjHSTc9jMlEZDPITQ==", null, false, "893fe8a5-0ead-4922-9afd-e5855c9dc4ee", false, new DateTime(2024, 4, 19, 14, 15, 11, 56, DateTimeKind.Local).AddTicks(9596), "analystfa" }
                });

            migrationBuilder.InsertData(
                table: "Delegado",
                columns: new[] { "Id", "CI", "ClientId", "Comments", "Creation", "Email", "Name", "Phone", "Update" },
                values: new object[,]
                {
                    { 1, "11111111", 2, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3476), "gonzalo.delegado@datalexion.lat", "Gonzalo", "099415831", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3477) },
                    { 2, "12345678", 2, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3479), "diego.delegado@datalexion.lat", "Diego", "099589896", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3480) },
                    { 3, "22222222", 1, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3482), "pablo.delegado@datalexion.lat", "Pablo", "099415831", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3482) },
                    { 4, "33333333", 3, null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3484), "gabriel.delegado@datalexion.lat", "Gabriel", "099415831", new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3484) }
                });

            migrationBuilder.InsertData(
                table: "Slate",
                columns: new[] { "Id", "CandidateId", "Color", "Comments", "Creation", "Name", "PhotoId", "ProvinceId", "Update", "Votes", "WingId" },
                values: new object[,]
                {
                    { 1, 6, "#3153dd", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3083), "5005", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3084), null, 1 },
                    { 2, 5, "#3153dd", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3164), "711", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3165), null, 1 },
                    { 3, 6, "#d62929", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3168), "90", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3168), null, 1 },
                    { 4, 5, "#b929d6", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3170), "609", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3171), null, 1 },
                    { 5, 1, "#bec11a", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3172), "71", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3173), null, 2 },
                    { 6, 1, "#3153dd", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3175), "404", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3175), null, 2 },
                    { 7, 2, "#ff0000", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3177), "40", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3178), null, 2 },
                    { 8, 2, "#00ff00", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3180), "250", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3180), null, 2 },
                    { 9, 2, "#0000ff", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3184), "880", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3184), null, 2 },
                    { 10, 11, "#ff00ff", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3186), "15", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3187), null, 3 },
                    { 11, 12, "#987654", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3189), "85", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3190), null, 3 },
                    { 12, 12, "#abcdef", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3192), "1", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3192), null, 3 },
                    { 13, 12, "#fedcba", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3194), "2000", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3195), null, 3 },
                    { 14, 11, "#012345", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3200), "1515", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3200), null, 3 },
                    { 15, 11, "#012345", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3201), "600", null, 1, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3202), null, 3 },
                    { 16, 5, "#abcdef", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3203), "500", null, 2, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3204), null, 1 },
                    { 17, 5, "#fedcba", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3205), "123", null, 2, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3205), null, 1 },
                    { 18, 5, "#012345", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3206), "999", null, 2, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3207), null, 1 },
                    { 19, 5, "#abcdef", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3208), "777", null, 2, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3209), null, 1 },
                    { 20, 1, "#fedcba", null, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3210), "333", null, 2, new DateTime(2024, 4, 19, 14, 15, 11, 338, DateTimeKind.Local).AddTicks(3210), null, 2 }
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
