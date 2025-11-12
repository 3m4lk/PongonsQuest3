using UnityEngine;

public class TouhouBullet : MonoBehaviour
{
    public bool isCurved, isSpawner;
    public float speed, turningSpeed, spawningSpeed;
    public int spawnQuantity;
    private float spawnProgress;
    public GameObject bulletPreset;
    private void Awake()
    {
        if (isCurved && Random.Range(0, 2) == 1) turningSpeed = -turningSpeed;

        if (transform.parent.name != "gameArea")
        {
            transform.SetParent(transform.parent.parent);
            transform.SetAsLastSibling();
        }
    }
    private void FixedUpdate()
    {
        transform.localPosition += transform.up * speed * Time.fixedDeltaTime;
        if (Mathf.Abs(transform.localPosition.x) >= 220 || Mathf.Abs(transform.localPosition.y) >= 280) Destroy(gameObject);
        if (isCurved) transform.localRotation = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z + turningSpeed * Time.fixedDeltaTime);
        if (isSpawner && spawningSpeed >= 0.01f)
        {
            for (spawnProgress -= Time.fixedDeltaTime; spawnQuantity > 0 && spawnProgress <= 0f; spawnProgress += spawningSpeed, spawnQuantity--)
            {
                GameObject newBullets = Instantiate(bulletPreset, transform.position, transform.rotation);
                newBullets.transform.parent = transform.parent;
                newBullets.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-360f, 360f));
                Destroy(newBullets, 10);
                newBullets.SetActive(true);
            }
        }
    }
}
