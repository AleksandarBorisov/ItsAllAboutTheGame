using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.Services.Data.DTO;
using System.Threading.Tasks;
using X.PagedList;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface IUserService
    {
        //Task<User> RegisterUserWithLoginProvider(ExternalLoginInfo info, Currency userCurrency, DateTime dateOfBirth);

        Task<UserInfoDTO> GetUserInfo(string userId);

        //Task<User> GetUser(string username);

        Task<IPagedList<UserDTO>> GetAllUsers(string searchByUsername = null, int page = 1, int size = 5, string sortOrder = GlobalConstants.DefaultUserSorting);

        //Task<IEnumerable<CreditCard>> UserCards(User user);

        Task<UserDTO> LockoutUser(string userId, int days);

        Task<UserDTO> DeleteUser(string userId);

        Task<UserDTO> ToggleAdmin(string userId);
    }
}
