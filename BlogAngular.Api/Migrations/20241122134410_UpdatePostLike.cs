using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAngular.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUnLiked",
                table: "PostLikes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "11872d42-f137-430d-a396-46498fc4e3a7",
                columns: new[] { "CreatedDate", "EncryptedPrivateKey", "PasswordHash", "PublicKey", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 11, 22, 13, 44, 10, 518, DateTimeKind.Utc).AddTicks(8911), "4XA7UhPJ5BSdRnhiOamoRA==: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", "<RSAKeyValue><Modulus>3mos4NdjIzOZXV/6TvwQX1gbmmN5v3VIxxa4F1rjyYYG1LkL4FHIolnXaWuL7N/FF8lMja0cQNvux+qW+GIpuBj12d8zWdh92IXqH2L25xKKz7ch+pAZifpxisfM/yxlb/kQiVIYd/wqihIvHJOYcWrm2d+yXZ/Rvk4ka/lZHRTJawkksWG5dc/LNiYYVaWRHoBi9M/ndrnivcYg1Tusxe7+BfJmORll93XCJ1qNpmhhA7tvrRGKYKwXWKOXZhnidQipu0kPV0uXb0lAQz67+60RcztLvci2pX9768PC0Cj3tdHdEVacOUU0L0xr24dpa6McXTDeUJzEn1jurkTTMQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", new DateTime(2024, 11, 22, 13, 44, 10, 518, DateTimeKind.Utc).AddTicks(8917) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUnLiked",
                table: "PostLikes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "11872d42-f137-430d-a396-46498fc4e3a7",
                columns: new[] { "CreatedDate", "EncryptedPrivateKey", "PasswordHash", "PublicKey", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 11, 18, 7, 53, 35, 724, DateTimeKind.Utc).AddTicks(5236), "nrJYiL6/e/g2lHpzTiAzoA==:gUBbEu0sX2nEDuv3uiE4XDrC/yjnjZPT1OHhIEuV3YwzUsd4x84+e2WDlxZwXczxnr46MVNtPtWTk9fRFf9C5pXiSurn3lmcWq55ojD4QGw0AV0hOK+uZaBPrm4pFE3FAZ5zI2QP0IMOkD8ZUdnPLwGyt+CuvrlxdmXu1gDNMi71ixo6ozU7ooKOrCHw0O51Q8BRaGdsnJFQDWRU3fzUBanJhHRo2VeBXoiayN4saz9M1Kth/HZfb9+C9zMaERmp2by49IxCBMY2xv2vsuQPUW59sP/LmQnRMW3CmeFbs5y5N/oayhfwkYL3gD6vdDorpLHsNvU6amB/18LsVggGgGobM8wThfODqiLyoikLUSM6JflpHxB9+k8dW6PK4IpvBIESzb4AqlYhtY2t7jgM1N2V9wBxKjsFuUtP+xvRlEta0wjl6JKEye+tkXDkjEBjCdUJX+sNXp//IVQwUot0ew8/tpPYiIfko1rN1ftufTZj9d2r5sEH/7AcaNxQDdCLrgRRoR82M02tpN/ob50da//GoPMn/kfDIrEz8ahNboDqS0X4BsGGbWOz7xHe/mt0ovlUaLS89rZ2S/kJWMWBccbsg+8I0fNc3MH+VeDmtvldfgqAgnNNROO/ZNh7xGUr1ZYm6iAh029Pdg7H6bh1/yU+/SS61rXbYaL6AZ5LNgZTiOhO1755G/wBxlJb6D5xRtctI037oGpvE/8h6JbxxXPdjPBeFYR9vrzTVH2kuDqA+XklsM617E4u5Ng0NFeO/e1GcUFe0ZRz1JZciaa4ic4uzukg4zUk8LZ5vv9YZx2QjJFKxUo8Yvy3zC2MWlrbSm+82o8UAwM/aPMpA8XD2oksvvy2qK8geY7S5OxvW3OZ+g4Ahlf+y/4+HNlWGoNAMQdeaEhvKa59/tvrUnfHFJ3UsGbIRF7D8tkMZRLicnh6iw05FlA1gY2LWNaDdgQbGYP8b8YiQ0rSKq5nLnHiyPHhuYxg92bFhzRn8t+f3CBdnEw+iuKbQs47zP2Ap2+qO4GJnSvD7ITWfDde2TZX7uSNuf6q/M8itzgtvQkpV/Ukp0CyBSNDn5T9fBNm1WB5X0+kCdThQYhPqM7uN7j1KZ8YgthsTMmjNYD2WrMz7EvwfnY/EEB3tuztqAVWUFXqZtE/FzGc5yBSR7H69p6gfisUWypl/ADB0H/1GFgTOqMCHimyDZrEu6TtKwpXIhToMrKunsjOgKWXP/tmXIkeVhgziT6I7w0ZUqzMq4HONAt+9Mkua5Jk38HJFSn4NvwiPRbT3jczoWPfAONXseR7balEjL+IpsBjtE2sEisIOlus9IPfpjFYMvvyUEZO1rMNAIg77bDW/Cra4pQd5dTsVt5AUKQcLxy818FiAylQV6ohBZIoYXPPDXpTDukIL73Y7snVpzaC2JPALfF8ZYT4IVgnnsfpeevK6FGZPsexwqe0vwr6tSl33l1zgZDdn8PRMAHyo0GJb8dqmbsH3/1r8uUINEs9Z0E5Sf3n2sPaoppfxlIwFLVjoHRN51h5DYWqwYbCJLwx0O8DWHL4pgKtU6mdTBev0bqb1LspEJVEMaepWL4cjQ6Gc4pfKg0tDR5Ja/zpL+3NiSLAPIH3GZ+AJNmQL2yW8ztMzSq0cK/+7LPn5I6fJfSzz/xEUNEIX82RnoBohMcfFWYqdYL/y78QOm9HQyZTW9OPkct4fx5t4j5otaC9lxxBESWPR15WYB2qjmyozNxcRXcK4okkh79NBJuT9sUXwyfYonbBLnZk0N8kG0O/Hd2VPu7DMrLbhoAY/o89mwvkavDVluqkUDe5aO/JiEnhJMbffvpMvljg18zthCOQL0OMowxcVVLk2Lh7RC3JgPglVDbWxkX9faSzctLeKS2HOWPeDnlpua47TZgHx7FZ2Dhe99SGVmuVMnnDfBRiuVc4rco9Ctb8FkiFBsIF3uWByVB3XqWxhym0vwBMnCfSeiBNJVvFjxNbMmsZbdCLXFKw2KHI2rzvVhpvLo/cmn1z4D1I85Yd++5r772ZLTB40smgHoxvlZdo7E3T3sgUUIAh77dBpjiI4N2DxK/Nkyhc/Nl7fLpc2xssFDBmHSxBV73yINbrrBGGEBVdunrf5BfWfPTr+KaOZY1VlWIwaXmUKBDWtqsU+qTV2Gs5enHLAdPMcQLKZVmMzQTMh+eosd7WTKXQHpinQo+iKLYHijj//G+5EOyalANMv5gPo24fvKZy0o4MGTxgczAR", "AQAAAAIAAYagAAAAEMeVbzVRLIFbCIbDMUfqQK2ZAvWaVSW8rTO+Z7y3z/2hGOoZnXwZjGv8OZPFnv6Q2w==", "<RSAKeyValue><Modulus>xF0kkphslzIuc1TJvX5u2MzmLjyOo/mJAdretmlw05lQo9XWDFtxr2ydZuw8PJnJT436LZRxoYr86YpAG9nMHPlb/1VVOsQ3/DCt5QBoBYqqueNvHJtg9L0HVeVMPjRa3xE9ctwoKIfguEk+zmxIoDEETKyF8jz0L8T3T0VD0GWk02rN1vMrbBslfGwFbVS5Ick4V1NxpALJ1eYCCeKkoFUsFUHVOhI2P9XeSgkKZAs0/AGERmgji77T7US56wsOQmq/rmFmqksK8CJzMCbnFdWmwwcyDdYXU4L8/KTNFGENk40NxAnn8JiwzStwal9w95+qOXyHMRX+xGWvc6vIBQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", new DateTime(2024, 11, 18, 7, 53, 35, 724, DateTimeKind.Utc).AddTicks(5241) });
        }
    }
}
