
namespace MvcNetCoreLinqToSqlInjection.Models;

public class Coche :ICoche
{
    public string Marca {get; set;}
    public string Modelo {get; set;}
    public string Imagen {get; set;}
    public int Velocidad {get; set;}
    public int VelocidadMaxima {get; set;}

    public Coche()
    {
        this.Marca="Cochecito";
        this.Modelo="Normal";
        this.Imagen="agente007.jpg";
        this.Velocidad = 0;
        this.VelocidadMaxima = 120;
    }

    public void Acelerar()
    {
        this.Velocidad+=20;
        if (Velocidad >= VelocidadMaxima)
        {
            Velocidad=VelocidadMaxima;
        }
    }

    public void Frenar()
    {
        Velocidad -= 10;
        if (Velocidad < 0)
        {
            Velocidad = 0;
        }
    }
}