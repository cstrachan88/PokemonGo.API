﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using PokemonGo.API.Enums;
using PokemonGo.API.Exceptions;
using PokemonGo.API.Login;
using PokemonGo.API.Extensions;
using PokemonGo.API.Helpers;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;

namespace PokemonGo.API.Rpc
{
    public delegate void GoogleDeviceCodeDelegate(string code, string uri);
    public class Login : BaseRpc
    {
        //public event GoogleDeviceCodeDelegate GoogleDeviceCodeEvent;
        private ILoginType login;

        public Login(Client client) : base(client)
        {
            login = SetLoginType(client.Settings);
        }

        private static ILoginType SetLoginType(Settings settings)
        {
            switch (settings.AuthType)
            {
                case AuthType.Google:
                    return new GoogleLogin(settings.Username, settings.Password);
                case AuthType.PokemonTrainerClub:
                    return new PtcLogin(settings.Username, settings.Password);
                default:
                    throw new ArgumentOutOfRangeException(nameof(settings.AuthType), "Unknown AuthType");
            }
        }

        public async Task DoLogin()
        {
            _client.AuthToken = await login.GetAccessToken().ConfigureAwait(false);
            await SetServer().ConfigureAwait(false);
        }

        private async Task SetServer()
        {
            #region Standard intial request messages in right Order

            var getPlayerMessage = new GetPlayerMessage();
            var getHatchedEggsMessage = new GetHatchedEggsMessage();
            var getInventoryMessage = new GetInventoryMessage
            {
                LastTimestampMs = DateTime.UtcNow.ToUnixTime()
            };
            var checkAwardedBadgesMessage = new CheckAwardedBadgesMessage();
            var downloadSettingsMessage = new DownloadSettingsMessage
            {
                Hash = "05daf51635c82611d1aac95c0b051d3ec088a930"
            };

            #endregion

            var serverRequest = RequestBuilder.GetInitialRequestEnvelope(
                new Request
                {
                    RequestType = RequestType.GetPlayer,
                    RequestMessage = getPlayerMessage.ToByteString()
                }, new Request
                {
                    RequestType = RequestType.GetHatchedEggs,
                    RequestMessage = getHatchedEggsMessage.ToByteString()
                }, new Request
                {
                    RequestType = RequestType.GetInventory,
                    RequestMessage = getInventoryMessage.ToByteString()
                }, new Request
                {
                    RequestType = RequestType.CheckAwardedBadges,
                    RequestMessage = checkAwardedBadgesMessage.ToByteString()
                }, new Request
                {
                    RequestType = RequestType.DownloadSettings,
                    RequestMessage = downloadSettingsMessage.ToByteString()
                });


            var serverResponse = await PostProto<Request>(Resources.RpcUrl, serverRequest);

            if (serverResponse.AuthTicket == null)
            {
                _client.AuthToken = null;
                throw new AccessTokenExpiredException();
            }

            _client.AuthTicket = serverResponse.AuthTicket;
            _client.ApiUrl = serverResponse.ApiUrl;
        }

    }
}
