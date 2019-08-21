using System.Linq;
using System.Threading.Tasks;
using Meeting_App.Models;
using Meeting_App.Response;

namespace Meeting_App.Interface
{
    public interface IUser
    {
        UserResponseModel CreateUser(User user);

        UserResponseModel GetUser(string userName);

        UserResponseModel GetUserById(int id);

        Task<ResponseModel> UpdateUser(User user);

        IQueryable GetAllUsers();

        UserResponseModel Delete(int id); 
    }
}