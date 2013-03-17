using System.Linq;
using System.Web.Mvc;
using Chat.Filters;
using Chat.Infrastructure.Abstract;
using Chat.ViewModels;
using Entities.Models;

namespace Chat.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class RoomController : Controller
    {
        private readonly IRoomUnitOfWork unitOfWork;

        public RoomController(IRoomUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        public ViewResult List()
        {
            return View(unitOfWork.Rooms);
        }
        
        public ViewResult Info(int roomId)
        {
            var room = unitOfWork.FindRoomById(roomId);
            var roomInfo = new RoomInfo(room);
            return View(roomInfo);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room room)
        {
            if (string.IsNullOrEmpty(room.Title))
                return View();

            unitOfWork.CreateRoom(room);
            unitOfWork.Commit();
            return RedirectToAction("List");
        }

        public ViewResult Join(int roomId)
        {
            var room = unitOfWork.JoinRoom(roomId);
            unitOfWork.Commit();
            var userId = unitOfWork.GetCurrentUserId();
            room.Members.Remove(room.Members.FirstOrDefault(member => member.UserId == userId));
            return View("Room", room);
        }

        [HttpPost]
        public JsonResult LoadRecords(int roomId, long lastRecordCreationDate)
        {
            var records = from record in unitOfWork.GetRecordsAfter(roomId, lastRecordCreationDate)
                          select
                              new JsonRecord {Text = record.ToString(), CreationDate = record.CreationDate.ToBinary()};
            return Json(records);
        }

        [HttpPost]
        public EmptyResult AddRecord(int roomId, string text)
        {
            unitOfWork.AddRecord(roomId, text);
            unitOfWork.Commit();
            return new EmptyResult();
        }

        public RedirectToRouteResult Exit(int roomId)
        {
            unitOfWork.ExitRoom(roomId);
            unitOfWork.Commit();
            return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult RoomsPartial()
        {
            var rooms = from room in unitOfWork.GetCurrentUserRooms()
                        select new JsonRoom {Id = room.Id, Title = room.Title};
            return Json(rooms);
        }
    }
}
