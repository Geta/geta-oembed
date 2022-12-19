// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Client.Models;
using Geta.OEmbed.Optimizely.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Geta.OEmbed.Optimizely
{
    public class OEmbedCmsController : ControllerBase
    {
        private readonly IOptimizelyOEmbedHandler _oEmbedHandler;

        public OEmbedCmsController(IOptimizelyOEmbedHandler embedHandler)
        {
            _oEmbedHandler = embedHandler;
        }
        
        public virtual async Task<IActionResult> Index([FromQuery] OEmbedRequest query, CancellationToken cancellationToken)
        {
            var oEmbedEntry = await _oEmbedHandler.HandleAsync(query, cancellationToken);
            if (oEmbedEntry is null)
            {
                return NotFound();
            }
            
            return Ok(oEmbedEntry);
        }
    }
}
