using LibreriaDatos.Cliente;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace Web_ApiCliente.Controllers
{
    public class ProductosController : Controller
    {
        public ActionResult Index()
        {
            //CODIGO CONTROLADOR DE INVOCACION A API REST QUE DEVUELVE EL LISTADO GENERAL
            //===========================================
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:44343/");
            var respuesta = cliente.GetAsync("api/ApiProducts").Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var resultado = respuesta.Content.ReadAsStringAsync().Result;
                var listado = JsonConvert.DeserializeObject<List<Productos>>(resultado);
                return View(listado);
            }
            return View();
        }
        //METODO QUE DEVUELVE LA VISTA DE INSERTAR
        //========================================
        [HttpGet]
        public ActionResult Nuevo()
        {
            return View();
        }
        //CODIGO CONTROLADOR DE INVOCACION A API REST DE INSERTAR
        //===========================================
        [HttpPost]
        public ActionResult Nuevo(Productos producto)
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:44343/");
            var respuesta = cliente.PostAsync("api/ApiProducts", producto, new JsonMediaTypeFormatter()).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var resultado = respuesta.Content.ReadAsStringAsync().Result;
                var estado = JsonConvert.DeserializeObject<bool>(resultado);
                if (estado)
                {
                    return RedirectToAction("Index");
                }
                
            }
            return View();
        }
    }

}