using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DbLogging.Tests;

public class LoggingDbContextTests
{
    [Theory]
    [InlineData(new object[] {"cdff037e-ca73-454f-8db9-7ceaad89e7fc", $"Test entity cdff037e-ca73-454f-8db9-7ceaad89e7fc was created"})]
    public async Task Test1(string idString, string comment)
    {
        var id = Guid.Parse(idString);
        var services = new ServiceCollection();

        services.AddDbContext<TestDbContext>(options => { options.UseInMemoryDatabase("TestDb"); });

        var provider = services.BuildServiceProvider();

        using (var scope =provider.CreateScope()) 
        using (var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>())
        {
            dbContext.TestEntities.Add(new TestEntity
            {
                Id = id
            });
            await dbContext.SaveChangesAsync(DateTimeOffset.Now);
        }
        
        using (var scope =provider.CreateScope())
        using (var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>())
        {
            var logEntry = await dbContext.SystemLog.AsNoTracking().FirstOrDefaultAsync(entry => entry.Comment == comment);
            Assert.NotNull(logEntry);
            Assert.Equal(id, logEntry.ResourceId);
        }
    }
}