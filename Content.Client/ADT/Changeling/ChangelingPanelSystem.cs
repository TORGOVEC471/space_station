/// Made for Adventure Time Project by ModerN. https://github.com/modern-nm mailto:modern-nm@yandex.by
/// see also https://github.com/DocNITE/liebendorf-station/tree/feature/emote-radial-panel
using Content.Client.Language.Systems;
using Content.Client.UserInterface.Systems.Radial;
using Content.Client.UserInterface.Systems.Radial.Controls;
using Content.Shared.Changeling;
using Content.Shared.Changeling.Components;
using Content.Shared.Polymorph;
using FastAccessors;
using Robust.Client.GameObjects;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Client.ADT.Language;

public sealed class ChangelingPanelSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;
    [Dependency] private readonly IPlayerManager _playerMan = default!;
    [Dependency] private readonly SpriteSystem _spriteSystem = default!;

    /// <summary>
    /// We should enable radial for single target
    /// </summary>
    private RadialContainer? _openedMenu;

    private const string DefaultIcon = "/Textures/Interface/AdminActions/play.png";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<PlayerDetachedEvent>(OnPlayerDetached);

        SubscribeNetworkEvent<RequestChangelingFormsMenuEvent>(HandleChangelingFormsMenuEvent);
    }

    private void HandleChangelingFormsMenuEvent(RequestChangelingFormsMenuEvent args)
    {
        if (_openedMenu != null)
            return;
        if (_playerMan.LocalEntity == null)
        {
            return;
        }

        //if (!TryComp<ChangelingComponent>(_playerMan.LocalEntity.Value, out var changelingComponent)) // нет на клиенте
        //    return;

        _openedMenu = _userInterfaceManager.GetUIController<RadialUiController>()
            .CreateRadialContainer();

        foreach (var humanoid in args.HumanoidData)
        {
            //var humanoidEntityUid = GetEntity(humanoid); // Entities on the client outside of the FOV are nonexistant. You can see that if you zoom out. //So it'll give you UID 0 which is EntityUid.Invalid.

            // foreach (var humanoidData in args.ChangelingComponent.StoredDNA)
            // {
            //     if (humanoidData.EntityUid == humanoidEntityUid)
            //     {
            //
            var actionName = humanoid.Name;
            var texturePath = _spriteSystem.Frame0(new SpriteSpecifier.Texture(new ResPath(DefaultIcon)));

            var emoteButton = _openedMenu.AddButton(actionName, texturePath);
            emoteButton.Opacity = 210;
            emoteButton.Tooltip = null;
            emoteButton.Controller.OnPressed += (_) =>
            {
                var ev = new SelectChangelingFormEvent(args.Target, entitySelected: humanoid.NetEntity);
                RaiseNetworkEvent(ev);
                _openedMenu.Dispose();
            };
            // }
            // }
        }
        _openedMenu.OnClose += (_) =>
        {
            _openedMenu = null;
        };
        if (_playerMan.LocalEntity != null)
            _openedMenu.OpenAttached(_playerMan.LocalEntity.Value);

    }

    // private void HandleLanguageMenuEvent(RequestLanguageMenuEvent args)
    // {
    //     if (_openedMenu != null)
    //         return;
    //     if (_playerMan.LocalEntity == null)
    //     {
    //         return;
    //     }
    //     TryComp<LanguageSpeakerComponent>(_playerMan.LocalEntity.Value, out var languageSpeakerComponent);

    //     _openedMenu = _userInterfaceManager.GetUIController<RadialUiController>()
    //         .CreateRadialContainer();

    //     foreach (var protoId in args.Languages)
    //     {
    //         var prototype = _languageSystem.GetLanguage(protoId);
    //         if (prototype == null)
    //         {
    //             continue;
    //         }
    //         var actionName = prototype.LocalizedName;
    //         var texturePath = _spriteSystem.Frame0(new SpriteSpecifier.Texture(new ResPath(DefaultIcon)));
    //         if (prototype.Icon != null)
    //             texturePath = _spriteSystem.Frame0(prototype.Icon);

    //         var languageButton = _openedMenu.AddButton(actionName, texturePath);
    //         languageButton.Opacity = 210;
    //         languageButton.Tooltip = null;
    //         languageButton.Controller.OnPressed += (_) =>
    //         {
    //             var ev = new SelectLanguageEvent(args.Target, protoId);
    //             RaiseNetworkEvent(ev);
    //             _openedMenu.Dispose();
    //         };
    //     }

    //     _openedMenu.OnClose += (_) =>
    //     {
    //         _openedMenu = null;
    //     };
    //     if (_playerMan.LocalEntity != null)
    //         _openedMenu.OpenAttached(_playerMan.LocalEntity.Value);

    // }

    private void OnPlayerAttached(PlayerAttachedEvent args)
    {
        _openedMenu?.Dispose();
    }

    private void OnPlayerDetached(PlayerDetachedEvent args)
    {
       _openedMenu?.Dispose();
    }
}
