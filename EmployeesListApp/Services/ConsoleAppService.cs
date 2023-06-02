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

        public ConsoleAppService()
        {           
            _appParams = GetAppParams();         
            _employeeService = new EmployeeService(ConfigurationManager.AppSettings["jsonFilePath"]);
            _employeeService.ErrorHandler += ConsoleAppErrorHandler;
        }

        private void ConsoleAppErrorHandler(string message)
        {
            Console.WriteLine(message);
        }

        public void Run()
        {
            if (_appParams == null)
            {
                ConsoleAppErrorHandler("Invalid app params");
                return;
            }
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

        private AppParams GetAppParams()
        {
            // TODO parameter case insensitivity
            // -add Id:5 FirstName:John LastName:Doe Salary:100.50 
            // Сorrect kind of parameters
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() == 1)
                return null;
            StringBuilder sb = new StringBuilder();
            if (args.Count() == 2)
            {
                sb.Append($"{{\"Method\" : \"{args[1]}\", \"Employee\" : {{}}}}");                
            }
            else
            {
                sb.Append($"{{\"Method\" : \"{args[1]}\", \"Employee\" : {{");
                for (int i = 2; i < args.Count(); i++)
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
            }                
            try
            {
                return JsonSerializer.Deserialize<AppParams>(sb.ToString());
            }
            catch (Exception)
            {
                return null;
                throw new Exception("Application settings deserialization error");
            }           
        }
    }
}
