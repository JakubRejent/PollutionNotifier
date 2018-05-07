using System;

namespace BotRun
{
    class Person
    {
        private string firstName;
        private string lastName;
        private string mail;
        private string city;
        private string province;
        private string street;
        private string lan, lng;
        Geocode geo = new Geocode();

        public Person(string firstName, string lastName, string mail, string city, string province, string street)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.mail = mail;
            this.city = city;
            this.province = province;
            this.street = street;
            lan = geo.getLan(city, province, street);
            lng = geo.getLng(city, province, street);
        }

        public Person()
        {

        }

        public string FirstName { get => this.firstName; set => this.firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Mail { get => mail; set => mail = value; }
        public string Street { get => street; set => street = value; }
        public string City { get => city; set => city = value; }
        public string Province { get => province; set => province = value; }
        public string Lan { get => lan; set => lan = value; }
        public string Lng { get => lng; set => lng = value; }
    }


}
