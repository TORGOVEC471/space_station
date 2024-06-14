using Content.Client.Actions;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Systems.Actions.Controls;

[Virtual]
public class ActionButtonContainer : GridContainer
{
    public event Action<GUIBoundKeyEventArgs, ActionButton>? ActionPressed;
    public event Action<GUIBoundKeyEventArgs, ActionButton>? ActionUnpressed;
    public event Action<ActionButton>? ActionFocusExited;

    public ActionButtonContainer()
    {
        IoCManager.InjectDependencies(this);
        UserInterfaceManager.GetUIController<ActionUIController>().RegisterActionContainer(this);
    }

    public ActionButton this[int index]
    {
        get => (ActionButton) GetChild(index);
    }

<<<<<<< HEAD
=======
    private void BuildActionButtons(int count)
    {
        var keys = ContentKeyFunctions.GetHotbarBoundKeys();

        Children.Clear();
        for (var index = 0; index < count; index++)
        {
            Children.Add(MakeButton(index));
        }

        ActionButton MakeButton(int index)
        {
            var button = new ActionButton(_entity);

            if (!keys.TryGetValue(index, out var boundKey))
                return button;

            button.KeyBind = boundKey;
            if (_input.TryGetKeyBinding(boundKey, out var binding))
            {
                button.Label.Text = binding.GetKeyString();
            }

            return button;
        }
    }

>>>>>>> 24e7653c984da133283457da2089e629161a7ff2
    public void SetActionData(ActionsSystem system, params EntityUid?[] actionTypes)
    {
        ClearActionData();

        for (var i = 0; i < actionTypes.Length; i++)
        {
            var action = actionTypes[i];
            if (action == null)
                continue;

            ((ActionButton) GetChild(i)).UpdateData(action.Value, system);
        }
    }

    public void ClearActionData()
    {
        foreach (var button in Children)
        {
            ((ActionButton) button).ClearData();
        }
    }

    protected override void ChildAdded(Control newChild)
    {
        base.ChildAdded(newChild);

        if (newChild is not ActionButton button)
            return;

        button.ActionPressed += ActionPressed;
        button.ActionUnpressed += ActionUnpressed;
        button.ActionFocusExited += ActionFocusExited;
    }

    protected override void ChildRemoved(Control newChild)
    {
        if (newChild is not ActionButton button)
            return;

        button.ActionPressed -= ActionPressed;
        button.ActionUnpressed -= ActionUnpressed;
        button.ActionFocusExited -= ActionFocusExited;
    }

    public bool TryGetButtonIndex(ActionButton button, out int position)
    {
        if (button.Parent != this)
        {
            position = 0;
            return false;
        }

        position = button.GetPositionInParent();
        return true;
    }

    public IEnumerable<ActionButton> GetButtons()
    {
        foreach (var control in Children)
        {
            if (control is ActionButton button)
                yield return button;
        }
    }
}
