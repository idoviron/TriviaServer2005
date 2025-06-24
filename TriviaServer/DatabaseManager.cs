using Npgsql;

namespace TriviaServer
{
    public class DatabaseManager
    {
        private static DatabaseManager instance = null;
        private static readonly object padlock = new object();

        private string _connectionString = "Host=aws-0-eu-central-1.pooler.supabase.com;Database=postgres;Username=postgres.kixyfdqensgrtgvnuohp;Password=Tiltan1234!@#$;Port=5432;SSL Mode=Require;Trust Server Certificate=true";

        DatabaseManager()
        {
        }

        public static DatabaseManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DatabaseManager();
                    }
                    return instance;
                }
            }
        }


        public async Task<Question> GetQuestion(int id)
        {
            try
            {
                Question question = new Question();
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                // Query Players table
                string query = "SELECT * FROM \"public\".\"Questions\" WHERE id = " + id;
                using var command = new NpgsqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    question.QuestionText = reader.GetString(reader.GetOrdinal("question_text"));
                    question.Answer1Text = reader.GetString(reader.GetOrdinal("answer1_text"));
                    question.Answer2Text = reader.GetString(reader.GetOrdinal("answer2_text"));
                    question.Answer3Text = reader.GetString(reader.GetOrdinal("answer3_text"));
                    question.Answer4Text = reader.GetString(reader.GetOrdinal("answer4_text"));
                    question.CorrectAnswer = reader.GetInt16(reader.GetOrdinal("correct_answer"));
                }
                return question;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Question>> GetQuestions()
        {
            try
            {
                List<Question> questionList = new List<Question>();
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                // Query Players table
                string query = "SELECT * FROM \"public\".\"Questions\"";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Question question = new Question();
                    question.QuestionText = reader.GetString(reader.GetOrdinal("question_text"));
                    question.Answer1Text = reader.GetString(reader.GetOrdinal("answer1_text"));
                    question.Answer2Text = reader.GetString(reader.GetOrdinal("answer2_text"));
                    question.Answer3Text = reader.GetString(reader.GetOrdinal("answer3_text"));
                    question.Answer4Text = reader.GetString(reader.GetOrdinal("answer4_text"));
                    question.CorrectAnswer = reader.GetInt16(reader.GetOrdinal("correct_answer"));
                    questionList.Add(question);
                }
                return questionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
