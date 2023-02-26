using IntroSE.Kanban.Backend.BussinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer.Classes
{
    public class ServiceFactory
    {
        public BoardController boardController { get; private set; }
        public UserController userController { get; private set; }
        public BoardService boardService { get; private set; }
        public UserService userService { get; private set; }
        public ColumnService columnService { get; private set; }
        public TaskService taskService { get; private set; }

        public ServiceFactory()
        {
            this.boardController = new BoardController();
            this.userController = boardController.userController;
            this.boardService = new BoardService(boardController);
            this.userService = new UserService(boardController, userController);
            this.columnService = new ColumnService(boardController);
            this.taskService= new TaskService(boardController);
        }


        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LoadData()
        {
            try
            {
                boardController.userController.loadData();
                boardController.loadData();
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteData()
        {
            try
            {
                boardController.deleteData();
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }
    }

}
