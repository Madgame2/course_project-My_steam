using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig;

namespace My_steam_client.Scripts
{
    public static class MDReader
    {
        public static string MDtoHTML(string mdText)
        {
            var pipleine = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

            return Markdown.ToHtml(mdText, pipleine);
        }
    }
}
