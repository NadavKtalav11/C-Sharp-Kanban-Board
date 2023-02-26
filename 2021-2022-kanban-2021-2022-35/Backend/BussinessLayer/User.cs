using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;

using IntroSE.Kanban.Backend.DataAccessLayer.DAO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer.DAOs;

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class User
    {
        //fields
        public string email { get; private set; }
        public string password { get; private set; }
        public bool loggedIn { get; private set; }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private UserDTO userDTO;
        private UserDAO userDAO;

        //constractor
        public User(string email, string password)
        {
            this.email = email;
            this.password = password;
            loggedIn = false;
            userDAO = new UserDAO();
            userDTO = new UserDTO(email, password);
            bool success = false;
            success = userDAO.Insert(userDTO);
            if (!success)
            {
                throw new Exception(" the insertion as failed");
            }
        }

        //constractor

        internal User(UserDTO userDTO)
        {
            this.email = userDTO.email;
            this.password = userDTO.password;
            loggedIn = false;

        }

        /// <summary>
        /// This method log in the user. 
        /// </summary>
        /// <param name="password">The user's password.</param>
        /// <summary>
        public bool logIn(string password)
        {
            log.Info("logIn - " + this.email);
            if (password.Equals(this.password))
            {
                if (loggedIn == true)
                {
                    log.Error("user already looged in");
                    throw new Exception("user already looged in");

                }
                else
                {
                    log.Debug("logged in successfully");
                    loggedIn = true;
                    return true;
                }
            }
            else
            {
                log.Error("wrong password, try again!");
                throw new Exception("wrong password, try again!");
            }
        }


        /// <summary>
        /// This method log out the user. 
        /// <summary>
        public bool logOut()
        {
            log.Info("loging out" + this.email);
            if (!loggedIn)
            {
                log.Error("cannot log out user that dosent loggedIn");
                throw new Exception("cannot log out user that dosent loggedIn");
            }
            loggedIn = false;
            log.Info(this.email + "was logged out successfully");
            return true;
        }

        /// <summary>
        /// This method checks if user and password are mtach. 
        /// </summary>
        /// <param name="password">The password.</param>
        /// <summary>
        public string userMatchPassword(string password)
        {

            log.Warn("checks if" + password + "matches" + this.email);
            if (password.Equals(this.password))
            {
                log.ErrorFormat(password + "does match" + this.email);
                return "";
            }
            else
            {
                log.ErrorFormat(password + " does not match" + this.email);
                throw new Exception("Error: the password doesn't match");
            }
        }

        /// <summary>
        /// This method edits the user's password. 
        /// </summary>
        /// <param name="oldPassword">The old password of the user.</param>
        /// <param name="newPassword">The new password for the user.</param>
        /// <summary>
        public string userEditPassword(string oldPassword, string newPassword)
        {
            log.Warn("edit the user's password");
            if (!loggedIn)
            {
                log.Error("can't edit password if the user ins't logged in");
                throw new Exception("can't edit password if the user ins't logged in");
            }
            log.Warn("cheks if" + oldPassword + "matches" + this.password);
            if (userMatchPassword(oldPassword).Equals(""))
            {
                log.Warn("cheks if" + newPassword + "is legall");
                if (UserController.usersPasswordValid(newPassword).Equals(""))
                {
                    if ((newPassword).Equals(password))
                    {
                        throw new Exception("Error: you can't change the password to your old password");
                    }
                    password = newPassword;
                    bool success = false;
                    success = userDAO.Update(userDTO, "password", newPassword);
                    if (!success)
                    {
                        throw new Exception("the update has failed");
                    }
                    return "";
                }
                return UserController.usersPasswordValid(newPassword);
            }
            return userMatchPassword(oldPassword);
        }
    }


}

