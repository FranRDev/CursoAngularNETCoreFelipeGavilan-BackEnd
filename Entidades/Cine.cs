﻿using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades {

    public class Cine {

        public int ID { get; set; }
        [Required]
        [StringLength(maximumLength: 75)]
        public string Nombre { get; set; }
        public Point Ubicacion { get; set; }

        public List<PeliculaCine> Peliculas { get; set; }

    }

}