using UnityEngine;

public class Person : Actor
{
    public static GameObject GetAveragePerson(Transform parent)
    {
        var name = ActorHelper.GetRandomName();
        var gameObject = new GameObject(name);
        gameObject.AddComponent(typeof(Person));
        gameObject.transform.parent = parent;

        var person = gameObject.GetComponent<Person>();
        person.Instantiate(name, TextureHelper.GetRandomColor());
        person.Physical = Random.Range(40, 60);
        person.Cunning = Random.Range(40, 60);
        person.Mental = Random.Range(40, 60);
        person.Charisma = Random.Range(40, 60);

        return gameObject;
    }

}