using log4net;
using System;
using System.Data.SQLite;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DAO
{
    internal class ColumnDAO : DAOAbstract
    {

        //private static readonly string TableName = "Columns";
        private const string FirstColumn = "key"; // primary key
        private const string SecondColumn = "name";
        private const string thirdColumn = "limit";
        private const string fourthColumn = "board";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ColumnDAO() : base("Columns") { }

        protected override ColumnDTO convertReadertoObject(SQLiteDataReader reader)
        {
            ColumnDTO converted;
            if (reader.GetValue(2) is DBNull)
            {
                converted = new ColumnDTO((int)(long)reader.GetValue(0),
                    (String)reader.GetValue(1), -1,
                    (int)(long)reader.GetValue(3));
            }
            else
            {
                converted = new ColumnDTO((int)(long)reader.GetValue(0),
                    (String)reader.GetValue(1), (int)(long)reader.GetValue(2),
                    (int)(long)reader.GetValue(3));
            }
            return converted;
        }

        public bool Insert(ColumnDTO columnDTO)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = -1;

                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();

                    cmd.CommandText = $"INSERT INTO {tableName} ({FirstColumn}, {SecondColumn}, {fourthColumn} ) " + $"VALUES(@key, @name,  @boardid);";
                    SQLiteParameter keyParam = new SQLiteParameter(@"key", columnDTO.index);
                    SQLiteParameter nameParam = new SQLiteParameter(@"name", columnDTO.name);
                    //SQLiteParameter limitParam = new SQLiteParameter(@"limit", null);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardid", columnDTO.boardId);

                    cmd.Parameters.Add(keyParam);
                    cmd.Parameters.Add(nameParam);
                    //cmd.Parameters.Add(limitParam);
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


        public ColumnDTO Select(int boardId, int index)
        {
            ColumnDTO results = null;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {tableName} WHERE {fourthColumn}={boardId} AND {FirstColumn}={index};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results = convertReadertoObject(dataReader);
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

        public bool Update(ColumnDTO columnDTO, string rellevantColumnName, int newValue)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = -1;

                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();

                    cmd.CommandText = $"UPDATE {tableName} SET '{rellevantColumnName}'={newValue} WHERE {FirstColumn}= {columnDTO.index} AND {fourthColumn}={columnDTO.boardId}; ";

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

        public bool Update(ColumnDTO columnDTO, string rellevantColumnName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = -1;

                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();

                    cmd.CommandText = $"UPDATE {tableName} SET {rellevantColumnName}= '{null}' WHERE {FirstColumn}= '{columnDTO.index}' AND {fourthColumn}='{columnDTO.boardId}'; ";

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
                    CommandText = $"delete from {tableName} where {fourthColumn}={BoardId}"
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
