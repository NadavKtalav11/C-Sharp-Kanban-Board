using IntroSE.Kanban.Backend.BussinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DAO
{
    public class TaskDAO : DAOAbstract
    {
        private const string FirstColumn = "id"; // primary key
        private const string SecondColumn = "title";
        private const string thirdColumn = "description";
        private const string fourthColumn = "dueDate";
        private const string fifthColumn = "creationDate";
        private const string sixthColumn = "column";
        private const string seventhColumn = "assignee";
        private const string eigthColumn = "boardId";

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public TaskDAO() : base("Tasks") { }


        protected override TaskDTO convertReadertoObject(SQLiteDataReader reader)
        {
            TaskDTO converted;
            if (reader.GetValue(6) is DBNull)
            {
                converted = new TaskDTO((int)(long)reader.GetValue(0), (String)reader.GetValue(1),
                    (string)reader.GetValue(2), (string)reader.GetValue(3),
                    (string)reader.GetValue(4), (int)(long)reader.GetValue(5),
                    null, (int)(long)reader.GetValue(7));
            }
            else
            {
                converted = new TaskDTO((int)(long)reader.GetValue(0), (String)reader.GetValue(1),
                    (string)reader.GetValue(2), (string)reader.GetValue(3),
                    (string)reader.GetValue(4), (int)(long)reader.GetValue(5),
                    (string)reader.GetValue(6), (int)(long)reader.GetValue(7));
            }
            return converted;
        }

        protected int convertReadertoObjectMaxId(SQLiteDataReader reader)
        {
            int converted;
            if (reader.GetValue(0) is DBNull) {
                converted = 0;
            }
            else {

                converted = (int)(long)reader.GetValue(0);
            }
            return converted;
        }

        public bool Insert(TaskDTO taskDTO)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = -1;

                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();

                    cmd.CommandText = $"INSERT INTO {tableName} ({FirstColumn}, {SecondColumn}, {thirdColumn}," +
                        $"{fourthColumn} , {fifthColumn} , {sixthColumn} , {seventhColumn}, {eigthColumn}) " +
                        $"VALUES(@id, @title, @description, @dueDate, @creationDate, @column, @assignee, @boardId);";
                    SQLiteParameter idParam = new SQLiteParameter(@"id", taskDTO.id);
                    SQLiteParameter titleParam = new SQLiteParameter(@"title", taskDTO.title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"description", taskDTO.description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDate", taskDTO.dueDate);
                    SQLiteParameter creationDateParam = new SQLiteParameter(@"creationDate", taskDTO.creationDate);
                    SQLiteParameter columnParam = new SQLiteParameter(@"column", taskDTO.column);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assignee", taskDTO.assignee);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardId", taskDTO.boardId);



                    cmd.Parameters.Add(idParam);
                    cmd.Parameters.Add(titleParam);
                    cmd.Parameters.Add(descriptionParam);
                    cmd.Parameters.Add(dueDateParam);
                    cmd.Parameters.Add(creationDateParam);
                    cmd.Parameters.Add(columnParam);
                    cmd.Parameters.Add(assigneeParam);
                    cmd.Parameters.Add(boardIdParam);
                    
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


        public  bool Update(TaskDTO taskDTO, string rellevantColumnName, Object newValue)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = -1;

                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();

                    cmd.CommandText = $"UPDATE {tableName} SET {rellevantColumnName}= '{newValue}' WHERE ({FirstColumn}= @id AND {eigthColumn}='{taskDTO.boardId}'); ";
                    SQLiteParameter idParam = new SQLiteParameter(@"id", taskDTO.id); ;


                    cmd.Parameters.Add(idParam);
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
        public bool Update(int taskId, int boardID, string rellevantColumnName, int newValue)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = -1;

                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();

                    cmd.CommandText = $"UPDATE {tableName} SET {rellevantColumnName}= {newValue} WHERE {FirstColumn}= @id AND {eigthColumn}=@boardID; ";
                    SQLiteParameter idParam = new SQLiteParameter(@"id", taskId); 
                    SQLiteParameter boardidParam = new SQLiteParameter(@"boardID", boardID); 

                    
                    cmd.Parameters.Add(idParam);
                    cmd.Parameters.Add(boardidParam);
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


        public int SelectTasksNextId(int boardId)
        {

            int results = 0;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT MAX({FirstColumn}) FROM {tableName} WHERE {eigthColumn} ={boardId}; ";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results = convertReadertoObjectMaxId(dataReader);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        public List<TaskDTO> Select(int boardId, int index)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {tableName} WHERE {sixthColumn}={index} AND {eigthColumn}={boardId};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(convertReadertoObject(dataReader));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        public bool Delete(int BoardId)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where {eigthColumn}={BoardId}"
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


