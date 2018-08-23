using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;
    public static CameraController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Main Camera").GetComponent<CameraController>();
            }

            return _instance;
        }
    }


    [Range(1, 20)] public int Speed = 5;

    [Range(50, 800)] public int ZoomMax = 500;

    [Range(10, 50)] public int ZoomMin = 10;

    [Range(50, 300)] public int ZoomStep = 100;


    private bool _panning;

    private Vector3 _panSource;
    private Vector3 _panDesitnation;

    private float _startTime;

    private float _journeyLength;

    public void MoveToViewCell(HexCell cell)
    {
        _startTime = Time.time;
        _panSource = transform.position;
        _panDesitnation = new Vector3(cell.transform.position.x, cell.transform.position.y, transform.position.z);
        _journeyLength = Vector3.Distance(_panSource, _panDesitnation);

        _panning = true;
    }

    private void Update()
    {
        if (_panning)
        {
            var distCovered = (Time.time - _startTime) * 1000;
            var fracJourney = distCovered / _journeyLength;

            transform.position = Vector3.Lerp(_panSource, _panDesitnation, fracJourney);

            if (transform.position == _panDesitnation)
            {
                _panning = false;
            }
        }
        else
        {
            // RawOdds = (1 + Skill) / ((1 + Skill) + (1 + Difficulty))
            // AdjustedOdds = 1 / (1 + (e ^ ((RawOdds * Steepness) + Offset)))

            var xMove = Input.GetAxis("Horizontal") * Speed;
            var yMove = Input.GetAxis("Vertical") * Speed;

            var z = -1 * Mathf.Clamp(-transform.position.z - Input.GetAxis("Mouse ScrollWheel") * ZoomStep, ZoomMin, ZoomMax);

            transform.position = new Vector3(transform.position.x + xMove, transform.position.y + yMove, z);
        }
    }
}