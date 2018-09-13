using System;
using System.Collections.Generic;
using Pathoschild.Stardew.Common.Utilities;

namespace ContentPatcher.Framework.Tokens
{
    /// <summary>A simple token whose properties are set externally.</summary>
    internal class ManualToken : IToken
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The token name.</summary>
        public TokenName Name { get; }

        /// <summary>Whether this token recognises subkeys (e.g. <c>Relationship:Abigail</c> is a <c>Relationship</c> token with a <c>Abigail</c> subkey).</summary>
        public bool RequiresSubkeys { get; } = false;

        /// <summary>Whether the token is applicable in the current context.</summary>
        public bool IsValidInContext { get; set; }

        /// <summary>Whether the value can change after it's initialised.</summary>
        public bool IsMutable { get; }

        /// <summary>Whether the token may contain multiple values.</summary>
        public bool CanHaveMultipleValues { get; set; }

        /// <summary>The allowed values (or <c>null</c> if any value is allowed).</summary>
        public InvariantHashSet AllowedValues { get; set; } = new InvariantHashSet();

        /// <summary>The current values.</summary>
        public InvariantHashSet Values { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="name">The token name.</param>
        /// <param name="isMutable">Whether the value can change after it's initialised.</param>
        /// <param name="values">The current values.</param>
        /// <param name="canHaveMultipleValues">Whether the token may contain multiple values.</param>
        public ManualToken(TokenName name, bool isMutable, InvariantHashSet values = null, bool canHaveMultipleValues = false)
        {
            this.Name = name;
            this.IsMutable = isMutable;
            this.Values = values ?? new InvariantHashSet();
            this.CanHaveMultipleValues = canHaveMultipleValues;
        }

        /// <summary>Construct an instance.</summary>
        /// <param name="name">The token name.</param>
        /// <param name="isMutable">Whether the value can change after it's initialised.</param>
        /// <param name="values">The current values.</param>
        /// <param name="canHaveMultipleValues">Whether the token may contain multiple values.</param>
        public ManualToken(string name, bool isMutable, InvariantHashSet values = null, bool canHaveMultipleValues = false)
            : this(TokenName.Parse(name), isMutable, values, canHaveMultipleValues) { }

        /// <summary>Update the token data when the context changes.</summary>
        /// <param name="context">The condition context.</param>
        /// <returns>Returns whether the token data changed.</returns>
        public void UpdateContext(IContext context) { }

        /// <summary>Perform custom validation on a set of input values.</summary>
        /// <param name="values">The values to validate.</param>
        /// <param name="error">The validation error, if any.</param>
        /// <returns>Returns whether validation succeeded.</returns>
        public bool TryCustomValidation(InvariantHashSet values, out string error)
        {
            error = null;
            return true;
        }

        /// <summary>Get the current subkeys (if supported).</summary>
        public IEnumerable<TokenName> GetSubkeys()
        {
            yield break;
        }

        /// <summary>Get the current token values.</summary>
        /// <param name="name">The token name to check, if applicable.</param>
        /// <exception cref="InvalidOperationException">The key doesn't match this token, or the key does not respect <see cref="IToken.RequiresSubkeys"/>.</exception>
        public IEnumerable<string> GetValues(TokenName? name = null)
        {
            if (name != null)
            {
                if (!this.Name.IsSameRootKey(name.Value))
                    throw new InvalidOperationException($"The specified token key ({name}) is not handled by this token ({this.Name}).");
                if (name.Value.HasSubkey())
                    throw new InvalidOperationException($"The '{this.Name}' token does not support subkeys (:).");
            }

            return this.Values;
        }
    }
}
