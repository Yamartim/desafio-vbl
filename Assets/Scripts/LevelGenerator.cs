using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //[SerializeField] private GameObject groundStartPrefab;
    [SerializeField] private GameObject groundStreetPrefab;
    [SerializeField] private GameObject groundSidewalkPrefab;
    [SerializeField] private GameObject groundFinishPrefab;

    float finalPieceZ = 2.5f;
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

            GameObject newPiece = Instantiate(piecePrefab, new Vector3(0f, 0f, i * pieceZoffset), this.transform.rotation);
            newPiece.transform.forward = Random.value > 0.5f ? Vector3.forward : Vector3.back;
            newPiece.transform.parent = this.transform;
        }

        piecePrefab = groundFinishPrefab;
        GameObject finishPiece = Instantiate(piecePrefab, new Vector3(0f, 0f, finalPieceZ + levelLength * pieceZoffset), Quaternion.identity);
        finishPiece.transform.parent = this.transform;
    }

}
