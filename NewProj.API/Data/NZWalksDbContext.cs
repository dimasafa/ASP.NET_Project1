using Microsoft.EntityFrameworkCore;
using NewProj.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
        {

        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Difficulty> Difficulty { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties. (Вставляем данные таблицы в базу данных, разделяя ее на нужные части)
            // Easy, Medium, Hard
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    // Erstellen Guid-Code from Interactive C# (um beim Migration bleiben diese Zahlen(id) fest) und dannach parse zuruck.

                    Id = Guid.Parse("809e3323-dc8d-4783-9c3a-e828c691639f"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("8da76d4d-e6e1-497c-9df6-e4c945bf3f42"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("5f2b9fde-955f-4459-abbe-115e8e339789"),
                    Name = "Hard"
                }
            };

            //Seed difficulties to the database
            // После определения фиксированных данных таблицы выше мы передаем их в hasdata и указываем к какой таблице(Difficulties) они относятся.
            modelBuilder.Entity<Difficulty>().HasData(difficulties);


            // Seed data for Regions.
            var regions = new List<Region>()
            {
                new Region()
                {
                    id = Guid.Parse("e85ad1b8-2369-4d59-a7d9-e828780289a1"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://sportishka.com/uploads/posts/2022-02/1645512205_12-sportishka-com-p-oklend-novaya-zelandiya-turizm-krasivo-fot-14.jpg"
                },
                new Region()
                {
                    id = Guid.Parse("0a6b30a6-dd7a-4a4d-8cfc-3d671c67866b"),
                    Name = "Nothland",
                    Code = "NTL",
                    RegionImageUrl = "https://sportishka.com/uploads/posts/2022-02/1645512331_54-sportishka-com-p-oklend-novaya-zelandiya-turizm-krasivo-fot-59.jpg"
                },
                new Region()
                {
                    id = Guid.Parse("78ae69cc-3d57-4a72-acd8-0128101ec086"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = "https://www.journeysinternational.com/wp-content/uploads/2019/04/bay-of-plenty-aerial.jpg"
                },
                new Region()
                {
                    id = Guid.Parse("440436c3-82c9-45ad-9860-c640ffcd43fa"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://almode.ru/uploads/posts/2021-04/1618557252_57-p-vellingtoni-68.jpg"
                },
                new Region()
                {
                    id = Guid.Parse("b26c46b4-9961-4f00-9d58-0cf0c3f55a58"),
                    Name = "Nielson",
                    Code = "NSN",
                    RegionImageUrl = "http://i1.wallbox.ru/wallpapers/main2/201726/gorod-stadion-novaa-zelandia-vellington.jpg"
                },
                new Region()
                {
                    id = Guid.Parse("ccd72f74-88dc-4f7e-ab8d-28f6d6fde930"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = "https://www.cruisegid.ru/assets/gallery/889/14604.jpg"
                }
            };

            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}