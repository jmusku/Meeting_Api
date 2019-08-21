using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meeting_App.Models;
using Meeting_App.ORM;
using Meeting_App.Enum;
using Meeting_App.Interface;
using Meeting_App.Response;

namespace Meeting_App.Services
{
    public class AttendeeService : IAttendee, IDisposable
    {
        private Meeting_DBEntities _dbContext = new Meeting_DBEntities();

        //add attendee
        public AttendeeResponseModel CreateAttendee(Attendee attendee)
        {
            var model = new tbl_Attendee
            {
                First_Name = attendee.FirstName,
                Last_Name = attendee.LastName
            };

            _dbContext.tbl_Attendee.Add(model);

            var isSaved = _dbContext.SaveChanges();

            if (isSaved > 0)
                attendee.Id = model.Id;

            return new AttendeeResponseModel
            { Attendee = attendee, Success = true, StatusCode = StatusCode.Created.ToString() };

        }

        //update attendee
        public async Task<ResponseModel> UpdateAttendee(Attendee attendee)
        {
            var entity = _dbContext.tbl_Attendee.FirstOrDefault(x => x.Id == attendee.Id);

            if (entity == null)
                return new ResponseModel { StatusCode = StatusCode.DoesNotExist.ToString() };
            {
                entity.First_Name = attendee.FirstName;
                entity.Last_Name = attendee.LastName;
            }
            try
            {
                // _dbContext.Entry(entity).State = EntityState.Modified;
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

        // get a filtered collection of attendee based on comma separated attendee ids
        public AttendeeListResponseModel GetAttendeesInMeeting(string ids)
        {
            var attendees = GetAttendeeList(ids);

            return new AttendeeListResponseModel { AttendeeList = attendees, StatusCode = StatusCode.Ok.ToString() };
        }

        public List<Attendee> GetAttendeeList(string ids)
        {
            var attendees = new List<Attendee>();

            if (ids.Length > 0)
            {
                var values = ids.Split(';');

                foreach (var id in values)
                {
                    attendees.Add(GetAttendeeModel(Convert.ToInt16(id)));
                }

            }
            return attendees;
        }

        // returns a single attendee from attendee table
        public AttendeeResponseModel GetAttendee(int id)
        {
            var model = (from xx in _dbContext.tbl_Attendee
                         where xx.Id == id
                         select new Attendee
                         {
                             FirstName = xx.First_Name,
                             LastName = xx.Last_Name,
                             Id = xx.Id
                         }).FirstOrDefault();
            return model != null
                ? new AttendeeResponseModel { Attendee = model, StatusCode = StatusCode.Ok.ToString(), Success = true }
                : new AttendeeResponseModel
                { Attendee = null, StatusCode = StatusCode.DoesNotExist.ToString() };

        }

        public Attendee GetAttendeeModel(int id)
        {
            return (from xx in _dbContext.tbl_Attendee
                    where xx.Id == id
                    select new Attendee
                    {
                        FirstName = xx.First_Name,
                        LastName = xx.Last_Name,
                        Id = xx.Id
                    }).FirstOrDefault();
        }

        // returns all attendees from attendee table
        public AttendeeListResponseModel GetAttendeeList()
        {
            var attendees = (from xx in _dbContext.tbl_Attendee
                             select new Attendee()
                             {
                                 FirstName = xx.First_Name,
                                 LastName = xx.Last_Name,
                                 Id = xx.Id
                             }).ToList();
            if (attendees.Count > 0)
            {
                return new AttendeeListResponseModel { AttendeeList = attendees, StatusCode = StatusCode.Ok.ToString(), Success = true };
            }
            {
                return new AttendeeListResponseModel { AttendeeList = attendees, StatusCode = StatusCode.DoesNotExist.ToString() };
            }

        }

        // delete attendee
        public async Task<bool> DeleteAttendee(int id)
        {
            var attendee = await _dbContext.tbl_Attendee.FindAsync(id);

            if (attendee != null)
                _dbContext.tbl_Attendee.Remove(attendee);

            var isDeleted = await _dbContext.SaveChangesAsync();

            return isDeleted > 0;
        }

        private bool IsAttendeeExists(int id)
        {
            return _dbContext.tbl_Attendee.Count(e => e.Id == id) > 0;
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