using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    class DBCommunation
    {
        static SqlConnection connection=null;
        static SqlCommand command;
        static SqlDataReader reader;
        static int subjectIndex;
        static int sum=0;
        static int sumH = 0;
        static int count=0;
        static int ECTSum = 0;
        static bool entered = false;
        static bool enteredH = false;
        static List<Predmet> combo = new List<Predmet>();
        

        public static void insertData(string subjectName, int ECTS, int ocjena, int akGodina)
        {
            string queryPredmet = "INSERT INTO predmet VALUES(@value1,@value2,@value3)";
            string queryUpPred = "INSERT INTO upisanPredmet VALUES(0,@value1,@value4,@value5)";
            string queryString = "SELECT MAX(sifPredmet) AS max FROM  dbo.predmet";
            string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";
            subjectName = subjectName.ToUpper();
            using(connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(queryString, connection))
                {

                        reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            subjectIndex = reader.GetInt32(0);
                            subjectIndex++;
                        }
                        reader.Close();
                       
                }

                using (command = new SqlCommand(queryPredmet, connection))
                {

                    command.Parameters.AddWithValue("@value1", subjectIndex);
                    command.Parameters.AddWithValue("@value2", subjectName);
                    decimal d = (decimal)ECTS / 1;
                    command.Parameters.AddWithValue("@value3", d);
                    command.ExecuteNonQuery();
                }

                using (command = new SqlCommand(queryUpPred, connection))
                {
                    command.Parameters.AddWithValue("@value1", subjectIndex);
                    command.Parameters.AddWithValue("@value4", ocjena);
                    command.Parameters.AddWithValue("@value5", akGodina);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int checkExistence(string subjName)
        {
            string queryString = "SELECT Count(nazPredmet) FROM  dbo.predmet WHERE nazPredmet LIKE '%'+@subjName+'%'";
            string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";
            subjName = subjName.ToUpper();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@subjName", subjName);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetInt32(0) == 0)
                            return 1;
                        else
                            return 0;
                    }
                    reader.Close();

                }
            }
            //dodati nesto drugo u slucaju da je doslo do greske povezivanja s bazom
            return 0;
        }

        public static decimal calculate()
        {
            if (!entered)
            {
                entered = true;
                string queryString = "SELECT ocjena FROM  dbo.upisanPredmet";
                string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";

                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (command = new SqlCommand(queryString, connection))
                    {

                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            sum += reader.GetInt16(0);
                            count++;
                        }
                        reader.Close();

                    }
                }
            }
        
            return (decimal)sum/count;
        }

        public static decimal calculateHeavy()
        {
            if (!enteredH)
            {
                enteredH = true;
                string queryString = "SELECT ocjena, ECTS FROM  dbo.upisanPredmet JOIN dbo.predmet ON dbo.upisanPredmet.sifPredmet=dbo.predmet.sifPredmet";
                string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";

                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (command = new SqlCommand(queryString, connection))
                    {

                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            sumH += reader.GetInt16(0) * (int)reader.GetSqlDecimal(1);
                            ECTSum += (int)reader.GetSqlDecimal(1);
                        }
                        reader.Close();

                    }
                }
            }
            
            return (decimal)sumH / ECTSum;
        }
        
        public static List<Predmet> refreshComboBox()
        {
            combo.Clear();
            string queryString = "SELECT predmet.sifPredmet,nazPredmet,ECTS,ocjena,akGodina  FROM  dbo.upisanPredmet JOIN dbo.predmet ON dbo.upisanPredmet.sifPredmet=dbo.predmet.sifPredmet";
            string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(queryString, connection))
                {
                                               
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        combo.Add(new Predmet(reader.GetInt32(0), reader.GetString(1).TrimEnd(), Convert.ToInt32(reader.GetDecimal(2)), reader.GetInt16(3), reader.GetInt16(4)));
                    }
                    reader.Close();

                }
            }
            return combo;
        }

        public static void printProperty(string subjName, ref int ECTS, ref int ocjena, ref int akGodina)
        {
            string queryString = "SELECT ECTS,ocjena,akGodina  FROM  dbo.upisanPredmet JOIN dbo.predmet ON dbo.upisanPredmet.sifPredmet=dbo.predmet.sifPredmet WHERE nazPredmet=@nazPr";
            string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";
            List<Predmet> combo = new List<Predmet>();

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@nazPr", subjName);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        ECTS = Convert.ToInt32(reader.GetDecimal(0));
                        ocjena = reader.GetInt16(1);
                        akGodina = reader.GetInt16(2);
                    }
                    reader.Close();
                }
            }
        }
     }  
  }


