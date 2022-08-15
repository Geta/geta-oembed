// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geta.OEmbed.Optimizely
{
    public class OEmbedCmsController : ControllerBase
    {
        private readonly IOEmbedService _oEmbedService;

        public OEmbedCmsController(IOEmbedService embedService)
        {
            _oEmbedService = embedService;
        }
        
        public virtual async Task<IActionResult> Index([FromQuery] OEmbedRequest query, CancellationToken cancellationToken)
        {
            var oEmbedEntry = await _oEmbedService.GetAsync(query.Url, query, cancellationToken);
            if (oEmbedEntry is null)
            {
                return NotFound();
            }
            
            return Ok(oEmbedEntry);
        }
    }
}