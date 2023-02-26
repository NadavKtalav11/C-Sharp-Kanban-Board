using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel
{
    public class BoardViewModel : NotifiableObject
    {

        public Model.BackendController controller;
        public BoardModel Board { get; set; }


        public SolidColorBrush BackgroundColor
        {
            get
            {
                return new SolidColorBrush(Colors.Black);
            }
        }
        public string Title { get; private set; }

        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }



        internal void Logout(UserModel user)
        {
            controller.LogOut(user.Email);
        }



        public BoardViewModel(BoardModel boardModel) 
        {
            Board = boardModel;
            this.controller = boardModel.Controller;
            Title =  boardModel.Name ;
            boardModel.getTasks();
            
            
        }

    }
}

