using System.Threading.Tasks;
using Meeting_App.Models;
using Meeting_App.Response;

namespace Meeting_App.Interface
{
    public interface IMeeting
    {
        MeetingListResponseModel GetAllMeetings();

        MeetingResponseModel GetMeetingByMeetingId(int id);

        MeetingListResponseModel GetMeetingsByUserCreated(int createdBy);
       
        Task<MeetingResponseModel> CreateMeeting(Meeting meeting);

        Task<ResponseModel> UpdateMeeting(Meeting meeting);

        ResponseModel UpdateMeetingAttendees(int meetingId, string ids); 

        ResponseModel DeleteMeeting(int id);
    }
}