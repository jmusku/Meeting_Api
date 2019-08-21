using System;
using Meeting_App.ORM;
using System.Linq;
using Meeting_App.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using Meeting_App.Enum;
using Meeting_App.Interface;
using Meeting_App.Response;

namespace Meeting_App.Services
{
    public class UserService : IUser, IDisposable
    {
        private Meeting_DBEntities _dbContext = new Meeting_DBEntities();
        public UserResponseModel CreateUser(User user)
        {
            if (IsUserExist(user.UserName))
                return new UserResponseModel
                {
                    Success = true,
                    StatusCode = StatusCode.AlreadyExists.ToString()
                };

            var model = new tbl_User
            {
                User_Name = user.UserName,
                First_Name = user.FirstName,
                Last_Name = user.LastName,
                User_Role = user.UserRole,
                Password = user.Password.EnCrypt(),
            };

            _dbContext.tbl_User.Add(model);

            if (_dbContext.SaveChanges() > 0)
                user.Id = model.Id;

            return new UserResponseModel
            {
                User = user,
                StatusCode = StatusCode.Created.ToString(),
                Success = true
            };

        }

        private bool IsUserExist(string userName)
        {
            var isValidUser = _dbContext.tbl_User.SingleOrDefault(u => u.User_Name == userName);
            return isValidUser != null;
        }

        public UserResponseModel GetUser(string userName)
        {
            var user = (from xx in _dbContext.tbl_User
                        where xx.User_Name == userName
                        select new User
                        {
                            FirstName = xx.First_Name,
                            LastName = xx.Last_Name,
                            UserRole = xx.User_Role,
                            Id = xx.Id,
                            UserName = xx.User_Name,
                        }).FirstOrDefault();

            if (user != null)
                return new UserResponseModel
                {
                    User = user,
                    StatusCode = StatusCode.Ok.ToString(),
                    Success = true
                };
            {
                return new UserResponseModel { User = null, StatusCode = StatusCode.DoesNotExist.ToString() };
            }
        }

        public UserResponseModel GetUserById(int id)
        {
            var userDetails = GetUserDetailsByUserId(id);

            if (userDetails == null)
            {
                return new UserResponseModel { User = null, StatusCode = StatusCode.DoesNotExist.ToString() };
            }
            {
                return new UserResponseModel
                {
                    User = userDetails,
                    StatusCode = StatusCode.Ok.ToString(),
                    Success = true
                };
            }
        }

        public User GetUserDetailsByUserId(int id)
        {
            var userDetails = _dbContext.tbl_User.Where(xx => xx.Id == id).Select(
                xx => new User
                {
                    FirstName = xx.First_Name,
                    LastName = xx.Last_Name,
                    UserRole = xx.User_Role,
                    UserName = xx.User_Name,
                    Password = xx.Password,
                    Id = xx.Id
                });
            return userDetails.FirstOrDefault();
        }

        public async Task<ResponseModel> UpdateUser(User user)
        {
            var entity = _dbContext.tbl_User.FirstOrDefault(u => u.Id == user.Id);

            if (entity == null)
                return new ResponseModel { StatusCode = StatusCode.DoesNotExist.ToString() };
            {
                entity.First_Name = user.FirstName;
                entity.Last_Name = user.LastName;
                entity.User_Role = user.UserRole;
                entity.Password = user.Password;
                entity.User_Name = user.UserName;
            }
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return new ResponseModel
            {
                StatusCode = StatusCode.Updated.ToString(),
                Success = true
            };
        }

        public IQueryable GetAllUsers()
        {
            return from xx in _dbContext.tbl_User
                   select new User
                   {
                       FirstName = xx.First_Name,
                       UserRole = xx.User_Role,
                       Id = xx.Id,
                       UserName = xx.User_Name,
                       LastName = xx.Last_Name
                   };
        }

        public UserResponseModel Delete(int id)
        {
            var user = _dbContext.tbl_User.FirstOrDefault(s => s.Id == id);

            if (user == null)
                return new UserResponseModel
                {
                    StatusCode = StatusCode.DoesNotExist.ToString(),
                    Success = false
                };
            {
                _dbContext.Entry(user).State = EntityState.Deleted;
                _dbContext.SaveChangesAsync();

                return new UserResponseModel
                {
                    StatusCode = StatusCode.Deleted.ToString(),
                    Success = true
                };
            }

        }

        public bool ValidateUser(string username, string password)
        {
            var encryptPassword = password.EnCrypt();
            var isValidUser = _dbContext.tbl_User.SingleOrDefault(u => u.User_Name == username && u.Password.ToString() == encryptPassword);
            return isValidUser != null;
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}