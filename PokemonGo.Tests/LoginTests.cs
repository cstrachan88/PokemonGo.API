using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonGo.API;
using PokemonGo.API.Enums;
using PokemonGo.API.Exceptions;

namespace PokemonGo.Tests
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public async Task BadLoginTest()
        {
            var client = new Client(new Settings
            {
                AuthType = AuthType.Google,
                Latitude = 0,
                Longitude = 0,
                Altitude = 0,
                Username = "fake@gmail.com",
                Password = "fake"
            });

            try
            {
                await client.Login.DoLogin();

                Assert.Fail("Expected a GoogleException");
            }
            catch (GoogleException)
            {
                Debug.WriteLine("Failed with a GoogleException (expected)");
            }
        }

        [TestMethod]
        public async Task GoodLoginTest()
        {
            // Fill in your login information here...

            var client = new Client(new Settings
            {
                AuthType = AuthType.Google,
                Latitude = 0,
                Longitude = 0,
                Altitude = 0,
                Username = "real@gmail.com",
                Password = "real"
            });

            try
            {
                await client.Login.DoLogin();
                Debug.WriteLine("Logged in with auth token: " + client.AuthToken);
            }
            catch (GoogleException)
            {
                Debug.WriteLine("Check to make sure your login information is correct.");
                throw;
            }
        }
    }
}
