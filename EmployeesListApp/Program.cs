using EmployeesListApp;
using EmployeesListApp.Services;
using System.Configuration;

static class Program
{
    static void Main(string[] args)
    {
        EmployeeService employeeService = new EmployeeService(ConfigurationManager.AppSettings["jsonFilePath"]);
        Employee employee1 = new Employee { FirstName = "Test1", LastName = "Test1", SalaryPerHour = 1 };
        Employee employee2 = new Employee { FirstName = "Test2", LastName = "Test2", SalaryPerHour = 2 };
        Employee employee3 = new Employee { FirstName = "11111111", LastName = "1111111111111", SalaryPerHour = 11111 };
        //employeeService.Add(employee1);
        //employeeService.Add(employee2);
        //employeeService.Add(employee3);
        employeeService.Update(2, employee3);
        var t = employeeService.Get(2);

        Console.WriteLine(t);
        Console.ReadLine();
    }
}
