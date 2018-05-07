using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace BotRun
{
    class DatabaseConnection
    {
       public void getPersonListFromDatabase(List<Person>  personList)
        {
        

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "[CONNECTION_STRING]";
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Users", conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Person getNextPerson = new Person();

                        getNextPerson.FirstName = reader.GetString(1);
                        getNextPerson.LastName = reader.GetString(2);
                        getNextPerson.Mail = reader.GetString(3);
                        getNextPerson.Province = reader.GetString(4);
                        getNextPerson.City = reader.GetString(5);
                        getNextPerson.Street = reader.GetString(6);
                        personList.Add(getNextPerson);
                    }
                }
            }
         
                   }
    }
}
