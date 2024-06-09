﻿using Valuator.Caches.ValueObjects;

namespace Valuator.Repositories.Interfaces;

public interface IRankRepository
{
    void Add( RankId key, string value );
    string? Get( RankId key );
}