public class Structure : Entity
{
    public int Cost;

    private void Start()
    {
        AddTrait(new Sighted(1));
    }

    public void StartTurn()
    {
        foreach (var trait in Traits)
        {
            trait.Start();
        }
    }

    public void TakeTurn()
    {
    }

    public void EndTurn()
    {
        //foreach (var trait in Traits)
        //{
        //    trait.Start();
        //}
    }
}