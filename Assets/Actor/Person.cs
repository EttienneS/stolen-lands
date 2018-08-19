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

        var sentient = new Sentient(person)
        {
            Physical = Random.Range(20, 80),
            Cunning = Random.Range(20, 80),
            Mental = Random.Range(20, 80),
            Charisma = Random.Range(20, 80)
        };

        person.Traits.Add(sentient);
        person.Traits.Add(new HexClaimer(person));
        person.Instantiate(name, TextureHelper.GetRandomColor());

        return gameObject;
    }
}