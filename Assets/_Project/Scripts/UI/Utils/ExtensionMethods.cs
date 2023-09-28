using UnityEngine;

public static class ExtensionMethods
{
    public static void ReleaseItem(this Component c)
    {
        GenericPool.ReleaseItem(c.GetType(), c);
    }
}