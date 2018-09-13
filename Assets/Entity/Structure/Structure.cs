public class Structure : Entity
{
    public int Cost;

    private void Start()
    {
        AddTrait(new Sighted(1));
    }

    public override void StartTurn()
    {
        foreach (var trait in Traits)
        {
            trait.Start();
        }
    }

    public override void TakeTurn()
    {
    }

    public override void EndTurn()
    {
    }
}