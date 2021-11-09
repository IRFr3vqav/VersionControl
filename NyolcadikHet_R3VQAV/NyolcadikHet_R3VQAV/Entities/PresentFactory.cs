using NyolcadikHet_R3VQAV.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyolcadikHet_R3VQAV.Entities
{
    public class PresentFactory: IToyFactory
    {
        public Color BoxColor { get; set; }
        public Color RibbonColor { get; set; }
        public Toy CreateNew()
        {
            return new Present(RibbonColor, BoxColor);
        }
    }
}
