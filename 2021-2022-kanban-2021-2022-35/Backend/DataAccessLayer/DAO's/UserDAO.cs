
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.BussinessLayer;

using log4net;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DAOs
{
    internal class UserDAO: DAOAbstract 
    {
        private const string FirstColumn = "email"; // primary key
        private const string SecondColumn = "password";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserDAO() : base("Users") { }

        protected  override UserDTO convertReadertoObject(SQLiteDataReader reader)
        {
            UserDTO converted = new UserDTO((string)reader.GetValue(0), (String)reader.GetValue(1));
            return converted;
        }
        
        public  bool Insert(UserDTO userDTO)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = -1;

                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();

                    cmd.CommandText = $"INSERT INTO {tableName} ({FirstColumn}, {SecondColumn}) " + $"VALUES(@email, @password);";
                    SQLiteParameter emailParam = new SQLiteParameter(@"email", userDTO.email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"password", userDTO.password);


                    cmd.Parameters.Add(emailParam);
                    cmd.Parameters.Add(passwordParam);
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


  


        public bool Delete(string email)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = -1;

                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();

                    cmd.CommandText = $"DELETE FROM {tableName} WHERE {FirstColumn}= @email;";
                    SQLiteParameter emailParam = new SQLiteParameter(@"email", email); 

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

                return res!= -1;

            }

        }

        public bool Update(UserDTO userDTO, string rellevantColumnName, Object newValue)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int res = -1;

                SQLiteCommand cmd = new SQLiteCommand(connection);
                try
                {
                    connection.Open();

                    cmd.CommandText = $"UPDATE {tableName} SET {rellevantColumnName}= '{newValue}' WHERE {FirstColumn}= @email; ";
                    SQLiteParameter emailParam = new SQLiteParameter(@"email", userDTO.email); ;


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
    }
}
