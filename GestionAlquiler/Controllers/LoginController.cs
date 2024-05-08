using GestionAlquiler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;


namespace GestionAlquiler.Controllers
{
     
    public class LoginController : Controller
    {
        const string SesionUsuario = "_User";
        private readonly IConfiguration _config;
        public LoginController( IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Inicio()
        {
            return View(new UsuarioModel());
        }
        [HttpPost]
        public IActionResult Inicio(UsuarioModel reg)
        {
            try
            {
                if (string.IsNullOrEmpty(reg.usuario) || string.IsNullOrEmpty(reg.clave))
                {
                    ModelState.AddModelError("", "Ingresar los datos solicitados");
                }
                else
                {
                    string connectionString = _config["ConnectionStrings:cadenaSQL"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand com = new SqlCommand("usp_Login", connection);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@usuario", reg.usuario);
                        com.Parameters.AddWithValue("@clave", reg.clave);

                        SqlDataReader dr = com.ExecuteReader();
                        if (dr.Read())
                        {
                            HttpContext.Session.SetString(SesionUsuario, reg.usuario);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Datos ingresados no válidos");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo básico de la excepción
                ModelState.AddModelError("", "Error de conexión a la base de datos: " + ex.Message);
            }

            return View(reg);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Inicio", "Login"); 
        }


    }
}
