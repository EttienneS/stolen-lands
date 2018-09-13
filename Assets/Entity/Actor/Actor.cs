using System.Collections.Generic;
using UnityEngine;

public class Actor : Entity
{
    private readonly List<SpriteRenderer> _indicators = new List<SpriteRenderer>();

    private MeshRenderer _head;
    private MeshRenderer _body;

    public SpriteRenderer ActionIndicatorPrefab;

    public Sprite Sprite { get; set; }

    private MeshRenderer Head
    {
        get
        {
            if (_head == null)
            {
                _head = transform.Find("head").GetComponent<MeshRenderer>();
            }

            return _head;
        }
    }

    private MeshRenderer Body
    {
        get
        {
            if (_body == null)
            {
                _body = transform.Find("body").GetComponent<MeshRenderer>();
            }

            return _body;
        }
    }


   

    public void Update()
    {
        if (Faction != ActorController.Instance.PlayerFaction)
            return;


        if (SystemController.Instance.SelectedActor == this)
        {
            MoveHead();
        }

        var radius = 0.5f;
        if (_indicators.Count != ActionPoints)
        {
            foreach (var i in _indicators)
            {
                Destroy(i.gameObject);
            }

            _indicators.Clear();

            for (var i = 0; i < ActionPoints; i++)
            {
                var indicator = Instantiate(ActionIndicatorPrefab, transform);

                var angle = i * Mathf.PI * 2f / ActionPoints;

                indicator.transform.localPosition = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius,
                    indicator.transform.localPosition.z);

                _indicators.Add(indicator);
            }
        }
    }

    private void MoveHead()
    {
        var mousePosition = CameraController.Instance.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, Input.mousePosition.z - CameraController.Instance.Camera.transform.position.z));

        //Rotates toward the mouse
        Head.transform.eulerAngles = new Vector3(0, 0,
            Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) *
            Mathf.Rad2Deg - 90);

    }

    public void Start()
    {
        var res = 32;

        var texture = TextureCreator.GetTexture(null, res, TextureHelper.GetRandomColor());
        Sprite = Sprite.Create(texture, new Rect(new Vector2(), new Vector2(res, res)), new Vector2());

        Head.material.mainTexture = texture;
        Body.material.color = Faction.Color;
    }

    private void OnMouseDown()
    {
        SystemController.Instance.SetSelectedActor(this);
    }

    public void EnableOutline()
    {
        var outline = Shader.Find("Custom/Outline");

        Head.material.shader = outline;
        Head.material.SetFloat("_Outline", 0.05f);
        Head.material.SetColor("_OutlineColor", Faction.Color);
    }

    public void DisableOutline()
    {
        Head.material.SetFloat("_Outline", 0f);
    }

    public override void StartTurn()
    {
        ActionPoints = 3;
        foreach (var trait in Traits)
        {
            trait.Start();
        }
    }

    public override void TakeTurn()
    {
        if (Mind == null)
        {
            return;
        }

        if (Mind is Player)
        {
            if (SystemController.Instance.SelectedActor == this)
            {
                Mind.Act();
            }
        }
        else
        {
            Mind.Act();
        }
    }

    public override void EndTurn()
    {
        foreach (var trait in Traits)
        {
            trait.Finish();
        }
    }
}