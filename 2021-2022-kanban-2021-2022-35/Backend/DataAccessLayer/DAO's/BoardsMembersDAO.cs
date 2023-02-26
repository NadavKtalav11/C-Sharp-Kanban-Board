using IntroSE.Kanban.Backend.DataAccessLayer.DTO_s;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DAO_s
{
    internal class BoardsMembersDAO : DAOAbstract
    {
     
        private const string FirstColumn = "BoardId"; // primary key
        private const string SecondColumn = "UserEmail";


        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BoardsMembersDAO() : base("BoardUsers") { }

        protected override BoardsMembersDTO convertReadertoObject(SQLiteDataReader reader)
        {
            BoardsMembersDTO converted = new BoardsMembersDTO((int)(long)reader.GetValue(0), (String)reader.GetValue(1));
            return converted;
        }

        public bool Insert(BoardsMembersDTO boardsMembersDTO)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = 1;
                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();
                    cmd.CommandText = $"INSERT INTO {tableName} ({FirstColumn}, {SecondColumn}) " + $"VALUES(@id, @email);";
                    SQLiteParameter IDParam = new SQLiteParameter(@"id", boardsMembersDTO.boardId);
                    SQLiteParameter emailParam = new SQLiteParameter(@"email", boardsMembersDTO.email);
                    

                    cmd.Parameters.Add(IDParam);
                    cmd.Parameters.Add(emailParam);
                    cmd.Prepare();

                    res = cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    string output = ex.Message;
                    log.Error(output);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }

                return res != -1;
            }
        }
        public bool Delete(int BoardId)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where {FirstColumn}={BoardId}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        public bool Delete( string email)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where {SecondColumn}='{email}';"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        public bool Delete(string email, int BoardID)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where {SecondColumn}='{email}' AND {FirstColumn}= '{BoardID}';"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
    }
}
