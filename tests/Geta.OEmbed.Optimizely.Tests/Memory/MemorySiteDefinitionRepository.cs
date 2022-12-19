// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Web;

namespace Geta.OEmbed.Optimizely.Tests.Memory
{
    public class MemorySiteDefinitionRepository : ISiteDefinitionRepository
    {
        public event EventHandler<EventArgs>? SiteDefinitionChanged;

        private readonly Dictionary<Guid, SiteDefinition> _definitions;

        public MemorySiteDefinitionRepository(IEnumerable<SiteDefinition> siteDefinitions)
        { 
            _definitions = siteDefinitions.ToDictionary(x => x.Id);
            
            // NOTE: This is just to get one usage of the event handler
            SiteDefinitionChanged?.Invoke(this, new EventArgs());
        }

        public void Delete(Guid id)
        {
            _definitions.Remove(id);
        }

        public SiteDefinition? Get(Guid id)
        {
            if (_definitions.TryGetValue(id, out var definition))
                return definition;

            return null;
        }

        public IEnumerable<SiteDefinition> List()
        {
            return _definitions.Values;
        }

        public void Save(SiteDefinition siteDefinition)
        {
            throw new NotImplementedException();
        }
    }
}
