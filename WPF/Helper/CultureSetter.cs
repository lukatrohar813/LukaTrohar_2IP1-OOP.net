using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using WPF.Helper;
using System.Threading.Tasks;

namespace WPF.Helper
{
    public class CultureSetter
    {
        const string DefaultLanguage = "en";
        


        public void SetCulture(IFileRepository repo)
        {
            try
            {
                var language = repo.GetLanguage();
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            }
            catch
            {
               
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(DefaultLanguage);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(DefaultLanguage);
            }
        }
    }
}
