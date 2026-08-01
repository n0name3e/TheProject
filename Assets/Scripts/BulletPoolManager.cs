using UnityEngine;
using UnityEngine.Pool;

public class BulletPoolManager : MonoBehaviour
{
    private ObjectPool<Bullet> pool;
    [SerializeField] private GameObject bulletPrefab;

    public static BulletPoolManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        pool = new ObjectPool<Bullet>(
    CreateBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyBullet, true, 30, 50);
    }
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
        bullet.pool = pool;
        return bullet;
    }

    private void OnTakeFromPool(Bullet bullet) => bullet.gameObject.SetActive(true);

    private void OnReturnedToPool(Bullet bullet) => bullet.gameObject.SetActive(false);

    private void OnDestroyBullet(Bullet bullet) => print("vasya"); //Destroy(bullet.gameObject);

    public Bullet GetBullet()
    {
        return pool.Get();
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic()
    {
        Instance = null;
    }
}
