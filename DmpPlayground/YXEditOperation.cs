using System.Text;

namespace DmpPlayground
{
    /// <summary>
    /// A general purpose edit operation generated from diffing and based
    /// on Y/X coordinates.
    /// </summary>
    public class YXEditOperation
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the text value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the old text value. This is used for replacements only.
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Gets or sets the group identifier. This is used for movements only.
        /// The two delete and insert operations involved in a move operation
        /// are grouped under the same ID.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Location).Append(' ').Append(Operator).Append(' ');

            if (OldValue != null)
                sb.Append(OldValue).Append("->").Append(Value);
            else
                sb.Append(Value);

            sb.Append(GroupId > 0? $" ({GroupId})" : "");
            return sb.ToString();
        }
    }
}
