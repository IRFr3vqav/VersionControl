using NyolcadikHet_R3VQAV.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyolcadikHet_R3VQAV.Entities
{
    public class CarFactory: IToyFactory
    {
        public Toy CreateNew()
        {
            return new Car();
        }
    }
}
