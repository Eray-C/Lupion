using Lupion.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Lupion.Business.Services;

public class EmailService(ManagementDBContext managementDBContext)
{
    public async Task SendPasswordResetAsync(string email, string token)
    {
        var account = await managementDBContext.EmailAccounts
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.IsActive);

        if (account == null)
            throw new Exception("Aktif e-posta hesabı bulunamadı.");

        using var client = new SmtpClient(account.Host, account.Port)
        {
            Credentials = new NetworkCredential(account.UserName, account.Password),
            EnableSsl = account.EnableSsl
        };

        using var message = new MailMessage(account.From ?? account.UserName, email)
        {
            Subject = " - Şifre Sıfırlama",
            Body = $"Şifre sıfırlama kodunuz: {token}\n\nBu kod 10 dakika geçerlidir. Kodunuzu kimseyle paylaşmayın."
        };

        await client.SendMailAsync(message);
    }

}
