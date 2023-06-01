using System.Text;
using System.Text.Json.Serialization;

namespace EmployeesListApp.Models
{
    public class EmployeesList
    {
        public List<Employee> Employees { get; set; }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [JsonPropertyName("Salary")]
        public decimal SalaryPerHour { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{{ Id = {Id}, FirstName = {FirstName}, LastName = {LastName}, SalaryPerHour = {SalaryPerHour} }}");
            return sb.ToString();
        }

    }
}
