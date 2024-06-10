using System.Drawing.Printing;

namespace WinForms.HelperClasses;

public class PrintPageSetup
{
    private int _controlCounter;

    public void PrintControls(PrintPageEventArgs e, FlowLayoutPanel flowLayoutPanel)
    {
        if (_controlCounter == 0)
        {
            e.HasMorePages = false; 
        }

        var margin = 10; 
        var y = e.MarginBounds.Top;
        var x = e.MarginBounds.Left;

        for (var i = _controlCounter; i < flowLayoutPanel.Controls.Count; i++)
        {
            var control = flowLayoutPanel.Controls[i];
            var controlWidth = control.Width;
            var controlHeight = control.Height;

            // Wrap to the next row if the control exceeds the right margin
            if (x + controlWidth > e.MarginBounds.Right)
            {
                x = e.MarginBounds.Left;
                y += controlHeight + margin;
            }

            // Check if we have reached the end of the page
            if (y + controlHeight + margin > e.MarginBounds.Bottom)
            {
                e.HasMorePages = true;
                return;
            }

            var rectangle = new Rectangle(0, 0, controlWidth, controlHeight);
            var bitmap = new Bitmap(controlWidth, controlHeight);

            control.DrawToBitmap(bitmap, rectangle);
            e.Graphics.DrawImage(bitmap, new Point(x, y));

            // Move to the next position
            x += controlWidth + margin;
            _controlCounter++;
        }

        
        e.HasMorePages = false;
        _controlCounter = 0; 
    }
}