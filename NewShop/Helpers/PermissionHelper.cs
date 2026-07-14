using System.Collections.Generic;
using System.Web;

public static class PermissionHelper
{
    public static bool HasPermission(string permission) {
        var permissions = HttpContext.Current.Session["Permissions"] as List<string>;

        if (permissions == null) {
            return false;
        }

        return permissions.Contains(permission);
    }
}