// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace EditingExaminerDemo
{
    /// <summary>
    ///     The type of invoke we support (or plan to support).
    /// </summary>
    public enum InvokeType
    {
        /// <summary>
        ///     Invoke static method
        /// </summary>
        StaticMethod = 0,

        /// <summary>
        ///     Invoke instance method (need this pointer to be passed as first parameter in InvokeStaticOrInstanceMethod)
        /// </summary>
        InstanceMethod,

        /// <summary>
        ///     Invoke static get field
        /// </summary>
        GetStaticField,

        /// <summary>
        ///     Invoke static set field
        /// </summary>
        SetStaticField,

        /// <summary>
        ///     Invoke instance get field
        /// </summary>
        GetInstanceField,

        /// <summary>
        ///     Invoke instance set field
        /// </summary>
        SetInstanceField,

        /// <summary>
        ///     Invoke static get property
        /// </summary>
        GetStaticProperty,

        /// <summary>
        ///     Invoke static set property
        /// </summary>
        SetStaticProperty,

        /// <summary>
        ///     Invoke instance get property
        /// </summary>
        GetInstanceProperty,

        /// <summary>
        ///     Invoke instance set property
        /// </summary>
        SetInstanceProperty
    }
}