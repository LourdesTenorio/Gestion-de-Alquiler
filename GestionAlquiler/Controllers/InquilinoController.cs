using GestionAlquiler.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GestionAlquiler.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly string? _cadenaSQL = "";
        public InquilinoController(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSQL");
        }
        IEnumerable<Inquilino> inquilinos()
        {
            List<Inquilino> _lista = new List<Inquilino>();
            
            using (var conexion = new SqlConnection(_cadenaSQL))
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
                            nombre = dr["nombres"].ToString(),
                            dni = dr["dni"].ToString(),
                            telefono = dr["telefono"].ToString(),
                            email = dr["email"].ToString()

                        });
                    }
                }
            }
            return _lista;

        }
        public IActionResult Index()
        {
            var listado = inquilinos();
            return View(listado);
        }

        public String Inserccion(Inquilino modelo)
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("usp_AgregarInquilino", conexion);
                    cmd.Parameters.AddWithValue("nombre", modelo.nombre);
                    cmd.Parameters.AddWithValue("dni", modelo.dni);
                    cmd.Parameters.AddWithValue("telefono", modelo.telefono);
                    cmd.Parameters.AddWithValue("email", modelo.email);
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
                return ViewBag.mensajeError = "El DNI ingresado ya existe";
            }

        }
        Inquilino Buscar(int id)
        {
            Inquilino model;
            model = inquilinos().Where(i => i.idInquilino == id).FirstOrDefault();
            return model;
        }
        public String Edicion(Inquilino modelo)
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("usp_ActualizarInquilino", conexion);
                    cmd.Parameters.AddWithValue("@idInquilino", modelo.idInquilino); 
                    cmd.Parameters.AddWithValue("@nombre", modelo.nombre);
                    cmd.Parameters.AddWithValue("@dni", modelo.dni);
                    cmd.Parameters.AddWithValue("@telefono", modelo.telefono);
                    cmd.Parameters.AddWithValue("@email", modelo.email);
                    cmd.CommandType = CommandType.StoredProcedure;

                    int filas_afectadas = cmd.ExecuteNonQuery();

                    if (filas_afectadas > 0)
                        return ViewBag.mensaje = "¡Inquilino actualizado!";
                    else
                        return ViewBag.mensaje = "Algo falló";
                }
            }
            catch (SqlException ex)
            {
                return ViewBag.mensajeError = "El DNI ingresado ya existe";
            }

        }
        public ActionResult Create()
        {
            return View(new Inquilino());
        }
        [HttpPost]
        public ActionResult Create(Inquilino model)
        {
            Inserccion(model);
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            Inquilino model = Buscar(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Inquilino model)
        {
            Edicion(model);
            return View(model);
        }
        public bool EliminarPorId(int id)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("usp_EliminarInquilino", conexion);
                cmd.Parameters.AddWithValue("idInquilino", id);
                cmd.CommandType = CommandType.StoredProcedure;

                int filas_afectadas =  cmd.ExecuteNonQuery();

                if (filas_afectadas > 0)
                    return true;
                else
                    return false;
            }
        }
        [HttpPost]
        public  ActionResult Eliminar(int id)
        {
            bool eliminacionExitosa =   EliminarPorId(id); // Espera el resultado de EliminarPorId

            if (eliminacionExitosa)
            {
                TempData["Mensaje"] = "El inquilino se eliminó correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "No se pudo eliminar el inquilino.";
            }

            return RedirectToAction("Index");
        }
    }
}
