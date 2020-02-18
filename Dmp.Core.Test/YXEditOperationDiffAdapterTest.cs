using DiffMatchPatch;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dmp.Core.Test
{
    public sealed class YXEditOperationDiffAdapterTest
    {
        private static IList<YXEditOperation> GetOperations(string a, string b)
        {
            diff_match_patch dmp = new diff_match_patch();
            List<Diff> diffs = dmp.diff_main(a, b);
            dmp.diff_cleanupSemanticLossless(diffs);
            YXEditOperationDiffAdapter adapter = new YXEditOperationDiffAdapter();

            return adapter.Adapt(diffs);
        }

        [Fact]
        public void Adapt_AllEqual_Ok()
        {
            const string a = "alpha beta\ngamma";
            const string b = a;

            IList<YXEditOperation> operations = GetOperations(a, b);

            Assert.Equal(3, operations.Count);
            Assert.True(operations.All(o => o.Operator == YXEditOperation.EQU));

            YXEditOperation op = operations[0];
            Assert.Equal("alpha", op.Value);
            Assert.Equal("1.1", op.Location);
            Assert.Equal("1.1", op.OldLocation);

            op = operations[1];
            Assert.Equal("beta", op.Value);
            Assert.Equal("1.2", op.Location);
            Assert.Equal("1.2", op.OldLocation);

            op = operations[2];
            Assert.Equal("gamma", op.Value);
            Assert.Equal("2.1", op.Location);
            Assert.Equal("2.1", op.OldLocation);
        }

        [Fact]
        public void Adapt_HrzDelFirst_Ok()
        {
            const string a = "alpha beta gamma\ndelta";
            const string b = "beta gamma\ndelta";

            IList<YXEditOperation> operations = GetOperations(a, b);

            Assert.Equal(4, operations.Count);

            YXEditOperation op = operations[0];
            Assert.Equal(YXEditOperation.DEL, op.Operator);
            Assert.Equal("alpha", op.Value);
            Assert.Equal("1.1", op.Location);
            Assert.Equal("1.1", op.OldLocation);

            op = operations[1];
            Assert.Equal(YXEditOperation.EQU, op.Operator);
            Assert.Equal("beta", op.Value);
            Assert.Equal("1.1", op.Location);
            Assert.Equal("1.2", op.OldLocation);

            op = operations[2];
            Assert.Equal(YXEditOperation.EQU, op.Operator);
            Assert.Equal("gamma", op.Value);
            Assert.Equal("1.2", op.Location);
            Assert.Equal("1.3", op.OldLocation);

            op = operations[3];
            Assert.Equal(YXEditOperation.EQU, op.Operator);
            Assert.Equal("delta", op.Value);
            Assert.Equal("2.1", op.Location);
            Assert.Equal("2.1", op.OldLocation);
        }

        [Fact]
        public void Adapt_HrzDelMid_Ok()
        {
            const string a = "alpha beta gamma\ndelta";
            const string b = "alpha gamma\ndelta";

            IList<YXEditOperation> operations = GetOperations(a, b);

            Assert.Equal(4, operations.Count);

            YXEditOperation op = operations[0];
            Assert.Equal(YXEditOperation.EQU, op.Operator);
            Assert.Equal("alpha", op.Value);
            Assert.Equal("1.1", op.Location);
            Assert.Equal("1.1", op.OldLocation);

            op = operations[1];
            Assert.Equal(YXEditOperation.DEL, op.Operator);
            Assert.Equal("beta", op.Value);
            Assert.Equal("1.2", op.Location);
            Assert.Equal("1.2", op.OldLocation);

            op = operations[2];
            Assert.Equal(YXEditOperation.EQU, op.Operator);
            Assert.Equal("gamma", op.Value);
            Assert.Equal("1.2", op.Location);
            Assert.Equal("1.3", op.OldLocation);

            op = operations[3];
            Assert.Equal(YXEditOperation.EQU, op.Operator);
            Assert.Equal("delta", op.Value);
            Assert.Equal("2.1", op.Location);
            Assert.Equal("2.1", op.OldLocation);
        }

        [Fact]
        public void Adapt_HrzDelLast_Ok()
        {
            const string a = "alpha beta gamma\ndelta";
            const string b = "alpha beta\ndelta";

            IList<YXEditOperation> operations = GetOperations(a, b);

            Assert.Equal(4, operations.Count);

            YXEditOperation op = operations[0];
            Assert.Equal(YXEditOperation.EQU, op.Operator);
            Assert.Equal("alpha", op.Value);
            Assert.Equal("1.1", op.Location);
            Assert.Equal("1.1", op.OldLocation);

            op = operations[1];
            Assert.Equal(YXEditOperation.EQU, op.Operator);
            Assert.Equal("beta", op.Value);
            Assert.Equal("1.2", op.Location);
            Assert.Equal("1.2", op.OldLocation);

            op = operations[2];
            Assert.Equal(YXEditOperation.DEL, op.Operator);
            Assert.Equal("gamma", op.Value);
            Assert.Equal("1.2", op.Location);
            Assert.Equal("1.3", op.OldLocation);

            op = operations[3];
            Assert.Equal(YXEditOperation.EQU, op.Operator);
            Assert.Equal("delta", op.Value);
            Assert.Equal("2.1", op.Location);
            Assert.Equal("2.1", op.OldLocation);
        }
    }
}
