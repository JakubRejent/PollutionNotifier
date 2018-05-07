using System;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace BotRun
{
    class MailSender
    {
        private string buildMessageString(Person per, Pollution pol, bool data, bool service)
        {
            StringBuilder message = new StringBuilder();
            string header = "Dzień dobry " + per.FirstName + "! <br />";
            message.Append(header).AppendLine().AppendLine();
            if (data == true)
            {
                message.Append("Przesyłam informację o dzisiejszym stanie powietrza w Twojej okolicy: <br /><br />").AppendLine().AppendLine();
                message.Append("Nazwa stacji pomiarowej: " + pol.Name + "<br />").AppendLine();
                message.Append("Data pomiaru: " + pol.Date + "<br /><br />").AppendLine();
                message.Append("<b>PM10: " + pol.Pm10 + "µg/m3 </b><br />").AppendLine();

                if (pol.Pm10 > 50)
                {
                    message.Append("Norma przekroczona! <font color=\"red\">" + pol.Pm10 * 100 / 50 + "%</font> normy<br />").AppendLine();
                }
                else
                {
                    message.Append("Norma nie przekroczona. <font color=\"teal\">" + pol.Pm10 * 100 / 50 + "%</font> normy<br />").AppendLine();
                }

                message.Append("<i>Norma średniego dobowego stężenia pyłu: 50 µg/m3</i><br /><br />");
                message.Append("<b>PM2,5: " + pol.Pm25 + "µg/m3 </b><br />").AppendLine();

                if (pol.Pm25 > 25)
                {
                    message.Append("Norma przekroczona! <font color=\"red\">" + pol.Pm25 * 100 / 25 + "%</font> normy<br />").AppendLine();
                }
                else
                {
                    message.Append("Norma nie przekroczona. <font color=\"teal\">" + pol.Pm25 * 100 / 25 + "</font>% normy<br />").AppendLine();
                }

                message.Append("<i>Norma średniego dobowego stężenia pyłu: 25 µg / m3</i><br /><br />");

                if(!String.IsNullOrWhiteSpace(pol.State))
                message.Append("Indeks jakości powietrza: " + pol.State + "<br /><br />").AppendLine().AppendLine();

                if (service == true)
                {
                    message.Append("<font color=\"gray\">Dostęp do danych został udostępniony przez firmę:<br />").AppendLine();
                    message.Append(" LookO2   http://www.looko2.com/ <br /><br /></font>").AppendLine().AppendLine();
                }
                else
                {
                    message.Append("<font color=\"gray\">Dostęp do danych został udostępniony przez firmę:<br />").AppendLine();
                    message.Append(" Airly   https://airly.eu/pl/ <br /><br /></font>").AppendLine().AppendLine();
                }

            }
            else
            {
                message.Append("Niestety nie udało się pobrać dzisiejszych danych z pobliskiej stacji.<br />").AppendLine();
                message.Append("Jeżeli problem będzie się powtarzał skontaktuj się z administratorem.<br /><br />").AppendLine().AppendLine(); ;
            }
            message.Append("<font color=\"gray\">Twórca systemu: Jakub Rejent</font><br />");

            return message.ToString();
        }

        public bool sendMail(Person per, Pollution pol, bool data, bool service)
        {
            bool notSend = true;
            using (SmtpClient client = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "[MAIL]",
                    Password = "[PASS]"
                };
                client.Credentials = credential;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                var message = new MailMessage();

                message.To.Add(new MailAddress(per.Mail));
                message.From = new MailAddress("[MAIL]");
                String sDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
                message.Subject = "Informacja o zanieczyszczeniu powietrza " + datevalue.Day.ToString() + "-" + datevalue.Month.ToString() + "-" + datevalue.Year.ToString();
                message.Body = buildMessageString(per, pol, data, service);
                message.IsBodyHtml = true;
                
                try
                {
                    client.Send(message);
                    Console.WriteLine("Wiadomość została wysłana.");
                }
                catch (SmtpFailedRecipientException ex)
                {
                    Console.WriteLine("Błąd wysyłania!");
                    notSend = false;
                }
            }
            return notSend;
        }

        public void sendDiagnostic(int members, string time, int looko2, int airly, int errors)
        {

            using (SmtpClient client = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "[MAIL]",
                    Password = "[PASS]"
                };
                client.Credentials = credential;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                var message = new MailMessage();

                message.To.Add(new MailAddress("[ADMIN_MAIL]"));

                message.From = new MailAddress("[MAIL]");
                String sDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
                message.Subject = "Raport z wysłania danych " + datevalue.Day.ToString() + "-" + datevalue.Month.ToString() + "-" + datevalue.Year.ToString();
                message.Body = "Witaj Jakub! <br />Wysyłam dane o rozesłaniu raportów! <br /> <font color=\"teal\">" + DateTime.Now + "</font><br /> <br />" +
                "Statystyki: <br />" + "Liczba użytkowników: " + members + " <br /> czas wykonania: " + time + " <br />looko2: " +
                looko2 + " <br />airly: " + airly+ "<br />Ilość błędów podczas wysyłania: "+errors;
                message.IsBodyHtml = true;

                try
                {
                    client.Send(message);
                    Console.WriteLine("-- Wiadomość diagnostyczna została wysłana.");
                }
                catch (SmtpFailedRecipientException ex)
                {
                    Console.WriteLine("-- Błąd wysyłania diagnostyki!");
                }

            }

        }
    }
}
