using Microsoft.AspNetCore.Mvc;

namespace Prueba_2.Controllers;

[ApiController]
[Route("[controller]")]
public class Empleado : ControllerBase
{
    private readonly EmpleadoService _service;
    public Empleado(EmpleadoService service) {
        _service = service;
    }

    //Listar top 10 usuarios
    [HttpGet("topten")]
    public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetTopTen(){
        return await _service.GetTopTen();
    }

    //Listar top 10 usuarios
    [HttpGet("exportCsv")]
    public async Task<FileResult> ExportCsv(){
        var bytes = await _service.ExportCsv();
        return File(bytes,"text/csv","empleados.csv");
    }

    // Actualizar salario de un empleado
    [HttpPut("{userId}")]
    public async Task<IActionResult> Update(int userId, UpdateSueldoEmpleadoDto uSueldoEmpleadoDto) 
    {
        Console.WriteLine(uSueldoEmpleadoDto.Sueldo);
        if (uSueldoEmpleadoDto.Sueldo  <= 0) return BadRequest("El sueldo no puede ser menor a cero.");
        var empleado = await _service.UpdateSueldo(userId, uSueldoEmpleadoDto.Sueldo);
        if (empleado == null) return BadRequest($"El usuario con id {userId} no existe.");
        return Ok(empleado);
    }

    // Agregar un nuevo usuario
    [HttpPost]
    public async Task<IActionResult> Create(CreateEmpleadoDto empleadoDto) {
        if (!_service.Validate(empleadoDto)) return BadRequest(_service.Errors);
        var usuario = await _service.Create(empleadoDto);
        return Ok(usuario);
    }
}