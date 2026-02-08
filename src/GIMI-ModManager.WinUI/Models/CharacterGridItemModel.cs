﻿﻿﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GIMI_ModManager.Core.GamesService.Interfaces;
using GIMI_ModManager.WinUI.Services.Notifications;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace GIMI_ModManager.WinUI.Models;

public partial class CharacterGridItemModel : ObservableObject, IEquatable<CharacterGridItemModel>,
    IEquatable<IModdableObject>
{
    [ObservableProperty] private IModdableObject _character;
    [ObservableProperty] private Uri _imageUri;

    [ObservableProperty] private bool _isPinned;
    [ObservableProperty] private bool _warning;
    [ObservableProperty] private bool _isHidden;

    [ObservableProperty] private bool _notification;
    [ObservableProperty] private AttentionType _notificationType;

    [ObservableProperty] private int _modCount;

    [ObservableProperty] private string _modCountString = string.Empty;

    [ObservableProperty] private bool _hasMods;
    [ObservableProperty] private bool _hasEnabledMods;

    [ObservableProperty] private Brush _statusBrush = new SolidColorBrush(Colors.Gray);
    [ObservableProperty] private Brush _modCountBrush = new SolidColorBrush(Colors.Gray);

    public ObservableCollection<CharacterModItem> Mods { get; } = new();

    public CharacterGridItemModel(IModdableObject character)
    {
        Character = character;
        ImageUri = character.ImageUri ?? ModModel.PlaceholderImagePath;
    }

    public void SetMods(IEnumerable<CharacterModItem> mods)
    {
        Mods.Clear();
        var enabledMods = 0;
        foreach (var mod in mods)
        {
            Mods.Add(mod);
            if (mod.IsEnabled)
                enabledMods++;
        }

        ModCount = Mods.Count;
        HasMods = ModCount > 0;
        HasEnabledMods = enabledMods > 0;

        ModCountString = $"{enabledMods} / {ModCount}";

        if (enabledMods == 0)
        {
            StatusBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 128, 128, 128)); // Gray
            ModCountBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 128, 128, 128)); // Gray
        }
        else
        {
            ModCountBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 30, 144, 255)); // DodgerBlue

            if (enabledMods == 1)
            {
                StatusBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 0)); // Neon Green
            }
            else
            {
                StatusBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 50, 50)); // Bright Red
            }
        }
    }

    public bool Equals(CharacterGridItemModel? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Character.Equals(other.Character);
    }

    public bool Equals(IModdableObject? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Character.InternalNameEquals(other.InternalName);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is CharacterGridItemModel other && Equals(other);
    }

    public override int GetHashCode() => Character.GetHashCode();

    public static bool operator ==(CharacterGridItemModel? left, CharacterGridItemModel? right) => Equals(left, right);

    public static bool operator !=(CharacterGridItemModel? left, CharacterGridItemModel? right) => !Equals(left, right);


    public override string ToString()
    {
        return Character.DisplayName;
    }
}

public record CharacterModItem(string Name, bool IsEnabled, DateTime DateAdded);