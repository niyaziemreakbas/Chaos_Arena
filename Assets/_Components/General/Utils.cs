using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Verilen Transform nesnesinin en üst parent Transform'unu döner.
    /// </summary>
    public static Transform GetRootParent(Transform obj)
    {
        if (obj == null) return null;

        while (obj.parent != null)
        {
            obj = obj.parent;
        }
        return obj;
    }
}
