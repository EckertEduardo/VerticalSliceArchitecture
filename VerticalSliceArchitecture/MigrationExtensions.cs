using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.DataBase;

namespace VerticalSliceArchitecture;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<MyContext>();

        dbContext.Database.Migrate();
    }
}
