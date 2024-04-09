using UnityEngine;

public class ZombieSpawnPoint : MonoBehaviour
{
    private AudioSource mAudioSource;

    public GameObject PrefabZombie;
    public float SpawnPriodSecs = 10;
    private float mTimeToNextSpawn = 0;
    public float ActivationThresholdM = 30;

    private void Awake()
    {
        mAudioSource = this.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mTimeToNextSpawn = Random.Range(1f, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        float distToPlayer = Mathf.Abs(GameManager.DistanceToPlayerInX(this.transform));

        if (distToPlayer < ActivationThresholdM)
        {
            mTimeToNextSpawn -= Time.deltaTime;
            if (mTimeToNextSpawn <= 0)
            {
                SpawnZombie(distToPlayer < 10);
                mTimeToNextSpawn = SpawnPriodSecs + Random.Range(-5f, 1f);
            }
        }
    }

    private void SpawnZombie(bool pPlaySound)
    {
        var newObj = GameObject.Instantiate(this.PrefabZombie, this.transform.position, Quaternion.identity);

        if (Random.Range(0, 100) <= 15)
        {
            newObj.GetComponent<Zombie>().PickupType = (ePickupType)Random.Range(0, (int)ePickupType.None);
        }
        var dir = newObj.GetComponent<LookDirection>();
        dir.LookLeft = GameManager.DistanceToPlayerInX(this.transform) < 0;

        if (pPlaySound)
        {
            mAudioSource.Play();
        }
    }
}
