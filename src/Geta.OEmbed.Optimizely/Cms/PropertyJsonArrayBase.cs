// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.OEmbed.Optimizely.Cms
{
    public abstract class PropertyJsonArrayBase<T, TElement> : PropertyJsonBase<T> where T : class, ICollection<TElement>
    {
    }
}