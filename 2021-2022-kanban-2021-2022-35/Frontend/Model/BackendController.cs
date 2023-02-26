using Backend.Service;
using IntroSE.Kanban.Backend.BussinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Classes;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;



namespace Frontend.Model
{
    public class BackendController
    {
        private ServiceFactory Service { get; set; }
        public BackendController(ServiceFactory service)
        {
            this.Service = service;
        }

        public BackendController()
        {
            this.Service = new ServiceFactory();
            Service.LoadData();
        }

        public UserModel Login(string username, string password)
        {
            JavaScriptSerializer oJS = new JavaScriptSerializer();
            Response res1 = new Response();
            res1 = oJS.Deserialize<Response>(Service.userService.userLogin(username, password));
            if (res1.ErrorMessage!=null)
            {
                throw new Exception(res1.ErrorMessage);
            }
            return new UserModel(this, username);
        }

        internal List<int> GetAllUserBoards(string email)
        {
            JavaScriptSerializer oJS = new JavaScriptSerializer();
            Response res1 = new Response();
            res1 = JsonConvert.DeserializeObject<Response>(Service.userService.GetUserBoards(email));
            if (res1.ErrorMessage != null)
            {
                throw new Exception(res1.ErrorMessage);
            }

            List<int> BoardsIds = new List<int>();
            foreach(int item in (JArray)res1.ReturnValue)
            {
                BoardsIds.Add(item);
            }
            return BoardsIds;
        }

        internal (int Id, string name ) GetBoard(string email ,int BoardId)
        {
            string json = Service.boardService.getBoard(email, BoardId);
            Response<Board> res  = JsonController.DeSerialize<Response<Board>>(json);

            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }
            Board board = (Board)res.ReturnValue;

            return (board.boardId, board.name);
        }
        internal (int index , string name , int limit) GetColumn(string email, int BoardId , int columnIndex)
        {
            string json = Service.columnService.getColumn(email, BoardId, columnIndex);
            Response<Column> res = JsonController.DeSerialize<Response<Column>>(json);
            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }
            Column column = (Column)res.ReturnValue;
            return (column.ColumnIndex, column.name , column.limit);
        }


        internal List<Taskk> GetColumnTasks(string email, int BoardId, int columnIndex)

        {
            string json = Service.columnService.getColumnTasks(email, BoardId, columnIndex);
            Response<List<Taskk>> res = JsonController.DeSerialize<Response<List<Taskk>>>(json);
            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }
            List<Taskk> tasks = (List<Taskk>)res.ReturnValue;
            return tasks;

        }

        public void LogOut(string email) {
            string json =Service.userService.userLogout(email);
            Response res = JsonController.DeSerialize<Response>(json);
            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }
        }





        internal void Register(string Email, string password)
        {
            Response res = System.Text.Json.JsonSerializer.Deserialize<Response>(Service.userService.userRegister(Email, password));
            if (res.ErrorMessage!=null)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

    }
}
