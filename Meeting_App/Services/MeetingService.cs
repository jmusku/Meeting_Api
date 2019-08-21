using System;
using Meeting_App.Models;
using Meeting_App.ORM;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Meeting_App.Enum;
using Meeting_App.Interface;
using Meeting_App.Response;

namespace Meeting_App.Services
{
    public class MeetingService : IMeeting, IDisposable
    {
        private Meeting_DBEntities _dbContext = new Meeting_DBEntities();

        private readonly UserService _userDetails = new UserService();

        public MeetingListResponseModel GetAllMeetings()
        {
            var allMeeting = (from xx in _dbContext.tbl_Meeting
                              select new Meeting
                              {
                                  Attendees = xx.Attendees,
                                  Agenda = xx.Agenda,
                                  Date = xx.Date,
                                  Id = xx.Id,
                                  Subject = xx.Subject,
                                  CreatedById = xx.Created_By,
                              }).ToList();

            if (allMeeting.Count > 0)
            {
                return GetMeetingAttendeesAndCreatedBy(allMeeting);
            }

            return new MeetingListResponseModel
            { AllMeetings = null, Success = true, StatusCode = StatusCode.DoesNotExist.ToString() };
        }

        public MeetingListResponseModel GetMeetingsByUserCreated(int createdBy)
        {
            var allMeeting = GetMeetingsCreatedBy(createdBy);

            if (allMeeting.Count > 0)
            {
                return GetMeetingAttendeesAndCreatedBy(allMeeting);
            }
            return new MeetingListResponseModel
            { AllMeetings = null, Success = true, StatusCode = StatusCode.DoesNotExist.ToString() };
        }

        private List<Meeting> GetMeetingsCreatedBy(int id)
        {
            var meetings = _dbContext.tbl_Meeting.Where(xx => xx.Created_By == id).Select(
                xx => new Meeting
                {
                    Attendees = xx.Attendees,
                    Subject = xx.Subject,
                    Agenda = xx.Agenda,
                    Date = xx.Date,
                }).ToList();
            return meetings;
        }

        private MeetingListResponseModel GetMeetingAttendeesAndCreatedBy(IEnumerable<Meeting> allMeeting)
        {
            var meetingList = new List<MeetingResponseModel>();

            foreach (var meeting in allMeeting)
            {
                var meetingResponseModel = new MeetingResponseModel{ Success = true, StatusCode = StatusCode.Ok.ToString()};
                var attendees = GetAttendeeDetails(meeting);
                var createdBy = _userDetails.GetUserDetailsByUserId(meeting.CreatedById);
                meetingResponseModel.Attendees = attendees;
                meetingResponseModel.Meeting = meeting;
                meetingResponseModel.MeetingCreatedBy = createdBy;
                meetingList.Add(meetingResponseModel);
            }

            return new MeetingListResponseModel
            { AllMeetings = meetingList, Success = true, StatusCode = StatusCode.Ok.ToString() }; ;
        }

        public MeetingResponseModel GetMeetingByMeetingId(int id)
        {
            var meetingDetails = _dbContext.tbl_Meeting.Find(id);

            if (meetingDetails == null)
            {
                return new MeetingResponseModel { Meeting = null, StatusCode = StatusCode.DoesNotExist.ToString() };
            }

            {
                var createdBy = _userDetails.GetUserDetailsByUserId(meetingDetails.Created_By);

                var meeting = new Meeting
                {
                    Id = meetingDetails.Id,
                    Agenda = meetingDetails.Agenda,
                    Attendees = meetingDetails.Attendees,
                    Date = meetingDetails.Date,
                    Subject = meetingDetails.Subject,
                    CreatedById = meetingDetails.Created_By
                };

                var attendees = GetAttendeeDetails(meeting);

                return new MeetingResponseModel
                {
                    Meeting = meeting,
                    StatusCode = StatusCode.Ok.ToString(),
                    MeetingCreatedBy = createdBy,
                    Attendees = attendees,
                    Success = true
                };
            }
        }

        public async Task<MeetingResponseModel> CreateMeeting(Meeting meeting)
        {
            try
            {
                var model = new tbl_Meeting
                {
                    Attendees = meeting.Attendees,
                    Id = meeting.Id,
                    Agenda = meeting.Agenda,
                    Subject = meeting.Subject,
                    Date = meeting.Date,
                    Created_By = meeting.CreatedById
                };

                _dbContext.tbl_Meeting.Add(model);

                var id = await _dbContext.SaveChangesAsync();

                if (id > 0)
                    meeting.Id = model.Id;

                return new MeetingResponseModel
                    {Meeting = meeting, Success = true, StatusCode = StatusCode.Created.ToString()};
            }
            catch (Exception ex)
            {
                var exception = ex.ToString();
                return new MeetingResponseModel();
            }
        }

        public async Task<ResponseModel> UpdateMeeting(Meeting meeting)
        {
            var entity = _dbContext.tbl_Meeting.FirstOrDefault(m => m.Id == meeting.Id);

            if (entity == null)
                return new ResponseModel { StatusCode = StatusCode.DoesNotExist.ToString() };
            {
                entity.Agenda = meeting.Agenda;
                entity.Attendees = meeting.Attendees;
                entity.Date = meeting.Date;
                entity.Subject = meeting.Subject;
                entity.Created_By = meeting.CreatedById;

                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();

            return new ResponseModel
            {
                StatusCode = StatusCode.Updated.ToString(),
                Success = true
            };
        }

        public ResponseModel UpdateMeetingAttendees(int meetingId, string ids)
        {
            var result = _dbContext.tbl_Meeting.SingleOrDefault(b => b.Id == meetingId);

            if (result == null)
                return new ResponseModel
                {
                    StatusCode = StatusCode.DoesNotExist.ToString(),
                    Success = true
                };

            result.Attendees = ids;

            _dbContext.SaveChanges();

            return new ResponseModel
            {
                StatusCode = StatusCode.Updated.ToString(),
                Success = true
            };
        }

        public ResponseModel DeleteMeeting(int id)
        {
            var meeting = _dbContext.tbl_Meeting.FirstOrDefault(s => s.Id == id);

            if (meeting == null)
                return new ResponseModel
                {
                    StatusCode = StatusCode.DoesNotExist.ToString(),
                    Success = true
                };
            {
                _dbContext.Entry(meeting).State = EntityState.Deleted;
                _dbContext.SaveChangesAsync();

                return new ResponseModel()
                {
                    StatusCode = StatusCode.Deleted.ToString(),
                    Success = true
                };
            }

        }

        private List<Attendee> GetAttendeeDetails(Meeting meeting)
        {
            var attendees = new List<Attendee>();

            var attendeeService = new AttendeeService();

            if (!string.IsNullOrEmpty(meeting.Attendees))
                attendees = attendeeService.GetAttendeeList(meeting.Attendees);


            return attendees;
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
