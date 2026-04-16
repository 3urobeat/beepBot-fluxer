/*
 * File: Ping.cs
 * Project: beepBot-fluxer
 * Created Date: 2026-04-15 21:25:31
 * Author: 3urobeat
 *
 * Last Modified: 2026-04-16 21:13:51
 * Modified By: 3urobeat
 *
 * Copyright (c) 2026 3urobeat <https://github.com/3urobeat>
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
 * You should have received a copy of the GNU Affero General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.
 */


using Fluxify.Commands;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;


public class CommandPing : ICommand
{
    Bot bot = Bot.Instance;

    public string[] names       => [ "ping", "pong" ];
    public string   description => "";
    public string   usage       => "";
    public bool     allowdInDm  => true;

    public ICommandOption[] options => [];

    public ECommandPrivilege privilege => ECommandPrivilege.ALL;


    public async Task runAsync(CommandContext ctx)
    {
        // Send response message
        var messageCreate = new MessageBuilder()
            .WithEmbed(embed => embed
                .WithTitle("Ping?")
                .WithColor(System.Drawing.Color.FromArgb(0xFFA500))
            )
            .Build();

        var sentMessage = await ctx.Message.ReplyAsync(messageCreate);

        // Get our own message, calculate ping by comparing timestamps and edit our message with the result
        if (sentMessage != null)
        {
            var latency = sentMessage.CreatedAt - ctx.Message.CreatedAt;

            await sentMessage.EditAsync(edit =>
            {
                edit.Embeds = new[]
                {
                    new EmbedBuilder()
                        .WithTitle("Pong!")
                        .WithDescription($":ping_pong: {latency.TotalMilliseconds:F2}ms") // TODO: :heartbeat: _lastServerHeartbeatEvent
                        .WithColor(System.Drawing.Color.FromArgb(0x32CD32))
                        .Build()
                };
            });
        }
    }
}
