using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frutas : MonoBehaviour
{
    [SerializeField] public tipoFruta tipoFruta; 

    public int xIndex;
    public int yIndex;

    public bool isMatched;
    private Vector2 currentPosition;
    private Vector2 targetPos;

    public bool isMoving;

    public void Fruta(int _x,int _y)
    {
        xIndex = _x;
        yIndex = _y;
    }
    public void SetIndicies(int _x, int _y)
    {
        xIndex = _x;
        yIndex = _y;
    }
}

public enum tipoFruta
{
    Maçã, 
    Banana, 
    Manga, 
    Melancia,
    Pitaya, 
    Uva,
    Abacaxi
}
