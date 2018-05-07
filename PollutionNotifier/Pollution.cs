using System;

namespace BotRun
{
    class Pollution
    {
        private int stationId;
        private int pm25;
        private int pm10;
        private string name;
        private string state;
        private DateTime date;

        public Pollution(int stationId, int pm25, int pm10, string name, string state, DateTime date)
        {
            this.stationId = stationId;
            this.pm25 = pm25;
            this.pm10 = pm10;
            this.state = state;
            this.date = date;
        }

        public Pollution()
        {

        }

        public int StationId { get => stationId; set => stationId = value; }
        public int Pm25 { get => pm25; set => pm25 = value; }
        public int Pm10 { get => pm10; set => pm10 = value; }
        public string State { get => state; set => state = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Name { get => name; set => name = value; }
    }
}
