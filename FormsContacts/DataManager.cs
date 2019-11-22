using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace Questionary
{
    public class DataManager
    {
        private static readonly string connectionString = @"Server=WFPDESK1157;Database=questionary;Trusted_Connection=True;";

        public static void Test()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                Debug.WriteLine($"Conexión establecida...");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Hubo un error: {e.Message}");
            }
        }

        public static List<QuestionaryModel> GetQuestionary()
        {
            List<QuestionaryModel> questi = new List<QuestionaryModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM questions_answers ORDER BY id DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            QuestionaryModel quest = new QuestionaryModel();
                            quest.Id = (int)reader["id"];
                            quest.Question = (string)reader["question"];
                            quest.Answer = (string)reader["answer"];
                            questi.Add(quest);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Hubo un error: {e.Message}");
                    }
                }
            }

            return questi;
        }

        public static int InsertOrUpdate(QuestionaryModel questy)
        {
            //int result = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO questions_answers(question, answer) VALUES(@question, @answer)";
               
                if (questy.Id > 0)
                {
                    query = "UPDATE questions_answers set question=@question, answer=@answer WHERE id=@id";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add(new SqlParameter("@id", questy.Id));
                    command.Parameters.Add(new SqlParameter("@question", questy.Question));
                    command.Parameters.Add(new SqlParameter("@answer", questy.Answer));


                    try
                    {
                        return command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Hubo un error: {e.Message}");
                    }
                }
            }
        }


        public static int Delete(int id)
        {
            //int result = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM questions_answers where id=@id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));

                    try
                    {
                        return command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Hubo un error: {e.Message}");
                    }
                }
            }
        }

    }
}
