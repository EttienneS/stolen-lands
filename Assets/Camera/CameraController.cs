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

    private Vector2 touchOrigin = -Vector2.one;

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
            float horizontal = 0;
            float vertical = 0;

            // RawOdds = (1 + Skill) / ((1 + Skill) + (1 + Difficulty))
            // AdjustedOdds = 1 / (1 + (e ^ ((RawOdds * Steepness) + Offset)))
#if UNITY_STANDALONE || UNITY_WEBPLAYER

             horizontal = Input.GetAxis("Horizontal");
             vertical = Input.GetAxis("Vertical");



#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE


            //Check if Input has registered more than zero touches
            if (Input.touchCount > 0)
            {
                Touch myTouch = Input.touches[0];

                if (myTouch.phase == TouchPhase.Began)
                {
                    //If so, set touchOrigin to the position of that touch
                    touchOrigin = myTouch.position;
                }

                //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
                else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {
                    //Set touchEnd to equal the position of this touch
                    Vector2 touchEnd = myTouch.position;

                    //Calculate the difference between the beginning and end of the touch on the x axis.
                    float x = touchEnd.x - touchOrigin.x;

                    //Calculate the difference between the beginning and end of the touch on the y axis.
                    float y = touchEnd.y - touchOrigin.y;

                    //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
                    touchOrigin.x = -1;

                    //Check if the difference along the x axis is greater than the difference along the y axis.
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                        //If x is greater than zero, set horizontal to 1, otherwise set it to -1
                        horizontal = x > 0 ? 1 : -1;
                    else
                        //If y is greater than zero, set horizontal to 1, otherwise set it to -1
                        vertical = y > 0 ? 1 : -1;
                }
            }

#endif //End of mobile platform dependendent compilation section started above with #elif

            var z = -1 * Mathf.Clamp(-transform.position.z - Input.GetAxis("Mouse ScrollWheel") * ZoomStep, ZoomMin, ZoomMax);

            transform.position = new Vector3(transform.position.x + (horizontal * Speed), transform.position.y + (vertical * Speed), z);

        }
    }
}