/*
 * File: Lang.cs
 * Project: beepBot-fluxer
 * Created Date: 2026-04-10 15:09:22
 * Author: 3urobeat
 *
 * Last Modified: 2026-04-18 19:02:12
 * Modified By: 3urobeat
 *
 * Copyright (c) 2026 3urobeat <https://github.com/3urobeat>
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.
 */


using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;


// Supported languages
enum Lang
{
    EN,
    DE
}


// Lang Singleton
sealed class I18n
{
    private static I18n? _instance;

    // Loaded languages
    readonly LangItems langEn;
    readonly LangItems langDe;


    // (Creates and) Returns Singleton instance
    public static I18n I
    {
        get
        {
            _instance ??= new I18n();
            return _instance!;
        }
    }


    // Constructor
    private I18n()
    {
        Logger.Instance.LogInformation("Loading language file...");

        // Load language files
        langEn = JsonSerializer.Deserialize<LangItems>(File.ReadAllText("src/lang/languages/en.json"))!;
        langDe = JsonSerializer.Deserialize<LangItems>(File.ReadAllText("src/lang/languages/de.json"))!;

        Logger.Instance.LogTrace("Languages loaded!");
    }


    // Dummy function to "call" constructor via Instance
    public void create() {}


    // Get a language
    public LangItems get(Lang key)
    {
        return key switch
        {
            Lang.EN => langEn,
            Lang.DE => langDe,
            _ => throw new Exception("Unsupported language"),
        };
    }

}


// Items present in a language file
public class LangItems
{
    public Dictionary<string, CommandLang> cmd { get; set; } = new();

    public string error { get; set; } = "";
}

public class CommandLang
{
    public string description { get; set; } = "";
    public Dictionary<string, ParamLang>? @params { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
}

public class ParamLang
{
    public string name { get; set; } = "";
    public string description { get; set; } = "";
}
