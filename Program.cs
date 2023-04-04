using System;
using System.IO;
using System.Data.SqlClient;


namespace CSV_to_SQL_ADONET
{
    class Program
    {
        static void Main(string[] args)
        {
            var lineNumber = 0;

            string connectionString = "Server=YourServer;Database=Test;User Id=YourID;Password=YourPWD;";
            string filePath = "example2.csv";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "INSERT INTO Users VALUES (@Id, @Name, @Gender, @Age, @Date, @Country)";
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar);
                    command.Parameters.Add("@Gender", System.Data.SqlDbType.NVarChar);
                    command.Parameters.Add("@Age", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@Date", System.Data.SqlDbType.Date);
                    command.Parameters.Add("@Country", System.Data.SqlDbType.NVarChar);

                    connection.Open();

                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (lineNumber > 0)
                            {
                                string[] values = line.Split(',');
                                command.Parameters["@Id"].Value = int.Parse(values[0]);
                                command.Parameters["@Name"].Value = values[1];
                                command.Parameters["@Gender"].Value = values[2];
                                command.Parameters["@Age"].Value = int.Parse(values[3]);
                                command.Parameters["@Date"].Value = DateTime.Parse(values[4]);
                                command.Parameters["@Country"].Value = values[5];
                                command.ExecuteNonQuery();
                            }
                            lineNumber++;
                        }
                    }
                    connection.Close();
                }
                Console.WriteLine("Rows inserted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.ReadLine();
        }
    }
}
