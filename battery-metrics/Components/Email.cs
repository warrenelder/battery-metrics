using System.IO;
using System.Net;
using System.Net.Mail;
using CsvHelper;
namespace batterymetrics.Components
{
    public class Email
    {

        private static SmtpClient SmtpClient = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("ENTER GMAIL EMAIL", "ENTER GMAIL APP PASSWORD")
        };

        public static void SendEmail(string email)
        {
            var fromAddress = new MailAddress("noreplay@batterymetrics.net", "Battery Metrics");
            var toAddress = new MailAddress(email, "To Name");
            const string subject = "Battery Metrics";
            const string body = "Analysis of device data upload is attached as a csv file.";

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))    // using UTF-8 encoding by default
            using (var csvWriter = new CsvWriter(writer))
            using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
            {
                var _metrics = MetricFactory.MetricList;
                csvWriter.WriteRecords(_metrics);
                writer.Flush();
                stream.Position = 0;
                message.Attachments.Add(new Attachment(stream, "batterymetrics.csv", "text/csv"));
                SmtpClient.Send(message);
            }
        }
    }
}
