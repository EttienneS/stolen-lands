using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Mill : Structure
{
    public override void Init()
    {
        AddTrait(new Sighted(1));
        RotateOnZ();
    }
}
