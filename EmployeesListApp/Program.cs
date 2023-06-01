using EmployeesListApp.Services;

static class Program
{
    static void Main(string[] args)
    {
        ConsoleAppService app = new ConsoleAppService(args);
        app.Run();
    }
}
