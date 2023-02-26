using Frontend.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel

{
    public class UserBoardsViewModel : NotifiableObject
    {
        private BackendController controller {  get; set; }

        public BoardControllerModel boards { get; set; }


        private BoardModel _selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }


        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                RaisePropertyChanged("message");
            }
        }
        public string Title { get; private set; }


        private BoardModel _selectedBoards;
        public BoardModel SelectedBoards
        {
            get
            {
                return _selectedBoards;
            }
            set
            {
                _selectedBoards = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoards");
            }
        }
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



        internal void Logout(UserModel userModel)
        {
            controller.LogOut(userModel.Email); 
        }


        public BoardModel selectBoard()
        {
            message = "";
            try
            {
                
                return _selectedBoards;
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot remove message. " + e.Message);
                return null;
            }

        }

        public UserBoardsViewModel(UserModel user)
        {
            this.controller = user.Controller;
            Title = "Boards of " + user.Email;
            boards = user.GetBoards();
        }

    }
}