using System;
using System.ComponentModel;

namespace USALauncher;

public static class ISynchronizeInvokeExtensions
{
    public static void InvokeEx<T>(this T @this, Action<T> action) where T : ISynchronizeInvoke
    {
        if (@this.InvokeRequired)
        {
            @this.Invoke(action, new object[1] { @this });
        }
        else
        {
            action(@this);
        }
    }
}
