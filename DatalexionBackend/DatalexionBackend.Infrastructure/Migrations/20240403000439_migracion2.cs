using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatalexionBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migracion2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c",
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 34, 125, DateTimeKind.Local).AddTicks(7955), new DateTime(2024, 4, 2, 21, 4, 34, 125, DateTimeKind.Local).AddTicks(7956) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c",
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 34, 125, DateTimeKind.Local).AddTicks(7986), new DateTime(2024, 4, 2, 21, 4, 34, 125, DateTimeKind.Local).AddTicks(7987) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a765d8b-9204-4e0f-b4ce-453f6e1bb592",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "db5cb7a0-17d5-4025-a218-67fef773384e", new DateTime(2024, 4, 2, 21, 4, 34, 125, DateTimeKind.Local).AddTicks(8727), "AQAAAAIAAYagAAAAEJgAo5qR4POQf49qt37n2f4H1dw5Lum2CcrDArPQ6Mvdaf65zOut77d4yDP8yCwBEA==", "7934d0d9-2c08-4552-ba9c-e1be3e571346", new DateTime(2024, 4, 2, 21, 4, 34, 125, DateTimeKind.Local).AddTicks(8728) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6c762a89-a7b6-4ee3-96d0-105b219dcaa6",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "a6855919-c349-495e-b721-45bb115fe454", new DateTime(2024, 4, 2, 21, 4, 34, 569, DateTimeKind.Local).AddTicks(3330), "AQAAAAIAAYagAAAAEAdzx1mjiNlGcc7bJnOiQpvzN/ZTDh96G40LvdVLGc9IrLtnMdhWHnvqV8Ylkpx8sA==", "905c766b-9562-4da4-b6e5-fa989df8d89b", new DateTime(2024, 4, 2, 21, 4, 34, 569, DateTimeKind.Local).AddTicks(3336) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8498a3ff-ca69-4b93-9a37-49a73c8dec77",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "8887534e-e5da-4a60-8411-3fcb3d1df3e7", new DateTime(2024, 4, 2, 21, 4, 34, 332, DateTimeKind.Local).AddTicks(9047), "AQAAAAIAAYagAAAAEGkgGfcU2xt5f5E+kOWmadESwAzG0MnIF/H++clNSZzXGhVH7q3t1MsDpS79iSThrQ==", "f68a2b51-7042-431a-b061-75bc002fa0be", new DateTime(2024, 4, 2, 21, 4, 34, 332, DateTimeKind.Local).AddTicks(9052) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5172b14-f9e4-48f6-9634-2241c87f1719",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "780891c9-f2e1-4d15-9e91-9888615edcd9", new DateTime(2024, 4, 2, 21, 4, 35, 18, DateTimeKind.Local).AddTicks(5813), "AQAAAAIAAYagAAAAEFRfzq/7E0AoMsq4x+Cb3rcEygSXa+SzAHqymWnWAxc3+OXwAa7bvtKTJhUqlMKFbg==", "cc760ff0-3f38-4f07-aea0-b6e83dce7c91", new DateTime(2024, 4, 2, 21, 4, 35, 18, DateTimeKind.Local).AddTicks(5817) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ddc18aa2-c5c7-40c9-9db3-246d2a05a06c",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "7a249696-c173-446c-938a-cfbc3e93d2a5", new DateTime(2024, 4, 2, 21, 4, 34, 912, DateTimeKind.Local).AddTicks(3228), "AQAAAAIAAYagAAAAEFWIB+nySSRAihv+bO94TIUim/nLs3W1LPoBsyl2Fz65xdIeNezaiZUC2tPIAMVo4w==", "ab1a31d7-60c0-4859-a6d0-b13ebf3d0f9a", new DateTime(2024, 4, 2, 21, 4, 34, 912, DateTimeKind.Local).AddTicks(3233) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e15e9299-d3b5-42fc-b101-44da6ad799de",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "5de22118-1a35-480f-8c89-d35261c1efa1", new DateTime(2024, 4, 2, 21, 4, 34, 778, DateTimeKind.Local).AddTicks(1582), "AQAAAAIAAYagAAAAEH2Hla43daumBYjkkWxbcr8Lhdu8LaPtHBcbgVISdS2El2N1d5u0+JI8fwQZ/IXz3g==", "155912d7-9a6e-44c5-8ed7-2ebb875fbcd3", new DateTime(2024, 4, 2, 21, 4, 34, 778, DateTimeKind.Local).AddTicks(1593) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5910), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5910) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5914), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5914) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5916), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5917) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5919), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5919) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5921), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5922) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5924), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5925) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5926), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5927) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5929), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5929) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5931), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5932) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5933), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5934) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5936), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5937) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5938), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5939) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5940), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5941) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5943), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5945), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5945) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5947), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5948) });

            migrationBuilder.UpdateData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6743), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6744) });

            migrationBuilder.UpdateData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6750), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6751) });

            migrationBuilder.UpdateData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6755), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6756) });

            migrationBuilder.UpdateData(
                table: "Delegado",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6827), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6827) });

            migrationBuilder.UpdateData(
                table: "Delegado",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6834), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6834) });

            migrationBuilder.UpdateData(
                table: "Delegado",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6837), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6838) });

            migrationBuilder.UpdateData(
                table: "Delegado",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6840), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6841) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6447), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6448) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6483), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6483) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6486), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6487) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6491), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6492) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6598), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6599) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6605), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6606) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5604), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5609) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5638), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5638) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5734), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5735) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5738), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5739) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5741), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5766) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5783), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5784) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5786), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5787) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5790), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5791) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5793), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5794) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5799), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5799) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5802), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5802) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5805), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5805) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5808), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5808) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5811), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5811) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5814), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5815) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5817), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(5817) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6345), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6346) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6353), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6353) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6356), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6358) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6361), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6361) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6364), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6365) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6371), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6372) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6374), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6375) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6377), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6380), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6380) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6384), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6385) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6388), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6389) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6391), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6392) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6993), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6995) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6997), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7001) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7002), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7003) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7004), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7005) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7006), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7007) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7008), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7009) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7010), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7012) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7013), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7014) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7016), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7017) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7018), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7019) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7021), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7022) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7023), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7024) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7026), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7027) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7029), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7030) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7031), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7032) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7034), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7035) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7036), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7037) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7039), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7039) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7041), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(7042) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6021), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6022) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6049), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6050) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6053), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6054) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6057), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6057) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6060), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6061) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6063), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6064) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6066), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6067) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6217), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6218) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6222), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6222) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6225), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6225) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6228), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6229) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6231), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6232) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6234), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6235) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6238), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6238) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6241), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6241) });

            migrationBuilder.InsertData(
                table: "Slate",
                columns: new[] { "Id", "CandidateId", "Color", "Comments", "Creation", "Name", "PhotoId", "ProvinceId", "Update", "Votes", "WingId" },
                values: new object[,]
                {
                    { 16, 5, "#abcdef", null, new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6244), "500", null, 2, new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6245), null, 1 },
                    { 17, 5, "#fedcba", null, new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6247), "123", null, 2, new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6248), null, 1 },
                    { 18, 5, "#012345", null, new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6250), "999", null, 2, new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6251), null, 1 },
                    { 19, 5, "#abcdef", null, new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6254), "777", null, 2, new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6254), null, 1 },
                    { 20, 1, "#fedcba", null, new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6256), "333", null, 2, new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6257), null, 2 }
                });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6888), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6889) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6905), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6905) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6907), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6908) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6911), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6911) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6913), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6913) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6915), new DateTime(2024, 4, 2, 21, 4, 35, 168, DateTimeKind.Local).AddTicks(6916) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c",
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(1618), new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(1619) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c",
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(1643), new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(1643) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a765d8b-9204-4e0f-b4ce-453f6e1bb592",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "c37c3042-4f45-4b02-b464-3a1cef4d02ee", new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(2043), "AQAAAAIAAYagAAAAECmZNWkzPmDXCWY/pe2IMkOQrCfLHusVmx6UfQux/D/DrxfF3e4PTk1ht+FE9JGaxA==", "5d222482-cac1-4993-9245-5bbf3d8f7708", new DateTime(2024, 3, 31, 22, 56, 14, 24, DateTimeKind.Local).AddTicks(2044) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6c762a89-a7b6-4ee3-96d0-105b219dcaa6",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "866fad3b-28b8-4cb2-82a3-5a77b254c468", new DateTime(2024, 3, 31, 22, 56, 14, 203, DateTimeKind.Local).AddTicks(2066), "AQAAAAIAAYagAAAAEGw4tq3fD5Ua5n5LLv/2legxTcmScC0WQu8KpSGGnae4KSBInSUQ9IibjI8kvNlPpg==", "8dc8a874-349f-46ec-ad7d-adb45aa4b512", new DateTime(2024, 3, 31, 22, 56, 14, 203, DateTimeKind.Local).AddTicks(2071) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8498a3ff-ca69-4b93-9a37-49a73c8dec77",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "a37e0bf9-3e54-42c9-98ef-3a07b933137f", new DateTime(2024, 3, 31, 22, 56, 14, 124, DateTimeKind.Local).AddTicks(3742), "AQAAAAIAAYagAAAAEK3ZcA6q6vOCe15KrCgcxR5DBw+cla2Lhsfj6tl08ArwaJOJoWYOWwlzsVGoxsjwnQ==", "e3fdbcdc-c711-40e2-a00a-e44578f59754", new DateTime(2024, 3, 31, 22, 56, 14, 124, DateTimeKind.Local).AddTicks(3749) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5172b14-f9e4-48f6-9634-2241c87f1719",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "37457df8-2f27-411c-9c8a-37f4defb8d52", new DateTime(2024, 3, 31, 22, 56, 14, 449, DateTimeKind.Local).AddTicks(3613), "AQAAAAIAAYagAAAAEDb2/SaGvBOBPwltpKxJj9sdFwx6j6v1je/v0YAlpIeyztzZTy1/fFqgSNGl4fN1hA==", "3f7b893e-0146-4115-8af2-b0b102bdc21e", new DateTime(2024, 3, 31, 22, 56, 14, 449, DateTimeKind.Local).AddTicks(3620) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ddc18aa2-c5c7-40c9-9db3-246d2a05a06c",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "78ca5ac6-e55c-4ef6-a02d-cdf1ff95f52a", new DateTime(2024, 3, 31, 22, 56, 14, 370, DateTimeKind.Local).AddTicks(6298), "AQAAAAIAAYagAAAAEGs4qW7nQ5TwTBNmRIkgmeQu9Oot4iW0NVzmXaqdPmhvRn7zwzuJKVhKcNvAP6TtuQ==", "9b112a8d-71da-49df-8ca2-884c934d86a6", new DateTime(2024, 3, 31, 22, 56, 14, 370, DateTimeKind.Local).AddTicks(6305) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e15e9299-d3b5-42fc-b101-44da6ad799de",
                columns: new[] { "ConcurrencyStamp", "Creation", "PasswordHash", "SecurityStamp", "Update" },
                values: new object[] { "fc8aa862-bce6-4176-afaf-5e5d8585181c", new DateTime(2024, 3, 31, 22, 56, 14, 281, DateTimeKind.Local).AddTicks(8311), "AQAAAAIAAYagAAAAEDAJpJ4o9d/G5Ghi2zcBiRbmtekb9Lmz29MLCnpyfofc/h+qtzWoW4QfhyTxK32v3Q==", "d7dc4b64-6904-434e-ae10-70d2526d6e15", new DateTime(2024, 3, 31, 22, 56, 14, 281, DateTimeKind.Local).AddTicks(8317) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3969), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3970) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3971), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3972) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3973), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3973) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3974), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3974) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3975), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3976) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3977), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3977) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3978), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3979) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3980), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3980) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3981), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3981) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3982), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3983) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3984), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3985) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3985), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3986) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3987), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3987) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3988), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3989) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3990), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3990) });

            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3991), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3992) });

            migrationBuilder.UpdateData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5193), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5194) });

            migrationBuilder.UpdateData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5198), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5198) });

            migrationBuilder.UpdateData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5201), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5201) });

            migrationBuilder.UpdateData(
                table: "Delegado",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5238), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5239) });

            migrationBuilder.UpdateData(
                table: "Delegado",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5243), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5244) });

            migrationBuilder.UpdateData(
                table: "Delegado",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5245), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5246) });

            migrationBuilder.UpdateData(
                table: "Delegado",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5247), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5247) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4227), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4227) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5072), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5075) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5080), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5081) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5082), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5083) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5084), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5085) });

            migrationBuilder.UpdateData(
                table: "Party",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5086), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5086) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3694), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3699) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3718), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3719) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3721), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3722) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3723), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3724) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3725), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3726) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3735), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3735) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3737), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3737) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3739), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3739) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3741), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3741) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3743), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3743) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3875), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3876) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3877), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3878) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3885), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3885) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3887), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3887) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3889), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3890) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3891), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(3892) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4152), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4153) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4155), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4156) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4168), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4168) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4170), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4170) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4172), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4173) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4175), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4176) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4178), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4178) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4179), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4180) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4181), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4182) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4189), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4189) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4191), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4191) });

            migrationBuilder.UpdateData(
                table: "Photo",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4195), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4195) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5361), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5362) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5421), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5421) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5422), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5422) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5423), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5424) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5424), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5425) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5426), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5426) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5427), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5427) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5428), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5428) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5429), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5429) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5430), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5430) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5431), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5431) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5432), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5432) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5433), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5434) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5434), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5435) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5435), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5436) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5436), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5437) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5437), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5438) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5439), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5439) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5440), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5440) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4051), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4051) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4074), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4074) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4076), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4076) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4078), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4078) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4080), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4081) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4082), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4083) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4084), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4084) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4086), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4086) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4088), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4088) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4089), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4090) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4091), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4091) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4093), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4094) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4095), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4095) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4096), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4097) });

            migrationBuilder.UpdateData(
                table: "Slate",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4098), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(4099) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5292), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5292) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5302), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5302) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5304), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5304) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5305), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5305) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5306), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5307) });

            migrationBuilder.UpdateData(
                table: "Wing",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Creation", "Update" },
                values: new object[] { new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5307), new DateTime(2024, 3, 31, 22, 56, 14, 528, DateTimeKind.Local).AddTicks(5308) });
        }
    }
}
