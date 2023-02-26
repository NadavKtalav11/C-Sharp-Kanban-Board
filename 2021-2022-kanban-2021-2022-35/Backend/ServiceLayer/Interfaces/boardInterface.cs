using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer.Interfaces
{
    internal interface boardInterface
    {
        /*
        This method adds a new board.

        "email" - Email of the user. The user must be logged in
        "name" - the new board name

        returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
         */
        public string boardAdd(string name);



        /*
        This method remove a board.

        "email" - Email of the user. The user must be logged in
        "Name" - The name of the board to remove

        returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
         */
        public string BoardRemove(string name);

        /*
        This method remove task from one column to another .

        "email" - Email of the user. The user must be logged in
        "boardName" - The name of the board
        "title" - Title of the task

        returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
         */
        public string taskMove(string title);

            /*
       This method return the column .

       "email" - Email of the user. 
       "boardName" - name of the board
       "index" - the index of the column in the board 

       returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
       */
        public string GetColumn( int index);


        public string getName();

    }
}
