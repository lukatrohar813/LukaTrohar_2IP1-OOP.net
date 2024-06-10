namespace WinForms.HelperClasses
{

    public class CloseConfirmation
    {
       
        public static void ConfirmFormClose(FormClosingEventArgs e)
        {

          

                if (MessageBox.Show(Resources.Resources.exitConfirmationBody, Resources.Resources.exitConfirmationTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            
           

        }

    }
}
