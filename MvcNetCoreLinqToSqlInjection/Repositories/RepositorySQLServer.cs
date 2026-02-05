using System.Data;
using Microsoft.Data.SqlClient;
using MvcNetCoreLinqToSqlInjection.Models;

namespace MvcNetCoreLinqToSqlInjection.Repositories;

public class RepositorySQLServer
{
    private SqlConnection cn;
    private SqlCommand com;
    private SqlDataReader reader;
    private DataTable tablaDoctor;

    public RepositorySQLServer()
    {
        string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
        cn=new SqlConnection(connectionString);
        com=new SqlCommand();
        
        com.Connection = cn;

        string sql = "SELECT * FROM DOCTOR";
        
        SqlDataAdapter ad=new SqlDataAdapter(sql, connectionString);
        tablaDoctor = new DataTable();
        ad.Fill(tablaDoctor);
    }


    public List<Doctor> GetDoctores()
    {
        var consulta= from datos in tablaDoctor.AsEnumerable()
                        select datos;

        List<Doctor> doctores = new List<Doctor>();


        foreach (var row in consulta)
        {
            Doctor doc= new Doctor
            {
                IdDoctor = row.Field<int>("DOCTOR_NO"),
                Apellido =  row.Field<string>("APELLIDO"),
                Especialidad = row.Field<string>("ESPECIALIDAD"),
                Salario =  row.Field<int>("SALARIO"),
                IdHospital =  row.Field<int>("HOSPITAL_COD"),
            };
            doctores.Add(doc);
        }
        return doctores;

    }

    public async Task CreateDoctorAsync(int idDoctor,string apellido,string especialidad,int salario,int idHospital)
    {
        string sql = "Insert into DOCTOR values(@hospital_cod,@idDoctor," +
                     "@apellido,@especialidad,@salario)";
        
        com.Parameters.AddWithValue("@hospital_cod", idHospital);
        com.Parameters.AddWithValue("@idDoctor", idDoctor);
        com.Parameters.AddWithValue("@apellido", apellido);
        com.Parameters.AddWithValue("@especialidad", especialidad);
        com.Parameters.AddWithValue("@salario", salario);

        com.CommandType = CommandType.Text;
        com.CommandText = sql;
        await cn.OpenAsync();
        
        await com.ExecuteNonQueryAsync();
        
        await cn.CloseAsync();
        com.Parameters.Clear();
    }
}