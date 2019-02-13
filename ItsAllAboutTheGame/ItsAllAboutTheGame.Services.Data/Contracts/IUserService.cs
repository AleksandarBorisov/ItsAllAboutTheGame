using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.Services.Data.DTO;
using System.Threading.Tasks;
using X.PagedList;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<UserInfoDTO> GetUserInfo(string userId);

        Task<IPagedList<UserDTO>> GetAllUsers(string searchByUsername = null, int page = 1, int size = 5, string sortOrder = GlobalConstants.DefaultUserSorting);

        Task<UserDTO> LockoutUser(string userId, int days);

        Task<UserDTO> DeleteUser(string userId);

        Task<UserDTO> ToggleAdmin(string userId);
    }
}
