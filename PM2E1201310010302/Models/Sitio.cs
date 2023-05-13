using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM2E1201610010417.Models
{
    internal class Pagos
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string desc { get; set; }
        public string monto { get; set; }
        public DateTime fecha { get; set; }
        public byte[] save_image { get; set; }

        public override string ToString()
        {
            return this.desc + " | " + this.monto + " " + this.fecha + " " + save_image;
        }
    }
}
