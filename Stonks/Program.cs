using System;
using System.Collections.Generic;
using System.Linq;

namespace Stonks
{
    class Program {
        static void Main(string[] args) {

            State s = new State() {
                capital = 20000,
                salary = 1700,
                salary_appreciation = 250,
                capital_compounding = 0
            };

            List<TimeEvent> events = new List<TimeEvent>();
            events.Add(new TimeEvent("Buy rental property", 6, () => { s.capital_compounding = 0.025; return 0; }));
            events.Add(new TimeEvent("Salary stops appreciating", 4, () => { s.capital_compounding = 0.025; return 0; }));

            for (int i = 0; i < 13; i++) {
                s.capital = s.JobFactor() + s.MeanCapitalInYear() * s.capital_compounding;
                s.salary = s.salary + s.salary_appreciation;
                string executedEvents = string.Join(',', events.Select(x => x.Execute(i)).ToArray()).Replace(",", "");
                Console.WriteLine($"{28 + i} Capital: {string.Format("{0:0.#}", s.capital)}e Salary: {s.salary}e Events: {executedEvents}");
                
            }
        }
    }

    class State {
        public double capital { get; set; }
        public double salary { get; set; }
        public double salary_appreciation { get; set; }
        public double capital_compounding { get; set; }

        public double JobFactor() {
            return capital + 12 * salary;
        }

        public double MeanCapitalInYear() {
            return (2 * capital + 12 * salary) / 2;
        }
    }
    class TimeEvent {
        public string Name { get; set; }
        int executeInYear;
        Func<int> toExecute;

        public TimeEvent(string name, int executeInYear, Func<int> toExecute) {
            this.Name = name;
            this.toExecute = toExecute;
            this.executeInYear = executeInYear;
        }

        public string Execute(int year) {
            if (year == executeInYear) {
                toExecute();
                return Name;
            }
            return "";
        }
    }
}
