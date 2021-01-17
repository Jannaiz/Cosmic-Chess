public interface IBehaviour 
{

    float[,] BasesPos
    {
        get;
    }

    float[] BaseTransformation(float[] Base, int n);



}