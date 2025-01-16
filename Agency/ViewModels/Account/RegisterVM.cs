using System.ComponentModel.DataAnnotations;

namespace Agency.ViewModels.Account
{
    public class RegisterVM
    {
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }

        [MaxLength(50)]
        [MinLength(2)]
        public string Surname { get; set; }

        [MaxLength(256)]
        [MinLength(4)]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [MaxLength(256)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(8)]
        public string Password { get; set; }
        [MaxLength(8)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
       
    }
}
