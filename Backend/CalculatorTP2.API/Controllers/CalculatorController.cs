using CalculatriceLibrary;
using CalculatriceLibrary.Data;
using CalculatriceLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorTP2.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly Calculator _calculator;
        private readonly AppDbContext _db;

        public CalculatorController(Calculator calculator, AppDbContext db)
        {
            _calculator = calculator;
            _db = db;
        }


        // Endpoint pour calculer une expression et sauvegarder le résultat en base de données
        [HttpPost("calculer")]
        public IActionResult Calculer([FromBody] CalculatorRequest request)
        {
            try
            {
                var resultat = _calculator.EvaluerExpression(request.Expression);

                // Link the calculation to the user who performed it
                _db.CalculationLogs.Add(new CalculationLog
                {
                    Expression = request.Expression,
                    Result = resultat.ToString(),
                    CreatedAt = DateTime.UtcNow,
                    UserId = request.UserId // CRITICAL: Link to the User table
                });
                _db.SaveChanges();

                return Ok(new { res = resultat });
            }
            catch (DivideByZeroException)
            {
                return Ok(new { res = "Error: Division by zero" });
            }
            catch (Exception)
            {
                return Ok(new { res = "Invalid Expression" });
            }
        }


        // Endpoint pour récupérer l'historique des calculs
        [HttpGet("historique")]
        public IActionResult GetHistorique()
        {
            var logs = _db.CalculationLogs.OrderByDescending(l => l.Id).ToList();
            return Ok(logs);
        }

        // Endpoint pour supprimer un calcul de l'historique par son Id
        [HttpGet("historique/{userId}")]
        public IActionResult GetHistorique(int userId)
        {
            // Only return logs belonging to this specific user
            var logs = _db.CalculationLogs
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.Id)
                .ToList();

            return Ok(logs);
        }
    }
}
