using System.ComponentModel.DataAnnotations;

namespace Broker.Users.Dto
{
    public class ResetPasswordDto
    {
        [Required]
        public string AdminPassword { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public string NewPassword { get; set; }
        public string UsernameOrEmailAddress { get; set; }
        public string ResetCode { get; set; }

    }
}
