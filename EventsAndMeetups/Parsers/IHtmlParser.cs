using System;
using System.Threading.Tasks;

namespace EventsAndMeetups.Parsers
{
    public interface IHTMLParser
    {
        Task ParseAsync();
    }
}
