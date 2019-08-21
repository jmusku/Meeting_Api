using System.Threading.Tasks;
using Meeting_App.Models;
using Meeting_App.Response;

namespace Meeting_App.Interface
{
    public interface IAttendee
    {
        AttendeeResponseModel CreateAttendee(Attendee attendee);
        Task<ResponseModel> UpdateAttendee(Attendee attendee); 
        
        AttendeeListResponseModel GetAttendeesInMeeting(string ids);

        AttendeeResponseModel GetAttendee(int id);

        Attendee GetAttendeeModel(int id);

        AttendeeListResponseModel GetAttendeeList();

        Task<bool> DeleteAttendee(int id);

    }
}