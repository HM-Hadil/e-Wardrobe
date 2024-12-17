namespace e_commercedotNet.Services

{
    public static class EmailService
    {
        public static void SendEmail(string to, string subject, string body)
        {
            // Simulation de l'envoi d'email
            Console.WriteLine($"Email envoyé à {to} :\nSujet : {subject}\n{body}");
        }
    }
}
