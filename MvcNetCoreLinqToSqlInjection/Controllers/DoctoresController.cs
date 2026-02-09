using Microsoft.AspNetCore.Mvc;
using MvcNetCoreLinqToSqlInjection.Models;
using MvcNetCoreLinqToSqlInjection.Repositories;

namespace MvcNetCoreLinqToSqlInjection.Controllers;

public class DoctoresController : Controller
{
    //private RepositorySQLServer _repo;
    // private RepositoryDoctoresOracle _repo;
    private IRepositoryDoctores _repo;

    public DoctoresController(IRepositoryDoctores repo)
    {
        _repo = repo;
    }


    public IActionResult Create()
    {
       return View(); 
    }

    public async Task<IActionResult> Delete(int iddoctor)
    {
        await _repo.DeleteDoctorAsync(iddoctor);

        return RedirectToAction("Index");
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

    
    public IActionResult Update(int idDoctor)
    {
        Doctor doctor = _repo.GetDoctor(idDoctor);
        
        return View(doctor);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(Doctor doc)
    {

        await _repo.UpdateDoctorAsync(doc.IdHospital ,doc.IdDoctor, doc.Apellido, doc.Especialidad, doc.Salario);
        return RedirectToAction("Index");
    }


    public IActionResult DoctoresEspecialidad()
    {
        List<Doctor> doctores = _repo.GetDoctores();
        return View(doctores);
    } 
    
    [HttpPost]
    public IActionResult DoctoresEspecialidad(string especialidad)
    {
        List<Doctor> doctores = _repo.GetDoctoresEspecialidad(especialidad);
        return View(doctores);
    }
    
    
}