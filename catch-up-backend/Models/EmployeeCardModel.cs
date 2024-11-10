using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Models
{
    public class EmployeeCardModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public string AdditionalInformation { get; set; }
        public string AvatarSource { get; set; }

        public EmployeeCardModel(string name, string surname, string email, string telephoneNumber, string additionalInformation, string avatarSource)
        {
            Name = name;
            Surname = surname;
            Email = email;
            TelephoneNumber = telephoneNumber;
            AdditionalInformation = additionalInformation;
            AvatarSource = avatarSource;
        }
    }
}
