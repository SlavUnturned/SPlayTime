using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Core.Utils;

namespace SPlayTime;

public class Command : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Both;
    public string Name => "playtime";
    public string Help => $"/{Name}";
    public string Syntax => Help;
    public List<string> Aliases => new();
    public List<string> Permissions => new() { Name };

    public void Execute(IRocketPlayer caller, string[] command)
    {
#if !DEBUG
        Task.Run(() => {
#endif
            var arg = string.Join(" ", command);
            var target = caller;
            var color = Color.green;
            var msg = "";
            if (caller.HasPermission($"{Name}.other") && !string.IsNullOrWhiteSpace(arg))
            {
                target = UP.FromName(arg);
                if (target is null && ulong.TryParse(arg, out var id))
                    target = new RocketPlayer(arg, arg);
            }
            if(target is not null)
                msg = GetMessage(caller, target);
            else
            {
                msg = TranslatePlayerNotFound(arg);
                color = Color.red;
            }
            if (string.IsNullOrWhiteSpace(msg))
                return;
            TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, msg, color, true));
#if !DEBUG
        });
#endif
    }
    static string GetMessage(IRocketPlayer caller, IRocketPlayer target)
    {
        var seconds = Main.DBConnector.GetSeconds(target.Id);
        var time = TimeSpan.FromSeconds(seconds);
        var format = TranslateTimeFormat();
        var formattedTime = time.ToString(format);
        return caller.Id == target.Id ?
            TranslateMyPlaytimeFormat(formattedTime) :
            TranslateOtherPlaytimeFormat(target.DisplayName, formattedTime);
    }
}
public static partial class Translations
{
    public const string 
        MyPlaytimeFormat = "Your playtime is: {0}.",
        OtherPlaytimeFormat = "Playtime of '{0}' is: {1}.",
        PlayerNotFound = "Player '{0}' is not found.",
        TimeFormat = @"dd' Days 'hh' Hours 'mm' Minutes 'ss' Seconds'";
}
