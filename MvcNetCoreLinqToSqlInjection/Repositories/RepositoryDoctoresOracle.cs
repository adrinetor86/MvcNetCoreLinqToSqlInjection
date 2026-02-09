using System.Data;
using MvcNetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;

namespace MvcNetCoreLinqToSqlInjection.Repositories;



#region STORED PROCEDURES

// CREATE OR REPLACE PROCEDURE SP_DELETE_DOCTOR
//     (p_iddoctor DOCTOR.DOCTOR_NO%type)
// AS
//     BEGIN
// DELETE FROM DOCTOR WHERE DOCTOR_NO=p_iddoctor;
// COMMIT;
// END;

// CREATE OR REPLACE PROCEDURE SP_UPDATE_DOCTOR
// (p_iddoctor DOCTOR.DOCTOR_NO%TYPE,
//     p_apellido DOCTOR.APELLIDO%TYPE,
//     p_especialidad DOCTOR.ESPECIALIDAD%TYPE,
//     p_salario DOCTOR.SALARIO%TYPE,
//     p_idhospital DOCTOR.HOSPITAL_COD%TYPE)
// AS
//     BEGIN
// UPDATE DOCTOR SET
//     APELLIDO=p_apellido,
//     ESPECIALIDAD=p_especialidad,
//     SALARIO=p_salario,
//     HOSPITAL_COD=p_idhospital
// WHERE DOCTOR_NO=p_iddoctor;
// COMMIT;
// END;

#endregion
public class RepositoryDoctoresOracle :IRepositoryDoctores
{
    
        private DataTable tablaDoctor;
        private OracleConnection cn;
        private OracleCommand com;
 
        public RepositoryDoctoresOracle()
        {
            //TODO CAMBIAR LA CONEXION POR LA DEL DOCKER
            string connectionString = @"Data Source=LOCALHOST:1521/XE;Persist Security Info=true;User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            this.tablaDoctor = new DataTable();
            string sql = "select * from DOCTOR";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            ad.Fill(this.tablaDoctor);
        }
 
        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctor.AsEnumerable()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor
                {
                    IdDoctor = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO"),
                    IdHospital = row.Field<int>("HOSPITAL_COD")
                };
                doctores.Add(doc);
            }
            return doctores;
        }
        
        public Doctor GetDoctor(int idDoctor)
        {
            var consulta= from datos in tablaDoctor.AsEnumerable()
                where datos.Field<int>("DOCTOR_NO")==idDoctor
                select datos;

            var row = consulta.First();

            Doctor doc = new Doctor
            {
                IdDoctor = row.Field<int>("DOCTOR_NO"),
                Apellido = row.Field<string>("APELLIDO"),
                Especialidad = row.Field<string>("ESPECIALIDAD"),
                Salario = row.Field<int>("SALARIO"),
                IdHospital = row.Field<int>("HOSPITAL_COD"),
            };

            return doc;

        }
 
        public async Task CreateDoctorAsync(int idDoctor, string apellido, string especialidad, int salario, int idHospital)
        {
            
            //ASI ES LA CONSULTA EN SQL
           // string sql = "insert into DOCTOR values (@idHospital,@id,@apellido, @especialidad, @salario ) ";
            
            //ASI EN ORACLE (se usan : en vez de @)
            string sql = "insert into DOCTOR values (:idHospital,:id,:apellido, :especialidad, :salario ) ";
            
            OracleParameter pamIdHospital = new OracleParameter(":idHospital", idHospital);
            OracleParameter pamIdDoctor = new OracleParameter(":id", idDoctor);
            OracleParameter pamApellido = new OracleParameter(":apellido", apellido);
            OracleParameter pamEspe= new OracleParameter(":especialidad", especialidad);
            OracleParameter pamSalario = new OracleParameter(":salario", salario);
            
            //EN ORACLE EL ORDEN ES IMPORTANTE
            com.Parameters.Add(pamIdHospital);
            com.Parameters.Add(pamIdDoctor);
            com.Parameters.Add(pamApellido);
            com.Parameters.Add(pamEspe);
            com.Parameters.Add(pamSalario);
            
            
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    
        public async Task DeleteDoctorAsync(int idDoctor)
        {
            string sql = "SP_DELETE_DOCTOR";
                
            OracleParameter pamId =
                new OracleParameter(":p_iddoctor", idDoctor);

            com.Parameters.Add(pamId);

            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = sql;
            
            await cn.OpenAsync();
            await com.ExecuteNonQueryAsync();
            await cn.CloseAsync();
            com.Parameters.Clear();

        }
    
        public async Task UpdateDoctorAsync(   
            int idHospital,
            int idDoctor,
            string apellido,
            string especialidad,
            int salario)
        {
            string sql = "SP_UPDATE_DOCTOR";

            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = sql;

            
            OracleParameter pamIdDoctor = new OracleParameter(":p_iddoctor", idDoctor);
            OracleParameter pamApellido = new OracleParameter(":p_apellido", apellido);
            OracleParameter pamEspe = new OracleParameter(":p_especialidad", especialidad);
            OracleParameter pamSalario = new OracleParameter(":p_salario", salario);
            OracleParameter pamIdHospital = new OracleParameter(":p_idhospital", idHospital);
            
            com.Parameters.Add(pamIdDoctor);
            com.Parameters.Add(pamApellido);
            com.Parameters.Add(pamEspe);
            com.Parameters.Add(pamSalario);
            com.Parameters.Add(pamIdHospital);
        
            await cn.OpenAsync();

            await com.ExecuteNonQueryAsync();

            await cn.CloseAsync();
            com.Parameters.Clear();
        }
        
        public List<Doctor> GetDoctoresEspecialidad(string especialidad)
        {
            var consulta = from datos in tablaDoctor.AsEnumerable()
                where (datos.Field<string>("ESPECIALIDAD")
                    .ToUpper()
                    .StartsWith(especialidad))
                select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor
                {
                    IdDoctor = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO"),
                    IdHospital = row.Field<int>("HOSPITAL_COD"),
                };
                doctores.Add(doc);
            }

            return doctores;
        }
}