﻿using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serein
{
    class Command
    {
        public static string[] Sexs = { "unknown", "male", "female" };
        public static string[] Sexs_Chinese = { "未知", "男", "女" };
        public static string[] Roles = { "owner", "admin", "member" };
        public static string[] Roles_Chinese = { "群主", "管理员", "成员" };
        public static void StartCmd(string Command)
        {
            Process CMDProcess = new Process();
            CMDProcess.StartInfo.FileName = "cmd.exe";
            CMDProcess.StartInfo.UseShellExecute = false;
            CMDProcess.StartInfo.RedirectStandardInput = true;
            CMDProcess.StartInfo.CreateNoWindow = true;
            CMDProcess.Start();
            StreamWriter CommandWriter = new StreamWriter(CMDProcess.StandardInput.BaseStream, Encoding.Default);
            CommandWriter.AutoFlush = true;
            CommandWriter.NewLine = "\r\n";
            CommandWriter.WriteLine("chcp 936");
            CommandWriter.WriteLine($"cd \"{Global.Path}\"");
            CommandWriter.WriteLine("cls");
            CommandWriter.WriteLine(Command.TrimEnd('\r', '\n') + "&exit");
            CommandWriter.Close();
            CMDProcess.WaitForExit();
            CMDProcess.Close();
        }
        public static void Run(string Command, Match InputMatch = null)
        {
            int Type = GetType(Command);
            if (Type == -1)
            {
                return;
            }
            string Value = GetValue(Command, InputMatch);
            Value = GetVariables(Value);
            if (Type == 1)
            {
                Task CMDTask = new Task(() =>
                  {
                      StartCmd(Value);
                  });
                CMDTask.Start();
            }
            else if (Type == 2)
            {
                Server.InputCommand(Value, true);
            }
            else if (Type == 3 && Websocket.Status)
            {
                Websocket.Send(false, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 4 && Websocket.Status)
            {
                Websocket.Send(true, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 5 && Websocket.Status && Global.Settings_Bot.GroupList.Length >= 1)
            {
                Websocket.Send(false, Value, Global.Settings_Bot.GroupList[0].ToString());
            }
        }

        public static void Run(JObject JsonObject, string Command, Match MsgMatch, long UserId, long GroupId = -1)
        {
            int Type = GetType(Command);
            if (Type == -1)
            {
                return;
            }
            string Value = GetValue(Command, MsgMatch);
            Value = GetVariables(Value, JsonObject);
            if (Type == 1)
            {
                Task CMDTask = new Task(() =>
                {
                    StartCmd(Value);
                });
                CMDTask.Start();
            }
            else if (Type == 2)
            {
                Server.InputCommand(Value, true);
            }
            else if (Type == 3 && Websocket.Status)
            {
                Websocket.Send(false, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 4 && Websocket.Status)
            {
                Websocket.Send(true, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 5 && Websocket.Status)
            {
                Websocket.Send(false, Value, GroupId.ToString());
            }
            else if (Type == 6 && Websocket.Status)
            {
                Websocket.Send(true, Value, UserId.ToString());
            }
        }

        public static int GetType(string Command)
        {
            /*
            Type类型     描述
            -1          错误的、不合法的、未知的
            1           cmd
            2           服务器命令
            3           群聊（带参）
            4           私聊（带参）
            5           群聊
            6           私聊
            */
            if (!Command.Contains("|"))
            {
                return -1;
            }
            if (!Regex.IsMatch(Command, @"^.+?\|.+$", RegexOptions.IgnoreCase))
            {
                return -1;
            }
            if (Regex.IsMatch(Command, @"^cmd\|", RegexOptions.IgnoreCase))
            {
                return 1;
            }
            else if (Regex.IsMatch(Command, @"^s\|", RegexOptions.IgnoreCase) || Regex.IsMatch(Command, @"^server\|", RegexOptions.IgnoreCase))
            {
                return 2;
            }
            else if (Regex.IsMatch(Command, @"^g:\d+\|", RegexOptions.IgnoreCase) || Regex.IsMatch(Command, @"^group:\d+\|", RegexOptions.IgnoreCase))
            {
                return 3;
            }
            else if (Regex.IsMatch(Command, @"^p:\d+\|", RegexOptions.IgnoreCase) || Regex.IsMatch(Command, @"^private:\d+\|", RegexOptions.IgnoreCase))
            {
                return 4;
            }
            else if (Regex.IsMatch(Command, @"^g\|", RegexOptions.IgnoreCase) || Regex.IsMatch(Command, @"^group\|", RegexOptions.IgnoreCase))
            {
                return 5;
            }
            else if (Regex.IsMatch(Command, @"^p\|", RegexOptions.IgnoreCase) || Regex.IsMatch(Command, @"^private\|", RegexOptions.IgnoreCase))
            {
                return 6;
            }
            else
            {
                return -1;
            }
        }
        public static string GetValue(string command, Match MsgMatch)
        {
            int index = command.IndexOf('|');
            string Value = command.Substring(index + 1);
            if (MsgMatch == null)
            {
                return Value;
            }
            for (int i = MsgMatch.Groups.Count; i >= 0; i--)
            {
                Value = Value.Replace($"${i}", MsgMatch.Groups[i].Value);
            }
            return Value;
        }
        public static string GetVariables(string Text, JObject JsonObject = null)
        {
            if (!Text.Contains("%"))
            {
                return Text.Replace("\\n", "\n");
            }
            DateTime CurrentTime = DateTime.Now;
            Text = Regex.Replace(Text, "%Year%", CurrentTime.Year.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Month%", CurrentTime.Month.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Day%", CurrentTime.Day.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Hour%", CurrentTime.Hour.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Minute%", CurrentTime.Minute.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Second%", CurrentTime.Second.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%DateTime%", CurrentTime.ToString(), RegexOptions.IgnoreCase);
            if (JsonObject != null)
            {
                try
                {
                    Text = Regex.Replace(Text, "%Age%", JsonObject["sender"]["age"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%ID%", JsonObject["sender"]["user_id"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Area%", JsonObject["sender"]["area"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Card%", JsonObject["sender"]["card"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Level%", JsonObject["sender"]["level"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Nickname%", JsonObject["sender"]["nickname"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Title%", JsonObject["sender"]["title"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Role%", Roles_Chinese[Array.IndexOf(Roles, JsonObject["sender"]["role"].ToString())], RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Sex%", Sexs_Chinese[Array.IndexOf(Sexs, JsonObject["sender"]["sex"].ToString())], RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%ShownName%", string.IsNullOrEmpty(JsonObject["sender"]["card"].ToString()) ? JsonObject["sender"]["nickname"].ToString() : JsonObject["sender"]["card"].ToString(), RegexOptions.IgnoreCase);
                }
                catch
                {

                }
            }
            Text = Regex.Replace(Text, "%OS%", SystemInfo.OS, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%CPUName%", SystemInfo.CPUName, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%UsedRAM%", SystemInfo.UsedRAM, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%TotalRAM%", SystemInfo.TotalRAM, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%RAMPercentage%", SystemInfo.RAMPercentage, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%CPUPercentage%", SystemInfo.OS, RegexOptions.IgnoreCase);
            if (Server.Status)
            {
                Text = Regex.Replace(Text, "%LevelName%", Server.LevelName, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Version%", Server.Version, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Difficulty%", Server.Difficulty, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%RunTime%", Server.GetTime(), RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Percentage%", Server.CPUPersent.ToString("N1"), RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Status%", "已启动", RegexOptions.IgnoreCase);
            }
            else
            {
                Text = Regex.Replace(Text, "%LevelName%", "", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Version%", "", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Difficulty%", "", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%RunTime%", "-", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Percentage%", "-", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Status%", "未启动", RegexOptions.IgnoreCase);
            }
            return Text.Replace("\\n", "\n");
        }
    }
}
