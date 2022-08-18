// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Web;

namespace Geta.OEmbed.Optimizely.Tests.Common.Repositories
{
    public class MemorySiteDefinitionRepository : ISiteDefinitionRepository
    {
        private readonly IDictionary<Guid, SiteDefinition> _definitions;

        public MemorySiteDefinitionRepository(IEnumerable<SiteDefinition> definitions)
        {
            _definitions = definitions.ToDictionary(x => x.Id);
        }

#pragma warning disable CS0067
        public event EventHandler<EventArgs>? SiteDefinitionChanged;
#pragma warning restore CS0067

        public virtual void Delete(Guid id)
        {
            if (_definitions.ContainsKey(id))
                _definitions.Remove(id);
        }

        public SiteDefinition? Get(Guid id)
        {
            if (_definitions.TryGetValue(id, out var siteDefinition))
                return siteDefinition;

            return null;
        }

        public IEnumerable<SiteDefinition> List()
        {
            return _definitions.Values;
        }

        public void Save(SiteDefinition siteDefinition)
        {
            // This method needs no implementation as all definitions are held in memory.
        }
    }
}
