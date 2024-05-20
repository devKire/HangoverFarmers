using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //Determinar espaço
    public bool isUsable;
    public GameObject fruta;

    public Node(bool _isUsable, GameObject _fruta)
    {
        isUsable = _isUsable;
        fruta = _fruta;
    }
}
