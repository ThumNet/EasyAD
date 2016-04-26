using ClientDependency.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Models;
using ThumNet.EasyAD.ViewModels;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using ThumNet.EasyAD.Repositories;
using ThumNet.EasyAD.Handlers;

namespace ThumNet.EasyAD.Controllers
{
    [PluginController(AppConstants.PluginControllerName)]
    public class EasyADApiController : UmbracoAuthorizedJsonController
    {
        public OptionsViewModel GetOptions()
        {
            return new OptionsViewModel
            {
                AllUserTypes = Services.UserService.GetAllUserTypes().ToDictionary(t => t.Id, t => t.Name),
                AllSections = Services.SectionService.GetSections().ToDictionary(t => t.Alias, t => t.Name)
            };
        }

        // sample url: /umbraco/backoffice/EasyAD/EasyADApi/GetById?groupId=1
        public EasyADGroup GetById(int id)
        {
            var repo = new EasyADRepository(DatabaseContext.Database, DatabaseContext.SqlSyntax);
            return repo.GetById(id);
        }

        public IEnumerable<EasyADGroup> GetAll()
        {
            var repo = new EasyADRepository(DatabaseContext.Database, DatabaseContext.SqlSyntax);
            return repo.GetAll();
        }

        public int DeleteById(int id)
        {
            var groupManager = ManagerFactory.GetManager();
            var repo = new EasyADRepository(DatabaseContext.Database, DatabaseContext.SqlSyntax);
            var handler = new DeleteGroupHandler(repo, groupManager, Services.UserService);
            return handler.Handle(id);
        }

        public GroupSaveViewModel PostSave(EasyADGroup group)
        {
            var groupManager = ManagerFactory.GetManager();

            string message = "";

            if (!ValidateGroup(group.Name, groupManager, ref message))
            {
                return FailedSave(message);
            }

            if (!ValidUserType(group.UserType))
            {
                return FailedSave("Invalid User Type!");
            }

            if (!ValidateSections(group.Sections, ref message))
            {
                return FailedSave(message);
            }

            var repo = new EasyADRepository(DatabaseContext.Database, DatabaseContext.SqlSyntax);
            var handler = new SaveGroupHandler(repo, groupManager, Services.UserService);
            var groupId = handler.Handle(group);
            return new GroupSaveViewModel
            {
                GroupId = groupId,
                Success = true,
                Message = "Group created!"
            };
        }

        private bool ValidateGroup(string name, IGroupBasedUserManager groupManager, ref string message)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                message = "Group name is required!";
                return false;
            }

            bool groupExists = groupManager.GroupExists(name);
            if (!groupExists)
            {
                message = string.Format("Group with name '{0}' not found in AD!", name);
                return false;
            }

            return true;
        }

        private bool ValidUserType(int userType)
        {
            return Services.UserService.GetAllUserTypes().Any(u => u.Id == userType);
        }

        private bool ValidateSections(string sections, ref string message)
        {
            sections = sections ?? string.Empty;
            var groupSections = sections.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (groupSections.Length == 0)
            {
                message = "At least one sections is required!";
                return false;
            }

            var allSections = Services.SectionService.GetSections().Select(s => s.Alias);
            var validSections = groupSections.All(s => allSections.Contains(s));
            if (!validSections)
            {
                message = "Invalid section selected!";
                return false;
            }

            return true;
        }

        private GroupSaveViewModel FailedSave(string message)
        {
            return new GroupSaveViewModel
            {
                Success = false,
                Message = message
            };
        }
    }
}