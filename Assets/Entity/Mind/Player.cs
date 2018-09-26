public class Player : Mind
{
    public override void Act()
    {
        ControlPanelController.Instance.ClearPlayerActions();

        foreach (var action in Entity.AvailableActions)
        {
            ControlPanelController.Instance.AddAction(action);
        }
    }

    public override string Save()
    {
        return string.Empty;
    }

    public override void Load(string data)
    {
    }
}