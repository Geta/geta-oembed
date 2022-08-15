using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using Geta.OEmbed.Optimizely.Editing;
using Geta.OEmbed.Optimizely.Models;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.OEmbedBlock
{
    [ContentType(DisplayName = "oEmbed Block",
        GUID = "05d07afb-b1a5-48fc-8490-6c850c2c5059",
        Description = "Display oEmbed video",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/video.png")]
    public class OEmbedBlock : FoundationBlockData
    {
        [BackingType(typeof(PropertyOEmbed))]
        public virtual OEmbedModel Video { get; set; }

        [Editable(false)]
        public bool HasVideo => !string.IsNullOrEmpty(Video?.Html);

        [Editable(false)]
        public string InverseAspect
        {
            get
            {
                var width = Video.Width;
                var height = Video.Height;
                var aspect = width / (double)height;
                var padding = 1 / aspect * 100;

                return $"{padding}%";
            }            
        }
    }
}
