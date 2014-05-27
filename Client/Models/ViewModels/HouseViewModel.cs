using System.Drawing;
using System.Web;
using DbFirstModel;

namespace Client.Models.ViewModels
{
    public class HouseViewModel
    {
        public int Id { get; set; }
        public int RoomCount { get; set; }
        public int TypeId { get; set; }
        public int Price { get; set; }
        public string Owner { get; set; }
        public HttpPostedFileBase Photo { get; set; }

        public virtual Type Type { get; set; }
    }
}