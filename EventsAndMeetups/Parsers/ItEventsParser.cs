using AngleSharp;
using System.Net;
using AngleSharp.Dom;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EventsAndMeetups.Parsers
{
    public class ItEventsParser : IHTMLParser
    { 
        public async Task ParseAsync()
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            for (int page = 0; page <= 10; page++)
            {
                await ParsePageAsync(context, GetHTMLPageString($"https://it-events.com/?page={page}"));
            }
        }

        private string GetHTMLPageString(string siteUrl)
        {
            using var client = new WebClient();
            return client.DownloadString(siteUrl);
        }

        private async Task ParsePageAsync(IBrowsingContext context, string htmlString)
        {
            var document = await context.OpenAsync(req => req.Content(htmlString));
            var events = document.QuerySelectorAll(".event-list-item");
            var parsedEvents = new List<Event>();

            foreach (var e in events)
            {
                parsedEvents.Add(ParseEvent(e));
            }

            foreach (var e in parsedEvents)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private Event ParseEvent(IElement evnt)
        {
            var type = getElementText(evnt, ".event-list-item__type");
            var name = getElementText(evnt, ".event-list-item__title");
            var time = getElementText(evnt, ".event-list-item__info");
            var location = getElementText(evnt,
                ".event-list-item__info.event-list-item__info_location," +
                ".event-list-item__info.event-list-item__info_online");
            return new Event(type, name, time, location);
        }

        private string getElementText(IElement evnt, string selector)
        {
            return evnt.QuerySelector(selector)?.InnerHtml.Replace("\n", "") ?? "";
        }
    }
}
