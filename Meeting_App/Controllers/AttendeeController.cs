using Meeting_App.Models;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Meeting_App.Interface;
using Meeting_App.Response;

namespace Meeting_App.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public class AttendeeController : ApiController
    {
        private readonly IAttendee _attendeeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttendeeController"/> class.
        /// </summary>
        /// <param name="attendeeService">The Attendee service</param>
        public AttendeeController(IAttendee attendeeService)
        {
            _attendeeService = attendeeService;
        }

        // GET: api/Attendees
        [HttpGet]
        public IHttpActionResult GetAttendeeList()
        {
            return Ok(_attendeeService.GetAttendeeList());
        }

        // GET: api/Attendee/id
        [HttpGet]
        public IHttpActionResult GetAttendee(int id)
        {
            return Ok(_attendeeService.GetAttendee(id));
        }

        // GET: api/Attendee collection with ids
        [HttpGet]
        //[Route("Attendee/{ids}")]
        public IHttpActionResult GetAttendeesInAMeeting(string ids)
        {
            return Ok(_attendeeService.GetAttendeesInMeeting(ids));
        }

        // POST: api/Attendees
        [HttpPost]
        public IHttpActionResult AddAttendee(Attendee attendee)
        {
            return Ok(_attendeeService.CreateAttendee(attendee));
        }

        // PUT: api/Attendees/5
        [ResponseType(typeof(ResponseModel))]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAttendee(Attendee attendee)
        {
            var response = await _attendeeService.UpdateAttendee(attendee);

            return Ok(response);

        }

        // DELETE: api/Attendees/5
        [ResponseType(typeof(bool))]
        [HttpDelete]
        public async Task<bool> DeleteAttendee(int id)
        {
            return await _attendeeService.DeleteAttendee(id);
        }
    }
}
