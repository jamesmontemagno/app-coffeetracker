using SQLite;
using MoreCoffee.Models;

namespace MoreCoffee.Services;

public class BagOfCoffeeService
{
    SQLiteAsyncConnection Database;
    bool isInitialized;
    SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

    public BagOfCoffeeService()
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "coffee.db");
        Database = new SQLiteAsyncConnection(databasePath);
    }

    public async Task InitializeAsync()
    {
        await semaphoreSlim.WaitAsync();
        try
        {
            if (!isInitialized)
            {
                await Init();
                isInitialized = true;
            }
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    async Task Init()
    {
        await Database.CreateTableAsync<BagOfCoffee>();
    }

    public async Task<List<BagOfCoffee>> GetBagsOfCoffeeAsync()
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<BagOfCoffee>().OrderByDescending(c => c.DateAdded).ToListAsync();
    }

    public async Task<int> AddBagOfCoffeeAsync(BagOfCoffee coffee)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.InsertAsync(coffee);
    }

    public async Task<int> UpdateBagOfCoffeeAsync(BagOfCoffee coffee)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.UpdateAsync(coffee);
    }

    public async Task<int> DeleteBagOfCoffeeAsync(BagOfCoffee coffee)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.DeleteAsync(coffee);
    }
}