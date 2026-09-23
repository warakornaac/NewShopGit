using System.Collections.Generic;
using System.Web;

public static class PermissionHelper
{
    public static bool HasPermission(string permission) {
        var context = HttpContext.Current;

        if (context == null || context.Session == null)
        {
            return false;
        }

        var permissions = context.Session["Permissions"] as List<string>;

        if (permissions == null) {
            return false;
        }

        return permissions.Contains(permission);
    }
}