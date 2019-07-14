using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ApiGoal.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiGoal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clsControlValores : ControllerBase
    {

        public clsInventarioMaestro Inventario = clsInventarioMaestro.Instance;

        /// <summary>
        /// Obtenemos aquellos objetos que estén caducados, es decir, cuya fecha de caducidad sea inferior o igual al dia actual.
        /// </summary>
        /// <remarks>
        /// Se obtendrá una lista de objetos como el siguiente:
        ///     {
        ///        "nombre": C00A11,
        ///        "Caducidad": "02/10/2018",
        ///        "tipo": "Agua de Solar de Cabrales",
        ///        "pvp": 100.50
        ///     }
        /// </remarks>
        /// <response code="201">Se han obtenido los datos sin problemas, y se ha limpiado la lista</response>
        /// <response code="412">Ha ocurrido un error (Excepción)</response>
        [HttpGet("Obsoletes")]
        [ProducesResponseType(201)]
        [ProducesResponseType(412)]
        public List<clsInventario> GetObsolete()
        {
            try
            {
                //Ha terminado bien, devolvemos un OK (código 201)
                this.HttpContext.Response.StatusCode = 201;
                return Inventario.GetObsolete();
            }
            catch (Exception)
            {
                //Hubo un error al añadir el objeto. Devolvemos el código que indica error (código 412)
                this.HttpContext.Response.StatusCode = 412;
                return null;
            }
        }

        /// <summary>
        /// Obtenemos aquellos objetos que hayan sido eliminados del inventariado. Una vez se han obtenido, se limpia el buffer de eliminados
        /// </summary>
        /// <remarks>
        /// Se obtendrá una lista de objetos como el siguiente:
        ///     {
        ///        "nombre": C00A10,
        ///        "Caducidad": "02/10/2018",
        ///        "tipo": "Agua de Solar de Cabrales",
        ///        "pvp": 100.50
        ///     }
        /// </remarks>
        /// <response code="201">Se han obtenido los datos correctamente</response>
        /// <response code="412">Ha ocurrido un error (Excepción)</response>
        [HttpGet("Eliminados")]
        [ProducesResponseType(201)]
        [ProducesResponseType(412)]
        public List<clsInventario> GetEliminados()
        {
            try
            {
                //Ha terminado bien, devolvemos un OK (código 201)
                this.HttpContext.Response.StatusCode = 201;
                return Inventario.GetRemoved();
            }
            catch (Exception)
            {
                //Hubo un error al añadir el objeto. Devolvemos el código que indica error (código 412)
                this.HttpContext.Response.StatusCode = 412;
                return null;
            }
        }

        /// <summary>
        /// Introducimos un objeto dentro del inventario
        /// </summary>
        /// <param name="nombre">Identificador único. No puede repetirse con uno ya existente</param>
        /// <param name="caducidad">Fecha de caducidad en formato DD/MM/AAAA</param>
        /// <param name="tipo">Tipo del objeto de inventario</param>
        /// <param name="pvp">Precio en formato String (decimales con puntos)</param>
        /// <remarks>
        /// Solicitud de ejemplo:
        ///
        ///     PUT /nombre
        ///     {
        ///        "nombre": C00A12,
        ///        "Caducidad": "02/10/2020",
        ///        "tipo": "Agua de Solar de Cabrales",
        ///        "pvp": 100.50
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Se ha generado correctamente el objeto y se ha integrado en el inventario</response>
        /// <response code="208">El objeto ya existía previamente en el inventario</response>
        /// <response code="412">Ha ocurrido un error: O bien el PVP no tenía el decimal correcto o bien la fecha no seguía el formato esperado</response>
        [HttpPut]
        [ProducesResponseType(201)]
        [ProducesResponseType(208)]
        [ProducesResponseType(412)]
        public void Put([Required]string nombre, [Required]string caducidad, string tipo, string pvp)
        {
            DateTime Caducidad;
            float PVP = 0;

            try
            {
                Caducidad = DateTime.ParseExact(caducidad, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(pvp))
                    PVP = float.Parse(pvp, CultureInfo.InvariantCulture);

                if (Inventario.Add(new clsInventario(nombre, Caducidad, tipo, PVP)))
                {
                    //Ha terminado bien, devolvemos un OK (código 201)
                    this.HttpContext.Response.StatusCode = 201;
                }
                else
                {
                    //Parece que ya existía un registro con ese nombre: Indicamos que ya estaba reportado (código 208)
                    this.HttpContext.Response.StatusCode = 208;
                }
            }
            catch (Exception)
            {
                //Hubo un error al añadir el objeto. Devolvemos el código que indica error (código 412)
                this.HttpContext.Response.StatusCode = 412;
            }

        }

        /// <summary>
        /// Eliminamos un objeto del inventario por nombre.
        /// </summary>
        /// <param name="nombre">Identificador único. No puede repetirse</param>
        /// <remarks>
        /// Solicitud de ejemplo:
        ///
        ///     DELETE /nombre
        ///     {
        ///        "nombre": C00A12
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Se ha eliminado correctamente el objeto</response>
        /// <response code="412">Ha ocurrido un error: El objeto no existía en el inventario</response>
        [HttpDelete]
        [ProducesResponseType(201)]
        [ProducesResponseType(412)]
        public void Delete([Required]string nombre)
        {
            if (Inventario.Remove(nombre))
            {
                //Ha terminado bien, devolvemos un OK (código 201)
                this.HttpContext.Response.StatusCode = 201;
            }
            else
            {
                //Parece que no existía un registro con ese nombre: Indicamos que no se ha podido completar (código 412)
                this.HttpContext.Response.StatusCode = 412;
            }
        }
    }
}
