using Robust.Shared.Serialization;

namespace Content.Shared.VoiceMask;

[Serializable, NetSerializable]
public enum VoiceMaskUIKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class VoiceMaskBuiState : BoundUserInterfaceState
{
<<<<<<< HEAD
    public string Name { get; }
    public string Voice { get; } // Corvax-TTS

    public VoiceMaskBuiState(string name, string voice)  // Corvax-TTS
    {
        Name = name;
        Voice = voice;  // Corvax-TTS
=======
    public readonly string Name;
    public readonly string? Verb;

    public VoiceMaskBuiState(string name, string? verb)
    {
        Name = name;
        Verb = verb;
>>>>>>> 24e7653c984da133283457da2089e629161a7ff2
    }
}

[Serializable, NetSerializable]
public sealed class VoiceMaskChangeNameMessage : BoundUserInterfaceMessage
{
    public readonly string Name;

    public VoiceMaskChangeNameMessage(string name)
    {
        Name = name;
    }
}

/// <summary>
/// Change the speech verb prototype to override, or null to use the user's verb.
/// </summary>
[Serializable, NetSerializable]
public sealed class VoiceMaskChangeVerbMessage : BoundUserInterfaceMessage
{
    public readonly string? Verb;

    public VoiceMaskChangeVerbMessage(string? verb)
    {
        Verb = verb;
    }
}
