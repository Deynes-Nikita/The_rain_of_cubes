using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BoxCollider))]
public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _repeatRate = 0.5f;

    private ObjectPool<Cube> _cubePool;
    private BoxCollider _spawnArea;
    private float LastSpawnTime;

    private void Awake()
    {
        _spawnArea = GetComponent<BoxCollider>();

        _cubePool = new ObjectPool<Cube>(
            createFunc: () => CreatePooledCube(),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => ActionOnRelease(cube),
            actionOnDestroy: (cube) => DestroyCube(cube),
            collectionCheck: false);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0f, _repeatRate);
    }

    private Cube CreatePooledCube()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.gameObject.SetActive(false);

        cube.Died += ReturnCubeToPool;

        return cube;
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = SelectSpawnPoint();
        cube.transform.rotation = Random.rotation;

        cube.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void DestroyCube(Cube cube)
    {
        cube.Died -= ReturnCubeToPool;

        Destroy(cube);
    }

    private void GetCube()
    {
        _cubePool.Get();
    }

    private void ReturnCubeToPool(Cube cube)
    {
        _cubePool.Release(cube);
    }

    private Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(
             _spawnArea.transform.position.x + _spawnArea.center.x + Random.Range(-1 * _spawnArea.bounds.extents.x, _spawnArea.bounds.extents.x),
             _spawnArea.transform.position.y + _spawnArea.center.y + Random.Range(-1 * _spawnArea.bounds.extents.y, _spawnArea.bounds.extents.y),
             _spawnArea.transform.position.z + _spawnArea.center.z + Random.Range(-1 * _spawnArea.bounds.extents.z, _spawnArea.bounds.extents.z));

        return spawnPoint;
    }
}
