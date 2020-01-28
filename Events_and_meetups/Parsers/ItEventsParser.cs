using AngleSharp;
using System.Net;
using AngleSharp.Dom;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Events_and_meetups.Parsers
{
    public class ItEventsParser : HTMLParser
    {
        public ItEventsParser() {}

        public struct Event
        {
            readonly string _type, _name, _time, _location;

            public Event(string type, string name, string time, string location)
            {
                _type = type;
                _name = name;
                _time = time;
                _location = location;
            }

            public string toString()
            {
                return $"{_name}, {_type} будет проходить {_time}, {_location}";
            }
        }

        public async void Parse()
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            for(int page=0; page <= 10; page++)
            {
                await parsePage(context, getHTMLPageString($"https://it-events.com/?page={page}"));
            }
        }

        private string getHTMLPageString(string siteUrl)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(siteUrl);
            }
        }

        async private Task parsePage(IBrowsingContext context, string htmlString)
        {
            var document = await context.OpenAsync(req => req.Content(htmlString));
            var events = document.QuerySelectorAll(".event-list-item");
            var parsedEvents = new List<Event>();

            foreach (var e in events)
            {
                parsedEvents.Add(parseEvent(e));
            }

            foreach(var e in parsedEvents)
            {
                Console.WriteLine(e.toString());
            }
        }

        private Event parseEvent(IElement evnt)
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
