using EmployeesListApp.Models;
using EmployeesListApp.Services.Interfaces;
using System.Text.Json;

namespace EmployeesListApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _jsonFilePath;
        private int _lastId = 0;
        public event IEmployeeService.EventHandler ErrorHandler;

        public EmployeeService(string jsonFilePath)
        {
            if (jsonFilePath == null)
                throw new ArgumentNullException(nameof(jsonFilePath));
            _jsonFilePath = jsonFilePath;
            if (!File.Exists(_jsonFilePath))
                UpdateJsonFile(new EmployeesList { Employees = new List<Employee> { } });
            var employees = GetAll().Employees;
            if (employees.Count > 0)
                _lastId = employees.Max(x => x.Id);
        }

        public EmployeesList GetAll()
        {
            string json = File.ReadAllText(_jsonFilePath);
            if (string.IsNullOrEmpty(json))
                UpdateJsonFile(new EmployeesList { Employees = new List<Employee> { } });
            json = File.ReadAllText(_jsonFilePath);
            return JsonSerializer.Deserialize<EmployeesList>(json);
        }

        public int Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            _lastId++;
            employee.Id = _lastId;
            var employees = GetAll();
            employees.Employees.Add(employee);
            UpdateJsonFile(employees);
            return employee.Id;
        }

        public void Delete(int id)
        {
            var employees = GetAll();
            var deletedEmployee = employees.Employees.FirstOrDefault(x => x.Id == id);
            if (deletedEmployee == null)
                OnError(ErrorMessage(id));
            employees.Employees.Remove(deletedEmployee);
            UpdateJsonFile(employees);
        }

        public Employee Get(int id)
        {
            var employees = GetAll();
            var employee = employees.Employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
                OnError(ErrorMessage(id));
            return employee;
        }

        public void Update(int id, Employee employee)
        {
            var employees = GetAll();
            var updatedEmployee = employees.Employees.FirstOrDefault(x => x.Id == id);
            if (updatedEmployee == null)
                OnError(ErrorMessage(id));
            employees.Employees.Where(x => x.Id == id).Select(x =>
            {
                x.FirstName = employee.FirstName;
                x.LastName = employee.LastName;
                x.SalaryPerHour = employee.SalaryPerHour;
                return x;
            }).ToList();
            UpdateJsonFile(employees);
        }

        protected virtual void OnError(string message)
        {
            ErrorHandler.Invoke(message);
        }

        private void UpdateJsonFile<TValue>(TValue value)
        {
            File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(value));
        }

        private string ErrorMessage(int id) => $"Employee with id: {id} does not exist";
    }
}
