using System;
using System.Collections.Generic;

namespace Prueba_2.Models;

public partial class Empleado
{
    public int EmpleadoId { get; set; }

    public int UserId { get; set; }

    public double? Sueldo { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public virtual Usuario? User { get; set; }
}
