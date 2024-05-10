public class CreateEmpleadoDto
{
    public double Sueldo { get; set; }
    public string Login { get; set; } = "";

    public string Nombre { get; set; } = "";

    public string Paterno { get; set; } = "";

    public string Materno { get; set; } = "";
}

public class EmpleadoDto : CreateEmpleadoDto
{
    public int userId { get; set; }
    public DateTime FechaIngreso { get; set; }
}

public class UpdateSueldoEmpleadoDto 
{
    public double Sueldo { get; set; }
}