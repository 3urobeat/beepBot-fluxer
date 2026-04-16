/*
 * File: Command.cs
 * Project: beepBot-fluxer
 * Created Date: 2026-04-10 16:01:14
 * Author: 3urobeat
 *
 * Last Modified: 2026-04-16 21:28:46
 * Modified By: 3urobeat
 *
 * Copyright (c) 2026 3urobeat <https://github.com/3urobeat>
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.
 */


using Fluxify.Commands;


public enum ECommandPrivilege
{
    ALL,
    MODERATOR,
    ADMIN,
    BOTOWNER
}


public interface ICommandOption
{
    string name        { get; }    // Interface members are by default abstract and public
    string description { get; }
    bool   required    { get; }
    string prefix      { get; }
}


// Command Definition
public interface ICommand
{
    // Meta
    string[] names       { get; }
    string   description { get; }
    string   usage       { get; }
    bool     allowdInDm  { get; }

    ICommandOption[]  options   { get; }
    ECommandPrivilege privilege { get; }

    // Called by Fluxify when command with matching name got executed
    Task runAsync(CommandContext ctx);
}


// Provides command handling functions
public static class CommandHandler
{
    // Finds all classes implementing ICommand
    public static IEnumerable<Type> getAllCommands()
    {
        return typeof(ICommand).Assembly.GetTypes().Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
    }

    // Creates an instance of a Command type
    public static ICommand getCommandInstance(Type cmdType)
    {
        return (ICommand) Activator.CreateInstance(cmdType)!;
    }
}
