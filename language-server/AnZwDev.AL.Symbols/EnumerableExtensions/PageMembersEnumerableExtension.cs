using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.EnumerableExtensions
{
    public static class PageMembersEnumerableExtension
    {

        public static IEnumerable<PageControlSymbol> GetAllControls(this PageSymbol pageSymbol, HashSet<PageControlKind>? controlKindFilter = null)
        {
            return GetAllControls(pageSymbol.Controls, controlKindFilter);
        }

        public static IEnumerable<PageControlSymbol> GetAllControls(this List<PageControlSymbol>? controls, HashSet<PageControlKind>? controlKindFilter = null)
        {
            if ((controls != null) && (controls.Count > 0))
            {
                var stack = new Stack<EnumerableStackItem<PageControlSymbol>>();
                var currentStackItem = new EnumerableStackItem<PageControlSymbol>() { Items = controls, CurrentItemIndex = 0 };

                while (currentStackItem.CurrentItemIndex < currentStackItem.Items.Count)
                {
                    var currentControl = currentStackItem.Items[currentStackItem.CurrentItemIndex];

                    if ((controlKindFilter == null) || (controlKindFilter.Contains(currentControl.Kind)))
                        yield return currentControl;

                    currentStackItem.CurrentItemIndex++;

                    if ((currentControl.Controls != null) && (currentControl.Controls.Count > 0))
                    {
                        stack.Push(currentStackItem);
                        currentStackItem = new EnumerableStackItem<PageControlSymbol>() { Items = currentControl.Controls, CurrentItemIndex = 0 };
                    } 
                    else
                        while ((currentStackItem.CurrentItemIndex >= currentStackItem.Items.Count) && (stack.Count > 0))
                            currentStackItem = stack.Pop();
                }
            }
        }

        public static IEnumerable<PageActionSymbol> GetAllActions(this PageSymbol pageSymbol, HashSet<PageActionKind>? actionKindFilter = null)
        {
            return GetAllActions(pageSymbol.Actions, actionKindFilter);
        }

        public static IEnumerable<PageActionSymbol> GetAllActions(this List<PageActionSymbol>? actions, HashSet<PageActionKind>? actionKindFilter = null)
        {
            if ((actions != null) && (actions.Count > 0))
            {
                var stack = new Stack<EnumerableStackItem<PageActionSymbol>>();
                var currentStackItem = new EnumerableStackItem<PageActionSymbol>() { Items = actions, CurrentItemIndex = 0 };

                while (currentStackItem.CurrentItemIndex < currentStackItem.Items.Count)
                {
                    var currentAction = currentStackItem.Items[currentStackItem.CurrentItemIndex];

                    if ((actionKindFilter == null) || (actionKindFilter.Contains(currentAction.Kind)))
                        yield return currentAction;

                    currentStackItem.CurrentItemIndex++;

                    if ((currentAction.Actions != null) && (currentAction.Actions.Count > 0))
                    {
                        stack.Push(currentStackItem);
                        currentStackItem = new EnumerableStackItem<PageActionSymbol>() { Items = currentAction.Actions, CurrentItemIndex = 0 };
                    }
                    else
                        while ((currentStackItem.CurrentItemIndex >= currentStackItem.Items.Count) && (stack.Count > 0))
                            currentStackItem = stack.Pop();
                }
            }
        }


    }
}
