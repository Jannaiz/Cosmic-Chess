using System;
public static class HighMath
{
   

    public static int[] getPieceMathPos(Piece piece)
    {
        int[] mathPos = CombineDimention(piece.mathPos, removeLowerDimention(piece.getPlane().originMathPos));
        return mathPos;


    }

    /// <summary>
    /// Combine the lower 2D part and higher dimention part of Posistions
    /// </summary>
    /// <param name="LowerMathPos"> list of 2D Posistions</param>
    /// <param name="HighMathPos"><list of Posistions above 2D</param>
    /// <returns>The combined Posistions</returns>
    public static int[] CombineDimention(int[] LowerMathPos, int[] HighMathPos)
    {
        int[] FullMathPos = { LowerMathPos[0], LowerMathPos[1], HighMathPos[0], HighMathPos[1] };
        return FullMathPos;
    }


    public static int[] removeHigherDimention(int[] FullMathPos)
    {
        int[] LowerMathPos = { FullMathPos[0], FullMathPos[1] };
        return LowerMathPos;
    }

    public static int[] removeLowerDimention(int[] FullMathPos)
    {
        int[] HighMathPos = { FullMathPos[2], FullMathPos[3] };
        return HighMathPos;
    }



    public static int[,] multiplyMatrix(int[,] matrix1, int[,] matrix2)
    {

        int[,] result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];

        for(int i = 0; i < matrix1.GetLength(0); i++)
        {

            for (int j = 0; j < matrix2.GetLength(1); j++)
            {

                result[i, j] = 0;

                for (int n = 0; n < matrix1.GetLength(1); n++)
                {
                    result[i, j] = matrix1[i,n] * matrix2[n,j];

                }

            }
        }


        return result;
    }


    public static HighVector MapVectorTo(HighVector vector2D, dimention[] place, dimention higestDimention)
    {
        int[] numericDimentions = new int[place.Length];

        for (int i = 0; i < place.Length; i++)
        {
            numericDimentions[i] = (int)place[i];
        }


        MapVectorTo(vector2D, numericDimentions, higestDimention);


        return null;
    }


    public static HighVector MapVectorTo(HighVector vector2D, int[] place, dimention higestDimention)
    {
        HighVector mappedVector = new HighVector((int)higestDimention);

        mappedVector.endPoint[place[0]] = vector2D.endPoint[0];
        mappedVector.endPoint[place[1]] = vector2D.endPoint[1];


        return null;
    }


}
