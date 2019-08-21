using System.Collections.Generic;
using Meeting_App.Models;

namespace Meeting_App.Response
{
    public class MeetingResponseModel : ResponseModel
    {
        public Meeting Meeting { get; set; }

        public User MeetingCreatedBy { get; set; }

        public List<Attendee> Attendees { get; set; }

    }

    public class MeetingListResponseModel : ResponseModel
    {
        public List<MeetingResponseModel> AllMeetings { get; set; }
    }
}