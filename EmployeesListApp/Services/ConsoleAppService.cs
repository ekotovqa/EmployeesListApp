using EmployeesListApp.Models;
using EmployeesListApp.Services.Interfaces;
using System.Configuration;
using System.Text;
using System.Text.Json;

namespace EmployeesListApp.Services
{
    public class ConsoleAppService : IAppService
    {
        private readonly AppParams _appParams;
        private readonly EmployeeService _employeeService;

        public ConsoleAppService(string[] appArgs)
        {
            if (appArgs.Count() == 0)
                throw new ArgumentNullException(nameof(appArgs));
            _appParams = Convert(appArgs);         
            _employeeService = new EmployeeService(ConfigurationManager.AppSettings["jsonFilePath"]);
            _employeeService.ErrorHandler += ConsoleAppErrorHandler;
        }

        private void ConsoleAppErrorHandler(string message)
        {
            Console.WriteLine(message);
        }

        public void Run()
        {
            switch (_appParams.Method)
            {
                case "-add":
                    _employeeService.Add(_appParams.Employee);
                    break;
                case "-delete":
                    _employeeService.Delete(_appParams.Employee.Id);
                    break;
                case "-getall":
                    var employees =_employeeService.GetAll();
                    foreach (var employee in employees.Employees)
                        Console.WriteLine(employee);
                    break;
                case "-get":
                    Console.WriteLine(_employeeService.Get(_appParams.Employee.Id));
                    break;
                case "-update":
                    _employeeService.Update(_appParams.Employee.Id, _appParams.Employee);
                    break;
                default:
                    ConsoleAppErrorHandler("Invalid method specified");
                    break;
            }
        }

        private AppParams Convert(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{{\"Method\" : \"{args[0]}\", \"Employee\" : {{");
            for (int i = 1; i < args.Count(); i++)
            {
                var arg = args[i].Split(':');
                if (arg[0] == "Salary" || arg[0] == "Id")
                {
                    sb.Append($"\"{arg[0]}\" : {arg[1]}, ");
                    continue;
                }
                sb.Append($"\"{arg[0]}\" : \"{arg[1]}\", ");
            }
            sb.Remove(sb.Length - 2, 2).Append("}}");
            return JsonSerializer.Deserialize<AppParams>(sb.ToString());
        }
    }
}
