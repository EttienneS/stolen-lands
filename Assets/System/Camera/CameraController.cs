using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;

    private float _journeyLength;
    private Vector3 _panDesitnation;

    private bool _panning;

    private Vector3 _panSource;

    private float _startTime;

    public Camera Camera;

    [Range(1, 20)] public int Speed = 2;

    [Range(50, 100)] public int ZoomMax = 100;

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

    public void Start()
    {
        Camera = GetComponent<Camera>();
    }

    public void MoveToViewCell(HexCell cell)
    {
        _startTime = Time.time;
        _panSource = transform.position;
        _panDesitnation = new Vector3(cell.transform.position.x,
            cell.transform.position.y - (ZoomMax - Camera.fieldOfView), transform.position.z);
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
            var oldFov = Camera.fieldOfView;

#if UNITY_STANDALONE || UNITY_WEBPLAYER
            float horizontal = 0;
            float vertical = 0;
            var z = transform.position.z;
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            Camera.fieldOfView = Mathf.Clamp(Camera.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * ZoomStep,
                ZoomMin, ZoomMax);
            transform.position = new Vector3(transform.position.x + horizontal * Speed,
                transform.position.y + vertical * Speed, z);

#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            var zoomSpeed = 0.4f;
            var scrollSpeed = 0.05f;
            if (Input.touchCount > 0)
            {
                var touchZero = Input.GetTouch(0);

                if (Input.touchCount == 2)
                {
                    var touchOne = Input.GetTouch(1);

                    var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    var prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    var touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    var deltaMagnitudeDiff = (prevTouchDeltaMag - touchDeltaMag) * zoomSpeed;

                    Camera.fieldOfView += deltaMagnitudeDiff * zoomSpeed;
                    Camera.fieldOfView = Mathf.Clamp(Camera.fieldOfView, ZoomMin, ZoomMax);
                }
                else
                {
                    if (touchZero.phase == TouchPhase.Moved)
                    {
                        var touchDeltaPosition = touchZero.deltaPosition;
                        transform.Translate(-touchDeltaPosition.x * scrollSpeed, -touchDeltaPosition.y * scrollSpeed, 0);
                    }
                }
            }

#endif //End of mobile platform dependendent compilation section started above with #elif

            // todo: clamp the camera to stop it from moving off screen
            //var x = transform.position.x;
            //var y = transform.position.y;

            //transform.position = new Vector3(x, y, z);

            // move camera to match with change in FOV
            RotateAndScale(oldFov);
        }
    }

    private void RotateAndScale(float oldFov)
    {
        transform.position -= new Vector3(0, oldFov - Camera.fieldOfView);

        var zoomPercentage = 1 - Camera.fieldOfView / ZoomMax;
        transform.eulerAngles = new Vector3(-(5 + 50 * zoomPercentage), 0);
    }
}