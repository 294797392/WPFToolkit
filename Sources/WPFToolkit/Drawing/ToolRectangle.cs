using System.Windows;
using System.Windows.Input;
using System.IO;


namespace WPFToolkit.Drawing
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    class ToolRectangle : ToolRectangleBase
    {
        public ToolRectangle()
        {
            MemoryStream stream = new MemoryStream(Properties.Resources.Rectangle);
            ToolCursor = new Cursor(stream);
        }

        /// <summary>
        /// Create new rectangle
        /// </summary>
        public override void OnMouseDown(DrawingCanvas drawingCanvas, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(drawingCanvas);

            AddNewObject(drawingCanvas, 
                new GraphicsRectangle(
                p.X,
                p.Y,
                p.X + 1,
                p.Y + 1,
                drawingCanvas.LineWidth,
                drawingCanvas.ObjectColor,
                drawingCanvas.ActualScale));
        }
    }
}
