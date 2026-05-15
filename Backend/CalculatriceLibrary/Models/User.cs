using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatriceLibrary.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<CalculationLog> Calculations { get; set; }

    }
}
