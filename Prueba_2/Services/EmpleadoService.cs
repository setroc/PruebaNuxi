using System.Text;
using Microsoft.EntityFrameworkCore;
using Prueba_2.Models;

namespace Prueba_2.Controllers;

public class EmpleadoService
{
    private readonly TestContext _testContext;
    public List<string> Errors { get; }
    public EmpleadoService(TestContext testContext) {
        _testContext = testContext;
        Errors = new List<string>();
    }

    public async Task<byte[]> ExportCsv() {
        string csv = "Login,Nombre completo,Sueldo,Fecha Ingreso\r\n";
        var empleados = await  _testContext.Empleados.Include(e => e.User).ToListAsync();
        foreach (var empleado in empleados) {
            var login = empleado.User.Login;
            var nombre = $"{empleado.User.Nombre} {empleado.User.Paterno} {empleado.User.Materno}";
            var sueldo = $"{empleado.Sueldo}";
            var fechaIngreso = empleado.FechaIngreso?.ToString("yyyy-MM-dd");
            csv += $"{login},{nombre},{sueldo},{fechaIngreso}\r\n";
        }
        return Encoding.UTF8.GetBytes(csv);
    }

    public async Task<List<EmpleadoDto>> GetTopTen(){
        var empleadosDto = new List<EmpleadoDto>();
        var empleados = await  _testContext.Empleados.Include(e => e.User).OrderByDescending(e => e.Sueldo).Take(10).ToListAsync();
        foreach(var empleado in empleados) {
            var empleadoDto = new EmpleadoDto();
            empleadoDto.userId = empleado.UserId;
            empleadoDto.Sueldo = (double)empleado.Sueldo;
            empleadoDto.Login = empleado.User.Login;
            empleadoDto.Nombre = empleado.User.Nombre;
            empleadoDto.Paterno = empleado.User.Paterno;
            empleadoDto.Materno = empleado.User.Materno;
            empleadoDto.FechaIngreso = (DateTime)empleado.FechaIngreso;

            empleadosDto.Add(empleadoDto);
        }

        return empleadosDto;
    }

    public async Task<EmpleadoDto?> UpdateSueldo(int userId, double sueldo) {
        var empleado = await _testContext.Empleados.FirstOrDefaultAsync(e => e.UserId == userId);
        if(empleado == null) return null;
        empleado.Sueldo = sueldo;
        await _testContext.SaveChangesAsync();
        empleado = await _testContext.Empleados.Include(e => e.User).FirstOrDefaultAsync(e => e.UserId == userId);
        return new EmpleadoDto {
            userId = empleado.UserId,
            Sueldo = (double)empleado.Sueldo,
            Login = empleado.User.Login,
            Nombre = empleado.User.Nombre,
            Paterno = empleado.User.Paterno,
            Materno = empleado.User.Materno,
            FechaIngreso = (DateTime)empleado.FechaIngreso
        };
    }

    public async Task<EmpleadoDto> Create(CreateEmpleadoDto empleadoDto) {
        var lastUserId = _testContext.Usuarios.OrderByDescending(u => u.UserId).Select(u => u.UserId).FirstOrDefault();
        var usuario = new Models.Usuario
        {
            UserId = lastUserId + 1,
            Login = empleadoDto.Login,
            Nombre = empleadoDto.Nombre,
            Paterno = empleadoDto.Paterno,
            Materno = empleadoDto.Materno
        };
        _testContext.Usuarios.Add(usuario);
        await _testContext.SaveChangesAsync();

        var empleado = new Models.Empleado
        {
            EmpleadoId = usuario.UserId,
            UserId = usuario.UserId,
            Sueldo = empleadoDto.Sueldo,
            FechaIngreso = DateTime.Today
        };
        _testContext.Empleados.Add(empleado);
        await _testContext.SaveChangesAsync();
        return new EmpleadoDto {
            userId = empleado.UserId,
            Sueldo = (double)empleado.Sueldo,
            Login = usuario.Login,
            Nombre = usuario.Nombre,
            Paterno = usuario.Paterno,
            Materno = usuario.Materno,
            FechaIngreso = (DateTime)empleado.FechaIngreso
        };
    }

    public bool Validate(CreateEmpleadoDto empleado) {
        bool flag  = true;
        if (empleado.Sueldo <= 0) {
            Errors.Add("El sueldo no puede ser cero o menor a cero.");
            flag = false;
        }
        if (empleado.Login.Trim() == "") {
            Errors.Add("El login es requerido");
            flag = false;
        }
        if (empleado.Nombre.Trim() == "") {
            Errors.Add("El nombre es requerido.");
            flag = false;
        }
        if (empleado.Paterno.Trim() == "") {
            Errors.Add("El apellido paterno es requerido.");
            flag = false;
        }
        if (empleado.Materno.Trim() == "") {
            Errors.Add("El apellido materno es requerido.");
            flag = false;
        }
        return flag;
    }   
}