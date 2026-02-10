using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ElectroStore.Models;

namespace ElectroStore.Data;

public class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Create Admin Role
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Create Customer Role
        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            await roleManager.CreateAsync(new IdentityRole("Customer"));
        }

        // Create Admin User
        const string adminEmail = "admin@electrostore.com";
        const string adminPassword = "Admin123!";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FullName = "Admin User"
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            // Ensure admin user has Admin role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Seed Categories
        await SeedCategoriesAsync(context);

        // Seed Products
        await SeedProductsAsync(context);
    }

    private static async Task SeedCategoriesAsync(ApplicationDbContext context)
    {
        // Check if categories exist, if not create them
        if (!await context.Categories.AnyAsync())
        {
            var categories = new[]
        {
            new Category { Name = "Mobil", Description = "Mobiltelefoner och smartphones" },
            new Category { Name = "Laptop", Description = "Laptops och bärbara datorer" },
            new Category { Name = "Surfplatta", Description = "Surfplattor och iPads" },
            new Category { Name = "TV", Description = "TV-apparater och skärmar" },
            new Category { Name = "Hörlurar", Description = "Hörlurar och trådlösa öronsnäckor" },
            new Category { Name = "Spelkonsol", Description = "Spelkonsoler och video spelkonsoler" },
            new Category { Name = "Smartklocka", Description = "Smartklockor och fitnessklockor" },
            new Category { Name = "Kamera", Description = "Digitalkameror och fotoutrustning" }
        };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        // Check if products exist, if not create them
        if (!await context.Products.AnyAsync())
        {
            var categories = await context.Categories.ToListAsync();
        
        var mobileCategory = categories.FirstOrDefault(c => c.Name == "Mobil");
        var laptopCategory = categories.FirstOrDefault(c => c.Name == "Laptop");
        var tabletCategory = categories.FirstOrDefault(c => c.Name == "Surfplatta");
        var tvCategory = categories.FirstOrDefault(c => c.Name == "TV");
        var headphoneCategory = categories.FirstOrDefault(c => c.Name == "Hörlurar");
        var consoleCategory = categories.FirstOrDefault(c => c.Name == "Spelkonsol");
        var watchCategory = categories.FirstOrDefault(c => c.Name == "Smartklocka");
        var cameraCategory = categories.FirstOrDefault(c => c.Name == "Kamera");

        var products = new List<Product>();

        // Mobile Products
        if (mobileCategory != null)
        {
            products.AddRange(new[]
            {
                new Product
                {
                    Name = "Samsung Galaxy S24 Ultra",
                    Price = 45000000,
                    OldPrice = 50000000,
                    Description = "Samsungs flaggskeppssmartphone med 6.8 tum skärm, Snapdragon 8 Gen 3 processor, 200 megapixel kamera",
                    ImageUrl = "/images/products/samsung-galaxy-s24-ultra.jpg",
                    Rating = 5,
                    IsNew = true,
                    CategoryId = mobileCategory.Id
                },
                new Product
                {
                    Name = "iPhone 15 Pro Max",
                    Price = 55000000,
                    OldPrice = 60000000,
                    Description = "Apples senaste iPhone med A17 Pro-chip, trippelkamera på 48 megapixel, 6.7 tum Super Retina XDR-skärm",
                    ImageUrl = "/images/products/iphone-15-pro-max.jpg",
                    Rating = 5,
                    IsNew = true,
                    CategoryId = mobileCategory.Id
                },
                new Product
                {
                    Name = "Xiaomi Redmi Note 13",
                    Price = 8500000,
                    Description = "Ekonomisk smartphone med 6.67 tum AMOLED-skärm, Snapdragon 685 processor, 108 megapixel kamera",
                    ImageUrl = "/images/products/xiaomi-redmi-note-13.jpg",
                    Rating = 4,
                    IsNew = false,
                    CategoryId = mobileCategory.Id
                }
            });
        }

        // Laptop Products
        if (laptopCategory != null)
        {
            products.AddRange(new[]
            {
                new Product
                {
                    Name = "Dell XPS 15 Laptop",
                    Price = 65000000,
                    OldPrice = 70000000,
                    Description = "Professionell laptop med 15.6 tum 4K OLED-skärm, Intel Core i7-13700H processor, 16GB RAM, 512GB SSD",
                    ImageUrl = "/images/products/dell-xps-15.jpg",
                    Rating = 5,
                    IsNew = true,
                    CategoryId = laptopCategory.Id
                },
                new Product
                {
                    Name = "MacBook Pro 14 tum",
                    Price = 80000000,
                    Description = "Apples laptop med M3 Pro-chip, 14.2 tum Liquid Retina XDR-skärm, 18GB enhetlig RAM",
                    ImageUrl = "/images/products/macbook-pro-14.jpg",
                    Rating = 5,
                    IsNew = true,
                    CategoryId = laptopCategory.Id
                },
                new Product
                {
                    Name = "Asus ROG Strix G15 Laptop",
                    Price = 45000000,
                    Description = "Spel-laptop med AMD Ryzen 9 processor, RTX 4060 grafikkort, 15.6 tum 144Hz skärm",
                    ImageUrl = "/images/products/asus-rog-strix-g15.jpg",
                    Rating = 4,
                    IsNew = false,
                    CategoryId = laptopCategory.Id
                },
                new Product
                {
                    Name = "Lenovo ThinkPad X1 Carbon Laptop",
                    Price = 55000000,
                    Description = "Affärslaptop med Intel Core i7-1355U processor, 14 tum 2.8K skärm, 32GB RAM",
                    ImageUrl = "/images/products/lenovo-thinkpad-x1-carbon.jpg",
                    Rating = 4,
                    IsNew = false,
                    CategoryId = laptopCategory.Id
                }
            });
        }

        // Tablet Products
        if (tabletCategory != null)
        {
            products.AddRange(new[]
            {
                new Product
                {
                    Name = "iPad Pro 12.9 tum",
                    Price = 35000000,
                    Description = "Apples professionella surfplatta med M2-chip, 12.9 tum Liquid Retina XDR-skärm, 256GB lagring",
                    ImageUrl = "/images/products/ipad-pro-12-9.jpg",
                    Rating = 5,
                    IsNew = true,
                    CategoryId = tabletCategory.Id
                },
                new Product
                {
                    Name = "Samsung Galaxy Tab S9 Ultra",
                    Price = 32000000,
                    Description = "Android-surfplatta med 14.6 tum Dynamic AMOLED 2X-skärm, Snapdragon 8 Gen 2 processor",
                    ImageUrl = "/images/products/samsung-galaxy-tab-s9-ultra.jpg",
                    Rating = 4,
                    IsNew = false,
                    CategoryId = tabletCategory.Id
                }
            });
        }

        // TV Products
        if (tvCategory != null)
        {
            products.AddRange(new[]
            {
                new Product
                {
                    Name = "Samsung QLED TV 65 tum",
                    Price = 45000000,
                    Description = "Smart 4K QLED-TV med HDR10+, Dolby Vision-stöd, Tizen operativsystem",
                    ImageUrl = "/images/products/samsung-qled-tv-65.jpg",
                    Rating = 5,
                    IsNew = true,
                    CategoryId = tvCategory.Id
                },
                new Product
                {
                    Name = "LG OLED TV 55 tum",
                    Price = 38000000,
                    Description = "4K OLED-TV med självlysande skärm, α9 Gen6 AI processor, webOS-system",
                    ImageUrl = "/images/products/lg-oled-tv-55.jpg",
                    Rating = 5,
                    IsNew = false,
                    CategoryId = tvCategory.Id
                },
                new Product
                {
                    Name = "Sony Bravia TV 75 tum",
                    Price = 60000000,
                    Description = "4K LED-TV med Cognitive Processor XR, Android TV-system",
                    ImageUrl = "/images/products/sony-bravia-tv-75.jpg",
                    Rating = 4,
                    IsNew = false,
                    CategoryId = tvCategory.Id
                }
            });
        }

        // Headphone Products
        if (headphoneCategory != null)
        {
            products.AddRange(new[]
            {
                new Product
                {
                    Name = "Sony WH-1000XM5 Hörlurar",
                    Price = 12000000,
                    Description = "Trådlösa brusreducerande hörlurar med Hi-Res ljudkvalitet, 30 timmars batteritid, snabbladdning",
                    ImageUrl = "/images/products/sony-wh-1000xm5.jpg",
                    Rating = 5,
                    IsNew = true,
                    CategoryId = headphoneCategory.Id
                },
                new Product
                {
                    Name = "AirPods Pro 2",
                    Price = 8000000,
                    Description = "Apples trådlösa öronsnäckor med aktiv brusreducering, Spatial Audio-ljudkvalitet, upp till 6 timmars batteritid",
                    ImageUrl = "/images/products/airpods-pro-2.jpg",
                    Rating = 5,
                    IsNew = false,
                    CategoryId = headphoneCategory.Id
                },
                new Product
                {
                    Name = "Beyerdynamic DT 770 Pro Hörlurar",
                    Price = 4500000,
                    Description = "Professionella studiörlurar med utmärkt ljudkvalitet, 80 ohm impedans, 3 meters kabel",
                    ImageUrl = "/images/products/beyerdynamic-dt-770-pro.jpg",
                    Rating = 4,
                    IsNew = false,
                    CategoryId = headphoneCategory.Id
                }
            });
        }

        // Console Products
        if (consoleCategory != null)
        {
            products.AddRange(new[]
            {
                new Product
                {
                    Name = "PlayStation 5",
                    Price = 25000000,
                    Description = "Sonys nästa generationsspelkonsol med anpassad AMD Zen 2 processor, 825GB SSD, DualSense-kontroll",
                    ImageUrl = "/images/products/R.jpg",
                    Rating = 5,
                    IsNew = true,
                    CategoryId = consoleCategory.Id
                },
                new Product
                {
                    Name = "Xbox Series X",
                    Price = 23000000,
                    Description = "Microsofts spelkonsol med 12 TFLOPS kraft, 1TB SSD, stöd för 4K/60fps",
                    ImageUrl = "/images/products/xbox-series-x.jpg",
                    Rating = 5,
                    IsNew = false,
                    CategoryId = consoleCategory.Id
                }
            });
        }

        // Watch Products
        if (watchCategory != null)
        {
            products.AddRange(new[]
            {
                new Product
                {
                    Name = "Apple Watch Series 9",
                    Price = 15000000,
                    Description = "Apples smartklocka med Always-On Retina-skärm, S9-chip, GPS, vattenbeständig",
                    ImageUrl = "/images/products/apple-watch-series-9.jpg",
                    Rating = 5,
                    IsNew = true,
                    CategoryId = watchCategory.Id
                },
                new Product
                {
                    Name = "Samsung Galaxy Watch 6",
                    Price = 12000000,
                    Description = "Android-smartklocka med AMOLED-skärm, hälsoövervakning, 40 timmars batteritid",
                    ImageUrl = "/images/products/samsung-galaxy-watch-6.jpg",
                    Rating = 4,
                    IsNew = false,
                    CategoryId = watchCategory.Id
                }
            });
        }

        // Camera Products
        if (cameraCategory != null)
        {
            products.AddRange(new[]
            {
                new Product
                {
                    Name = "Canon EOS R6 Mark II Kamera",
                    Price = 55000000,
                    Description = "Fullformat spegellös kamera med 24.2 megapixel sensor, 4K/60p videoinspelning, bildstabilisering",
                    ImageUrl = "/images/products/Canon.webp",
                    Rating = 5,
                    IsNew = true,
                    CategoryId = cameraCategory.Id
                },
                new Product
                {
                    Name = "Sony A7 IV Kamera",
                    Price = 48000000,
                    Description = "Fullformat spegellös kamera med 33 megapixel sensor, 4K/30p videoinspelning, BIONZ XR processor",
                    ImageUrl = "/images/products/sony.jpeg",
                    Rating = 5,
                    IsNew = false,
                    CategoryId = cameraCategory.Id
                }
            });
        }

        // Make sure we have exactly 20 products
        if (products.Count < 20)
        {
            // Add more mobile products to reach 20
            if (mobileCategory != null)
            {
                products.AddRange(new[]
                {
                    new Product
                    {
                        Name = "Google Pixel 8 Pro",
                        Price = 35000000,
                        Description = "Googles smartphone med 50 megapixel kamera, Google Tensor G3-chip, 6.7 tum skärm",
                        ImageUrl = "/images/products/google-pixel-8-pro.jpg",
                        Rating = 4,
                        IsNew = false,
                        CategoryId = mobileCategory.Id
                    }
                });
            }
        }

            await context.Products.AddRangeAsync(products.Take(20));
            await context.SaveChangesAsync();
        }
    }
}
