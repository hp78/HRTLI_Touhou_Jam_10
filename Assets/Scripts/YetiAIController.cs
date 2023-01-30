using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiAIController : MonoBehaviour
{
    [Space(5)]
    [SerializeField] SpriteRenderer _idle;
    [SerializeField] SpriteRenderer _walk;
    [SerializeField] ParticleSystem _bloodsplatter;
    [SerializeField] AudioSource _eatenSFX;

    [Space(5)]
    [SerializeField] float _speed;
    [SerializeField] float _speedUpMod;
    [SerializeField] float _catchUpDistThreshold = 6;
    [SerializeField] List<Transform> _playerList = new List<Transform>();

    [Space(5)]
    [SerializeField] Transform _currentTarget = null;
    [SerializeField] Vector3 _currDirection;

    [Space(5)]
    [SerializeField] Sprite _interactionIcon;
    [SerializeField] string _unitName;
    [SerializeField] string _spawnMessage;

    float _checkDistanceCooldown;
    DistanceComparer distanceComparer;
    PlayerControllerMP _currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        distanceComparer = new DistanceComparer(transform);
        var temp = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in temp) _playerList.Add(go.transform);
        CheckDistanceWithPlayers();

        GameControllerMultiplayer.instance.SendFeedEvent(_spawnMessage);
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
                _checkDistanceCooldown = Random.Range(0.5f, 1f);
            }
        }

        _checkDistanceCooldown -= Time.deltaTime;
    }

    public void AddPlayer(Transform playerTF)
    {
        _playerList.Add(playerTF);
        CheckDistanceWithPlayers();
    }

    void ChaseTarget()
    {
        

        float totalSpeed;
        if (Vector3.Distance(_currentTarget.position, transform.position) > _catchUpDistThreshold) totalSpeed = _speed * _speedUpMod;
        else totalSpeed = _speed *Random.Range(0.8f,1f);

        _currDirection = (_currentTarget.position - transform.position).normalized;

        this.transform.position += (_currDirection * totalSpeed) * Time.deltaTime;
        _walk.flipX = _currDirection.x < 0.0f;

        if(_currentPlayer)
        if(!_currentPlayer.isAlive)
        {
            _playerList.Remove(_currentPlayer.transform);
            CheckDistanceWithPlayers();
        }
    }

    void CheckDistanceWithPlayers()
    {
        if (_playerList.Count >= 1)
        {
            _playerList.Sort(distanceComparer);

            _currentTarget = _playerList[0];
            _currentPlayer = _playerList[0].GetComponent<PlayerControllerMP>();

            _walk.gameObject.SetActive(true);
            _idle.gameObject.SetActive(false);
        }
        else
        {
            _currentTarget = null;
            _currentPlayer = null;
            _walk.gameObject.SetActive(false);
            _idle.gameObject.SetActive(true);

            GameControllerMultiplayer.instance.LooseGame();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerControllerMP player = collision.GetComponent<PlayerControllerMP>();
            if(player.isAlive)
            {
                _eatenSFX.Play();

                string playerName = "Player " + collision.GetComponent<PlayerControllerMP>().playerIndex;
                GameControllerMultiplayer.instance.SendFeedInteraction(_unitName, playerName, Color.red, Color.green, _interactionIcon);
            }

            _playerList.Remove(collision.gameObject.transform);
            CheckDistanceWithPlayers();
            _bloodsplatter.Play();



        }
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
