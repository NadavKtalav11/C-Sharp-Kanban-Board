using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Frontend.Model
{
    public class BoardControllerModel : NotifiableModelObject
    {
        private readonly UserModel user;
        public ObservableCollection<BoardModel> Boards { get; set; }      

        private BoardControllerModel(BackendController controller, ObservableCollection<BoardModel> boards) : base(controller)
        {
            this.Boards = boards;

        }

        public BoardControllerModel(BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;
            Boards = new ObservableCollection<BoardModel>(controller.GetAllUserBoards(user.Email).
                Select((c, i) => new BoardModel(controller, controller.GetBoard(user.Email, c), user)).ToList());
        }
    }
}
