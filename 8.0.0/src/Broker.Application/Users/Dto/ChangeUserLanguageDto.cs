using System.ComponentModel.DataAnnotations;

namespace Broker.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}