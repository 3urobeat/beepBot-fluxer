/*
 * File: Config.cs
 * Project: beepBot-fluxer
 * Created Date: 2026-04-09 22:46:12
 * Author: 3urobeat
 *
 * Last Modified: 2026-04-10 15:41:34
 * Modified By: 3urobeat
 *
 * Copyright (c) 2026 3urobeat <https://github.com/3urobeat>
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.
 */


using System.Text.Json;
using Microsoft.Extensions.Logging;


// Config Singleton
sealed class Config
{
    private static Config? _instance;
    public ConfigItems items;


    // (Creates and) Returns Singleton instance
    public static Config Instance
    {
        get
        {
            _instance ??= new Config();
            return _instance!;
        }
    }


    // Constructor
    private Config()
    {
        Logger.Instance.LogInformation("Loading configuration file...");

        // Load file content
        string text = File.ReadAllText("data/config.json");
        if (text == null) throw new Exception("Failed to read config file!");

        // Parse content into items
        items = JsonSerializer.Deserialize<ConfigItems>(text)!;

        Logger.Instance.LogTrace("Configuration File loaded!");
    }

    // Dummy function to "call" constructor via Instance
    public void create() {}
}


// Items in config file
class ConfigItems
{
    public string? version { get; set; }
    public string? status { get; set; }
    public string? gametype { get; set; }
    public string[]? gamerotation { get; set; }
    public int? gamerotateseconds { get; set; }
    public string? gameoverwrite { get; set; }
}
