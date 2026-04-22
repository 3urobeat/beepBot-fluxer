/*
 * File: Avatar.cs
 * Project: beepBot-fluxer
 * Created Date: 2026-04-18 23:25:49
 * Author: 3urobeat
 *
 * Last Modified: 2026-04-22 19:49:51
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
using Fluxify.Application.Entities.Users;
using Microsoft.Extensions.Logging;
using Fluxify.Commands.TextCommand;


public class CommandAvatar : ICommand
{
    Bot bot = Bot.Instance;

    public string[]         names       => [ "avatar", "useravatar" ];
    public ECommandCategory category    => ECommandCategory.INFO;
    public string           usage       => "[mention/username/userid]";
    public bool             allowdInDm  => true;

    public ICommandOption[]  options    => [
        new ICommandOption("user", false, "")
    ];
    public ECommandPrivilege privilege  => ECommandPrivilege.ALL;


    public async Task runAsync(CommandContext ctx)
    {
        var   lang       = bot.getGuildLang(ctx.Guild!);
        IUser avatarUser = ctx.Author;

        // Attempt to parse arguments to get avatar from non-author
        bool mentionParam = ctx.Reader.TryGetNext<Mentionable.User>(out var userMention);
        bool strParam     = ctx.Reader.TryGetNext<string>(out var nameOrIdStr);

        if (mentionParam)   // Is a mention?
        {
            Logger.Instance.LogDebug("Avatar: Is mention");
            var member = ctx.Guild!.Members.FirstOrDefault(m => m.Id == userMention!.Id);
            if (member != null) avatarUser = member;
        }
        else if (strParam)  // Is a username/id?
        {
            // Attempt to parse value as long and treat it as an ID, if not search guild for username
            if (ulong.TryParse(nameOrIdStr, out var userId))
            {
                Logger.Instance.LogDebug("Avatar: Is ID");
                var member = ctx.Guild!.Members.FirstOrDefault(m => m.Id == userId);
                if (member != null) avatarUser = member;
            }
            else
            {
                Logger.Instance.LogDebug("Avatar: Is username");
                var member = ctx.Guild!.Members.FirstOrDefault(m => m.DisplayName.Contains(nameOrIdStr!, StringComparison.OrdinalIgnoreCase));
                if (member != null) avatarUser = member;
            }
        }

        // Construct response
        var url   = avatarUser.GetAvatarUri().ToString().Replace("size=128", "size=512"); // Hotfix: Why is the size 128 by default when 512 is supported?
        var embed = new EmbedBuilder();

        embed
            .WithTitle(lang.cmd["avatar"].AdditionalProperties?["openInBrowser"].GetString() ?? "Click here to open the image in your browser")
            .WithUrl(url)
            .WithImage(url)
            .Build();

        await ctx.ReplyAsync(new MessageCreate { Embeds = [embed.Build()] });
    }
}
