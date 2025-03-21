namespace EmployeeManagement
{
    public class Employee
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string Title { get; set; } = string.Empty;

        public required string Department { get; set; }

        public string Address { get; set; } = string.Empty;

        public required string Email { get; set; }
    }
}
