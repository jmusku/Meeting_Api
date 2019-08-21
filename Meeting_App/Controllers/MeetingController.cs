using Meeting_App.Models;
using System.Threading.Tasks;
using System.Web.Http.Description;
using Meeting_App.Interface;
using Meeting_App.Response;
using System.Web.Http;

namespace Meeting_App.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public class MeetingController : ApiController
    {
        private readonly IMeeting _meetingService;

        public MeetingController(IMeeting repository)
        {
            _meetingService = repository;
        }

        // GET: Meeting/GetMeetings
        [HttpGet]
        public IHttpActionResult GetMeetings()
        {
            var responseModel = _meetingService.GetAllMeetings();
            return Ok(responseModel);
        }

        // GET: Meeting/GetMeetingByMeetingId/id
        [HttpGet]
        public IHttpActionResult GetMeetingByMeetingId(int id)
        {
            var responseModel = _meetingService.GetMeetingByMeetingId(id);
            return Ok(responseModel);
        }
        // POST: Meeting/GetMeetingByMeetingId
        [HttpPost]
        public async Task<IHttpActionResult> CreateMeeting(Meeting meeting)
        {

            var responseModel = await _meetingService.CreateMeeting(meeting);

            return Ok(responseModel);
        }

        // PUT:  Meeting/UpdateMeeting
        [HttpPut]
        [ResponseType(typeof(Task<ResponseModel>))]
        public async Task<IHttpActionResult> UpdateMeeting(Meeting meeting)
        {
            var responseModel = await _meetingService.UpdateMeeting(meeting);

            return Ok(responseModel);
        }

        // PUT: Meeting/UpdateAttendees/4
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public IHttpActionResult UpdateMeetingAttendeesByMeetingId(UpdateMeetingAttendee updateMeetingAttendees)
        {
            var responseModel = _meetingService.UpdateMeetingAttendees(updateMeetingAttendees.MeetingId, updateMeetingAttendees.AttendeeIds);

            return Ok(responseModel);
        }

        // DELETE: Meeting/DeleteMeeting/4
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
        public IHttpActionResult DeleteMeeting(int id)
        {
            var responseModel = _meetingService.DeleteMeeting(id);

            return Ok(responseModel);
        }
    }
}
