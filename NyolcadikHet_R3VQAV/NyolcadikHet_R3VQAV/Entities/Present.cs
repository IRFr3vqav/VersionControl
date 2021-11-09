using NyolcadikHet_R3VQAV.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyolcadikHet_R3VQAV.Entities
{
    public class Present: Toy
    {
        public SolidBrush BoxColor { get; private set; }
        public SolidBrush RibbonColor { get; private set; }
        public Present(Color ribbon, Color box)
        {
            BoxColor = new SolidBrush(box);
            RibbonColor = new SolidBrush(ribbon);
        }
        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(BoxColor, 0, 0, Width, Height);
            g.FillRectangle(RibbonColor, Width/2-(Width/5)/2, 0, Width / 5, Height);
            g.FillRectangle(RibbonColor, 0, Height/2 - (Height / 5) / 2, Width, Height/5);
        }
    }
}
