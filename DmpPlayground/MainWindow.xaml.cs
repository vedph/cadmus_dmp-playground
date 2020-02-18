using DiffMatchPatch;
using Dmp.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DmpPlayground
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly diff_match_patch _dmp;
        private readonly YXEditOperationDiffAdapter _adapter;

        private string _textA;
        private string _textB;
        private bool _isMoveEnabled;
        private bool _isReplaceEnabled;
        private string _result;

        public event PropertyChangedEventHandler PropertyChanged;

        public string TextA
        {
            get => _textA;
            set
            {
                _textA = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(TextA)));
            }
        }

        public string TextB
        {
            get => _textB;
            set
            {
                _textB = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(TextB)));
            }
        }

        public bool IsMoveEnabled
        {
            get => _isMoveEnabled;
            set
            {
                _isMoveEnabled = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(IsMoveEnabled)));
            }
        }

        public bool IsReplaceEnabled
        {
            get => _isReplaceEnabled;
            set
            {
                _isReplaceEnabled = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(IsReplaceEnabled)));
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Result)));
            }
        }

        public MainWindow()
        {
            _dmp = new diff_match_patch();
            _isMoveEnabled = true;
            _isReplaceEnabled = true;
            _adapter = new YXEditOperationDiffAdapter();
            DataContext = this;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TextA = TextB = "alpha beta\r\n" +
                    "gamma\r\n" +
                    "delta epsilon waw\r\n" +
                    "eta";
            _txtA.Focus();
        }

        private string DumpDiffs(IList<Diff> diffs)
        {
            StringBuilder sb = new StringBuilder();
            int n = 0;
            foreach (Diff diff in diffs)
            {
                sb.Append('(').Append(++n).AppendLine(")");
                sb.AppendLine(YXEditOperation.FilterTextForDisplay(diff.ToString()));
            }
            return sb.ToString();
        }

        private string DumpEditOperations(IList<YXEditOperation> operations)
        {
            StringBuilder sb = new StringBuilder();
            int n = 0;
            foreach (YXEditOperation op in operations)
            {
                sb.Append('(').Append(++n).AppendLine(")");
                sb.AppendLine(op.ToString());
            }
            return sb.ToString();
        }

        private void Diff(bool semanticCleanup)
        {
            List<Diff> diffs = _dmp.diff_main(_textA, _textB);

            if (semanticCleanup) _dmp.diff_cleanupSemanticLossless(diffs);
            Result = DumpDiffs(diffs);
        }

        private void CopyAToBCommand_Executed(object sender,
            ExecutedRoutedEventArgs e)
        {
            TextB = _textA;
            _txtB.Focus();
        }

        private void CopyBToACommand_Executed(object sender,
            ExecutedRoutedEventArgs e)
        {
            TextA = _textB;
            _txtA.Focus();
        }

        private void SwapAWithBCommand_Executed(object sender,
            ExecutedRoutedEventArgs e)
        {
            string b = _textB;
            TextB = TextA;
            TextA = b;
        }

        private void DiffCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Diff(false);
        }

        private void CleanDiffCommand_Executed(object sender,
            ExecutedRoutedEventArgs e)
        {
            Diff(true);
        }

        private void EditOperationsCommand_Executed(object sender,
            ExecutedRoutedEventArgs e)
        {
            List<Diff> diffs = _dmp.diff_main(_textA, _textB);
            _dmp.diff_cleanupSemanticLossless(diffs);

            _adapter.IsMoveEnabled = _isMoveEnabled;
            _adapter.IsReplaceEnabled = _isReplaceEnabled;
            IList<YXEditOperation> operations = _adapter.Adapt(diffs);
            Result = DumpEditOperations(operations);
        }
    }
}
