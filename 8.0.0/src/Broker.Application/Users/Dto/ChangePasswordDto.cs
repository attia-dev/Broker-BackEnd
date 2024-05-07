using System.ComponentModel.DataAnnotations;

namespace Broker.Users.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
    public class ForgetPasswordDto
    {
        public string UserNameOrEmailAddress { get; set; }
    }
}
