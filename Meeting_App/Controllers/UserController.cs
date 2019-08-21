using Meeting_App.Models;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Meeting_App.Interface;
using Meeting_App.Response;

namespace Meeting_App.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public class UserController : ApiController
    {
        private readonly IUser _apiSecurity;

        public UserController(IUser repository)
        {
            _apiSecurity = repository;
        }

        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            var userList = _apiSecurity.GetAllUsers();
            return Ok(userList);
        }

        [HttpGet]
        [ResponseType(typeof(ResponseModel))]

        // [Route("user/{userName}")]
        public IHttpActionResult GetUserByUserName(string userName)
        {
            var responseModel = _apiSecurity.GetUser(userName);
            return Ok(responseModel);
        }

        [HttpGet]
        public IHttpActionResult GetUsersById(int id)
        {
            var userList = _apiSecurity.GetUserById(id);

            return Ok(userList);
        }

        [HttpPost]
        public IHttpActionResult CreateUser(User user)
        {
            var responseModel = _apiSecurity.CreateUser(user);

            return Ok(responseModel);
        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdateUser(User user)
        {
            var response = await _apiSecurity.UpdateUser(user);
            return Ok(response);
        }

        [HttpDelete]
        public IHttpActionResult DeleteUser(int id)
        {
            var userResponseModel = _apiSecurity.Delete(id);

            return Ok(userResponseModel);
        }
    }
}
