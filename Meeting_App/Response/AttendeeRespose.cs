using Meeting_App.Models;
using System.Collections.Generic;


namespace Meeting_App.Response 
{
    public class AttendeeResponseModel : ResponseModel
    {
        public Attendee Attendee { get; set; }
    }

    public class AttendeeListResponseModel : ResponseModel
    {
        public List<Attendee> AttendeeList { get; set; }
    }
}