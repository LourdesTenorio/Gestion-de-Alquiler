using GestionAlquiler.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionAlquiler.Repositorio
{
    public class InquilinoRepositoryImpl : IGenericRepository<Inquilino>
    {
        private readonly string _cadenaSQL = "";
        public InquilinoRepositoryImpl(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSQL");
        }
        public Task<bool> Actualizar(Inquilino entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Editar(Inquilino modelo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Guardar(Inquilino modelo)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Inquilino>> Lista()
        {
            List<Inquilino> _lista = new List<Inquilino>();

            /*using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_ListarInquilinos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        _lista.Add(new Inquilino
                        {
                            idInquilino = Convert.ToInt32(dr["idInquilino"]),
                            nombre = dr["nombre"].ToString(),
                            apellido = dr["apellido"].ToString(),
                            fechaNacimiento = Convert.ToDateTime(dr["fechaNacimiento"]),
                            telefono = dr["telefono"].ToString(),
                            email = dr["email"].ToString()

                        });
                    }
                }
            }*/
            return _lista;
        }
    }
}
