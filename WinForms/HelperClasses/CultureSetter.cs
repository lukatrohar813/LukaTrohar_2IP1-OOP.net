using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace WinForms.HelperClasses;

internal static class CultureSetter
{
    public static void SetFormCulture(string language, Type formType, Control.ControlCollection controls)
    {
        var culture = new CultureInfo(language);
        Thread.CurrentThread.CurrentUICulture = culture;

        var resourceManager = new ComponentResourceManager(formType);
        ApplyResources(resourceManager, controls, culture);
    }

    private static void ApplyResources(ComponentResourceManager resourceManager, Control.ControlCollection controls, CultureInfo culture)
    {
        foreach (Control control in controls)
        {
            resourceManager.ApplyResources(control, control.Name, culture);
            Debug.WriteLine($"Setting {control.Name} to {resourceManager.GetString($"{control.Name}.Text", culture)}");

            if (control.HasChildren)
            {
                ApplyResources(resourceManager, control.Controls, culture);
            }
        }
    }
}