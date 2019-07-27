using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class Part : IEquatable<Part>, IComparable<Part>
    {
        public string PartName { get; set; }

        public int PartScore { get; set; }

        public override string ToString()
        {
            return "Score: " + PartScore + "   Name: " + PartName;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Part objAsPart = obj as Part;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public int SortByNameAscending(string name1, string name2)
        {

            return name1.CompareTo(name2);
        }

        // Default comparer for Part type.
        public int CompareTo(Part comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
                return 1;

            else
                return this.PartScore.CompareTo(comparePart.PartScore);
        }
        public override int GetHashCode()
        {
            return PartScore;
        }
        public bool Equals(Part other)
        {
            if (other == null) return false;
            return (this.PartScore.Equals(other.PartScore));
        }
        // Should also override == and != operators.
    }
}
