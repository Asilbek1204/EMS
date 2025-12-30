using EMS.Api.Entities;

namespace EMS.Api.Data;

public static class SeedData
{
    public static void Init(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.EnsureCreated();

        // Roles
        if (!db.Roles.Any())
        {
            db.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Manager" }
            );
            db.SaveChanges();
        }

        // Admin user
        if (!db.Users.Any())
        {
            var admin = new User
            {
                UserName = "admin",
                FirstName = "System",
                LastName = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!")
            };

            db.Users.Add(admin);
            db.SaveChanges();

            var adminRole = db.Roles.First(r => r.Name == "Admin");

            db.UserRoles.Add(new UserRole
            {
                UserId = admin.Id,
                RoleId = adminRole.Id
            });

            db.SaveChanges();
        }
    }
}
