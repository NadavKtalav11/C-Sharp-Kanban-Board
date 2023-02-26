using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataAccessLayer.DAO;

using IntroSE.Kanban.Backend.DataAccessLayer.DAOs;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer.DAO_s;
using Fare;

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class UserController
    {
        //fields
        public Dictionary<string, User> users { get; private set; }
        private UserDAO userDAO;
        private BoardsMembersDAO boardsMembersDAO;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
  

        //constractor
        public UserController()
        {
            userDAO = new UserDAO();
            boardsMembersDAO = new BoardsMembersDAO();
            users = new Dictionary<string, User>();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }

        /// <summary>
        /// This method loads the data to the system. 
        /// <summary>
        public void loadData()
        {
            foreach (UserDTO userDTO in userDAO.Select())
            {
                users[userDTO.email.ToLower()] = new User(userDTO);
            }

        }

        /// <summary>
        /// This method deletes all the data from the system. 
        /// <summary>
        public void deleteData()
        {
            this.users.Clear();
        }


        /// <summary>
        /// This method ckecks if the password is valid. 
        /// </summary>
        /// <param name="password">The password that is being cheked.</param>
        /// <summary>
        public static string usersPasswordValid(string password)
        {
            log.Info("chekcs if" + password + "is valid");
            if (password == null)
            {
                log.Error("the password cannot be null");
                throw new Exception("the password cannot be null");
            }
            if (password.Length < 6 | password.Length > 20)
            {
                log.Error("the password needs to be of size 6-20 charcters");
                throw new Exception("the password needs to be of size 6-20 charcters");
            }

            bool lowerCase = false;
            bool upperCase = false;
            bool number = false;
            for (int charIndex = 0; charIndex < password.Length & (!lowerCase || !upperCase || !number); charIndex++)
            {
                if (password[charIndex] >= '0' && password[charIndex] <= '9')
                {
                    number = true;
                }
                if (password[charIndex] >= 'a' && password[charIndex] <= 'z')
                {
                    lowerCase = true;
                }
                if (password[charIndex] >= 'A' && password[charIndex] <= 'Z')
                {
                    upperCase = true;
                }
            }
            if (!lowerCase | !upperCase | !number)
            {
                log.Error("the password must contain at least one lower, one upper case letter and one number ");
                throw new Exception("the password must contain at least one lower, one upper case letter and one number ");
            }

            log.Info(password + "is valid");
            return "";
        }

        /// <summary>
        /// This method if the email is availble. 
        /// </summary>
        /// <param name="email">The email name we check.</param>
        /// <summary>
        public bool usersAvailability(string email)
        {
            log.Warn("chekcs if" + email + "is available");
            User user = users.GetValueOrDefault(email.ToLower());
            if (user == null)
            {
                log.Debug(email + "is available");
                return true;

            }

            
            return false;
        }

        /// <summary>
        /// This method checks if the email is legal. 
        /// </summary>
        /// <param name="email">The email we check.</param>
        /// <summary>
        private bool ValidateLegalEmail(string email)
        {
            const string emailPattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";

            log.Warn("chekcs if" + email + "is legal");
            if (email != null && Regex.IsMatch(email, emailPattern))
            {
                log.Debug(email + "is legal");
                return true;
            }
            log.Error(email + "isn't legal");
            return false;
        }

        /// <summary>
        /// This method adds new user to the system. 
        /// </summary>
        /// <param name="email">The user's email .</param>
        /// <param name="password">The user's password .</param>
        /// <summary>
        public void userRegister(string email, string password)
        {
            if (email == null)
            {
                throw new Exception("Email is null");
            }
            if (password == null)
            {
                throw new Exception("password is null");
            }
            string regularEmail = email;
            email = email.ToLower();
            log.Warn("user register for: " + email);
            if (!ValidateLegalEmail(email))
            {
                log.Error(regularEmail + "is illegal");
                throw new Exception($"email is illegal");
            }
            if (!usersAvailability(email))
            {
                log.Error(regularEmail + "is not available");
                throw new Exception("Error: this email is already assigned to a user, please try to reset password");
            }

            if (!usersPasswordValid(password).Equals("")) //return "" if its valid
            {
                log.Error($"Register() failed:" + password + "is illegal");
                throw new Exception("not valid password");
            }
            User newUser = new User(regularEmail, password);
            log.Debug(regularEmail + " was registered successfully");
            users.Add(email, newUser);
        }


        /// <summary>
        /// This method removes a user from the system. 
        /// </summary>
        /// <param name="email">The user's email .</param>
        /// <summary>
        public bool userRemove(string email)
        {
            if (email == null)
            {
                throw new Exception("email cant be null");
            }
            log.Info("removing" + email);
            User user = users.GetValueOrDefault(email.ToLower());
            if (user != null) {
                if (user. loggedIn== false)
                {
                    throw new Exception("please log in first");
                }
                users.Remove(email.ToLower());
                bool successUser = false;
                successUser=  userDAO.Delete(email);
                if (!successUser)
                {
                    throw new Exception("the user delete has failed");
                }
                log.Debug(email + "was removed successfully");
                return true;
            }

            throw new Exception("this email dosent registerd");
        }


        /// <summary>
        /// This method return a list with all the users that are in the system. 
        /// <summary>
        public List<User> getUsersList()

        {
            List<User> usersList = new List<User>();
            foreach (User user in users.Values)
            {
                usersList.Add(user);
            }
            log.Warn("get the list of all the users");
            return usersList;
        }

        /// <summary>
        /// This method login user. 
        /// </summary>
        /// <param name="email">The user's email .</param>
        /// <param name="password">The user's password .</param>
        /// <summary>
        public string login(string email, string password)
        {
            log.Warn("logging for" + email);
            bool loggedIn = false;
            User user = users.GetValueOrDefault(email.ToLower());
            if (user != null)
            {
                loggedIn = user.logIn(password);
                if (loggedIn)
                {
                    log.Debug(email + "logged in successfully");
                    return email;
                }
            }

            log.Error(email + "isn't registered");
            throw new Exception("user is not registerd");

        }

        /// <summary>
        /// This method logs out user. 
        /// </summary>
        /// <param name="email">The user's email .</param>
        /// <summary>
        public string logOut(string email)
        {
            email = email.ToLower();
            log.Error(email + "log out");
            bool loggedOut = false;
            User user = users.GetValueOrDefault(email);
            if (user != null) {
                if (user.loggedIn) {
                    loggedOut = user.logOut();
                    if (loggedOut)
                    {
                        log.Debug(email + "logged out successfully");
                        return "";
                    }
                }
                throw new Exception("you are alredy logged out");
            
            }
            throw new Exception("user dosent registerd");
        }

        

        /// <summary>
        /// This method return a user. 
        /// </summary>
        /// <param name="email">The user's email .</param>
        /// <summary>
        public User getUser(string email)
        {
            
            email = email.ToLower();
            log.Info("return the user -" + email);
            User user = users.GetValueOrDefault(email);
            if (user != null)
            {

                return user;
            }
            log.Error("user is not existed");
            throw new Exception("user is not existed");
        }


    }

}
