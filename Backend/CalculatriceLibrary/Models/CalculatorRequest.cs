using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatriceLibrary.Models
{
    public class CalculatorRequest
    {
        public string Expression { get; set; }
        public int UserId { get; set; } // The ID of the logged-in user
    }
}
