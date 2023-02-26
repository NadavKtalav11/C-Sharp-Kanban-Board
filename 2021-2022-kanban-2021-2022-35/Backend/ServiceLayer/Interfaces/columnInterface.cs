using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer.Interfaces
{
    internal interface columnInterface
    {
            /*
           This method limited the number of task at the column.

           "email" - Email of the user. 
           "boardname" - the name of the board
           "index" - the index of the column in the board 

           returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
           */
       // public string columnLimit(string email, string boardName, int index, int limit);


                    /*
            This method return the column task limit .

            "email" - Email of the user. 
            "boardname" - the name of the board
            "index" - the index of the column in the board 

            returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
            */
       // public string getLimit(string email, string boardName, int index);



                 /*
            This method return  the column name.

            "email" - Email of the user. 
            "boardName" - name of the board
            "index" - the index of the column in the board 

            returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
            */
       // public string columnGetName(string email, string boardName, int index);





        /*
            This method adds a new task.

            task1 - the task we want to add to the column.

            returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
            */
        public string taskAdd(BussinessLayer.Taskk task1);


                /*
            This method removess a new task.
               
            task1 - the task we want to remove.

            returns - Response with {}, unless an error occurs, the error message will appear in the perentesis
            */


        public string taskRemove(BussinessLayer.Taskk task1);


    }
}
