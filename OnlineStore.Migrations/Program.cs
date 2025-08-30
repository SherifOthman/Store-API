using DbUp;

class Program
{
    static int Main(string[] args)
    {
        string connectionString = "Server=localhost;Database=StoreDBV2;User Id=sa;Password=sa123456;TrustServerCertificate=True;";

        var scriptsPath = Path.Combine(
            AppContext.BaseDirectory,
            @"..\..\..\..\OnlineStore.Migrations\Migrations"
        );

        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsFromFileSystem(scriptsPath)
            .LogToConsole()
            .Build();


        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
            return -1;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
        return 0;
    }
}
