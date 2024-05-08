using GestionAlquiler.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionAlquiler.Repositorio
{
    public class AlquilerRepositoryImpl : IGenericRepository<Alquiler>
    {
        private readonly string _cadenaSQL = "";
        public AlquilerRepositoryImpl(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSQL");
        }
        public Task<bool> Actualizar(Alquiler entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Editar(Alquiler modelo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Guardar(Alquiler modelo)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Alquiler>> Lista()
        {
            List<Alquiler> _lista = new List<Alquiler>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_ListarAlquileres", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        _lista.Add(new Alquiler
                        {
                            idAlquiler = Convert.ToInt32(dr["idAlquiler"]),
                            refInquilino = new Inquilino()
                            {
                                nombre = dr["NombreInquilino"].ToString()
                            },
                            refHabitacion = new Habitacion()
                            {
                                NumeroHabitacion = dr["NombreHabitacion"].ToString()
                            },
                            FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                            Duracion = Convert.ToInt32(dr["Duracion"]),
                            MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                            EstadoPago = dr["EstadoPago"].ToString()

                        });
                    }
                }
            }
            return _lista;
        }
    }
}
