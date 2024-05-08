using GestionAlquiler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Security.Cryptography.Xml;

namespace GestionAlquiler.Controllers
{
    public class HabitacionController : Controller
    {
        private readonly string? _cadenaSQL = "";
        public HabitacionController(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSQL");
        }

        IEnumerable<Habitacion> habitaciones()
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
        // GET: HabitacionController
        public ActionResult ListadoHabitaciones()
        {
            var hab = habitaciones();
            return View(hab);
        }

        // GET: HabitacionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public String Inserccion(Habitacion modelo)
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("usp_AgregarHabitacion", conexion);
                    cmd.Parameters.AddWithValue("NumeroHabitacion", modelo.NumeroHabitacion);
                    cmd.Parameters.AddWithValue("Tipo", modelo.Tipo);
                    cmd.Parameters.AddWithValue("PrecioAlquiler", modelo.PrecioAlquiler);
                    cmd.Parameters.AddWithValue("Estado", modelo.Estado);
                    cmd.CommandType = CommandType.StoredProcedure;

                    int filas_afectadas = cmd.ExecuteNonQuery();

                    if (filas_afectadas > 0)
                        return ViewBag.mensaje = "Exitoso";
                    else
                        return ViewBag.mensaje = "Algo falló";
                }
            }
            catch(SqlException ex)
            {
                return ViewBag.mensajeError="El número de habitación ya existe en la base de datos";

            }

        }
        // GET: HabitacionController/Create
        public ActionResult Create()
        {
            return View(new Habitacion());
        }

        // POST: HabitacionController/Create
        [HttpPost]
        public ActionResult Create(Habitacion model)
        {
            Inserccion(model);
            return View(model);

        }
        Habitacion Buscar(int id)
        {
            Habitacion model;
            model = habitaciones().Where(i => i.idHabitacion == id).FirstOrDefault();
            
            return model;
        }
        public String Edicion(Habitacion modelo)
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("usp_ActualizarHabitacion", conexion);
                    cmd.Parameters.AddWithValue("@idHabitacion", modelo.idHabitacion);
                    cmd.Parameters.AddWithValue("@NumeroHabitacion", modelo.NumeroHabitacion);
                    cmd.Parameters.AddWithValue("@PrecioAlquiler", modelo.PrecioAlquiler);
                    cmd.Parameters.AddWithValue("@Tipo", modelo.Tipo);
                    cmd.Parameters.AddWithValue("@Estado", modelo.Estado);
                    cmd.CommandType = CommandType.StoredProcedure;

                    int filas_afectadas = cmd.ExecuteNonQuery();

                    if (filas_afectadas > 0)
                        return ViewBag.mensaje = "¡Habitacion actualizada!";
                    else
                        return ViewBag.mensaje = "Algo falló";
                }
            }
            catch (SqlException ex)
            {
                return ViewBag.mensajeError = "error: "+ex;
            }

        }
        public ActionResult Edit(int id)
        {
            Habitacion model = Buscar(id);
            ViewData["EstadoActual"] = model.Estado;

            ViewData["opcionesEstado"] = new List<string>() { "Disponible", "Ocupado", "En Mantenimiento" };

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Habitacion model)
        {
            Edicion(model);
            return View(model);
        }
        public bool EliminarPorId(int id)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_EliminarHabitacion", conexion);
                cmd.Parameters.AddWithValue("idHabitacion", id);
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
                TempData["Mensaje"] = "La habitación se eliminó correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "No se pudo eliminar la habitacion.";
            }

            return RedirectToAction("ListadoHabitaciones");
        }
    }
}
