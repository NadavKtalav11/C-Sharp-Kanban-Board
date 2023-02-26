
using IntroSE.Kanban.Backend.BussinessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {

        public ColumnModel Backlog { get; set; }
        public ColumnModel inProgess { get; set; }
        public ColumnModel done { get; set; }
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }

        private BackendController controller;

        private string name;
        public string Name
        {
            get => name;
            set { name = value; }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string _body;
        public string Body
        {
            get => _body;
            set
            {
                this._body = value;
                RaisePropertyChanged("Body");
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set { email = value; }
        }


        public void getTasks()
        {
            Backlog =  new ColumnModel(controller, controller.GetColumn(email, Id, 0), this);
            inProgess = new ColumnModel(controller, controller.GetColumn(email, Id, 1), this);
            done = new ColumnModel(controller, controller.GetColumn(email, Id, 2), this);
           
        }


    



  
        public BoardModel(BackendController controller, int id, string boardName, UserModel user) : base(controller)
        {
            this.controller = controller;
            this.Id = id;
            email = user.Email;
            this.name = boardName;
            this.Title = "board Id- " + Id;
            this.Body = "Board name- " + boardName;

        }
        public BoardModel(BackendController controller, (int Boardid ,string boardname)board,  UserModel user) : this(controller, board.Boardid , board.boardname , user) { }


    }
}
