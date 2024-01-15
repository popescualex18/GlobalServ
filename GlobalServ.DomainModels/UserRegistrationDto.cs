namespace GlobalServ.DomainModels
{
    public class UserRegistrationDto : LoginUserDto
    {
        public string SurName { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}