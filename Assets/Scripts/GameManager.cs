using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public GameObject ballPrefab;

    public float xBound = 3f;
    public float yBound = 3f;
    public float ballSpeed = 3f;
    public float respawnDelay = 2f;
    public int[] playerScores;

    public TMP_Text mainText;
    public TMP_Text[] playerTexts;

    private Entity ballEntityPrefab;
    private EntityManager manager;

    private WaitForSeconds oneSecond;
    private WaitForSeconds delay;
    private BlobAssetStore _blobAssetStore;

    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(gameObject);
            return;
        }

        main = this;
        playerScores = new int[2];

        manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        _blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
        ballEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ballPrefab, settings);

        oneSecond = new WaitForSeconds(1f);
        delay = new WaitForSeconds(respawnDelay);

        StartCoroutine(CountdownAndSpawnBall());
    }

    public void PlayerScored(int playerID)
    {
        playerScores[playerID]++;
        for (int i = 0; i < playerScores.Length && i < playerTexts.Length; i++)
        {
            playerTexts[i].text = playerScores[i].ToString();
        }

        StartCoroutine(CountdownAndSpawnBall());
    }
    
    private IEnumerator CountdownAndSpawnBall()
    {
        mainText.text = "Get Ready";
        yield return delay;

        mainText.text = "3";
        yield return oneSecond;
        mainText.text = "2";
        yield return oneSecond;
        mainText.text = "1";
        yield return oneSecond;
        
        mainText.text = "";

        SpawnBall();
    }

    private void SpawnBall()
    {
        Entity ball = manager.Instantiate(ballEntityPrefab);
        
        Vector3 direction = new Vector3(UnityEngine.Random.Range(0,2) == 0 ? -1 : 1, UnityEngine.Random.Range(-0.5f,0.5f), 0f).normalized;
        Vector3 speed = direction * ballSpeed;

        PhysicsVelocity velocity = new PhysicsVelocity()
        {
            Linear = speed,
            Angular = float3.zero
        };

        manager.AddComponentData(ball, velocity);
    }

    private void OnDisable()
    {
        _blobAssetStore.Dispose();
    }
}
