using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using umbraco.BasePages;
using umbraco.businesslogic;
using umbraco.BusinessLogic;
using umbraco.BusinessLogic.Actions;
using umbraco.cms.presentation.Trees;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Configuration;

namespace ThumNet.EasyAD.DemoSite.Test
{
    //[Tree(Constants.Applications.Users, "users", "Users")]
    //public class CustomUsersTree : umbraco.loadUsers
    //{

    //    public override void Render(ref XmlTree tree)
    //    {
    //        var users = new List<User>(User.getAll());

    //        User currUser = UmbracoEnsuredPage.CurrentUser;

    //        bool currUserIsAdmin = currUser.IsAdmin();
    //        foreach (User u in users.OrderBy(x => x.Disabled))
    //        {
    //            if (!UmbracoConfig.For.UmbracoSettings().Security.HideDisabledUsersInBackoffice
    //                || (UmbracoConfig.For.UmbracoSettings().Security.HideDisabledUsersInBackoffice && !u.Disabled))
    //            {

    //                XmlTreeNode xNode = XmlTreeNode.Create(this);

    //                // special check for ROOT user
    //                if (u.Id == 0)
    //                {
    //                    //if its the administrator, don't create a menu
    //                    xNode.Menu = null;
    //                    //if the current user is not the administrator, then don't add this node.
    //                    if (currUser.Id != 0)
    //                        continue;
    //                }
    //                // Special check for admins in general (only show admins to admins)
    //                else if (!currUserIsAdmin && u.IsAdmin())
    //                {
    //                    continue;
    //                }





    //                //xNode.IconClass = "umbraco-tree-icon-grey";

    //                xNode.NodeID = u.Id.ToString();
    //                xNode.Text = u.Name;
    //                xNode.Action = "javascript:openUser(" + u.Id + ");";
    //                xNode.Icon = "icon-user2";
    //                xNode.OpenIcon = "icon-user2";

    //                if (u.Disabled)
    //                {
    //                    xNode.Style.DimNode();
    //                }

    //                OnBeforeNodeRender(ref tree, ref xNode, EventArgs.Empty);
    //                if (xNode != null)
    //                {
    //                    tree.Add(xNode);
    //                    OnAfterNodeRender(ref tree, ref xNode, EventArgs.Empty);
    //                }


    //            }


    //        }
    //    }

    //}
}