using System.ComponentModel.DataAnnotations;

namespace RecruitApi.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public required string UserName {  get; set; }
        public required string Password { get; set; }
        public required DateTime CreationDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public required Guid CreatedByUserId {  get; set; }
        public Guid? UpdatedByUserId { get; set; }
        public required bool Active { get; set; }
    }
}
