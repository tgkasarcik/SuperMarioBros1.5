using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
    public interface ISimpleMovementType
    {
        //Implementing object has simpler movement functionality, and therefore just physics will apply
        IPhysicsX xPhysics { get; set; }
        IPhysicsY yPhysics { get; set; }
    }

    public interface IAdvancedMovementType
    {
        //Implementing object has more advanced movement functionality, and therefore it must have an xMove and yMove state
        IXMove xMove { get; set; }
        IYMove yMove { get; set; }
    }
}
