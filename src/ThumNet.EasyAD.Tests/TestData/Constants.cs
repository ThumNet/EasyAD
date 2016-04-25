using System.Collections.Generic;
using System.Linq;
using ThumNet.EasyAD.Models;
using Umbraco.Core.Models.Membership;

namespace ThumNet.EasyAD.UnitTests.TestData
{
    internal static class Constants
    {

        internal class ADUsers
        {
            internal static EasyADUser JohnsMi = new EasyADUser { DiplayName = "Michael Johnson", Login = "JohnsMi", Email = "JohnsMi@test.local" };
            internal static EasyADUser WilliJo = new EasyADUser { DiplayName = "Joshua Williams", Login = "WilliJo", Email = "WilliJo@test.local" };
            internal static EasyADUser JonesMa = new EasyADUser { DiplayName = "Matthew Jones", Login = "JonesMa", Email = "JonesMa@test.local" };
            internal static EasyADUser BrownEt = new EasyADUser { DiplayName = "Ethan Brown", Login = "BrownEt", Email = "BrownEt@test.local" };
            internal static EasyADUser DavisAn = new EasyADUser { DiplayName = "Andrew Davis", Login = "DavisAn", Email = "DavisAn@test.local" };
            internal static EasyADUser MilleDa = new EasyADUser { DiplayName = "Daniel Miller", Login = "MilleDa", Email = "MilleDa@test.local" };
            internal static EasyADUser WilsoAn = new EasyADUser { DiplayName = "Anthony Wilson", Login = "WilsoAn", Email = "WilsoAn@test.local" };
            internal static EasyADUser MooreCh = new EasyADUser { DiplayName = "Christopher Moore", Login = "MooreCh", Email = "MooreCh@test.local" };
            internal static EasyADUser TayloJo = new EasyADUser { DiplayName = "Joseph Taylor", Login = "TayloJo", Email = "TayloJo@test.local" };
            internal static EasyADUser BrookJe = new EasyADUser { DiplayName = "Jesus Brooks", Login = "BrookJe", Email = "BrookJe@test.local" };
            internal static EasyADUser ColemCh = new EasyADUser { DiplayName = "Chase Coleman", Login = "ColemCh", Email = "ColemCh@test.local" };
            internal static EasyADUser FloreJe = new EasyADUser { DiplayName = "Jesse Flores", Login = "FloreJe", Email = "FloreJe@test.local" };
            internal static EasyADUser ButleSe = new EasyADUser { DiplayName = "Seth Butler", Login = "ButleSe", Email = "ButleSe@test.local" };
            internal static EasyADUser LuedkAr = new EasyADUser { DiplayName = "Ariana Luedke", Login = "LuedkAr", Email = "LuedkAr@test.local" };
            internal static EasyADUser MetzgMa = new EasyADUser { DiplayName = "Marissa Metzger", Login = "MetzgMa", Email = "MetzgMa@test.local" };
            internal static EasyADUser MetzlAu = new EasyADUser { DiplayName = "Autumn Metzler", Login = "MetzlAu", Email = "MetzlAu@test.local" };
            internal static EasyADUser MuhlbGr = new EasyADUser { DiplayName = "Gracie Muhlberg", Login = "MuhlbGr", Email = "MuhlbGr@test.local" };
            internal static EasyADUser NergeMy = new EasyADUser { DiplayName = "Mya Nerger", Login = "NergeMy", Email = "NergeMy@test.local" };
        }

        internal class GroupNames
        {
            internal static string Developers = "Developers";
            internal static string Testers = "Testers";
            internal static string Operators = "Operators";
            internal static string Writers = "Writers";
            internal static string Approvers = "Approvers";
        }

        internal class UserTypes
        {
            internal static IUserType Administrator = new TestUserType { Alias = "admin", Id = 1, Name = "Administrator", Permissions = "F;:;C;5;4;Z;D;M;O;S;R;P;K;A;U;H;I;7;Z".Split(new[] { ';' }).ToList() };
            internal static IUserType Editor = new TestUserType { Alias = "editor", Id = 2, Name = "Editor", Permissions = "Z;F;O;D;M;C;P;U;K;5;S;A".Split(new[] { ';' }).ToList() };
            internal static IUserType Writer = new TestUserType { Alias = "writer", Id = 3, Name = "Writer", Permissions = "F;C;H;A".Split(new[] { ';' }).ToList() };
            internal static IUserType Translator = new TestUserType { Alias = "translator", Id = 4, Name = "Translator", Permissions = "F;A".Split(new[] { ';' }).ToList() };

            internal static IEnumerable<IUserType> All = new List<IUserType> { Administrator, Editor, Writer, Translator };

        }

        internal class BackofficeGroups
        {
            internal static EasyADGroup Developers = new EasyADGroup { Id = 1, Name = GroupNames.Developers, Sections = "content;media;settings;developer;members;forms;translation", UserType = UserTypes.Editor.Id };
            internal static EasyADGroup Operators = new EasyADGroup { Id = 2, Name = GroupNames.Operators, Sections = "content;media;settings;developer;users;members;forms;translation", UserType = UserTypes.Administrator.Id };
            internal static EasyADGroup Testers = new EasyADGroup { Id = 3, Name = GroupNames.Testers, Sections = "content;media;members;forms;translation", UserType = UserTypes.Editor.Id };
            internal static EasyADGroup Writers = new EasyADGroup { Id = 4, Name = GroupNames.Writers, Sections = "content;media", UserType = UserTypes.Writer.Id };
            internal static EasyADGroup Approvers = new EasyADGroup { Id = 5, Name = GroupNames.Approvers, Sections = "content;media;translation", UserType = UserTypes.Editor.Id };
        }

        internal class BackofficeUsers
        {
            internal static IUser JohnsMiAsDeveloper = new TestUser { Id = 101, Username = ADUsers.JohnsMi.Login, UserType = UserTypes.Editor, Sections = BackofficeGroups.Developers.Sections.Split(new[] { ';' }).ToList() };
            internal static IUser NergeMyAsDeveloper = new TestUser { Id = 102, Username = ADUsers.NergeMy.Login, UserType = UserTypes.Editor, Sections = BackofficeGroups.Developers.Sections.Split(new[] { ';' }).ToList() };
            internal static IUser ButleSeAsDeveloper = new TestUser { Id = 103, Username = ADUsers.ButleSe.Login, UserType = UserTypes.Editor, Sections = BackofficeGroups.Developers.Sections.Split(new[] { ';' }).ToList() };

            internal static IUser MuhlbGrAsTester = new TestUser  { Id = 201, Username = ADUsers.MuhlbGr.Login, UserType = UserTypes.Editor, Sections = BackofficeGroups.Testers.Sections.Split(new[] { ';' }).ToList() };

            internal static IUser BrookJeAsWriter = new TestUser { Id = 301, Username = ADUsers.BrookJe.Login, UserType = UserTypes.Writer, Sections = BackofficeGroups.Writers.Sections.Split(new[] { ';' }).ToList() };

            
        }
    }
}
