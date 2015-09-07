﻿using System;
using ImaginationServer.Common;
using ImaginationServer.World.Handlers.World;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.ClientWorldPacketId;

namespace ImaginationServer.World
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                ServerId type;

                if (args.Length < 1)
                {
                    Console.WriteLine("!---FAIL---!");
                    Console.WriteLine("NO SERVER ID");
                    Console.ReadKey(true);
                    return;
                }

                if (!Enum.TryParse(args[0], true, out type))
                {
                    Console.WriteLine("!---FAIL---!");
                    Console.WriteLine("INVALID SERVER ID");
                    Console.ReadKey(true);
                    return;
                }

                Console.WriteLine("Starting Imagination Server World {0}", type);

                type = type | ServerId.World;
                var server = new LuServer(ServerId.World,
                    type.HasFlag(ServerId.Character) ? 2006 : 2006 + (int) (ZoneId)Enum.Parse(typeof (ZoneId), args[0]), 1000,
                    "127.0.0.1");
                server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientValidation, new ClientValidationHandler());
                server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientLoginRequest, new ClientLoginRequestHandler());

                if (type.HasFlag(ServerId.Character))
                {
                    server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientCharacterListRequest, new CharacterListRequestHandler());
                    server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientCharacterCreateRequest, new ClientCharacterCreateRequestHandler());
                }
                Console.WriteLine("->OK");
                server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey(true);
            }
        }
    }
}