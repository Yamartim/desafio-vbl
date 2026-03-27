using System.Collections.Generic;
using UnityEngine;

// Class for dynamically generating levels that get progressively more difficult as the player increases their score
public class LevelGenerator : MonoBehaviour
{
    // using the groundStartPrefab ended up being unnecessaty since we can just start the scene with it already instantiated
    //[SerializeField] private GameObject groundStartPrefab;
    [SerializeField] private GameObject groundStreetPrefab;
    [SerializeField] private GameObject groundSidewalkPrefab;
    [SerializeField] private GameObject groundFinishPrefab;

    float finalPieceZ = 2.5f;
    float pieceZoffset = 5f;

    // the higher this chance the lower number of safe sidewalks will spawn
    [SerializeField] [Range(0,1)] float streetChance = 0.7f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int currentLevel = GameManager.instance.CurrentScore;

        int levelLength = 1 + currentLevel;


        GameObject piecePrefab;

        // for loop that instantiates the level pieces one after another in the game world
        for (int i = 0; i < levelLength; i++)
        {
            piecePrefab = (Random.value < streetChance) ? groundStreetPrefab : groundSidewalkPrefab;

            GameObject newPiece = Instantiate(piecePrefab, new Vector3(0f, 0f, i * pieceZoffset), this.transform.rotation);

            // with a random 180 degree rotation, we can make cars come from both sides of the level
            newPiece.transform.forward = Random.value > 0.5f ? Vector3.forward : Vector3.back;
            newPiece.transform.parent = this.transform;
        }

        // put in the last piece at the end that allows the player to beat the level
        piecePrefab = groundFinishPrefab;

        GameObject finishPiece = Instantiate(piecePrefab, new Vector3(0f, 0f, finalPieceZ + levelLength * pieceZoffset), Quaternion.identity);
        
        finishPiece.transform.parent = this.transform;
    }

}
