using PokemonGo.API.Enums;

namespace PokemonGo.API
{
    public interface ISettings
    {
        AuthType AuthType { get; set; }

        double DefaultLatitude { get; set; }
        double DefaultLongitude { get; set; }
        double DefaultAltitude { get; set; }

        string Username { get; set; }
        string Password { get; set; }

        string GoogleRefreshToken { get; set; }
    }
}