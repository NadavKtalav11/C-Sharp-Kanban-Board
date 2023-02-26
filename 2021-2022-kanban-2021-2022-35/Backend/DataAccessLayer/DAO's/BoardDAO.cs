using IntroSE.Kanban.Backend.BussinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;


namespace IntroSE.Kanban.Backend.DataAccessLayer.DAOs
{
    public class BoardDAO : DAOAbstract
    {

        private const string FirstColumn = "id"; // primary key
        private const string SecondColumn = "name";
        private const string ThirdColumn = "owner";

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BoardDAO() : base("Boards") { }


        protected  override BoardDTO convertReadertoObject(SQLiteDataReader reader)
        {
            BoardDTO converted = new BoardDTO((int)(long)reader.GetValue(0), (String)reader.GetValue(1), (String)reader.GetValue(2));
            return converted;
        }

        public  bool Insert(BoardDTO boardDTO)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = 1;
                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();
                    cmd.CommandText = $"INSERT INTO {tableName} ({FirstColumn}, {SecondColumn}, {ThirdColumn}) " + $"VALUES(@id, @name, @owner);";
                    SQLiteParameter IDParam = new SQLiteParameter(@"id", boardDTO.id);
                    SQLiteParameter nameParam = new SQLiteParameter(@"name", boardDTO.name);
                    SQLiteParameter ownerParam = new SQLiteParameter(@"owner", boardDTO.owner);

                    cmd.Parameters.Add(IDParam);
                    cmd.Parameters.Add(nameParam);
                    cmd.Parameters.Add(ownerParam);
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
                    CommandText = $"delete from {tableName} where {FirstColumn}='{BoardId}'"
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
