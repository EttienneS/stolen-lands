using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;

    private float _journeyLength;
    private Vector3 _panDesitnation;


    private bool _panning;

    private Vector3 _panSource;

    private float _startTime;

    private Camera Camera;


    [Range(1, 20)] public int Speed = 5;

    [Range(50, 800)] public int ZoomMax = 500;

    [Range(10, 50)] public int ZoomMin = 10;

    [Range(50, 300)] public int ZoomStep = 100;

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

    public void MoveToViewCell(HexCell cell)
    {
        _startTime = Time.time;
        _panSource = transform.position;
        _panDesitnation = new Vector3(cell.transform.position.x, cell.transform.position.y, transform.position.z);
        _journeyLength = Vector3.Distance(_panSource, _panDesitnation);

        _panning = true;
    }

    private void Start()
    {
        Camera = GetComponent<Camera>();
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
#if UNITY_STANDALONE || UNITY_WEBPLAYER

            float horizontal = 0;
            float vertical = 0;
            var z = transform.position.z;
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            z = -1 * Mathf.Clamp(-transform.position.z - Input.GetAxis("Mouse ScrollWheel") * ZoomStep, ZoomMin,
                    ZoomMax);
            transform.position = new Vector3(transform.position.x + horizontal * Speed,
                transform.position.y + vertical * Speed, z);

#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            var speed = 0.25f;
            if (Input.touchCount > 0)
            {
                Touch touchZero = Input.GetTouch(0);

                if (Input.touchCount == 2)
                {
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    float deltaMagnitudeDiff = (prevTouchDeltaMag - touchDeltaMag) * speed;

                    //Camera.fieldOfView += deltaMagnitudeDiff * speed;
                    //Camera.fieldOfView = Mathf.Clamp(Camera.fieldOfView, 10f, 150f);

                    transform.Translate(0, 0, deltaMagnitudeDiff);


                    // todo: clamp the camera to stop it from moving off screen
                    //var x = transform.position.x;
                    //var y = transform.position.y;
                    //var z = transform.position.z;

                    //transform.position = new Vector3(x, y, z);
                }
                else
                {
                    if (touchZero.phase == TouchPhase.Moved)
                    {
                        Vector2 touchDeltaPosition = touchZero.deltaPosition;
                        transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
                    }
                }
            }

#endif //End of mobile platform dependendent compilation section started above with #elif
        }
    }
}