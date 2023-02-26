using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer.Interfaces
{
    internal interface userInterface
    {

        /*
    This method register the user.

    "email" - Email of the user. 
    "password" - password of the email

    returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
     */
        public string userRegister(string Email, string Password);

        /*
       This method login the user.

       "email" - Email of the user. 
       "password" - password of the email

       returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
       */


        public string userLogin(string Email, string Password);
        /*
      This method logout the user.

      returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
      */

        public string userLogout();

    }
}
