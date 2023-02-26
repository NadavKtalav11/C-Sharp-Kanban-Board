using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DAO;
using IntroSE.Kanban.Backend.DataAccessLayer.DAOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class UserDTO : DTOAbstract
    {
        //fields

        public const string userEmailColumnName = "email";
        public const string userPasswordColumnName = "password";


        private string _email;
        public string email { get => _email; set { _email = value; controller.Update(email, userEmailColumnName, value); } }
        private string _password;
        public string password { get => _password; set { _password = value; controller.Update(email, userPasswordColumnName, value); } }



        //constractor
        public UserDTO(string email, string password) : base(new UserDAO())
        {
        this._email = email;
        this._password = password;
        }


    }
}

