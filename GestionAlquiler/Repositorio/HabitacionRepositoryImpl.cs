using GestionAlquiler.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionAlquiler.Repositorio
{
    public class HabitacionRepositoryImpl : IGenericRepository<Habitacion>
    {
        private readonly string _cadenaSQL = "";
        public HabitacionRepositoryImpl(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSQL");
        }
        public Task<bool> Actualizar(Habitacion entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Editar(Habitacion modelo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Guardar(Habitacion modelo)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Habitacion>> Lista()
        {
            List<Habitacion> _lista = new List<Habitacion>();

            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_ListarHabitaciones", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        _lista.Add(new Habitacion
                        {
                            idHabitacion = Convert.ToInt32(dr["idHabitacion"]),
                            NumeroHabitacion = dr["NumeroHabitacion"].ToString(),
                            Tipo = dr["Tipo"].ToString(),
                            PrecioAlquiler = Convert.ToDecimal(dr["PrecioAlquiler"]),
                            Estado = dr["Estado"].ToString()

                        });
                    }
                }
            }
            return _lista;
        }
    }
}
