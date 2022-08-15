// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Microsoft.AspNetCore.Mvc;
using Geta.OEmbed.Models;

namespace Geta.OEmbed.AspNetCore.Mvc
{
    public class OEmbedController : ControllerBase
    {
        private readonly IOEmbedService _oEmbedService;

        public OEmbedController(IOEmbedService oEmbedService)
        {
            _oEmbedService = oEmbedService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get([FromQuery] OEmbedRequest query, CancellationToken cancellationToken)
        {
            var oEmbedEntry = await _oEmbedService.GetAsync(query.Url, query, cancellationToken);
            if (oEmbedEntry is not null)
            {
                return Ok(oEmbedEntry);
            }

            return NotFound();
        }
    }
}
