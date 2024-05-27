using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField] int width, height; // tamanho da grid
    [SerializeField] GameObject[] prefabTile;
    int randomFrut;

    private void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid() //metodo que cria grid 
    {
        
        for (int x = 0; x < width; x++) 
        {
          for(int y = 0; y < height; y++) 
            {
                var spawnedTile = Instantiate(prefabTile[RandomFrut()], new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
            }   
        }
    }

    int RandomFrut()
    {
        randomFrut = Random.Range(0, 13);
        return randomFrut;
    }
}
