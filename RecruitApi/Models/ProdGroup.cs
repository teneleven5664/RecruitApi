using System.ComponentModel.DataAnnotations;

namespace RecruitApi.Models
{
    public class ProdGroup
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }
    }
}
