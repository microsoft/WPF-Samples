// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    //--------------------------------------------------------------

    // TokenTypes are bit-exclusive so that we can easily group them
    //  together in sets using a bit-vector

    //--------------------------------------------------------------

    public class TokenSet
    {
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private readonly uint _tokens;
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public TokenSet(TokenType type)
        {
            _tokens = (uint) type;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public TokenSet(TokenSet t)
        {
            _tokens = t._tokens;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private TokenSet(uint tokens)
        {
            _tokens = tokens;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static TokenSet operator +(TokenSet t1, TokenSet t2) => new TokenSet(t1._tokens | t2._tokens);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static TokenSet operator +(TokenSet t1, TokenType t2) => new TokenSet(t1._tokens | (uint)t2);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool Contains(TokenType type) => (_tokens & (uint)type) != 0;
    }
}