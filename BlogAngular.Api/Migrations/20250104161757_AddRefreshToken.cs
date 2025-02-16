using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAngular.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "11872d42-f137-430d-a396-46498fc4e3a7",
                columns: new[] { "CreatedDate", "EncryptedPrivateKey", "PasswordHash", "PublicKey", "RefreshToken", "RefreshTokenExpiryTime", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 4, 16, 17, 56, 494, DateTimeKind.Utc).AddTicks(4708), "+tpZMZY5DPMpesOhjMnjMw==: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", "<RSAKeyValue><Modulus>12a7MZMbqqDnLYAbY9yTL5HFm3X6RtQv8xpJj6WJemnK4Y4O8GFUXAKU5WRVJvskvBvI9wT5qqCkaxfks1YRT+uHL4XY+chfzQrnBNpUoriYvRgJnGiDBVQadRLQYDNI7RXhlQlZdY0HlXxdG6CZWnW0xv1Am/tRAZu9KVF0VDmJkUvtWWj1lXFImf6+qLtZdjY1r97YCi5PWxHDbKSokn/uJdnYGRQuzIsuVLXkdY/162+3fPLnyyi/3M2AmGlcQcaf3PTQd6ymRBJzqvgT4cPvr0mNjI1Fk8f6xwU8A69LKVVJKWnRjtRSurK1fMJDVbbje1pZ7u1n5KXtBiIXQQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", null, null, new DateTime(2025, 1, 4, 16, 17, 56, 494, DateTimeKind.Utc).AddTicks(4714) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "11872d42-f137-430d-a396-46498fc4e3a7",
                columns: new[] { "CreatedDate", "EncryptedPrivateKey", "PasswordHash", "PublicKey", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 11, 30, 16, 31, 1, 803, DateTimeKind.Utc).AddTicks(9899), "Os/K4Onz29xH6k/6Nftq4Q==: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", "<RSAKeyValue><Modulus>yAEht/KzvWskIa+U/4djegg5cZ4b+e3FebvdDgCLEyhk67vgUzqL0G5fGw+IsXe/0NDwkugp10Y81qSMBVkv0bnty8U+RIQe5FVs9oVhXYLmSL6txFmuxqWZVGBjtHqhZLdOjx4w3qk08yw3ryu8Fvykvkgo7PbuuJs1WIp2PAmqa2Pe+l2Npsl9R/hv9d9h0crXi1rCc3IvyYuyvIu4gcfsc2WAG+3Ur8CWHeykykAuz8IJpBiEE9vf7bCsMgpK6DkqUg0B6BRODWpaLn8982+/G5JUelUMwYst6sS32dtA2PiOJCEpowO2B2NtwnhjQsxUPBoVk5uuz8IZIopvuQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", new DateTime(2024, 11, 30, 16, 31, 1, 803, DateTimeKind.Utc).AddTicks(9904) });
        }
    }
}
