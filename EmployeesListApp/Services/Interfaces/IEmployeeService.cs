using EmployeesListApp.Models;

namespace EmployeesListApp.Services.Interfaces
{
    public interface IEmployeeService
    {
        public Employee Get(int id);
        public EmployeesList GetAll();
        public void Add(Employee employee);
        public void Update(int id, Employee employee);
        public void Delete(int id);
        public event EventHandler ErrorHandler;
        public delegate void EventHandler(string message);
    }
}
