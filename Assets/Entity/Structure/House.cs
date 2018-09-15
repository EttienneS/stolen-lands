using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class House : Structure
{
    public override void Init()
    {
        AddTrait(new Sighted(1));
        RotateOnZ();
    }
}
