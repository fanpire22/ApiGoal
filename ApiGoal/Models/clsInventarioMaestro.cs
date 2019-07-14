using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGoal.Models
{
    /// <summary>
    /// Singleton que emula los datos que se encontrarían en Base de Datos
    /// </summary>
    public class clsInventarioMaestro
    {

        private readonly static clsInventarioMaestro _instance = new clsInventarioMaestro();
        private Dictionary<string, clsInventario> enUso;
        private List<clsInventario> eliminados;

        private clsInventarioMaestro()
        {
            enUso = new Dictionary<string, clsInventario>();
            eliminados = new List<clsInventario>();

            //Introducimos valores por defecto para tener datos con los que jugar:
            //- Cuatro productos en "En Uso" (dos caducados, dos vigentes)
            //- Un producto en el buffer de eliminados (para que podamos obtener información de él)
            enUso.Add("C00A09", new clsInventario("C00A09", new DateTime(2016, 3, 5), "Queso de Solán de Cabras", 10.5f));
            enUso.Add("C00A11", new clsInventario("C00A11", new DateTime(2018, 10, 2), "Agua de Solar de Cabrales", 100.5f));
            enUso.Add("C00A12", new clsInventario("C00A12", new DateTime(2020, 10, 2), "Agua de Solar de Cabrales", 100.5f));
            enUso.Add("C00A13", new clsInventario("C00A13", new DateTime(2030, 7, 14), "Gorka Kola, lata 33cl", 2.5f));
            eliminados.Add(new clsInventario("C00A10", new DateTime(2018, 10, 2), "Agua de Solar de Cabrales", 100.5f));
        }

        public static clsInventarioMaestro Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Función que añade un nuevo objeto al inventario, si no existía en la lista previamente
        /// </summary>
        /// <param name="objeto">Objeto a añadir</param>
        /// <returns></returns>
        public bool Add(clsInventario objeto)
        {
            if (!enUso.ContainsKey(objeto.Nombre))
            {
                //No existía, se añade
                enUso.Add(objeto.Nombre, objeto);
                return true;
            }
            else
            {
                //Ya existía ese nombre, por lo que devolvemos un falso
                return false;
            }
        }

        /// <summary>
        /// Función que elimina un objeto del inventario, y lo añade a la lista de notificación de eliminados
        /// </summary>
        /// <param name="nombre">Identificación del objeto a eliminar</param>
        /// <returns></returns>
        public bool Remove(string nombre)
        {
            if (enUso.ContainsKey(nombre))
            {
                eliminados.Add(enUso[nombre]);
                enUso.Remove(nombre);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Función que devuelve la lista de notificación de eliminados y la limpia (para evitar notificaciones redundantes)
        /// </summary>
        /// <returns></returns>
        public List<clsInventario> GetRemoved()
        {
            List<clsInventario> result = eliminados;
            eliminados = new List<clsInventario>();
            return result;
        }

        /// <summary>
        /// Función que devuelve qué objetos del inventario están obsoletos. Usamos un LinQ por simplicidad de código.
        /// </summary>
        /// <returns></returns>
        public List<clsInventario> GetObsolete()
        {
            return (from x in enUso.Values where x.Caducado select x).ToList();
        }
    }
}
