using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject[] piecePrefab;
    public Piece[,] pieces;
    private Piece selectedPiece;
    public Vector3 vector3Base;
    public GameObject obstaclePrefab;

    void Start()
    {
        pieces = new Piece[width, height];
        InitializeBoard();
    }

    void InitializeBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Random.value < 0.1f) // 10% de chance de ser um obstáculo
                {
                    GameObject newObstacle = Instantiate(obstaclePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    if (newObstacle != null)
                    {
                        pieces[x, y] = newObstacle.GetComponent<Piece>();
                        if (pieces[x, y] != null)
                        {
                            pieces[x, y].Init(x, y, this);
                        }
                    }
                }
                else
                {
                    GameObject newPiece = Instantiate(piecePrefab[RandomFrut()], new Vector3(x, y, 0), Quaternion.identity);
                    if (newPiece != null)
                    {
                        pieces[x, y] = newPiece.GetComponent<Piece>();
                        if (pieces[x, y] != null)
                        {
                            pieces[x, y].Init(x, y, this);
                        }
                    }
                }
            }
        }
        CheckForMatches();
    }

    int RandomFrut()
    {
        return Random.Range(0, piecePrefab.Length);
    }

    public void SelectPiece(Piece piece)
    {
        if (piece.frutType == FrutType.Obstacle) return; // Impede a seleção de peças de obstáculo

        if (selectedPiece == null)
        {
            selectedPiece = piece;
            selectedPiece.AnimateScale(vector3Base * 1.2f, 0.2f);
        }
        else
        {
            if (IsAdjacent(selectedPiece, piece))
            {
                selectedPiece.AnimateScale(vector3Base, 0.2f);
                piece.AnimateScale(vector3Base, 0.2f);
                SwapPieces(selectedPiece, piece);
            }
            else
            {
                selectedPiece.AnimateScale(vector3Base, 0.2f);
                selectedPiece = piece;
                selectedPiece.AnimateScale(vector3Base * 1.2f, 0.2f);
            }
        }
    }

    bool IsAdjacent(Piece piece1, Piece piece2)
    {
        return (Mathf.Abs(piece1.x - piece2.x) == 1 && piece1.y == piece2.y) ||
               (Mathf.Abs(piece1.y - piece2.y) == 1 && piece1.x == piece2.x);
    }

    void SwapPieces(Piece piece1, Piece piece2)
    {
        if (piece1.frutType == FrutType.Obstacle || piece2.frutType == FrutType.Obstacle) return; // Impede a troca se uma das peças for um obstáculo

        int tempX = piece1.x;
        int tempY = piece1.y;

        pieces[piece1.x, piece1.y] = piece2;
        pieces[piece2.x, piece2.y] = piece1;

        piece1.Init(piece2.x, piece2.y, this);
        piece2.Init(tempX, tempY, this);

        Vector3 tempPosition = piece1.transform.position;
        piece1.transform.position = piece2.transform.position;
        piece2.transform.position = tempPosition;

        piece1.AnimateScale(vector3Base, 0.2f);
        piece2.AnimateScale(vector3Base, 0.2f);
        selectedPiece = null;
        CheckForMatches();
    }

    void CheckForMatches()
    {
        List<Piece> piecesToDestroy = new List<Piece>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (pieces[x, y] == null) continue;

                if (x < width - 2)
                {
                    int matchLength = 1;
                    FrutType currentType = pieces[x, y].frutType;
                    for (int k = 1; k < width - x; k++)
                    {
                        if (pieces[x + k, y] != null && pieces[x + k, y].frutType == currentType)
                        {
                            matchLength++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (matchLength >= 3)
                    {
                        for (int k = 0; k < matchLength; k++)
                        {
                            piecesToDestroy.Add(pieces[x + k, y]);
                        }
                    }
                }

                if (y < height - 2)
                {
                    int matchLength = 1;
                    FrutType currentType = pieces[x, y].frutType;
                    for (int k = 1; k < height - y; k++)
                    {
                        if (pieces[x, y + k] != null && pieces[x, y + k].frutType == currentType)
                        {
                            matchLength++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (matchLength >= 3)
                    {
                        for (int k = 0; k < matchLength; k++)
                        {
                            piecesToDestroy.Add(pieces[x, y + k]);
                        }
                    }
                }
            }
        }

        foreach (Piece piece in piecesToDestroy)
        {
            if (piece != null)
            {
                pieces[piece.x, piece.y] = null;
                Destroy(piece.gameObject);
            }
        }

        DestroyAdjacentObstacles(piecesToDestroy);
        StartCoroutine(RefillBoard());
    }

    void DestroyAdjacentObstacles(List<Piece> matchedPieces)
    {
        HashSet<Piece> obstaclesToDestroy = new HashSet<Piece>();

        foreach (Piece piece in matchedPieces)
        {
            int x = piece.x;
            int y = piece.y;

            if (x > 0 && pieces[x - 1, y]?.frutType == FrutType.Obstacle)
            {
                obstaclesToDestroy.Add(pieces[x - 1, y]);
            }
            if (x < width - 1 && pieces[x + 1, y]?.frutType == FrutType.Obstacle)
            {
                obstaclesToDestroy.Add(pieces[x + 1, y]);
            }
            if (y > 0 && pieces[x, y - 1]?.frutType == FrutType.Obstacle)
            {
                obstaclesToDestroy.Add(pieces[x, y - 1]);
            }
            if (y < height - 1 && pieces[x, y + 1]?.frutType == FrutType.Obstacle)
            {
                obstaclesToDestroy.Add(pieces[x, y + 1]);
            }
        }

        foreach (Piece obstacle in obstaclesToDestroy)
        {
            if (obstacle != null)
            {
                pieces[obstacle.x, obstacle.y] = null;
                Destroy(obstacle.gameObject);
            }
        }
    }

    IEnumerator RefillBoard()
    {
        yield return new WaitForSeconds(0.5f);

        for (int x = 0; x < width; x++)
        {
            int emptyCount = 0;
            for (int y = 0; y < height; y++)
            {
                if (pieces[x, y] == null)
                {
                    emptyCount++;
                }
                else if (emptyCount > 0)
                {
                    pieces[x, y - emptyCount] = pieces[x, y];
                    pieces[x, y].Init(x, y - emptyCount, this);
                    StartCoroutine(MovePiece(pieces[x, y], new Vector3(x, y - emptyCount, 0)));
                    pieces[x, y] = null;
                }
            }

            for (int y = height - emptyCount; y < height; y++)
            {
                GameObject newPiece = Instantiate(piecePrefab[RandomFrut()], new Vector3(x, height, 0), Quaternion.identity);
                pieces[x, y] = newPiece.GetComponent<Piece>();
                pieces[x, y].Init(x, y, this);
                StartCoroutine(MovePiece(pieces[x, y], new Vector3(x, y, 0)));
            }
        }

        yield return new WaitForSeconds(0.5f); // Tempo extra para animação
        CheckForMatches(); // Verificar novas correspondências após preencher
    }

    IEnumerator MovePiece(Piece piece, Vector3 newPosition)
    {
        float timeToMove = 0.1f;
        float elapsedTime = 0;

        while (elapsedTime < timeToMove)
        {
            piece.transform.position = Vector3.MoveTowards(piece.transform.position, newPosition, (Time.deltaTime / timeToMove) * Vector3.Distance(piece.transform.position, newPosition));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        piece.transform.position = newPosition;
    }
}