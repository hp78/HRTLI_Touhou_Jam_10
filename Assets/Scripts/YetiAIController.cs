using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiAIController : MonoBehaviour
{

    [Space(5)]
    [SerializeField] float _speed;
    [SerializeField] List<Transform> _playerList = new List<Transform>();
    [SerializeField] Transform _currentTarget = null;

    [SerializeField] Vector3 _currDirection;


    float _checkDistanceCooldown;
    DistanceComparer distanceComparer;

    // Start is called before the first frame update
    void Start()
    {
        distanceComparer = new DistanceComparer(transform);
        var temp = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in temp) _playerList.Add(go.transform);
        CheckDistanceWithPlayers();

    }

    // Update is called once per frame
    void Update()
    {
        if (_currentTarget != null)
        {
            ChaseTarget();

            if (_checkDistanceCooldown < 0f)
            {
                CheckDistanceWithPlayers();
                _checkDistanceCooldown = Random.Range(1f, 3f);
            }
        }

        _checkDistanceCooldown -= Time.deltaTime;
    }

    void ChaseTarget()
    {
        _currDirection = (_currentTarget.position - transform.position).normalized;
        this.transform.position += (_currDirection * _speed) * Time.deltaTime;
    }

    void CheckDistanceWithPlayers()
    {
        _playerList.Sort(distanceComparer);
        if (_playerList.Count >= 1) _currentTarget = _playerList[0];
        else _currentTarget = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _playerList.Remove(collision.gameObject.transform);
            CheckDistanceWithPlayers();
        }
    }
    public class DistanceComparer : IComparer<Transform>
    {
        private Transform target;

        public DistanceComparer(Transform distanceToTarget)
        {
            target = distanceToTarget;
        }

        public int Compare(Transform a, Transform b)
        {
            var targetPosition = target.position;
            return Vector3.Distance(a.position, targetPosition).CompareTo(Vector3.Distance(b.position, targetPosition));
        }
    }
}
