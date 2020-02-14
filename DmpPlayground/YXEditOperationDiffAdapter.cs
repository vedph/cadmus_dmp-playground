using DiffMatchPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DmpPlayground
{
    /// <summary>
    /// YX-coordinates-based edit operation diff adapter.
    /// This adapter is used to produce a list of <see cref="YXEditOperation"/>'s
    /// from a list of <see cref="Diff"/> operations.
    /// </summary>
    public sealed class YXEditOperationDiffAdapter :
        IEditOperationDiffAdapter<YXEditOperation>
    {
        private readonly Regex _lineRegex;

        /// <summary>
        /// Gets or sets a value indicating whether the move edit operation
        /// is enabled. Default is true.
        /// </summary>
        public bool IsMoveEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the replace edit operation
        /// is enabled. Default is true.
        /// </summary>
        public bool IsReplaceEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YXEditOperationDiffAdapter"/>
        /// class.
        /// </summary>
        public YXEditOperationDiffAdapter()
        {
            _lineRegex = new Regex(@"([^\r\n]*)(\r?\n)?");
            IsMoveEnabled = true;
            IsReplaceEnabled = true;
        }

        private string GetEditOperation(Operation op)
        {
            return op switch
            {
                Operation.EQUAL => "equ",
                Operation.INSERT => "ins",
                Operation.DELETE => "del",
                _ => null,
            };
        }

        private static int LocateNextOperationWithValue(
            IList<YXEditOperation> operations,
            int start,
            string value)
        {
            int i = start;
            while (i < operations.Count && operations[i].Value != value)
                i++;
            return i == operations.Count ? -1 : i;
        }

        private static int LocatePrevOperationWithValue(
            IList<YXEditOperation> operations,
            int start,
            string value)
        {
            int i = start;
            while (i > -1 && operations[i].Value != value)
                i--;
            return i;
        }

        private void DetectForwardMoves(IList<YXEditOperation> operations)
        {
            int maxGroupId = operations.Max(o => o.GroupId);

            for (int i = 0; i < operations.Count - 1; i++)
            {
                if (operations[i].Operator == "del")
                {
                    int j = LocateNextOperationWithValue(
                        operations, i + 1, operations[i].Value);
                    if (j > -1)
                    {
                        operations[i].Operator = "mvd";
                        operations[i].GroupId = ++maxGroupId;
                        operations[j].Operator = "mvi";
                        operations[j].GroupId = maxGroupId;
                    }
                }
            }
        }

        private void DetectBackwardMoves(IList<YXEditOperation> operations)
        {
            int maxGroupId = operations.Max(o => o.GroupId);

            for (int i = operations.Count - 1; i > 0; i--)
            {
                if (operations[i].Operator == "del")
                {
                    int j = LocatePrevOperationWithValue(
                    operations, i - 1, operations[i].Value);
                    if (j > -1)
                    {
                        operations[i].Operator = "mvd";
                        operations[i].GroupId = ++maxGroupId;
                        operations[j].Operator = "mvi";
                        operations[j].GroupId = maxGroupId;
                    }
                }
            }
        }

        private void DetectReplacememts(IList<YXEditOperation> operations)
        {
            for (int i = operations.Count - 1; i > 0; i--)
            {
                if (operations[i].Location == operations[i - 1].Location
                    && operations[i - 1].Operator == "del"
                    && operations[i].Operator == "ins")
                {
                    operations[i - 1].Operator = "rep";
                    operations[i - 1].OldValue = operations[i - 1].Value;
                    operations[i - 1].Value = operations[i].Value;
                    operations.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Adapts the specified diffs list into a list of
        /// <see cref="YXEditOperation"/>'s.
        /// </summary>
        /// <param name="diffs">The diffs.</param>
        /// <returns>The edit operations.</returns>
        public IList<YXEditOperation> Adapt(IList<Diff> diffs)
        {
            List<YXEditOperation> operations = new List<YXEditOperation>();

            int y = 1, x = 0;
            foreach (Diff diff in diffs)
            {
                foreach (Match match in _lineRegex.Matches(diff.text))
                {
                    foreach (string token in match.Groups[1].Value.Split(' ',
                        StringSplitOptions.RemoveEmptyEntries))
                    {
                        x++;
                        operations.Add(new YXEditOperation
                        {
                            Location = $"{y}.{x}",
                            Operator = GetEditOperation(diff.operation),
                            Value = token
                        });
                        if (diff.operation == Operation.DELETE) x--;
                    }
                    if (match.Groups[2].Length > 0)
                    {
                        y++;
                        x = 0;
                    }
                }
            }

            if (IsMoveEnabled)
            {
                DetectForwardMoves(operations);
                DetectBackwardMoves(operations);
            }

            if (IsReplaceEnabled) DetectReplacememts(operations);

            return operations;
        }
    }
}
