﻿using UnityEngine;
using UnityEngine.UI;

public class SpriteCreator : MonoBehaviour
{
    public int resolution = 16;

    private void OnEnable()
    {
        GetComponent<Image>().sprite = Sprite.Create(TextureCreator.GetTexture(transform, resolution), new Rect(new Vector2(), new Vector2(resolution, resolution)), new Vector2());
    }
}