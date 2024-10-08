using WT_Lab.Domain;

namespace WT_Lab.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Uri проекта
            var uri = "https://localhost:7002/";
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // Заполнение данными
            if (!context.Category.Any() && !context.Asset.Any())
            {
                var categories = new Category[]
                    {
                    new Category { Name="Сервер", NormalizedName="server"},
                    new Category { Name="ПК", NormalizedName="pc"},
                    new Category { Name="Сетевое оборудование", NormalizedName="network"},
                    new Category { Name="Другое оборудование", NormalizedName="other"}
                    };
                await context.Category.AddRangeAsync(categories);
                await context.SaveChangesAsync();
                var assets = new List<Asset>
                {
                    new Asset {Name="Сервер DELL360",
                    Description="Основной сервер предприятия", InvNumber=45882,
                    Price =48500, Photo=uri+"images/1.png", Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("server"))},
                    new Asset {Name="ПК Авалон+",
                    Description="Компьютер главного бухгалтера", InvNumber=55971,
                    Price =4750, Photo=uri+"images/2.png",Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("pc"))},
                    new Asset { Name="Коммутатор Cisco SX-350",
                    Description="Коммутатор бухгалтерия", InvNumber=60795,
                    Price =12500, Photo=uri+"images/3.png",Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("network"))},
                    new Asset {Name="Сервер DELL VRTX",
                    Description="Основной сервер предприятия", InvNumber=45701,
                    Price =69250, Photo=uri+"images/4.png",Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("server"))},
                    new Asset {Name="Навигатор Garmin",
                    Description="Etrex 20x", InvNumber=11225,
                    Price =3665, Photo=uri + "images/5.png",Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("other"))}
                };
                await context.AddRangeAsync(assets);
                await context.SaveChangesAsync();
            }
        }
    }
}
