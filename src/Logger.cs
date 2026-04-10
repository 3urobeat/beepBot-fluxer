/*
 * File: Logger.cs
 * Project: beepBot-fluxer
 * Created Date: 2026-04-09 22:46:32
 * Author: 3urobeat
 *
 * Last Modified: 2026-04-10 14:54:10
 * Modified By: 3urobeat
 *
 * Copyright (c) 2026 3urobeat <https://github.com/3urobeat>
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.
 */


using Microsoft.Extensions.Logging;

using LoggerType = Microsoft.Extensions.Logging.ILogger;


// Logger Singleton
sealed class Logger
{
    private static LoggerType? _instance;


    // (Creates and) Returns Singleton instance
    public static LoggerType Instance
    {
        get
        {
            if (_instance == null) new Logger();
            return _instance!;
        }
    }


    // Constructor
    private Logger()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(LogLevel.Trace);
        });

        _instance = loggerFactory.CreateLogger<Program>();

        _instance.LogTrace("Logger was instantiated!");
    }
}
