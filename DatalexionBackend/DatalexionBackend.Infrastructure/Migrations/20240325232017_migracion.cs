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
                    { "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c", null, new DateTime(2024, 3, 25, 20, 20, 12, 183, DateTimeKind.Local).AddTicks(9651), "DatalexionRole", "Admin", "ADMIN", new DateTime(2024, 3, 25, 20, 20, 12, 183, DateTimeKind.Local).AddTicks(9654) },
                    { "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c", null, new DateTime(2024, 3, 25, 20, 20, 12, 183, DateTimeKind.Local).AddTicks(9683), "DatalexionRole", "Analyst", "ANALYST", new DateTime(2024, 3, 25, 20, 20, 12, 183, DateTimeKind.Local).AddTicks(9684) }
                });

            migrationBuilder.InsertData(
                table: "Photo",
                columns: new[] { "Id", "CandidateId", "CandidateId1", "CircuitId", "Creation", "PartyLongId", "PartyShortId", "SlateId", "SlateId1", "URL", "Update", "WingId", "WingId1" },
                values: new object[,]
                {
                    { 1, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4652), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4659), null, null },
                    { 2, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4671), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo1.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4672), null, null },
                    { 3, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4674), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo3.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4675), null, null },
                    { 4, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4677), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo4.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4678), null, null },
                    { 5, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4680), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo5.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4681), null, null },
                    { 6, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4685), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo6.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4701), null, null },
                    { 7, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4710), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo7.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4711), null, null },
                    { 8, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4830), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo8.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4831), null, null },
                    { 9, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4834), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo9.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4834), null, null },
                    { 10, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4840), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo10.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4841), null, null },
                    { 11, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4843), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo11.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4844), null, null },
                    { 12, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4846), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo12.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4847), null, null },
                    { 13, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4849), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo13.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4850), null, null },
                    { 14, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4852), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo14.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4853), null, null },
                    { 15, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4855), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo15.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4856), null, null },
                    { 16, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4858), null, null, null, null, "https://cienciasdesofa.lat/uploads/candidates/photo16.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4859), null, null },
                    { 101, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5348), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo101.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5349), null, null },
                    { 102, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5353), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo102.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5354), null, null },
                    { 103, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5357), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo103.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5357), null, null },
                    { 104, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5359), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo104.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5360), null, null },
                    { 105, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5363), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo105.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5363), null, null },
                    { 106, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5367), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesLong/photo106.jpg", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5367), null, null },
                    { 111, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5370), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo111.png", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5370), null, null },
                    { 112, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5372), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo112.png", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5373), null, null },
                    { 113, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5376), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo113.png", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5376), null, null },
                    { 114, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5379), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo114.png", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5379), null, null },
                    { 115, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5382), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo115.png", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5382), null, null },
                    { 116, null, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5384), null, null, null, null, "https://cienciasdesofa.lat/uploads/partiesShort/photo116.png", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5385), null, null }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "Id", "Center", "Comments", "Creation", "Name", "Update", "Zoom" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5829), "Montevideo", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5830), null },
                    { 2, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5834), "Canelones", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5834), null },
                    { 3, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5836), "Maldonado", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5837), null },
                    { 4, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5838), "Rocha", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5839), null },
                    { 5, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5840), "Colonia", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5841), null },
                    { 6, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5842), "Artigas", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5843), null },
                    { 7, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5844), "Salto", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5845), null },
                    { 8, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5847), "Paysandú", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5847), null },
                    { 9, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5849), "Tacuarembó", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5849), null },
                    { 10, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5851), "Rivera", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5851), null },
                    { 11, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5853), "San José", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5854), null },
                    { 12, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5855), "Durazno", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5855), null },
                    { 13, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5857), "Treinta y Tres", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5858), null },
                    { 14, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5859), "Cerro Largo", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5860), null },
                    { 15, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5861), "Rivera", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5862), null },
                    { 16, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5863), "Flores", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5864), null },
                    { 17, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5866), "Florida", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5866), null },
                    { 18, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5868), "Lavalleja", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5868), null },
                    { 19, null, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5869), "Soriano", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5870), null }
                });

            migrationBuilder.InsertData(
                table: "Candidate",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4927), "Álvaro Delgado", 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4928) },
                    { 2, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4931), "Laura Raffo", 2, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4932) },
                    { 3, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4934), "Jorge Gandini", 3, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4935) },
                    { 4, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4936), "Juan Sartori", 4, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4937) },
                    { 5, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4939), "Yamandú Orsi", 5, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4940) },
                    { 6, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4941), "Carolina Cosse", 6, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4942) },
                    { 7, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4944), "Mario Bergara", 7, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4945) },
                    { 8, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4946), "Pablo Mieres", 8, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4947) },
                    { 9, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4949), "Edgardo Novick", 9, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4950) },
                    { 10, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4951), "Andrés Lima", 10, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4952) },
                    { 11, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4954), "Gabriel Gurméndez", 11, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4954) },
                    { 12, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4956), "Robert Silva", 12, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4957) },
                    { 13, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4958), "Andrés Ojeda", 13, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4959) },
                    { 14, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4961), "Gustavo Zubía", 14, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4961) },
                    { 15, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4963), "Guzmán Acosta y Lara", 15, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4964) },
                    { 16, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4965), "Tabaré Viera", 16, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(4966) }
                });

            migrationBuilder.InsertData(
                table: "Party",
                columns: new[] { "Id", "Color", "Comments", "Creation", "Name", "PhotoLongId", "PhotoShortId", "ShortName", "Update", "Votes" },
                values: new object[,]
                {
                    { 1, "#3153dd", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5437), "Frente Amplio", 101, 111, "FA", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5438), null },
                    { 2, "#3153dd", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5446), "Partido Nacional", 102, 112, "PN", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5447), null },
                    { 3, "#d62929", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5450), "Partido Colorado", 103, 113, "PC", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5451), null },
                    { 4, "#b929d6", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5455), "Partido Independiente", 104, 114, "PI", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5456), null },
                    { 5, "#f9bb28", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5458), "Cabildo Abierto", 105, 115, "CA", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5459), null },
                    { 6, "#009001", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5462), "PERI", 106, 116, "PERI", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5463), null }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "Update" },
                values: new object[,]
                {
                    { 1, "Frente Amplio", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5530), "Frente Amplio", 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5531) },
                    { 2, "Partido Nacional", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5537), "Partido Nacional", 2, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5538) },
                    { 3, "Partido Colorado", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5542), "Partido Colorado", 3, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5543) }
                });

            migrationBuilder.InsertData(
                table: "Wing",
                columns: new[] { "Id", "Comments", "Creation", "Name", "PartyId", "PhotoId", "Update" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5674), "FA", 1, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5675) },
                    { 2, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5680), "PN", 2, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5681) },
                    { 3, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5683), "PC", 3, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5683) },
                    { 4, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5685), "PI", 4, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5686) },
                    { 5, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5688), "CA", 5, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5689) },
                    { 6, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5691), "PERI", 6, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5693) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Creation", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Update", "UserName" },
                values: new object[,]
                {
                    { "2a765d8b-9204-4e0f-b4ce-453f6e1bb592", 0, 1, "3a95d352-5f61-4e75-b454-3d70ad6a0366", new DateTime(2024, 3, 25, 20, 20, 12, 494, DateTimeKind.Local).AddTicks(8328), "DatalexionUser", "adminfa@datalexion.lat", false, false, null, "Admin FA", "ADMINFA@DATALEXION.LAT", "ADMINFA", "AQAAAAIAAYagAAAAED1JUAj4ZHzXJDp9172AgnQtjrGaAPJt+OFt5MJ9sO96AnC5eLLMEXuOstgPvydrnA==", null, false, "e24b63a9-d67c-4c85-bacf-ac78308a17ba", false, new DateTime(2024, 3, 25, 20, 20, 12, 494, DateTimeKind.Local).AddTicks(8340), "adminfa" },
                    { "6c762a89-a7b6-4ee3-96d0-105b219dcaa6", 0, 3, "92c4119e-1553-4ad5-b6c3-4468d87dd8a2", new DateTime(2024, 3, 25, 20, 20, 13, 222, DateTimeKind.Local).AddTicks(7619), "DatalexionUser", "adminpc@datalexion.lat", false, false, null, "Admin PC", "ADMINPC@DATALEXION.LAT", "ADMINPC", "AQAAAAIAAYagAAAAEApSV5L5lYL4itFiWuWJ1ys1Vyvq6Oq/NDMU9Tu0rw+6HKQsxPsIcK+pYiPi0CZ9ZQ==", null, false, "b0a3845e-61c9-441a-9b94-dec1cb2b80c8", false, new DateTime(2024, 3, 25, 20, 20, 13, 222, DateTimeKind.Local).AddTicks(7634), "adminpc" },
                    { "8498a3ff-ca69-4b93-9a37-49a73c8dec77", 0, 2, "5e8d8405-5b83-4307-b67e-de6850510c96", new DateTime(2024, 3, 25, 20, 20, 12, 921, DateTimeKind.Local).AddTicks(2215), "DatalexionUser", "adminpn@datalexion.lat", false, false, null, "Admin PN", "ADMINPN@DATALEXION.LAT", "ADMINPN", "AQAAAAIAAYagAAAAEJi+dQih4QLKBYDLKpawCK+hk0LD3jvGPWcSQ4ch7DX7si1oDEFmEt0777I66Nqh/Q==", null, false, "6d6d91d9-21c7-42d4-bb11-a077c72de717", false, new DateTime(2024, 3, 25, 20, 20, 12, 921, DateTimeKind.Local).AddTicks(2222), "adminpn" },
                    { "c2ee6493-5a73-46f3-a3f2-46d1d11d7176", 0, 2, "c46ec809-3818-4587-982d-cbc5af3f66ae", new DateTime(2024, 3, 25, 20, 20, 12, 183, DateTimeKind.Local).AddTicks(9707), "DatalexionUser", "admin@datalexion.lat", false, false, null, "Usuario administrador", "ADMIN@DATALEXION.LAT", "USERADMIN", "AQAAAAIAAYagAAAAECbLln9FOZHcDxMk2PqwokMKivGIX8zTA01KhLKUa0I5j3DgdFRhBZVQo4WadzLb9Q==", null, false, "e113f56a-85fc-4349-9998-417a5ae8def6", false, new DateTime(2024, 3, 25, 20, 20, 12, 183, DateTimeKind.Local).AddTicks(9709), "useradmin" },
                    { "e0765c93-676c-4199-b7ee-d7877c471821", 0, 2, "f976962d-8c0c-4f0a-9052-6a29d78029a2", new DateTime(2024, 3, 25, 20, 20, 13, 533, DateTimeKind.Local).AddTicks(5769), "DatalexionUser", "normal@datalexion.lat", false, false, null, "Usuario analista", "NORMAL@DATALEXION.LAT", "USERANALISTA", "AQAAAAIAAYagAAAAEHF/Fa6YiABXAGxs+GHdwR+sxtMxa/MjL5CxuuAn+0TTadTpz30gX0k054nfw0LwNg==", null, false, "3b58a2ac-8dfc-4c3d-aba2-adbaee01ca31", false, new DateTime(2024, 3, 25, 20, 20, 13, 533, DateTimeKind.Local).AddTicks(5779), "useranalista" }
                });

            migrationBuilder.InsertData(
                table: "Delegado",
                columns: new[] { "Id", "CI", "ClientId", "Comments", "Creation", "Email", "Name", "Phone", "Update" },
                values: new object[,]
                {
                    { 1, "11111111", 2, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5593), "gonzalo.delegado@datalexion.lat", "Gonzalo", "099415831", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5593) },
                    { 2, "12345678", 2, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5600), "diego.delegado@datalexion.lat", "Diego", "099589896", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5600) },
                    { 3, "22222222", 1, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5604), "pablo.delegado@datalexion.lat", "Pablo", "099415831", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5604) },
                    { 4, "33333333", 3, null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5606), "gabriel.delegado@datalexion.lat", "Gabriel", "099415831", new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5607) }
                });

            migrationBuilder.InsertData(
                table: "Slate",
                columns: new[] { "Id", "CandidateId", "Color", "Comments", "Creation", "Name", "PhotoId", "ProvinceId", "Update", "Votes", "WingId" },
                values: new object[,]
                {
                    { 1, 6, "#3153dd", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5043), "5005", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5044), null, 1 },
                    { 2, 5, "#3153dd", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5051), "711", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5052), null, 1 },
                    { 3, 6, "#d62929", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5055), "90", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5056), null, 1 },
                    { 4, 5, "#b929d6", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5058), "609", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5059), null, 1 },
                    { 5, 1, "#bec11a", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5062), "71", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5062), null, 2 },
                    { 6, 1, "#3153dd", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5065), "404", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5066), null, 2 },
                    { 7, 2, "#ff0000", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5068), "40", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5069), null, 2 },
                    { 8, 2, "#00ff00", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5074), "250", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5075), null, 2 },
                    { 9, 2, "#0000ff", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5077), "880", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5078), null, 2 },
                    { 10, 11, "#ff00ff", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5085), "15", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5086), null, 3 },
                    { 11, 12, "#987654", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5088), "85", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5089), null, 3 },
                    { 12, 12, "#abcdef", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5091), "1", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5091), null, 3 },
                    { 13, 12, "#fedcba", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5096), "2000", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5096), null, 3 },
                    { 14, 11, "#012345", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5099), "1515", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5099), null, 3 },
                    { 15, 11, "#012345", null, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5104), "600", null, 1, new DateTime(2024, 3, 25, 20, 20, 13, 912, DateTimeKind.Local).AddTicks(5105), null, 3 }
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
