using System;
namespace EventsAndMeetups.Parsers
{
    public class Event
    {
        private readonly string _type;
        private readonly string _name;
        private readonly string _time;
        private readonly string _location;

        public Event(string type, string name, string time, string location)
        {
            _type = type;
            _name = name;
            _time = time;
            _location = location;
        }

        public override string ToString()
        {
            return $"{_name}, {_type} будет проходить {_time}, {_location}";
        }
    }
}
