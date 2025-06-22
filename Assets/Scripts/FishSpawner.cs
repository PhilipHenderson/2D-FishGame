using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject playerfish;
    public int playerFishLevel;
    public GameObject rSpawnArea;
    public GameObject lSpawnArea;
    public GameObject fish;
    public int maxFishCount;
    public int fishCount;
    public float spawnSpeed;
    bool side;
    bool rgoingUp = true;
    bool lgoingUp = true;

    public void Start()
    {
        InvokeRepeating("SpawnFish", 2, spawnSpeed);
    }

    void Update()
    {
        if (playerfish == null)
        {
            playerfish = GameObject.Find("Player(Clone)");
            playerFishLevel = playerfish.GetComponent<FishController>().level;
        }

        if (rgoingUp)
        {
            if (rSpawnArea.transform.position.y >= 3)
            {
                rgoingUp = false;
            }
            else
                rSpawnArea.transform.position += new Vector3(0.0f, 0.1f);
        }
        else
        {
            if (rSpawnArea.transform.position.y <= -4)
            {
                rgoingUp = true;
            }
            else
                rSpawnArea.transform.position += new Vector3(0.0f, -0.1f);
        }

        if (lgoingUp)
        {
            if (lSpawnArea.transform.position.y >= 3)
            {
                lgoingUp = false;
            }
            else
                lSpawnArea.transform.position += new Vector3(0.0f, 0.1f);
        }
        else
        {
            if (lSpawnArea.transform.position.y <= -4)
            {
                lgoingUp = true;
            }
            else
                lSpawnArea.transform.position += new Vector3(0.0f, -0.1f);
        }
    }

    public void SpawnFish()
    {
        if (fishCount < maxFishCount)
        {
            fishCount++;

            var randomFishLevel = Random.Range(playerFishLevel - 2, playerFishLevel + 2);

            if (randomFishLevel < 0) 
            {
                randomFishLevel = 1;
            }

            fish.GetComponent<FishController>().level = randomFishLevel;
            fish.GetComponent<FishController>().size = randomFishLevel;

            Debug.Log(fish.GetComponent<FishController>().level);

            if (side == false)
            {
                Instantiate(fish, lSpawnArea.transform.position, transform.rotation);
                side = true;
            }
            else
            {
                Instantiate(fish, rSpawnArea.transform.position, transform.rotation);
                side = false;
            }
        }
    }

    public void ResetSpawner()
    {
        // Reset the fish count to 0
        fishCount = 0;

        // Reset the position of the spawn areas
        rSpawnArea.transform.position = new Vector3(rSpawnArea.transform.position.x, 0f, rSpawnArea.transform.position.z);
        lSpawnArea.transform.position = new Vector3(lSpawnArea.transform.position.x, 0f, lSpawnArea.transform.position.z);

        // Reset the direction of movement for the spawn areas
        rgoingUp = true;
        lgoingUp = true;
    }
}
