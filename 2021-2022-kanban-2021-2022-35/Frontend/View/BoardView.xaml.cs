using Frontend.Model;
using Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel viewModel;
        private UserModel user;
        public BoardView(BoardModel boardModel, UserModel userModel)
        {
            InitializeComponent();
            user = userModel;
            this.DataContext = new BoardViewModel(boardModel);
            this.viewModel = (BoardViewModel)DataContext;
            

        }
        private void return_Button(object sender, RoutedEventArgs e)
        {
            UserBoardsView userBoardsView = new UserBoardsView(user);
            userBoardsView.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout(user);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
