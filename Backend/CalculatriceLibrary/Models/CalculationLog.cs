namespace CalculatriceLibrary.Models
{
    /// Représente un calcul effectué et enregistré en base de données.
    /// Chaque instance = une ligne dans la table CalculationLogs.
    public class CalculationLog
    {
        public int Id { get; set; }
        public string Expression { get; set; }
        public string Result { get; set; } // Keep as string to store "Error" messages too
        public DateTime CreatedAt { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }
    }
}