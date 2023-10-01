using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewProj.API.Migrations
{
    /// <inheritdoc />
    public partial class new_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulty",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("5f2b9fde-955f-4459-abbe-115e8e339789"), "Hard" },
                    { new Guid("809e3323-dc8d-4783-9c3a-e828c691639f"), "Easy" },
                    { new Guid("8da76d4d-e6e1-497c-9df6-e4c945bf3f42"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("0a6b30a6-dd7a-4a4d-8cfc-3d671c67866b"), "NTL", "Nothland", "https://sportishka.com/uploads/posts/2022-02/1645512331_54-sportishka-com-p-oklend-novaya-zelandiya-turizm-krasivo-fot-59.jpg" },
                    { new Guid("440436c3-82c9-45ad-9860-c640ffcd43fa"), "WGN", "Wellington", "https://almode.ru/uploads/posts/2021-04/1618557252_57-p-vellingtoni-68.jpg" },
                    { new Guid("78ae69cc-3d57-4a72-acd8-0128101ec086"), "BOP", "Bay Of Plenty", "https://www.journeysinternational.com/wp-content/uploads/2019/04/bay-of-plenty-aerial.jpg" },
                    { new Guid("b26c46b4-9961-4f00-9d58-0cf0c3f55a58"), "NSN", "Nielson", "http://i1.wallbox.ru/wallpapers/main2/201726/gorod-stadion-novaa-zelandia-vellington.jpg" },
                    { new Guid("ccd72f74-88dc-4f7e-ab8d-28f6d6fde930"), "STL", "Southland", "https://www.cruisegid.ru/assets/gallery/889/14604.jpg" },
                    { new Guid("e85ad1b8-2369-4d59-a7d9-e828780289a1"), "AKL", "Auckland", "https://sportishka.com/uploads/posts/2022-02/1645512205_12-sportishka-com-p-oklend-novaya-zelandiya-turizm-krasivo-fot-14.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulty",
                keyColumn: "Id",
                keyValue: new Guid("5f2b9fde-955f-4459-abbe-115e8e339789"));

            migrationBuilder.DeleteData(
                table: "Difficulty",
                keyColumn: "Id",
                keyValue: new Guid("809e3323-dc8d-4783-9c3a-e828c691639f"));

            migrationBuilder.DeleteData(
                table: "Difficulty",
                keyColumn: "Id",
                keyValue: new Guid("8da76d4d-e6e1-497c-9df6-e4c945bf3f42"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "id",
                keyValue: new Guid("0a6b30a6-dd7a-4a4d-8cfc-3d671c67866b"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "id",
                keyValue: new Guid("440436c3-82c9-45ad-9860-c640ffcd43fa"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "id",
                keyValue: new Guid("78ae69cc-3d57-4a72-acd8-0128101ec086"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "id",
                keyValue: new Guid("b26c46b4-9961-4f00-9d58-0cf0c3f55a58"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "id",
                keyValue: new Guid("ccd72f74-88dc-4f7e-ab8d-28f6d6fde930"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "id",
                keyValue: new Guid("e85ad1b8-2369-4d59-a7d9-e828780289a1"));
        }
    }
}
