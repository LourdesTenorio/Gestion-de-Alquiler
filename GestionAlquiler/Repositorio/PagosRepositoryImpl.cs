using GestionAlquiler.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionAlquiler.Repositorio
{
    public class PagosRepositoryImpl : IGenericRepository<Pagos>
    {
        private readonly string _cadenaSQL = "";
        public PagosRepositoryImpl(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSQL");
        }
        public Task<bool> Actualizar(Pagos entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Editar(Pagos modelo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Guardar(Pagos modelo)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Pagos>> Lista()
        {
            List<Pagos> _lista = new List<Pagos>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_ListarPagos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        _lista.Add(new Pagos
                        {
                            idPago = Convert.ToInt32(dr["idPago"]),
                            idAlquiler = new Alquiler()
                            {
                                idAlquiler = Convert.ToInt32(dr["idAlquiler"])
                            },
                            FechaPago = Convert.ToDateTime(dr["FechaPago"]),
                            MontoPagado = Convert.ToDecimal(dr["MontoPagado"]),
                            MetodoPago = dr["MetodoPago"].ToString(),
                            Estado = dr["Estado"].ToString()

                        });
                    }
                }
            }
            return _lista;
        }
    }
}
