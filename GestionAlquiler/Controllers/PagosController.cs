using GestionAlquiler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace GestionAlquiler.Controllers
{
    public class PagosController : Controller
    {
        private readonly ILogger<PagosController> _pagosController;

        private readonly string? _cadenaSQL = "";
        public PagosController(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSQL");
        }
        IEnumerable<Pagos> listadoPagos()
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
        // GET: PagosController
        public ActionResult ListadoPagos()
        {
            var listado = listadoPagos();
            return View(listado);
        }

        // GET: PagosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        IEnumerable<Alquiler> listadoAlquiler()
        {
            List<Alquiler> _lista = new List<Alquiler>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_ListaCortaAlquiler", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        _lista.Add(new Alquiler
                        {
                            idAlquiler = Convert.ToInt32(dr["idAlquiler"])                           
                        });
                    }
                }
            }
            return _lista;
        }
        public String Inserccion(Pagos modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_AgregarPago", conexion);
                cmd.Parameters.AddWithValue("idAlquiler", modelo.idAlquiler.idAlquiler);
                cmd.Parameters.AddWithValue("FechaPago", modelo.FechaPago);
                cmd.Parameters.AddWithValue("MontoPagado", modelo.MontoPagado);
                cmd.Parameters.AddWithValue("MetodoPago", modelo.MetodoPago);
                cmd.Parameters.AddWithValue("Estado", modelo.Estado);

                cmd.CommandType = CommandType.StoredProcedure;

                int filas_afectadas = cmd.ExecuteNonQuery();

                if (filas_afectadas > 0)
                    return ViewBag.mensaje = "Exitoso";
                else
                    return ViewBag.mensaje = "Algo falló";
            }
        }
        // GET: PagosController/Create
        public ActionResult Create()
        {
            ViewBag.alquiler = new SelectList(listadoAlquiler(), "idAlquiler", "idAlquiler", "");

            return View(new Pagos());
        }

        // POST: PagosController/Create
        [HttpPost]
        public ActionResult Create(Pagos model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.alquiler = new SelectList(listadoAlquiler(), "idAlquiler", "idAlquiler",  model.idAlquiler.idAlquiler);
            }
            Inserccion(model);
            ViewBag.alquiler = new SelectList(listadoAlquiler(), "idAlquiler", "idAlquiler", model.idAlquiler.idAlquiler);
            return View(model);
        }

        Pagos Buscar(int id)
        {
            Pagos model;
            model = listadoPagos().Where(i => i.idPago == id).FirstOrDefault();
            return model;
        }
        public String Edicion(Pagos modelo)
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("usp_ActualizarPago", conexion);
                    cmd.Parameters.AddWithValue("@idPago", modelo.idPago);
                    cmd.Parameters.AddWithValue("@idAlquiler", modelo.idAlquiler.idAlquiler);
                    cmd.Parameters.AddWithValue("@FechaPago", modelo.FechaPago);
                    cmd.Parameters.AddWithValue("@MontoPagado", modelo.MontoPagado);
                    cmd.Parameters.AddWithValue("@MetodoPago", modelo.MetodoPago);
                    cmd.Parameters.AddWithValue("@Estado", modelo.Estado);

                    cmd.CommandType = CommandType.StoredProcedure;

                    int filas_afectadas = cmd.ExecuteNonQuery();

                    if (filas_afectadas > 0)
                        return ViewBag.mensaje = "¡Pago actualizado!";
                    else
                        return ViewBag.mensaje = "Algo falló";
                }
            }
            catch (SqlException ex)
            {
                return ViewBag.mensajeError = "error" +ex;
            }

        }
        public ActionResult Edit(int id)
        {
            Pagos model = Buscar(id);
            ViewBag.alquiler = new SelectList(listadoAlquiler(), "idAlquiler", "idAlquiler", model.idAlquiler.idAlquiler);
            ViewData["opcionesEstado"] = new List<string>() { "Pagado", "Pendiente" };
            ViewData["opcionesPago"] = new List<string>() { "Efectivo", "Tarjeta de crédito","Transferencia" };

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Pagos model)
        {
            Edicion(model);
            ViewBag.alquiler = new SelectList(listadoAlquiler(), "idAlquiler", "idAlquiler", model.idAlquiler.idAlquiler);

            return View(model);
        }
        public bool EliminarPorId(int id)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_EliminarPago", conexion);
                cmd.Parameters.AddWithValue("idPago", id);
                cmd.CommandType = CommandType.StoredProcedure;

                int filas_afectadas = cmd.ExecuteNonQuery();

                if (filas_afectadas > 0)
                    return true;
                else
                    return false;
            }
        }
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            bool eliminacionExitosa = EliminarPorId(id); // Espera el resultado de EliminarPorId

            if (eliminacionExitosa)
            {
                TempData["Mensaje"] = "El alquiler se eliminó correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "No se pudo eliminar el alquiler.";
            }

            return RedirectToAction("ListadoPagos");
        }
    }
}

