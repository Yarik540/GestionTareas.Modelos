﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTareas.Modelos.Auth
{
    public class Register
    {
        public string Nombre { get; set; } 
        public string Email { get; set; }
        public string Password { get; set; } 
        public string ConfirmPassword { get; set; } 

    }
}
