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
        
        person.Physical = Random.Range(20, 80);
        person.Cunning = Random.Range(20, 80);
        person.Mental = Random.Range(20, 80);
        person.Charisma = Random.Range(20, 80);

        person.Instantiate(name, TextureHelper.GetRandomColor());

        return gameObject;
    }

}