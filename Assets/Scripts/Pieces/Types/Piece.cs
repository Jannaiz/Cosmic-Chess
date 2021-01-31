using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] public bool white;
    [SerializeField]  public Board.PieceType pieceType;
    public int[] mathPos = { -1, -1};

    [SerializeField]  public bool isGhost = false;
    

    protected void setPos()
    {
        
        mathPos[0] = (int)(Mathf.Floor(transform.localPosition.x));
        mathPos[1] = (int)(Mathf.Floor(transform.localPosition.z));
        //Debug.Log(mathPos[0] + " " + mathPos[1]);
    }


    public void updatePos()
    {
        setPos();
    }


    public void move(Vector3 pos)
    {
        
        transform.position = new Vector3(pos.x, pos.y, pos.z);
        /*mathPos[0] = movingMathPos[0];
        mathPos[1] = movingMathPos[1];*/
    }


    public Plane getPlane()
    {
        return gameObject.transform.parent.GetComponent<Plane>();
    }
}
