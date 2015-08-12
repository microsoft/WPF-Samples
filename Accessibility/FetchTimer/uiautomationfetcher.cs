// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;

namespace FetchTimer
{
    internal class UiAutomationFetcher
    {
        // Number of properties fetched for each test.
        private const int NumberOfFetches = 5;
        // Member variables

        // Application form, for output.
        private readonly FetchTimerForm _appForm;
        private readonly AutomationElementMode _mode;
        private readonly Point _targetPoint;
        private readonly TreeScope _treeScope;
        private int _cachedPropCount;
        private int _currentPropCount;
        private int _elementCount;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="form">The application form.</param>
        /// <param name="targetPoint">The screen coordinates of the cursor.</param>
        /// <param name="scope">The TreeScope for caching.</param>
        /// <param name="mode">The mode for caching.</param>
        public UiAutomationFetcher(FetchTimerForm form,
            Point targetPt,
            TreeScope scope, AutomationElementMode cacheMode)
        {
            _appForm = form;
            _treeScope = scope;
            _targetPoint = targetPt;
            _mode = cacheMode;
        }

        public void DoWork()
        {
            long timeToGetUncached = 0;
            long timeToGetCached = 0;

            // Create a System.Diagnostics.Stopwatch.
            var stopWatchTimer = new Stopwatch();


            // TEST 1: Get the target element without caching, and retrieve
            //  current properties.

            stopWatchTimer.Start();
            AutomationElement targetNoCache = null;
            try
            {
                targetNoCache = AutomationElement.FromPoint(_targetPoint);
            }
            catch (ElementNotAvailableException)
            {
                OutputLine("Could not retrieve element.");
                return;
            }

            // Get current properties.
            _currentPropCount = 0;
            GetCurrentProperties(targetNoCache, 0);
            stopWatchTimer.Stop();
            timeToGetUncached = stopWatchTimer.Elapsed.Ticks;


            // TEST 2: Get the target element with caching, and retrieve
            //   cached properties.

            // Create CacheRequest.
            var fetchRequest = new CacheRequest();

            // Add properties to fetch.
            fetchRequest.Add(AutomationElement.NameProperty);
            fetchRequest.Add(AutomationElement.AutomationIdProperty);
            fetchRequest.Add(AutomationElement.ControlTypeProperty);
            fetchRequest.Add(AutomationElement.FrameworkIdProperty);
            fetchRequest.Add(AutomationElement.IsContentElementProperty);

            // Set options.
            fetchRequest.AutomationElementMode = _mode;
            fetchRequest.TreeScope = _treeScope;
            fetchRequest.TreeFilter = Automation.RawViewCondition;

            // Activate the CacheRequest and fetch the target.
            AutomationElement targetCached = null;
            using (fetchRequest.Activate())
            {
                stopWatchTimer.Reset();
                stopWatchTimer.Start();
                try
                {
                    targetCached = AutomationElement.FromPoint(_targetPoint);
                }
                catch (InvalidOperationException)
                {
                    OutputLine("InvalidOperationException. Could not retrieve element.");
                    return;
                }
                catch (ElementNotAvailableException)
                {
                    OutputLine("ElementNotAvailableException. Could not retrieve element.");
                    return;
                }
            } // CacheRequest is now inactive.

            // Get cached properties.
            GetCachedProperties(targetCached, true);
            stopWatchTimer.Stop();
            timeToGetCached = stopWatchTimer.Elapsed.Ticks;

            // TEST 3: Get updated cache.

            stopWatchTimer.Reset();
            stopWatchTimer.Start();
            var cacheUpdated = false;
            if (_mode == AutomationElementMode.Full)
            {
                var updatedTargetCached = targetCached.GetUpdatedCache(fetchRequest);
                GetCachedProperties(updatedTargetCached, false);
                // Fetches were counted again, so divide count by 2.
                _cachedPropCount /= 2;
                cacheUpdated = true;
                stopWatchTimer.Stop();
            }
            var updateTicks = stopWatchTimer.Elapsed.Ticks;

            // END OF TESTS. 

            // Display results

            var nameProperty = targetNoCache.Current.Name;
            var framework = targetNoCache.Current.FrameworkId;
            OutputLine("Name: " + nameProperty);
            OutputLine("Framework: " + framework);
            OutputLine(_elementCount + " cached element(s).");

            OutputLine(timeToGetUncached.ToString("N0") + " Ticks to retrieve element(s) and get "
                       + _currentPropCount + " current properties.");
            OutputLine(timeToGetCached.ToString("N0") + " Ticks to retrieve element(s) and get "
                       + _cachedPropCount + " cached properties.");

            // Show ratio between current and cached performance.
            var ratio = timeToGetUncached/(float) timeToGetCached;
            if (ratio > 2)
            {
                OutputLine("Current:Cached = " + ratio.ToString("N0") + ":1");
            }
            else
            {
                // Print with decimal.
                OutputLine("Current:Cached = " + ratio.ToString("N1") + ":1");
            }
            if (cacheUpdated)
            {
                OutputLine(updateTicks.ToString("N0") + " Ticks to update cache and get properties.");
            }
            else
            {
                OutputLine("Cannot update cache in None mode.");
            }
            OutputLine("");
        }

        /// <summary>
        ///     Walks the tree and gets properties from all elements found. Recursive.
        /// </summary>
        /// <param name="element">Node to walk.</param>
        /// <param name="depth">
        ///     Depth of this iteration (distance from initial node).
        /// </param>
        /// <remarks>
        ///     Nothing is done with the objects retrieved.
        /// </remarks>
        private void GetCurrentProperties(AutomationElement element, int depth)
        {
            if ((_treeScope == TreeScope.Element) && (depth > 0))
            {
                return;
            }
            if (((_treeScope & TreeScope.Descendants) == 0) && (depth > 1))
            {
                return;
            }
            var name = element.Current.Name;
            var id = element.Current.AutomationId;
            var controlType = element.Current.ControlType;
            var framework = element.Current.FrameworkId;
            _currentPropCount += NumberOfFetches;

            var walker = TreeWalker.ContentViewWalker;
            var elementChild = walker.GetFirstChild(element);
            while (elementChild != null)
            {
                GetCurrentProperties(elementChild, depth + 1);
                elementChild = walker.GetNextSibling(elementChild);
            }
        }

        /// <summary>
        ///     Gets a set of cached properties. Recursive.
        /// </summary>
        /// <param name="element">The target element.</param>
        /// <remarks>
        ///     Nothing is done with the objects retrieved.
        /// </remarks>
        private void GetCachedProperties(AutomationElement element, bool updateCount)
        {
            if (updateCount)
            {
                _elementCount++;
            }
            var name = element.Cached.Name;
            var s = element.Cached.AutomationId;
            var controlType = element.Cached.ControlType;
            var frame = element.Cached.FrameworkId;
            _cachedPropCount += NumberOfFetches;

            try
            {
                foreach (AutomationElement child in element.CachedChildren)
                {
                    GetCachedProperties(child, updateCount);
                }
            }
            catch (InvalidOperationException)
            {
                // Expected; there might be no CachedChildren, in which case an 
                //exception is raised when the property is accessed.
            }
        }

        /// <summary>
        ///     Prints a line of text to the textbox.
        /// </summary>
        /// <param name="outputStr">The string to print.</param>
        /// <remarks>
        ///     Must use Invoke so that UI is not being called directly from this thread.
        /// </remarks>
        private void OutputLine(string outputStr)
        {
            _appForm.Invoke(_appForm.OutputMethodInstance, outputStr + Environment.NewLine);
        }
    } // UIAutoWorker class.
}