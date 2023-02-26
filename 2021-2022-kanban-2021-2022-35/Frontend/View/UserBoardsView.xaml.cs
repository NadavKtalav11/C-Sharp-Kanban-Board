using Frontend.Model;
using Frontend.ViewModel;
using System;
using System.Windows;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class UserBoardsView : Window
    {
        private UserBoardsViewModel viewModel;
        private UserModel userModel;
        public UserBoardsView(UserModel u)
        {
            userModel = u;
            InitializeComponent();
            this.DataContext = new UserBoardsViewModel(u);
            viewModel = (UserBoardsViewModel)DataContext;
        }


        private void Select_Button(object sender, RoutedEventArgs e)
        {
            BoardModel board =viewModel.selectBoard();
            if (board != null)
            {
                BoardView boardView = new BoardView(board ,userModel);
                boardView.Show();
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout(userModel);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
