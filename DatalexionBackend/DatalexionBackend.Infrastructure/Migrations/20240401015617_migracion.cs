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
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", null, new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(1618), "DatalexionRole", "Admin", "ADMIN", new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(1619) },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", null, new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(1643), "DatalexionRole", "Analyst", "ANALYST", new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(1643) }
                });

            migrationBuilder.InsertData(
                table: "Photo",
                columns: new[] { "Id", "CandidateId", "CandidateId1", "CircuitId", "Creation", "PartyLongId", "PartyShortId", "SlateId", "SlateId1", "URL", "Update", "WingId", "WingId1" },
                values: new object[,]
                {
                    { 1, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3694), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3699), null, null },
                    { 2, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3718), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3719), null, null },
                    { 3, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3721), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo3.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3722), null, null },
                    { 4, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3723), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo4.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3724), null, null },
                    { 5, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3725), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo5.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3726), null, null },
                    { 6, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3735), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo6.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3735), null, null },
                    { 7, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3737), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo7.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3737), null, null },
                    { 8, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3739), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo8.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3739), null, null },
                    { 9, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3741), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo9.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3741), null, null },
                    { 10, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3743), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo10.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3743), null, null },
                    { 11, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3875), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo11.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3876), null, null },
                    { 12, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3877), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo12.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3878), null, null },
                    { 13, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3885), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo13.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3885), null, null },
                    { 14, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3887), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo14.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3887), null, null },
                    { 15, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3889), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo15.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3890), null, null },
                    { 16, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3891), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo16.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3892), null, null },
                    { 101, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4152), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo101.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4153), null, null },
                    { 102, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4155), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo102.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4156), null, null },
                    { 103, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4168), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo103.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4168), null, null },
                    { 104, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4170), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo104.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4170), null, null },
                    { 105, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4172), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo105.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4173), null, null },
                    { 106, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4175), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo106.jpg", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4176), null, null },
                    { 111, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4178), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo111.png", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4178), null, null },
                    { 112, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4179), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo112.png", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4180), null, null },
                    { 113, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4181), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo113.png", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4182), null, null },
                    { 114, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4189), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo114.png", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4189), null, null },
                    { 115, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4191), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo115.png", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4191), null, null },
                    { 116, null, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4195), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo116.png", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4195), null, null }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "Id", "Center", "Comments", "Creation", "Name", "Update", "Zoom" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5361), "Montevideo", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5362), null },
                    { 2, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5421), "Canelones", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5421), null },
                    { 3, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5422), "Maldonado", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5422), null },
                    { 4, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5423), "Rocha", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5424), null },
                    { 5, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5424), "Colonia", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5425), null },
                    { 6, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5426), "Artigas", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5426), null },
                    { 7, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5427), "Salto", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5427), null },
                    { 8, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5428), "Paysandú", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5428), null },
                    { 9, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5429), "Tacuarembó", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5429), null },
                    { 10, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5430), "Rivera", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5430), null },
                    { 11, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5431), "San José", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5431), null },
                    { 12, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5432), "Durazno", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5432), null },
                    { 13, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5433), "Treinta y Tres", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5434), null },
                    { 14, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5434), "Cerro Largo", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5435), null },
                    { 15, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5435), "Rivera", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5436), null },
                    { 16, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5436), "Flores", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5437), null },
                    { 17, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5437), "Florida", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5438), null },
                    { 18, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5439), "Lavalleja", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5439), null },
                    { 19, null, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5440), "Soriano", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5440), null }
                });

            migrationBuilder.InsertData(
                table: "Candidate",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3969), "Álvaro Delgado", 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3970) },
                    { 2, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3971), "Laura Raffo", 2, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3972) },
                    { 3, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3973), "Jorge Gandini", 3, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3973) },
                    { 4, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3974), "Juan Sartori", 4, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3974) },
                    { 5, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3975), "Yamandú Orsi", 5, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3976) },
                    { 6, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3977), "Carolina Cosse", 6, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3977) },
                    { 7, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3978), "Mario Bergara", 7, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3979) },
                    { 8, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3980), "Pablo Mieres", 8, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3980) },
                    { 9, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3981), "Edgardo Novick", 9, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3981) },
                    { 10, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3982), "Andrés Lima", 10, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3983) },
                    { 11, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3984), "Gabriel Gurméndez", 11, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3985) },
                    { 12, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3985), "Robert Silva", 12, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3986) },
                    { 13, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3987), "Andrés Ojeda", 13, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3987) },
                    { 14, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3988), "Gustavo Zubía", 14, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3989) },
                    { 15, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3990), "Guzmán Acosta y Lara", 15, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3990) },
                    { 16, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3991), "Tabaré Viera", 16, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3992) }
                });

            migrationBuilder.InsertData(
                table: "Party",
                columns: new[] { "Id", "Color", "Comments", "Creation", "Name", "PhotoLongId", "PhotoShortId", "ShortName", "Update", "Votes" },
                values: new object[,]
                {
                    { 1, "#3153dd", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4227), "Frente Amplio", 101, 111, "FA", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4227), null },
                    { 2, "#3153dd", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5072), "Partido Nacional", 102, 112, "PN", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5075), null },
                    { 3, "#d62929", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5080), "Partido Colorado", 103, 113, "PC", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5081), null },
                    { 4, "#b929d6", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5082), "Partido Independiente", 104, 114, "PI", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5083), null },
                    { 5, "#f9bb28", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5084), "Cabildo Abierto", 105, 115, "CA", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5085), null },
                    { 6, "#009001", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5086), "PERI", 106, 116, "PERI", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5086), null }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "Update" },
                values: new object[,]
                {
                    { 1, "Frente Amplio", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5193), "Frente Amplio", 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5194) },
                    { 2, "Partido Nacional", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5198), "Partido Nacional", 2, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5198) },
                    { 3, "Partido Colorado", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5201), "Partido Colorado", 3, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5201) }
                });

            migrationBuilder.InsertData(
                table: "Wing",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5292), "FA", 1, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5292) },
                    { 2, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5302), "PN", 2, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5302) },
                    { 3, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5304), "PC", 3, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5304) },
                    { 4, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5305), "PI", 4, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5305) },
                    { 5, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5306), "CA", 5, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5307) },
                    { 6, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5307), "PERI", 6, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5308) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Creation", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Update", "UserName" },
                values: new object[,]
                {
                    { "2a765d8b-9204-4e0f-b4ce-453f6e1bb592", 0, 1, "c37c3042-4f45-4b02-b464-3a1cef4d02ee", new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(2043), "DatalexionUser", "adminfa@datalexion.lat", false, false, null, "Admin FA", "ADMINFA@DATALEXION.LAT", "ADMINFA", "AQAAAAIAAYagAAAAECmZNWkzPmDXCWY/pe2IMkOQrCfLHusVmx6UfQux/D/DrxfF3e4PTk1ht+FE9JGaxA==", null, false, "5d222482-cac1-4993-9245-5bbf3d8f7708", false, new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(2044), "adminfa" },
                    { "6c762a89-a7b6-4ee3-96d0-105b219dcaa6", 0, 3, "866fad3b-28b8-4cb2-82a3-5a77b254c468", new DateTime(2024, 3, 31, 22, 56, 14, 203, DateTimeKind.Local).AddTicks(2066), "DatalexionUser", "adminpc@datalexion.lat", false, false, null, "Admin PC", "ADMINPC@DATALEXION.LAT", "ADMINPC", "AQAAAAIAAYagAAAAEGw4tq3fD5Ua5n5LLv/2legxTcmScC0WQu8KpSGGnae4KSBInSUQ9IibjI8kvNlPpg==", null, false, "8dc8a874-349f-46ec-ad7d-adb45aa4b512", false, new DateTime(2024, 3, 31, 22, 56, 14, 203, DateTimeKind.Local).AddTicks(2071), "adminpc" },
                    { "8498a3ff-ca69-4b93-9a37-49a73c8dec77", 0, 2, "a37e0bf9-3e54-42c9-98ef-3a07b933137f", new DateTime(2024, 3, 31, 22, 56, 14, 124, DateTimeKind.Local).AddTicks(3742), "DatalexionUser", "adminpn@datalexion.lat", false, false, null, "Admin PN", "ADMINPN@DATALEXION.LAT", "ADMINPN", "AQAAAAIAAYagAAAAEK3ZcA6q6vOCe15KrCgcxR5DBw+cla2Lhsfj6tl08ArwaJOJoWYOWwlzsVGoxsjwnQ==", null, false, "e3fdbcdc-c711-40e2-a00a-e44578f59754", false, new DateTime(2024, 3, 31, 22, 56, 14, 124, DateTimeKind.Local).AddTicks(3749), "adminpn" },
                    { "b5172b14-f9e4-48f6-9634-2241c87f1719", 0, 3, "37457df8-2f27-411c-9c8a-37f4defb8d52", new DateTime(2024, 3, 31, 22, 56, 14, 449, DateTimeKind.Local).AddTicks(3613), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista PC", "NORMAL@DATALEXION.LAT", "ANALYSTPC", "AQAAAAIAAYagAAAAEDb2/SaGvBOBPwltpKxJj9sdFwx6j6v1je/v0YAlpIeyztzZTy1/fFqgSNGl4fN1hA==", null, false, "3f7b893e-0146-4115-8af2-b0b102bdc21e", false, new DateTime(2024, 3, 31, 22, 56, 14, 449, DateTimeKind.Local).AddTicks(3620), "analystpc" },
                    { "ddc18aa2-c5c7-40c9-9db3-246d2a05a06c", 0, 2, "78ca5ac6-e55c-4ef6-a02d-cdf1ff95f52a", new DateTime(2024, 3, 31, 22, 56, 14, 370, DateTimeKind.Local).AddTicks(6298), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista PN", "NORMAL@DATALEXION.LAT", "ANALYSTPN", "AQAAAAIAAYagAAAAEGs4qW7nQ5TwTBNmRIkgmeQu9Oot4iW0NVzmXaqdPmhvRn7zwzuJKVhKcNvAP6TtuQ==", null, false, "9b112a8d-71da-49df-8ca2-884c934d86a6", false, new DateTime(2024, 3, 31, 22, 56, 14, 370, DateTimeKind.Local).AddTicks(6305), "analystpn" },
                    { "e15e9299-d3b5-42fc-b101-44da6ad799de", 0, 1, "fc8aa862-bce6-4176-afaf-5e5d8585181c", new DateTime(2024, 3, 31, 22, 56, 14, 281, DateTimeKind.Local).AddTicks(8311), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Analista FA", "NORMAL@DATALEXION.LAT", "ANALYSTFA", "AQAAAAIAAYagAAAAEDAJpJ4o9d/G5Ghi2zcBiRbmtekb9Lmz29MLCnpyfofc/h+qtzWoW4QfhyTxK32v3Q==", null, false, "d7dc4b64-6904-434e-ae10-70d2526d6e15", false, new DateTime(2024, 3, 31, 22, 56, 14, 281, DateTimeKind.Local).AddTicks(8317), "analystfa" }
                });

            migrationBuilder.InsertData(
                table: "Delegado",
                columns: new[] { "Id", "CI", "ClientId", "Comments", "Creation", "Email", "Name", "Phone", "Update" },
                values: new object[,]
                {
                    { 1, "11111111", 2, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5238), "gonzalo.delegado@datalexion.lat", "Gonzalo", "099415831", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5239) },
                    { 2, "12345678", 2, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5243), "diego.delegado@datalexion.lat", "Diego", "099589896", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5244) },
                    { 3, "22222222", 1, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5245), "pablo.delegado@datalexion.lat", "Pablo", "099415831", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5246) },
                    { 4, "33333333", 3, null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5247), "gabriel.delegado@datalexion.lat", "Gabriel", "099415831", new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5247) }
                });

            migrationBuilder.InsertData(
                table: "Slate",
                columns: new[] { "Id", "CandidateId", "Color", "Comments", "Creation", "Name", "PhotoId", "ProvinceId", "Update", "Votes", "WingId" },
                values: new object[,]
                {
                    { 1, 6, "#3153dd", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4051), "5005", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4051), null, 1 },
                    { 2, 5, "#3153dd", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4074), "711", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4074), null, 1 },
                    { 3, 6, "#d62929", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4076), "90", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4076), null, 1 },
                    { 4, 5, "#b929d6", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4078), "609", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4078), null, 1 },
                    { 5, 1, "#bec11a", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4080), "71", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4081), null, 2 },
                    { 6, 1, "#3153dd", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4082), "404", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4083), null, 2 },
                    { 7, 2, "#ff0000", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4084), "40", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4084), null, 2 },
                    { 8, 2, "#00ff00", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4086), "250", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4086), null, 2 },
                    { 9, 2, "#0000ff", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4088), "880", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4088), null, 2 },
                    { 10, 11, "#ff00ff", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4089), "15", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4090), null, 3 },
                    { 11, 12, "#987654", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4091), "85", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4091), null, 3 },
                    { 12, 12, "#abcdef", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4093), "1", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4094), null, 3 },
                    { 13, 12, "#fedcba", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4095), "2000", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4095), null, 3 },
                    { 14, 11, "#012345", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4096), "1515", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4097), null, 3 },
                    { 15, 11, "#012345", null, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4098), "600", null, 1, new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4099), null, 3 }
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
