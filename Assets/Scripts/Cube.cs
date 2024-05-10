using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private int _minSplitCubeAmount;
    [SerializeField] private int _maxSplitCubeAmount;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _vanishEffect;
    [SerializeField] private Cube _cubePrefab;

    private Renderer _renderer;
    private int _splitChance = 100;
    private int _maxSplitChance = 100;
    private float _splitCubeChangeCoef = 0.5f;
    private List<Rigidbody> _explodableObjects = new List<Rigidbody>();


    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = GetRandomColor();
    }

    private Color GetRandomColor()
    {
        float randomRed = Random.value;
        float randomGreen = Random.value;
        float randomBlue = Random.value;

        return new Color(randomRed, randomGreen, randomBlue);
    }

    private void OnMouseUpAsButton()
    {
        if (TryToSplit())
        {
            Explode();
            Instantiate(_explosionEffect, transform.position, transform.rotation);
        }
        else Instantiate(_vanishEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    private void Initialize(int oldSplitChance, Vector3 oldScale)
    {
        _splitChance = System.Convert.ToInt32(oldSplitChance * _splitCubeChangeCoef);
        transform.localScale = oldScale * _splitCubeChangeCoef;
    }

    private bool TryToSplit()
    {
        if (_splitChance >= Random.Range(0, _maxSplitChance + 1))
        {
            for (int i = 0; i < Random.Range(_minSplitCubeAmount, _maxSplitCubeAmount + 1); i++)
            {
                Cube newCube = Instantiate(_cubePrefab, transform.position, transform.rotation);
                newCube.Initialize(_splitChance, transform.localScale);
                _explodableObjects.Add(newCube.GetComponent<Rigidbody>());
            }

            return true;
        }
        else return false;
    }

    private void Explode()
    {
        foreach (Rigidbody explodableObject in _explodableObjects)
            explodableObject.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
    }
}
