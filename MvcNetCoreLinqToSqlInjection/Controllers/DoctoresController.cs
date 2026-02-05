using Microsoft.AspNetCore.Mvc;
using MvcNetCoreLinqToSqlInjection.Models;
using MvcNetCoreLinqToSqlInjection.Repositories;

namespace MvcNetCoreLinqToSqlInjection.Controllers;

public class DoctoresController : Controller
{
    private RepositorySQLServer _repo;


    public DoctoresController(RepositorySQLServer repo)
    {
        _repo = repo;
    }


    public IActionResult Create()
    {
       return View(); 
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Doctor doc)
    { 
        
        await _repo.CreateDoctorAsync(doc.IdDoctor,doc.Apellido,doc.Especialidad,doc.Salario,doc.IdHospital);
        
        return RedirectToAction("Index");
    }
    
    public IActionResult Index()
    {
        List<Doctor> doctores = _repo.GetDoctores();
        
        return View(doctores);
    }
    
    
}