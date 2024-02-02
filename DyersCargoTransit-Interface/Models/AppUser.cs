namespace DyersCargoTransit_Interface.Models
{
    public class AppUser
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public List<string> Roles { get; set; }
    }
    public class AppUserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
