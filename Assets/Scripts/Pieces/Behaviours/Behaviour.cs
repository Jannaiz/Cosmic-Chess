using System;
using System.Collections.Generic;

public abstract class Behaviour
{

    

    protected Behaviour()
    {


    }


    public abstract List<int[]> getGhostPos(int[] mathPos);
}
