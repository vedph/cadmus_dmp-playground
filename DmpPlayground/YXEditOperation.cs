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
        /// Gets or sets the old location.
        /// </summary>
        public string OldLocation { get; set; }

        /// <summary>
        /// Gets or sets the new location.
        /// </summary>
        public string NewLocation { get; set; }

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
        /// Filters the text for displaying CR, LF and space with symbols.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The filtered text</returns>
        public static string FilterTextForDisplay(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            // https://superuser.com/questions/382163/how-do-i-visualize-cr-lf-in-word
            StringBuilder sb = new StringBuilder(text);
            sb.Replace('\r', '\u21a9');
            sb.Replace('\n', '\u240d');
            sb.Replace(' ', '\u00b7');
            return sb.ToString();
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(OldLocation).Append("->")
                .Append(NewLocation).Append(' ')
                .Append(Operator).Append(' ');

            if (OldValue != null)
                sb.Append(OldValue).Append("->").Append(FilterTextForDisplay(Value));
            else
                sb.Append(FilterTextForDisplay(Value));

            sb.Append(GroupId > 0? $" ({GroupId})" : "");
            return sb.ToString();
        }
    }
}
