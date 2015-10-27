using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resourceManagement.Contracts
{
    internal class resourceMeasures
    {
        public int RPM { get; set; }
        public double Temperature { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTime LastMeasure { get; set; }
        public DateTime InServiceDate { get; set; }
        public bool IsRunning { get; set; }
    }
    internal class resourceMeasures2
    {
        public int RPM { get; set; }
        public double Temperature { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTime LastMeasure { get; set; }
        public DateTime InServiceDate { get; set; }
        public bool IsRunning { get; set; }
        public bool RequiresService { get; set; }
    }
}
