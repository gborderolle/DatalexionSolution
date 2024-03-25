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
                    ProvinceId1 = table.Column<int>(type: "int", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Municipality_Province_ProvinceId1",
                        column: x => x.ProvinceId1,
                        principalTable: "Province",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    SlateId = table.Column<int>(type: "int", nullable: false),
                    SlateId1 = table.Column<int>(type: "int", nullable: false)
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
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", null, new DateTime(2024, 3, 25, 15, 6, 22, 581, DateTimeKind.Local).AddTicks(2903), "DatalexionRole", "Admin", "ADMIN", new DateTime(2024, 3, 25, 15, 6, 22, 581, DateTimeKind.Local).AddTicks(2905) },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", null, new DateTime(2024, 3, 25, 15, 6, 22, 581, DateTimeKind.Local).AddTicks(2910), "DatalexionRole", "Analyst", "ANALYST", new DateTime(2024, 3, 25, 15, 6, 22, 581, DateTimeKind.Local).AddTicks(2911) }
                });

            migrationBuilder.InsertData(
                table: "Photo",
                columns: new[] { "Id", "CandidateId", "CandidateId1", "CircuitId", "Creation", "PartyLongId", "PartyShortId", "SlateId", "SlateId1", "URL", "Update", "WingId", "WingId1" },
                values: new object[,]
                {
                    { 1, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6456), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6464), null, null },
                    { 2, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6492), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6493), null, null },
                    { 3, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6495), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo3.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6496), null, null },
                    { 4, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6499), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo4.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6500), null, null },
                    { 5, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6504), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo5.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6505), null, null },
                    { 6, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6508), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo6.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6521), null, null },
                    { 7, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6524), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo7.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6524), null, null },
                    { 8, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6527), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo8.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6527), null, null },
                    { 9, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6530), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo9.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6531), null, null },
                    { 10, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6535), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo10.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6535), null, null },
                    { 11, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6538), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo11.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6538), null, null },
                    { 12, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6550), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo12.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6550), null, null },
                    { 13, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6553), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo13.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6554), null, null },
                    { 14, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6574), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo14.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6575), null, null },
                    { 15, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6579), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo15.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6580), null, null },
                    { 16, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6582), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo16.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 173, DateTimeKind.Local).AddTicks(6583), null, null },
                    { 101, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7933), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo101.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7934), null, null },
                    { 102, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7945), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo102.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7946), null, null },
                    { 103, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7951), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo103.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7952), null, null },
                    { 104, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7955), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo104.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7955), null, null },
                    { 105, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7958), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo105.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7958), null, null },
                    { 106, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7962), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo106.jpg", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7963), null, null },
                    { 111, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7965), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo111.png", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7966), null, null },
                    { 112, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7968), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo112.png", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7969), null, null },
                    { 113, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7971), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo113.png", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7971), null, null },
                    { 114, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7975), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo114.png", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7975), null, null },
                    { 115, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7978), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo115.png", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7979), null, null },
                    { 116, null, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7981), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo116.png", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7982), null, null }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "Id", "Center", "Comments", "Creation", "Name", "Update", "Zoom" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8644), "Montevideo", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8645), null },
                    { 2, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8648), "Canelones", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8648), null },
                    { 3, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8649), "Maldonado", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8650), null },
                    { 4, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8651), "Rocha", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8651), null },
                    { 5, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8652), "Colonia", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8653), null },
                    { 6, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8654), "Artigas", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8655), null },
                    { 7, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8656), "Salto", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8656), null },
                    { 8, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8657), "Paysandú", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8658), null },
                    { 9, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8658), "Tacuarembó", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8659), null },
                    { 10, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8660), "Rivera", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8660), null },
                    { 11, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8661), "San José", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8662), null },
                    { 12, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8663), "Durazno", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8664), null },
                    { 13, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8665), "Treinta y Tres", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8665), null },
                    { 14, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8666), "Cerro Largo", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8667), null },
                    { 15, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8667), "Rivera", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8668), null },
                    { 16, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8669), "Flores", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8669), null },
                    { 17, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8670), "Florida", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8671), null },
                    { 18, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8671), "Lavalleja", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8672), null },
                    { 19, null, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8673), "Soriano", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8673), null }
                });

            migrationBuilder.InsertData(
                table: "Candidate",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5379), "Álvaro Delgado", 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5391) },
                    { 2, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5393), "Laura Raffo", 2, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5393) },
                    { 3, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5395), "Jorge Gandini", 3, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5396) },
                    { 4, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5397), "Juan Sartori", 4, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5397) },
                    { 5, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5399), "Yamandú Orsi", 5, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5399) },
                    { 6, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5401), "Carolina Cosse", 6, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5401) },
                    { 7, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5402), "Mario Bergara", 7, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5403) },
                    { 8, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5404), "Pablo Mieres", 8, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5405) },
                    { 9, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5406), "Edgardo Novick", 9, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5407) },
                    { 10, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5408), "Andrés Lima", 10, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5409) },
                    { 11, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5410), "Gabriel Gurméndez", 11, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5411) },
                    { 12, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5412), "Robert Silva", 12, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5412) },
                    { 13, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5422), "Andrés Ojeda", 13, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5422) },
                    { 14, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5424), "Gustavo Zubía", 14, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5424) },
                    { 15, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5425), "Guzmán Acosta y Lara", 15, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5426) },
                    { 16, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5427), "Tabaré Viera", 16, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(5428) }
                });

            migrationBuilder.InsertData(
                table: "Party",
                columns: new[] { "Id", "Color", "Comments", "Creation", "Name", "PhotoLongId", "PhotoShortId", "ShortName", "Update", "Votes" },
                values: new object[,]
                {
                    { 1, "#3153dd", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8049), "Frente Amplio", 101, 111, "FA", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8050), null },
                    { 2, "#3153dd", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8078), "Partido Nacional", 102, 112, "PN", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8078), null },
                    { 3, "#d62929", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8081), "Partido Colorado", 103, 113, "PC", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8081), null },
                    { 4, "#b929d6", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8084), "Partido Independiente", 104, 114, "PI", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8084), null },
                    { 5, "#f9bb28", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8136), "Cabildo Abierto", 105, 115, "CA", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8137), null },
                    { 6, "#009001", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8139), "PERI", 106, 116, "PERI", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8140), null }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "Update" },
                values: new object[,]
                {
                    { 1, "Frente Amplio", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8271), "Frente Amplio", 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8272) },
                    { 2, "Partido Nacional", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8276), "Partido Nacional", 2, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8277) },
                    { 3, "Partido Colorado", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8281), "Partido Colorado", 3, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8282) }
                });

            migrationBuilder.InsertData(
                table: "Wing",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8545), "FA", 1, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8546) },
                    { 2, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8566), "PN", 2, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8567) },
                    { 3, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8569), "PC", 3, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8569) },
                    { 4, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8571), "PI", 4, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8572) },
                    { 5, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8573), "CA", 5, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8574) },
                    { 6, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8575), "PERI", 6, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8576) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Creation", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Update", "UserName" },
                values: new object[,]
                {
                    { "2a765d8b-9204-4e0f-b4ce-453f6e1bb592", 0, 1, "d18d1265-0dd5-45f7-82e1-b5447abb28ad", new DateTime(2024, 3, 25, 15, 6, 22, 697, DateTimeKind.Local).AddTicks(6132), "DatalexionUser", "adminfa@datalexion.lat", false, false, null, "Admin FA", "ADMINFA@DATALEXION.LAT", "ADMINFA", "AQAAAAIAAYagAAAAECKuBOgzRvrUjovUyGzOHMEEegS1xoUcbDYcy9uR+/nknQ035FI+aRwr6U/qKkYM4w==", null, false, "fb2829b7-75d3-4bd4-859c-c2066a14a155", false, new DateTime(2024, 3, 25, 15, 6, 22, 697, DateTimeKind.Local).AddTicks(6140), "adminfa" },
                    { "6c762a89-a7b6-4ee3-96d0-105b219dcaa6", 0, 3, "1b76c80c-2ed0-4514-ab93-f4f6157ee1c0", new DateTime(2024, 3, 25, 15, 6, 22, 921, DateTimeKind.Local).AddTicks(8212), "DatalexionUser", "adminpc@datalexion.lat", false, false, null, "Admin PC", "ADMINPC@DATALEXION.LAT", "ADMINPC", "AQAAAAIAAYagAAAAEMo0H6Kbw/VrY1+xj+PTXIlmxfgvq6nTQVdniS51o23OCb0n8BIrOUMcWjiBvUYBrg==", null, false, "786b9c49-4cf2-4caa-86b5-4b04653520f2", false, new DateTime(2024, 3, 25, 15, 6, 22, 921, DateTimeKind.Local).AddTicks(8218), "adminpc" },
                    { "8498a3ff-ca69-4b93-9a37-49a73c8dec77", 0, 2, "91e6c078-6b5e-4d54-a8d4-4908788c3801", new DateTime(2024, 3, 25, 15, 6, 22, 807, DateTimeKind.Local).AddTicks(6699), "DatalexionUser", "adminpn@datalexion.lat", false, false, null, "Admin PN", "ADMINPN@DATALEXION.LAT", "ADMINPN", "AQAAAAIAAYagAAAAEB2T9Z3IgcIV5Ve88HcyA25YdTDZtkuMGIlSNzRImxmNUcdrT6xgo7kPhvd3M+Izzg==", null, false, "1b83ba91-b508-40c9-98ea-5ab81dfce758", false, new DateTime(2024, 3, 25, 15, 6, 22, 807, DateTimeKind.Local).AddTicks(6712), "adminpn" },
                    { "c2ee6493-5a73-46f3-a3f2-46d1d11d7176", 0, 2, "77c30adf-739c-479d-982a-32148a2f1dbb", new DateTime(2024, 3, 25, 15, 6, 22, 581, DateTimeKind.Local).AddTicks(2925), "DatalexionUser", "admin@datalexion.lat", false, false, null, "Usuario administrador", "ADMIN@DATALEXION.LAT", "USERADMIN", "AQAAAAIAAYagAAAAEEnInSa5jHlQRxrJv+XEO2819xNIY1t2uA8OKYpzX4CBWrB3wvUu+WHadPftJDaaeg==", null, false, "a02bc44f-cac3-4938-9f0d-1b9efaeefab8", false, new DateTime(2024, 3, 25, 15, 6, 22, 581, DateTimeKind.Local).AddTicks(2925), "useradmin" },
                    { "e0765c93-676c-4199-b7ee-d7877c471821", 0, 2, "6131dea0-0716-44ae-9cab-11a00d7803e0", new DateTime(2024, 3, 25, 15, 6, 23, 35, DateTimeKind.Local).AddTicks(7502), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Usuario analista", "NORMAL@DATALEXION.LAT", "USERANALISTA", "AQAAAAIAAYagAAAAEJgLA+eI1op1/uTEgcDHwR36XupcEcU5MuHA2LJhS68Vieb5zysvYKjZ+lvBPYIn3A==", null, false, "490b370c-ee2c-4d32-b32a-2fe62e7b0a27", false, new DateTime(2024, 3, 25, 15, 6, 23, 35, DateTimeKind.Local).AddTicks(7515), "useranalista" }
                });

            migrationBuilder.InsertData(
                table: "Delegado",
                columns: new[] { "Id", "CI", "ClientId", "Comments", "Creation", "Email", "Name", "Phone", "Update" },
                values: new object[,]
                {
                    { 1, "11111111", 2, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8336), "gonzalo.delegado@datalexion.lat", "Gonzalo", "099415831", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8337) },
                    { 2, "12345678", 2, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8343), "diego.delegado@datalexion.lat", "Diego", "099589896", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8344) },
                    { 3, "22222222", 1, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8346), "pablo.delegado@datalexion.lat", "Pablo", "099415831", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8346) },
                    { 4, "33333333", 3, null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8348), "gabriel.delegado@datalexion.lat", "Gabriel", "099415831", new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(8349) }
                });

            migrationBuilder.InsertData(
                table: "Slate",
                columns: new[] { "Id", "CandidateId", "Color", "Comments", "Creation", "Name", "PhotoId", "ProvinceId", "Update", "Votes", "WingId" },
                values: new object[,]
                {
                    { 1, 6, "#3153dd", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7566), "5005", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7569), null, 1 },
                    { 2, 5, "#3153dd", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7774), "711", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7774), null, 1 },
                    { 3, 6, "#d62929", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7778), "90", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7778), null, 1 },
                    { 4, 5, "#b929d6", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7780), "609", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7781), null, 1 },
                    { 5, 1, "#bec11a", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7783), "71", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7783), null, 2 },
                    { 6, 1, "#3153dd", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7785), "404", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7786), null, 2 },
                    { 7, 2, "#ff0000", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7789), "40", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7789), null, 2 },
                    { 8, 2, "#00ff00", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7791), "250", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7792), null, 2 },
                    { 9, 2, "#0000ff", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7794), "880", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7795), null, 2 },
                    { 10, 11, "#ff00ff", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7796), "15", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7797), null, 3 },
                    { 11, 12, "#987654", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7799), "85", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7800), null, 3 },
                    { 12, 12, "#abcdef", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7802), "1", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7803), null, 3 },
                    { 13, 12, "#fedcba", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7805), "2000", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7806), null, 3 },
                    { 14, 11, "#012345", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7808), "1515", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7808), null, 3 },
                    { 15, 11, "#012345", null, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7811), "600", null, 1, new DateTime(2024, 3, 25, 15, 6, 23, 174, DateTimeKind.Local).AddTicks(7811), null, 3 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "role", "Admin", "c2ee6493-5a73-46f3-a3f2-46d1d11d7176" },
                    { 2, "role", "Analyst", "e0765c93-676c-4199-b7ee-d7877c471821" },
                    { 3, "role", "Admin", "2a765d8b-9204-4e0f-b4ce-453f6e1bb592" },
                    { 4, "role", "Admin", "8498a3ff-ca69-4b93-9a37-49a73c8dec77" },
                    { 5, "role", "Admin", "6c762a89-a7b6-4ee3-96d0-105b219dcaa6" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", "2a765d8b-9204-4e0f-b4ce-453f6e1bb592" },
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", "6c762a89-a7b6-4ee3-96d0-105b219dcaa6" },
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", "8498a3ff-ca69-4b93-9a37-49a73c8dec77" },
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", "c2ee6493-5a73-46f3-a3f2-46d1d11d7176" },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", "e0765c93-676c-4199-b7ee-d7877c471821" }
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
                name: "IX_Municipality_ProvinceId1",
                table: "Municipality",
                column: "ProvinceId1");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_SlateId",
                table: "Participant",
                column: "SlateId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_SlateId1",
                table: "Participant",
                column: "SlateId1");

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
                onDelete: ReferentialAction.Cascade);

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Slate_SlateId1",
                table: "Participant",
                column: "SlateId1",
                principalTable: "Slate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
