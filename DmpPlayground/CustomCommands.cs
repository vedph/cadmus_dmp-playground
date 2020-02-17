using System.Windows.Input;

namespace DmpPlayground
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand CopyAToBCommand =
            new RoutedUICommand("CopyAToB",
                "Copy the A text to B",
                typeof(CustomCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.F2)
                }
            );

        public static readonly RoutedUICommand CopyBToACommand =
            new RoutedUICommand("CopyBToA",
                "Copy the B text to A",
                typeof(CustomCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.F2, ModifierKeys.Shift)
                }
            );

        public static readonly RoutedUICommand SwapAWithBCommand =
            new RoutedUICommand("SwapAWithB",
                "Swap the A text with the B text",
                typeof(CustomCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.F2, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand DiffCommand =
            new RoutedUICommand("Diff",
                "Calculate plain diffs",
                typeof(CustomCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.F3)
                }
            );

        public static readonly RoutedUICommand CleanDiffCommand =
            new RoutedUICommand("CleanDiff",
                "Calculate semantically clean diffs",
                typeof(CustomCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.F4)
                }
            );

        public static readonly RoutedUICommand EditOperationsCommand =
            new RoutedUICommand("EditOperations",
            "Calculate layer edit operations",
            typeof(CustomCommands),
            new InputGestureCollection
            {
                new KeyGesture(Key.F5)
            }
        );
    }
}
