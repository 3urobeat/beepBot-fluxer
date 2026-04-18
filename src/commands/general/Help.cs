/*
 * File: Help.cs
 * Project: beepBot-fluxer
 * Created Date: 2026-04-16 21:35:11
 * Author: 3urobeat
 *
 * Last Modified: 2026-04-18 23:20:19
 * Modified By: 3urobeat
 *
 * Copyright (c) 2026 3urobeat <https://github.com/3urobeat>
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
 * You should have received a copy of the GNU Affero General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.
 */


using Fluxify.Commands;
using Fluxify.Application.Model.Messages;

using static CommandHandler;


public class CommandHelp : ICommand
{
    Bot bot = Bot.Instance;

    public string[]         names       => [ "help", "h", "commands" ];
    public ECommandCategory category    => ECommandCategory.GENERAL;
    public string           usage       => "[cmdName]";
    public bool             allowdInDm  => true;

    public ICommandOption[]  options    => [
        new ICommandOption("cmdName", false, "")
    ];
    public ECommandPrivilege privilege  => ECommandPrivilege.ALL;


    public async Task runAsync(CommandContext ctx)
    {
        var lang   = this.bot.getGuildLang(ctx.Guild);
        var prefix = this.bot.getGuildPrefix(ctx.Guild);

        // Get all commands, to either list all or query for one specific command
        var allCommands = getAllCommands().Select(t => getCommandInstance(t));

        // Pre-Construct message
        var embed = new EmbedBuilder();


        // Attempt to get first parameter and show detailed information
        string cmdNameParam = ctx.Reader.GetNext<string>();

        if (cmdNameParam.Length > 0) // Specific command help: Search for matching command with cmdName param
        {
            var targetCommand = allCommands.ToList().FirstOrDefault(c => c.names.Contains(cmdNameParam!));

            if (targetCommand == null) // Command not found
            {
                embed
                    .WithTitle(lang.error ?? "Error")
                    .WithDescription((lang.cmd["help"].AdditionalProperties?["cmdNotFound"].GetString() ?? "Command '{cmdName}' not found.").Replace("{cmdName}", cmdNameParam))
                    .WithColor(System.Drawing.Color.Red)
                    .Build();
                await ctx.Message.ReplyAsync(new MessageCreate { Embeds = [embed.Build()] });
                return;
            }

            // Command was found, construct message
            var primaryName = targetCommand.names[0];
            var cmdLang     = lang.cmd.TryGetValue(primaryName, out var langData) ? langData : null;

            embed
                .WithTitle($"`{primaryName}`")
                .WithDescription(cmdLang?.description ?? "");

            // List aliases if applicable
            if (targetCommand.names.Length > 1)
            {
                embed.WithField(lang.aliases, string.Join(", ", targetCommand.names.Skip(1).Select(n => $"`{n}`")));
            }

            embed.WithField(lang.usage, $"`{prefix}{primaryName} {targetCommand.usage}`");

            // Show parameters if the command has any
            if (targetCommand.options.Length > 0 && cmdLang?.@params != null)
            {
                var paramsInfo = string.Join("\n", targetCommand.options.Select(opt =>
                {
                    var paramLang = cmdLang.@params.TryGetValue(opt.optionName, out var p) ? p : null;
                    var required = opt.required ? $"({lang.required})" : $"({lang.optional})";

                    return $"`{opt.optionName}` {required} - {paramLang?.description ?? ""}";
                }));

                embed.WithField(lang.parameters, paramsInfo);
            }

            await ctx.Message.ReplyAsync(new MessageCreate { Embeds = [embed.Build()] });
        }
        else // General help: show all commands grouped by category
        {
            embed.WithTitle(lang.cmd["help"].AdditionalProperties?["commandList"].GetString() ?? "Command List");

            // Build field value with all commands in this category
            foreach (var group in allCommands.GroupBy(c => c.category))
            {
                var fieldValue = string.Join("\n", group.Select(cmd =>
                {
                    var name = cmd.names[0];
                    var description = lang.cmd.TryGetValue(name, out var cmdLang) ? cmdLang.description : "";

                    return $"`{prefix}{name}` - {description}";
                }));

                embed.WithField(group.Key.ToString(), fieldValue);
            }

            await ctx.Message.ReplyAsync(new MessageCreate { Embeds = [embed.Build()] });
        }
    }
}
