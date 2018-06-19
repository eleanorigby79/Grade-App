using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace WindowsFormsApplication1
{
    class DBCommunation
    {
        static SqlConnection connection = null;
        static SqlCommand command;
        static SqlDataReader reader;
        static SqlDataAdapter adapter;
        static int subjectIndex, noOfSem=0,ects;
        static int sum = 0;
        static int sumH = 0;
        static int count = 0;
        static int ECTSum = 0;
        static bool entered = false;
        static bool enteredH = false;
        static List<String> combo = new List<String>();


        public static void insertData(string subjectName, int ocjena, DateTime date, int semestar)
        {
            string queryUpPred = "INSERT INTO upisanPredmet VALUES(0,@value1,@valueOcjena,@valueS,@valueDate)";
            string queryString = "SELECT predmet.sifPredmet from predmet where nazPredmet LIKE '%'+@subjName+'%'";
            string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";
            subjectName = subjectName.ToUpper();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@subjName", subjectName); 
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        subjectIndex = reader.GetInt32(0);
                    }
                    reader.Close();

                }

                using (command = new SqlCommand(queryUpPred, connection))
                {
                    command.Parameters.AddWithValue("@value1", subjectIndex);
                    command.Parameters.AddWithValue("@valueOcjena", ocjena);
                    command.Parameters.AddWithValue("@valueDate", date);
                    command.Parameters.AddWithValue("@valueS", semestar);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int checkExistence(string subjName)
        {
            string queryString = "SELECT Count(nazPredmet) FROM  dbo.predmet right join dbo.upisanpredmet on predmet.sifpredmet = upisanpredmet.sifpredmet WHERE nazPredmet LIKE '%'+@subjName+'%'";
            string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";
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

            return (decimal)sum / count;
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

        public static List<String> refreshComboBox()
        {
            combo.Clear();
            string queryString = "SELECT nazPredmet  FROM  dbo.predmet";
            string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(queryString, connection))
                {

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        combo.Add(reader.GetString(0).TrimEnd());
                    }
                    reader.Close();

                }
            }
            return combo;
        }

        public static void refreshComboBoxSemester(ref List<String> sem)
        {
            string queryString = "select distinct semestar as sem from upisanPredmet";
            string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(queryString, connection))
                {

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        sem.Add(Convert.ToString(reader.GetInt16(0)));
                    }
                    reader.Close();

                }
            }
        }

        public static void refreshComboBoxSubject(ref ComboBox comboBox)
        {
            string queryString = "select nazPredmet from predmet";
            string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(queryString, connection))
                {

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader.GetString(0));
                    }
                    reader.Close();

                }
            }
        }

        public static void printProperty(string subjName, ref int ECTS, ref int ocjena, ref DateTime datum)
        {
            string queryString = "SELECT ECTS,ocjena,datumP  FROM  dbo.upisanPredmet JOIN dbo.predmet ON dbo.upisanPredmet.sifPredmet=dbo.predmet.sifPredmet WHERE nazPredmet=@nazPr";
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
                        datum = reader.GetDateTime(2);
                    }
                    reader.Close();
                }
            }
        }

        public static DataTable getContent(string sem)
        {
            string query = "select predmet.sifPredmet as sifra, nazPredmet as predmet, ects,ocjena, datumP as datum from predmet join upisanpredmet on predmet.sifpredmet = upisanpredmet.sifpredmet where semestar=@sem";
            string connectionString = "Server=LOCALHOST;Database=STUD;User Id=sa;Password=smece;";
         
            DataTable table = new DataTable();
            sem = sem.Substring(0, 1);
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sem", Convert.ToInt16(sem)); 
                    using(adapter = new SqlDataAdapter(command))
                    {
                      
                        adapter.Fill(table);

                    }
                }
            }
            return table;
        }
    }
}


