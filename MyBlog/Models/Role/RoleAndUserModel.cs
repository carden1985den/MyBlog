using BLL.Entity;
using BLL.Enum;

namespace WEB.Models.Role
{
    public class RoleAndUserModel
    {
        public List<RoleAvailable>? RoleAvailable { get; set; }
        public List<UserAvailable>? UserAvailables { get; set; }
        //public List<string> SelectedUserId { get; set; }
        //public List<string> SelectedTagsId { get; set; }
    }

    public class RoleAvailable
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool IsChecked { get; set; }
    }
    public class UserAvailable
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public bool IsChecked { get; set; }
        public string UserRole { get; set; } = null!;
    }
}
