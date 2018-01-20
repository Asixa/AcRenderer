
namespace ACEngineDX.Base
{
    public class DxConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DxConfiguration"/> class.
        /// </summary>
        public DxConfiguration() : this("DxWindow")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DxConfiguration"/> class.
        /// </summary>
        public DxConfiguration(string title) : this(title, 800, 600)
        {
        }

        public DxConfiguration(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
            WaitVerticalBlanking = false;
        }

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        public int Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        public int Height
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [wait vertical blanking].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [wait vertical blanking]; otherwise, <c>false</c>.
        /// </value>
        public bool WaitVerticalBlanking
        {
            get; set;
        }
    }
}
