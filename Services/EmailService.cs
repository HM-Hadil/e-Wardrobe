using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    public static async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient("smtp.votredomaine.com")
            {
                Port = 587, // ou 465 selon votre fournisseur
                Credentials = new NetworkCredential("votre_email@domaine.com", "votre_mot_de_passe"),
                EnableSsl = true // Important pour la sécurité
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("votre_email@domaine.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Gérer l'erreur, par exemple logger ou afficher un message
            Console.WriteLine($"Erreur d'envoi d'email : {ex.Message}");
            throw; // Ou gérer différemment selon votre besoin
        }
    }
}