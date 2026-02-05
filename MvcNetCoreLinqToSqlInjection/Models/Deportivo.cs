namespace MvcNetCoreLinqToSqlInjection.Models;

public class Deportivo:ICoche
{
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Imagen { get; set; }
    public int Velocidad { get; set; }
    public int VelocidadMaxima { get; set; }
    
    public void Acelerar()
    {
        Velocidad+=45;
        if (Velocidad > VelocidadMaxima)
        {
            Velocidad = VelocidadMaxima;
        }
    }

    public void Frenar()
    {
        Velocidad-=20;
        if (Velocidad < 0)
        {
            Velocidad = 0;
        }
    }
    
    public Deportivo()
    {
        Marca = "Ferrari";
        Modelo = "El caro";
        Imagen = "alumno.png";
        Velocidad = 0;
        VelocidadMaxima = 320;
    }

}