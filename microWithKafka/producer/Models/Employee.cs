namespace MicroWithKafka;


public record Employee( Guid Id, string Name, string Department) {
    public Guid Id { get; set; } = Id;
    public string Name { get; set; } = Name;
    public string Department { get; set; } = Department;
     
}
public record EmployeeDto( string Name, string Department) {
    public string Name { get; set; } = Name;
    public string Department { get; set; } = Department;
     
}

