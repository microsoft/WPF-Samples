// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace EditingExaminerDemo
{
    /// <summary>
    ///     Class used to hold a parsed object
    /// </summary>
    public class ObjectItem
    {
        /// <summary>
        ///     Constructor used to create an object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="error"></param>
        public ObjectItem(object value, string error)
        {
            Value = value;
            Error = error;
        }

        /// <summary>
        ///     Get the parsed object
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///     Get the error message.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        ///     Override the ToSring method
        /// </summary>
        /// <returns></returns>
        public override string ToString() => string.IsNullOrEmpty(Error) ? Value?.ToString() ?? "null" : Error;
    }
}