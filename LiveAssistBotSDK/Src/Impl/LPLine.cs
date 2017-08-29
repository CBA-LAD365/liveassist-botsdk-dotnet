using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Cafex.LiveAssist.Bot.Impl
{
    internal class LPLine
    {
        /*
           {
               "event": {
                   "@type":"line",
                   "text":"hello world"
               }
           }   
       */

        [JsonProperty(PropertyName = "event")]
        internal LineEvent lineEvent;

        internal LPLine(string text)
        {
            this.lineEvent = new LineEvent(text);
        }
    }

        internal class LineEvent
        {
            [JsonProperty(PropertyName = "@type")]
            public string type = "line";

            [JsonProperty(PropertyName = "text")]
            public string text { get; set; }

            public LineEvent(string text)
            {
                this.text = text;
            }
        }
 }