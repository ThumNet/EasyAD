using System;
using System.Net.Http.Formatting;
using umbraco.BusinessLogic.Actions;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using Umbraco.Core.Services;
using ThumNet.EasyAD.Controllers;

namespace ThumNet.EasyAD.Trees
{
    [Tree(Constants.Applications.Users, alias: AppConstants.Trees.EasyADGroups, title: "AD Groups", iconClosed: "icon-users", iconOpen: "icon-users")]
    [PluginController(AppConstants.PluginControllerName)]
    public class EasyADTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            // check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {
                var tree = new TreeNodeCollection();
                var ctrl = new EasyADApiController();

                foreach (var g in ctrl.GetAll())
                {
                    var node = CreateTreeNode(
                                    g.Id.ToInvariantString(),
                                    Constants.System.Root.ToInvariantString(),
                                    queryStrings,
                                    g.Name,
                                    "icon-users-alt");
                    tree.Add(node);
                }

                return tree;
            }

            // this tree doesn't support rendering more than 1 level
            throw new NotSupportedException();
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();

            if (id == Constants.System.Root.ToInvariantString())
            {
                menu.Items.Add<CreateChildEntity, ActionNew>(Services.TextService.Localize(string.Format("actions/{0}", ActionNew.Instance.Alias)));
                menu.Items.Add<RefreshNode, ActionRefresh>(Services.TextService.Localize(string.Format("actions/{0}", ActionRefresh.Instance.Alias)), hasSeparator: true);
            }
            else
            {
                menu.Items.Add<ActionDelete>(Services.TextService.Localize(string.Format("actions/{0}", ActionDelete.Instance.Alias)));
            }

            return menu;
        }
    }
}
