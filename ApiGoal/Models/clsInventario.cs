using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGoal.Models
{
    /// <summary>
    /// Objeto de inventario utilizado para introducir objetos en la lista global de inventariado
    /// </summary>
    public class clsInventario
    {
        private string _nombre;
        private DateTime _caducidad;
        private string _tipo;
        private float _pvp;

        public clsInventario()
        {
            _nombre = string.Empty;
            _caducidad = DateTime.Now;
            _tipo = string.Empty;
            _pvp = 0.0f;
        }

        public clsInventario(string _nombre, DateTime _caducidad, string _tipo, float _pvp)
        {
            this._nombre = _nombre;
            this._caducidad = _caducidad;
            this._tipo = _tipo;
            this._pvp = _pvp;
        }

        [Required]
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        [Required]
        public DateTime Caducidad
        {
            get { return _caducidad; }
            set { _caducidad = value; }
        }

        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        public float PVP
        {
            get { return _pvp; }
            set { _pvp = value; }
        }

        /// <summary>
        /// Con esta función determinamos si un elemento está caducado o no.
        /// </summary>
        public bool Caducado
        {
            get { return (_caducidad <= DateTime.Now); }
        }
    }
}
