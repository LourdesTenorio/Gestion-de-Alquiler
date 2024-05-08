using GestionAlquiler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace GestionAlquiler.Controllers
{
    public class AlquilerController : Controller
    {
        private readonly string? _cadenaSQL = "";
        public AlquilerController(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSQL");
        }
        IEnumerable<Alquiler> ListadoAlquileres()
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


        // GET: AlquilerController
        public ActionResult Listado()
        {
            var lista = ListadoAlquileres();
            return View(lista);
        }

        // GET: AlquilerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        IEnumerable<Inquilino> listadoInquilino()
        {
            List<Inquilino> _lista = new List<Inquilino>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_ListaCortaInquilinos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        _lista.Add(new Inquilino
                        {

                            idInquilino = Convert.ToInt32(dr["idInquilino"]),
                            nombre = dr["nombres"].ToString()

                        });
                    }
                }
            }
            return _lista;
        }
        IEnumerable<Habitacion> listadoHabitacion()
        {
            List<Habitacion> _lista = new List<Habitacion>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_ListaCortaHabitacion", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        _lista.Add(new Habitacion
                        {

                            idHabitacion = Convert.ToInt32(dr["idHabitacion"]),
                            NumeroHabitacion = dr["NumeroHabitacion"].ToString()

                        });
                    }
                }
            }
            return _lista;
        }
        public String Inserccion(Alquiler modelo)
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_CrearAlquilerPago", conexion);
                    cmd.Parameters.AddWithValue("idInquilino", modelo.refInquilino.idInquilino);
                    cmd.Parameters.AddWithValue("idHabitacion", modelo.refHabitacion.idHabitacion);
                    cmd.Parameters.AddWithValue("FechaInicio", modelo.FechaInicio);
                    cmd.Parameters.AddWithValue("Duracion", modelo.Duracion);

                    cmd.Parameters.AddWithValue("MontoTotal", modelo.MontoTotal);

                    cmd.CommandType = CommandType.StoredProcedure;
                    int filas_afectadas = cmd.ExecuteNonQuery();

                    if (filas_afectadas > 0)
                        return ViewBag.mensaje = "¡Alquiler registrado!";
                    else
                        return ViewBag.mensaje = "¡Alquiler registrado!";

                }
            }catch(Exception ex)
            {
                return ViewBag.mensaje.Error = "Error: " + ex;
            }
        }
        // GET: AlquilerController/Create
        public ActionResult Create()
        {
            ViewBag.inquilino = new SelectList(listadoInquilino(), "idInquilino", "nombre", "");
            ViewBag.habitacion = new SelectList(listadoHabitacion(), "idHabitacion", "NumeroHabitacion", "");

            return View();
        }

        // POST: AlquilerController/Create
        [HttpPost]
        public ActionResult Create(Alquiler modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.inquilino = new SelectList(listadoInquilino(), "idInquilino", "nombre", modelo.refInquilino.idInquilino);
                ViewBag.habitacion = new SelectList(listadoHabitacion(), "idHabitacion", "NumeroHabitacion", modelo.refHabitacion.idHabitacion);

            }
            Inserccion(modelo);
            ViewBag.inquilino = new SelectList(listadoInquilino(), "idInquilino", "nombre", modelo.refInquilino.idInquilino);
            ViewBag.habitacion = new SelectList(listadoHabitacion(), "idHabitacion", "NumeroHabitacion", modelo.refHabitacion.idHabitacion);

            return View(modelo);
        }

        Alquiler Buscar(int id)
        {
            Alquiler model;
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_BuscarAlquilerPorId", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idAlquiler", id);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new Alquiler
                        {
                            idAlquiler = Convert.ToInt32(dr["idAlquiler"]),
                            refInquilino = new Inquilino()
                            {
                                idInquilino = Convert.ToInt32(dr["idInquilino"]),
                                nombre = dr["NombreInquilino"].ToString()
                            },
                            refHabitacion = new Habitacion()
                            {
                                idHabitacion = Convert.ToInt32(dr["idHabitacion"]),
                                NumeroHabitacion = dr["NombreHabitacion"].ToString()
                            },
                            FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                            Duracion = Convert.ToInt32(dr["Duracion"]),
                            MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                            EstadoPago = dr["EstadoPago"].ToString()
                        };
                    }
                    else
                    {
                        // Manejar el caso cuando no se encuentra el alquiler con el ID dado
                        model = null;
                    }
                }
            }
            return model;
        }

        public String Edicion(Alquiler modelo)
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("usp_ActualizarAlquiler", conexion);
                    cmd.Parameters.AddWithValue("@idAlquiler", modelo.idAlquiler);
                    cmd.Parameters.AddWithValue("@idInquilino", modelo.refInquilino.idInquilino);
                    cmd.Parameters.AddWithValue("@idHabitacion", modelo.refHabitacion.idHabitacion);
                    cmd.Parameters.AddWithValue("@FechaInicio", modelo.FechaInicio);
                    cmd.Parameters.AddWithValue("@Duracion", modelo.Duracion);
                    cmd.Parameters.AddWithValue("@MontoTotal", modelo.MontoTotal);
                    cmd.Parameters.AddWithValue("@EstadoPago", modelo.EstadoPago);

                    cmd.CommandType = CommandType.StoredProcedure;

                    int filas_afectadas = cmd.ExecuteNonQuery();

                    if (filas_afectadas > 0)
                        return ViewBag.mensaje = "¡Alquiler actualizado!";
                    else
                        return ViewBag.mensaje = "Algo falló";
                }
            }
            catch (SqlException ex)
            {
                return ViewBag.mensajeError = "error "+ex;
            }

        }
        public ActionResult Edit(int id)
        {
            Alquiler model = Buscar(id);
            ViewBag.inquilino = new SelectList(listadoInquilino(), "idInquilino", "nombre", model.refInquilino.idInquilino);
            ViewBag.habitacion = new SelectList(listadoHabitacion(), "idHabitacion", "NumeroHabitacion", model.refHabitacion.idHabitacion);
            ViewData["opcionesEstado"] = new List<string>() { "Pagado", "Pendiente" };
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Alquiler model)
        {
            Edicion(model);
            ViewBag.inquilino = new SelectList(listadoInquilino(), "idInquilino", "nombre", model.refInquilino.idInquilino);
            ViewBag.habitacion = new SelectList(listadoHabitacion(), "idHabitacion", "NumeroHabitacion", model.refHabitacion.idHabitacion);
            return View(model);
        }
        public bool EliminarPorId(int id)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_EliminarAlquiler", conexion);
                cmd.Parameters.AddWithValue("idAlquiler", id);
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

            return RedirectToAction("Listado");
        }
        [HttpGet]
        public IActionResult ObtenerPrecioHabitacion(int idHabitacion)
        {
            decimal precioHabitacion = 0;

            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("usp_ObtenerPrecioHabitacionPorId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idHabitacion", idHabitacion);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            precioHabitacion = Convert.ToDecimal(dr["PrecioAlquiler"]);
                        }
                    }
                }

                return Json(precioHabitacion); // Devolver el precio como respuesta JSON
            }
            catch (Exception ex)
            {
                // Manejar el error
                return BadRequest("Error al obtener el precio de la habitación: " + ex.Message);
            }
        }
    }
}

