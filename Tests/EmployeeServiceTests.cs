using EmployeesListApp.Models;
using EmployeesListApp.Services;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("test.json")]
        public void AssertAddMethod(string jsonFile)
        {
            Employee addedEmployee = new Employee()
            { 
                FirstName = "FirstName" + DateTime.Now,
                LastName = "LastName" + DateTime.Now,
                SalaryPerHour = 50 
            };
            EmployeeService employeeService = new EmployeeService(jsonFile);
            int addedEmployeeId = employeeService.Add(addedEmployee);
            var actualEmployee = employeeService.Get(addedEmployeeId);
            Assert.That(addedEmployee.Id == actualEmployee.Id);
            Assert.That(addedEmployee.FirstName == actualEmployee.FirstName);
            Assert.That(addedEmployee.LastName == actualEmployee.LastName);
            Assert.That(addedEmployee.SalaryPerHour == actualEmployee.SalaryPerHour);
        }

        [Test]
        [TestCase(null, "test.json")]

        public void AssertAddMethodException(Employee employee, string jsonFile)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new EmployeeService(jsonFile).Add(employee));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter 'employee')"));
        }
    }
}