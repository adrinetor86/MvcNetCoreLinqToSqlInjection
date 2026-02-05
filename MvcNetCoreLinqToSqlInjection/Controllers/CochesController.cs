using Microsoft.AspNetCore.Mvc;
using MvcNetCoreLinqToSqlInjection.Models;

namespace MvcNetCoreLinqToSqlInjection.Controllers;

public class CochesController : Controller
{
    private ICoche car;

    public CochesController(ICoche car)
    {
        
        this.car = car;
    }
    
    public IActionResult Index()
    {
        return View(car);
    }
    
    [HttpPost]
    public IActionResult Index(string accion)
    {
        if (accion.ToLower() == "acelerar")
        {
            car.Acelerar();
        }
        else
        {
            car.Frenar();
        }
        return View(car);
    }
    
    
}