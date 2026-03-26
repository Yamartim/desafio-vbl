using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //[SerializeField] private GameObject groundStartPrefab;
    [SerializeField] private GameObject groundStreetPrefab;
    [SerializeField] private GameObject groundSidewalkPrefab;
    [SerializeField] private GameObject groundFinishPrefab;

    float firstPieceZ = 7.5f;
    float finalPieceZ = 10f;
    float pieceZoffset = 5f;
    float streetChance = 0.7f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int currentLevel = GameManager.instance.CurrentScore;

        int levelLength = 1 + currentLevel;


        GameObject piecePrefab;
        for (int i = 0; i < levelLength; i++)
        {
            piecePrefab = (Random.value < streetChance) ? groundStreetPrefab : groundSidewalkPrefab;

            // Randomly rotate the piece by 0 or 180 degrees to flip car spawn direction
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0, 1) * 180f, 0f); 

            GameObject newPiece = Instantiate(piecePrefab, new Vector3(0f, 0f, firstPieceZ + i * pieceZoffset), randomRotation);
            newPiece.transform.parent = this.transform;
        }

        piecePrefab = groundFinishPrefab;
        GameObject finishPiece = Instantiate(piecePrefab, new Vector3(0f, 0f, finalPieceZ + levelLength * pieceZoffset), Quaternion.identity);
        finishPiece.transform.parent = this.transform;
    }

}
