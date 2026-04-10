/*
 * File: Bot.cs
 * Project: beepBot-fluxer
 * Created Date: 2026-04-09 22:49:21
 * Author: 3urobeat
 *
 * Last Modified: 2026-04-10 14:20:34
 * Modified By: 3urobeat
 *
 * Copyright (c) 2026 3urobeat <https://github.com/3urobeat>
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.
 */


using Microsoft.Extensions.Logging;
using Fluxify.Bot;
using Fluxify.Commands;
using Fluxify.Commands.TextCommand;
using Fluxify.Core;
using Fluxify.Core.Credentials;


// Bot Singleton
sealed class Bot
{
    private static Bot? _instance;
    private readonly Fluxify.Bot.Bot bot;

    private readonly BotConfig cfg = new BotConfig("*")
    {
        Credentials = new BotTokenCredentials(Environment.GetEnvironmentVariable("FLUXER_BOT_TOKEN")!)
    };


    // (Creates and) Returns Singleton instance
    public static Bot Instance
    {
        get
        {
            _instance ??= new Bot();
            return _instance;
        }
    }


    // Constructor
    private Bot()
    {
        Logger.Instance.LogInformation("Creating Bot...");

        bot = new Fluxify.Bot.Bot(cfg);

        // Register our handlers
        registerCommands();
        registerEventHandlers();
    }


    // Registers all commands
    private void registerCommands()
    {
        bot.Commands.Command("ping", (CommandContext ctx) => ctx.ReplyAsync("Pong!"));
    }


    // Registers handlers for all interesting events
    private void registerEventHandlers()
    {

    }


    // Starts the bot
    public async Task run()
    {
        Logger.Instance.LogInformation("Logging in...");

        await bot.RunAsync();
    }
}
