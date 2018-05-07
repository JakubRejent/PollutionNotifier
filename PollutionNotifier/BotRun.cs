using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.SqlClient;

namespace BotRun
{
    public static class BotRun
    {
        [FunctionName("Bot")]
        public static void Run([TimerTrigger("0 0 6 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Bot function executed at: {DateTime.Now}");
            sendNotification(log);
        }

        private static void sendNotification(TraceWriter log)
        {

            List<Person> personList = new List<Person>();
            DatabaseConnection connect = new DatabaseConnection();
            GetPollution getter = new GetPollution();
            Task<string> result;
            MailSender send = new MailSender();
            DateTime start = DateTime.Now;
            bool look02Data;
            bool data;
            int members;
            int airly, looko2;
            bool notSend;
            int errors;

            airly = 0;
            looko2 = 0;
            errors = 0;

            connect.getPersonListFromDatabase(personList);
            members = personList.Count;

            for (int i = 0; i < personList.Count; i++)
            {
                data = true;
                look02Data = true;
                Pollution pol = new Pollution();
                log.Info($"Wysylam dane do: " + personList[i].FirstName + " " + personList[i].LastName + " " + personList[i].Mail);
                result = getter.get("http://api.looko2.com/?method=GPSGetClosestLooko&lat=" + personList[i].Lan + "&lon=" + personList[i].Lng + "&token=[TOKEN]");

                //airly
                if (String.IsNullOrWhiteSpace(result.Result) || !result.Result.Contains("PM"))
                {
                    look02Data = false;
                    result = getter.get("https://airapi.airly.eu/v1/nearestSensor/measurements?latitude=" + personList[i].Lan + "&longitude=" + personList[i].Lng + "&maxDistance=10000&apikey=[APIKEY]");
                    if (!String.IsNullOrWhiteSpace(result.Result) && result.Result.Contains("pm"))
                    {
                        JObject o = JObject.Parse(result.Result);

                        pol.Name = (string)o.SelectToken("name");
                        pol.Pm10 = (int)float.Parse(((string)o.SelectToken("pm10")), CultureInfo.InvariantCulture.NumberFormat);
                        pol.Pm25 = (int)float.Parse(((string)o.SelectToken("pm25")), CultureInfo.InvariantCulture.NumberFormat);
                        pol.State = (string)o.SelectToken("airQualityIndex");
                        pol.Date = DateTime.Now;
                        airly++;

                    }
                    else
                    {
                        log.Info("BRAK DANYCH DLA TEJ LOKACJI");
                        data = false;
                    }
                }
                else
                {
                    JObject o = JObject.Parse(result.Result);

                    pol.Name = (string)o.SelectToken("Name");
                    pol.Pm10 = Int32.Parse((string)o.SelectToken("PM10"));
                    pol.Pm25 = Int32.Parse((string)o.SelectToken("PM25"));
                    pol.State = (string)o.SelectToken("IJPString");
                    pol.Date = DateTime.Now;
                    looko2++;
                }
                
               notSend = send.sendMail(personList[i], pol, data, look02Data);

                if (notSend == false)
                {
                    errors++;
                }
            }
           send.sendDiagnostic(members, (DateTime.Now - start).ToString(), looko2, airly, errors);
        }
    }
}
