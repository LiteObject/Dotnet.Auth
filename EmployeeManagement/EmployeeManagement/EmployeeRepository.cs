namespace EmployeeManagement
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public List<Employee> GetEmployees()
        {
            return
            [
                new Employee
                {
                    Id = 1,
                    Name = "John Doe",
                    Title = "Manager",
                    Department = "Sales",
                    Address = "123 Main St",
                    Email = "john.doe@example.com"
                },
                new Employee
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Title = "Developer",
                    Department = "IT",
                    Address = "456 Elm St",
                    Email = "jane.smith@example.com"
                },
                new Employee
                {
                    Id = 3,
                    Name = "Alice Johnson",
                    Title = "Analyst",
                    Department = "Finance",
                    Address = "789 Oak St",
                    Email = "alice.johnson@example.com"
                }
            ];
        }
    }
}
