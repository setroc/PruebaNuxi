using System;
using System.Collections.Generic;

namespace Prueba_2.Models;

public partial class Usuario
{
    public int UserId { get; set; }

    public string? Login { get; set; }

    public string? Nombre { get; set; }

    public string? Paterno { get; set; }

    public string? Materno { get; set; }

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
