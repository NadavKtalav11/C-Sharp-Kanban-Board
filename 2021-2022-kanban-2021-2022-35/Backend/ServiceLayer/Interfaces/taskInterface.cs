using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer.Interfaces
{
    internal interface taskInterface
    {
        /*
        This method adds a new task.

        "email" - Email of the user. The user must be logged in
        "boardName" - The name of the board
        "title" - Title of the new task
        "description" - Description of the new task
        "dueDate" - The due date if the new task

        returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
         */

        public string taskEditDueDate( DateTime newDueDate);


        /*
        This method updates task title.

        "email" - Email of the user. The user must be logged in
        "boardName" - The name of the board
        "title" - The old title of the task (the task is identified by board-user-title uniqe combnation)
        "newTitle" - New title for the task

        returns - {}, unless an error occurs, the error message will appear in the perentesis
         */
        public string taskEditTitle( string newTitle);



        /*
        This method updates task description.

        "email" - Email of the user. The user must be logged in
        "boardName" - The name of the board
        "title" - The title of the task (the task is identified by board-user-title uniqe combnation)
        "description" - New description for the task

        returns - {}, unless an error occurs, the error message will appear in the perentesis
         */
        public string taskEditDescription(string newDescription);
        
    }
}
