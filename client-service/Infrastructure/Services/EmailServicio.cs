using client_service.Domain.Interfaces;
using client_service.Infrastructure.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace client_service.Infrastructure.Services {
    public class EmailServicio : IEmailService
    {
        private readonly EmailSettings emailSettings;
        private readonly ILogger<EmailServicio> logger;

        public EmailServicio(IOptions<EmailSettings> options, ILogger<EmailServicio> logger)
        {
            this.emailSettings = options.Value;
            this.logger = logger;
        }
        public async Task SendEmailAsync(string to, string subject, string body) {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(emailSettings.From));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            // Construir el cuerpo del mensaje
            var builder = new BodyBuilder {
                HtmlBody = body
            };
            message.Body = builder.ToMessageBody();


            // Enviar a través de MailKit
            using var client = new SmtpClient();
            try {
                var socketOption = emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;

                await client.ConnectAsync(
                    emailSettings.Host,
                    emailSettings.Puerto,
                    socketOption
                );



                if (!string.IsNullOrWhiteSpace(emailSettings.Usuario)) {
                    await client.AuthenticateAsync(
                        emailSettings.Usuario,
                        emailSettings.Contrasena
                    );
                }

                await client.SendAsync(message);
            }
            catch(Exception ex) {
                logger.LogError(ex, "Error al enviar el correo electrónico a {To}", to);
                throw new Exception("No se pudo enviar el correo electrónico", ex);
            }
            finally {
                await client.DisconnectAsync(true);
            }
        }
    }
}
