namespace AuthGuardCore.Models
{
    public class UserRoleAssignViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleAssignItemViewModel> Roles { get; set; }
    }
}
