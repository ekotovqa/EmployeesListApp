using EmployeesListApp.Services.Interfaces;
using System.Text.Json;

namespace EmployeesListApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _jsonFilePath;

        private int _lastId = 0;

        public EmployeeService(string jsonFilePath)
        {
            if (jsonFilePath == null)
                throw new ArgumentNullException(nameof(jsonFilePath));
            _jsonFilePath = jsonFilePath;
            var employees = GetAll().Employees;
            if (employees.Count > 0)
                _lastId = employees.Max(x => x.Id);
        }

        public EmployeesList GetAll()
        {
            if (!File.Exists(_jsonFilePath))
                File.Create(_jsonFilePath);
            string json = File.ReadAllText(_jsonFilePath);
            if (json == "")
                return new EmployeesList { Employees = new List<Employee> { } };
            return JsonSerializer.Deserialize<EmployeesList>(json);
        }

        public void Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            _lastId++;
            employee.Id = _lastId;
            var employees = GetAll();
            employees.Employees.Add(employee);
            File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(employees));
        }

        public void Delete(int id)
        {
            var employees = GetAll();
            var deletedEmployee = employees.Employees.FirstOrDefault(x => x.Id == id);
            employees.Employees.Remove(deletedEmployee);
            File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(employees));
        }

        public Employee Get(int id)
        {
            var employees = GetAll();
            var employee = employees.Employees.FirstOrDefault(x => x.Id == id);
            return employee;
        }

        public void Update(int id, Employee employee)
        {
            var employees = GetAll();
            var updatedEmployee = employees.Employees.FirstOrDefault(x => x.Id == id);
            employees.Employees.Where(x => x.Id == id).Select(x => 
            { 
                x.FirstName = employee.FirstName;
                x.LastName = employee.LastName;
                x.SalaryPerHour = employee.SalaryPerHour;
                return x; 
            }).ToList();
            File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(employees));
        }
    }
}
