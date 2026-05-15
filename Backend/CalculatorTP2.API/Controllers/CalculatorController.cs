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

                _db.CalculationLogs.Add(new CalculationLog
                {
                    Expression = request.Expression,
                    Result = resultat.ToString(),
                    CreatedAt = DateTime.Now
                });
                _db.SaveChanges();

                return Ok(new { res = resultat });
            }
            catch (DivideByZeroException)
            {
                return Ok(new { res = "Error: Division by zero" });
            }
            catch (Exception ex)
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
        [HttpDelete("historique/{id}")]
        public IActionResult DeleteLog(int id)
        {
            var log = _db.CalculationLogs.Find(id);

            if (log == null)
            {
                return NotFound(new { message = "Calcul non trouvé" });
            }

            _db.CalculationLogs.Remove(log);
            _db.SaveChanges();

            return Ok(new { message = $"Le calcul avec l'Id {id} a été supprimé." });
        }
    }
}
