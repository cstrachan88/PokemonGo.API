using PokemonGo.API.Enums;

namespace PokemonGo.API
{
    public class Settings
    {
        public AuthType AuthType { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}