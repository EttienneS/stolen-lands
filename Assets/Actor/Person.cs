using UnityEngine;

public class Person : Actor
{
    public static GameObject GetAveragePerson(Transform parent)
    {
        var name = ActorHelper.GetRandomName();
        var gameObject = new GameObject(name);
        gameObject.AddComponent(typeof(Person));
        gameObject.GetComponent<Person>().Instantiate(name, TextureHelper.GetRandomColor());
        gameObject.transform.parent = parent;

        return gameObject;
    }

}