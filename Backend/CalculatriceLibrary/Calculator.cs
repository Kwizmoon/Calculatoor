using System.Text.RegularExpressions;

namespace CalculatriceLibrary
{
    /// Logique de calcul pour la calculatrice.
    /// Contient toutes les opérations mathématiques supportées.
    public class Calculator
    {
        // ── Opérations de base ────────────────────────────────────

        public double Add(double a, double b) => a + b;

        public double Subtract(double a, double b) => a - b;

        public double Multiply(double a, double b) => a * b;

        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Division par zéro interdite.");
            return a / b;
        }

        // Exposant 2 : retourne a au carré.
        public double Square(double a) => a * a;

        // Exposant N : retourne a à la puissance n.
        public double Power(double a, double n) => Math.Pow(a, n);

        // Racine carrée : retourne √a.
        public double SquareRoot(double a)
        {
            if (a < 0)
                throw new ArgumentException("Racine carrée d'un nombre négatif non supportée.");
            return Math.Sqrt(a);
        }

        // ── BONUS : Évaluation d'expressions longues avec précédence ─
        // Supporte : + - * / ^ sqrt() et parenthèses ( )
        // Exemples : "(3+4)*2"   "sqrt(16)+2^3"   "10/(2+3)"
        //
        // Algorithme = descente récursive :
        //   Priorité 1 (basse)   : + et -
        //   Priorité 2 (moyenne) : * et /
        //   Priorité 3 (haute)   : ^
        //   Priorité 4 (max)     : () et nombres simples

        public double EvaluerExpression(string expression)
        {
            // Normaliser : minuscules, espaces retirés
            expression = expression.Trim().ToLower().Replace(" ", "");

            // Résoudre les sqrt() en premier (remplace sqrt(x) par sa valeur)
            expression = Regex.Replace(expression, @"sqrt\(([^()]+)\)", match =>
            {
                double val = EvalParsed(match.Groups[1].Value);
                return Math.Sqrt(val).ToString("G17",
                    System.Globalization.CultureInfo.InvariantCulture);
            });

            return EvalParsed(expression);
        }

        // Évalue récursivement l'expression en respectant la précédence.
        private double EvalParsed(string expr)
        {
            expr = expr.Trim();

            // Enlever les parenthèses extérieures inutiles : "(3+4)" → "3+4"
            while (expr.StartsWith("(") && expr.EndsWith(")")
                   && ParentheseCorrespondante(expr, 0) == expr.Length - 1)
            {
                expr = expr[1..^1].Trim();
            }

            // Priorité 1 : + et - (de droite à gauche pour respecter l'associativité)
            int profondeur = 0;
            for (int i = expr.Length - 1; i >= 0; i--)
            {
                char c = expr[i];
                if (c == ')') profondeur++;
                else if (c == '(') profondeur--;
                else if (profondeur == 0 && i > 0 && (c == '+' || c == '-'))
                {
                    double gauche = EvalParsed(expr[..i]);
                    double droite = EvalParsed(expr[(i + 1)..]);
                    return c == '+' ? gauche + droite : gauche - droite;
                }
            }

            // Priorité 2 : * et /
            profondeur = 0;
            for (int i = expr.Length - 1; i >= 0; i--)
            {
                char c = expr[i];
                if (c == ')') profondeur++;
                else if (c == '(') profondeur--;
                else if (profondeur == 0 && i > 0 && (c == '*' || c == '/'))
                {
                    double gauche = EvalParsed(expr[..i]);
                    double droite = EvalParsed(expr[(i + 1)..]);
                    if (c == '/' && droite == 0)
                        throw new DivideByZeroException("Division par zéro dans l'expression.");
                    return c == '*' ? gauche * droite : gauche / droite;
                }
            }

            // Priorité 3 : ^ (de gauche à droite)
            profondeur = 0;
            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];
                if (c == '(') profondeur++;
                else if (c == ')') profondeur--;
                else if (profondeur == 0 && c == '^')
                {
                    double gauche = EvalParsed(expr[..i]);
                    double droite = EvalParsed(expr[(i + 1)..]);
                    return Math.Pow(gauche, droite);
                }
            }

            // Priorité 4 : nombre simple
            if (double.TryParse(expr,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double nombre))
            {
                return nombre;
            }

            throw new InvalidOperationException($"Expression invalide : '{expr}'");
        }

        // Trouve la position de la parenthèse fermante correspondant à la position donnée.
        private int ParentheseCorrespondante(string expr, int posOuvrante)
        {
            int profondeur = 0;
            for (int i = posOuvrante; i < expr.Length; i++)
            {
                if (expr[i] == '(') profondeur++;
                else if (expr[i] == ')') { profondeur--; if (profondeur == 0) return i; }
            }
            return -1;
        }
    }
}