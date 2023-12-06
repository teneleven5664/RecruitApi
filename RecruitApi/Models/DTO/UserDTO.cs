using System.ComponentModel.DataAnnotations;

namespace RecruitApi.Models.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required bool Active { get; set; }
    }
}
