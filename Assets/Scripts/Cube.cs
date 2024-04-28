using UnityEngine;

[RequireComponent (typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private readonly string GroundLayer = "Ground";

    [SerializeField] private float _minLifeTimeSeconds = 2.0f;
    [SerializeField] private float _maxLifeTimeSeconds = 5.0f;
    [SerializeField] private LayerMask _groundLayer;

    private float _lifeTimeSeconds;
    private Material _material;
    private Color _defaultColor;
    private bool _canColorChange = true;

    public delegate void OnCallback(Cube cube);
    public OnCallback Died;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        _defaultColor = _material.color;
        _groundLayer = LayerMask.NameToLayer(GroundLayer);
    }

    private void OnEnable()
    {
        ResetParametrs();
        Invoke(nameof(Die), _lifeTimeSeconds);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_canColorChange == false)
            return;

        if (collision.gameObject.layer == _groundLayer)
        {
            _material.color = new Color(Random.value, Random.value, Random.value);
            _canColorChange = false;
        }
    }

    private void ResetParametrs()
    {
        _lifeTimeSeconds = Random.Range(_minLifeTimeSeconds, _maxLifeTimeSeconds);
        _canColorChange = true;
        _material.color = _defaultColor;
    }

    private void Die()
    {
        Died?.Invoke(this);
    }
}