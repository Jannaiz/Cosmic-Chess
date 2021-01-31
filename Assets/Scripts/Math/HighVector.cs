using System;
public class HighVector
{

    public int[] endPoint;



    public HighVector(dimention higestDimention)
    {

        int size = (int)higestDimention + 1;
        endPoint = new int[size];

    }

    public HighVector(int size)
    {
       endPoint = new int[size];

    }


    public double getLenght()
    {
        double sum = 0;

        foreach(int component in endPoint)
        {
            sum += (double)Math.Pow(component, 2);
        }

        return Math.Sqrt(sum);
    }




    public int getComponent(dimention componetToGet)
    {
        return getComponent((int)componetToGet);
    }






    public int getComponent(int componetNumberToGet)
    {
        if(componetNumberToGet >= endPoint.Length)
        {
            return 0;
        }
        return endPoint[componetNumberToGet];
    }



}
