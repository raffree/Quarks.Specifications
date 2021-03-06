﻿using System;
using System.Linq.Expressions;

namespace Quarks.Specifications
{
    /// <summary>
    /// Represent a Expression Specification
    /// </summary>
    public static class Specification
    {
        /// <summary>
        /// Returns an empty specification
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>Empty specification</returns>
        public static Specification<TEntity> Empty<TEntity>() where TEntity : class
        {
            return new TrueSpecification<TEntity>();
        }
    }

    /// <summary>
    /// Represent a Expression Specification
    /// <remarks>
    /// Specification overload operators for create AND, OR or NOT specifications.
    /// Additionally overload AND and OR operators with the same sense of ( binary And and binary Or ).
    /// C# couldn’t overload the AND and OR operators directly since the framework doesn’t allow such craziness. But
    /// with overloading false and true operators this is posible. For explain this behavior please read
    /// http://msdn.microsoft.com/en-us/library/aa691312(VS.71).aspx
    /// </remarks>
    /// </summary>
    /// <typeparam name="TEntity">Type of item in the criteria</typeparam>
    public abstract class Specification<TEntity>
         where TEntity : class
	{
		private Expression<Func<TEntity, bool>> _expression;

        /// <summary>
        /// Checks if specified entity satisfies this specification. 
        /// </summary>
        /// <param name="entity">Entity to be checked.</param>
        /// <returns>true - if entity satisfies; otherwise - false</returns>
		public bool IsSatisfiedBy(TEntity entity)
		{
			return Predicate == null || Predicate(entity);
		}

        /// <summary>
        /// Predicate used by this specification.
        /// </summary>
		public Func<TEntity, bool> Predicate { get; protected set; }

        /// <summary>
        /// Expression used by this specification.
        /// </summary>
        public Expression<Func<TEntity, bool>> Expression
		{
			get { return _expression; }
			protected set
			{
				_expression = value;
				Predicate = _expression?.Compile();
			}
		}

        /// <summary>
        ///  And operator
        /// </summary>
        /// <param name="left">left operand in this AND operation</param>
        /// <param name="right">right operand in this AND operation</param>
        /// <returns>New specification</returns>
        public static Specification<TEntity> operator &(Specification<TEntity> left, Specification<TEntity> right)
		{
			return left.And(right);
		}

        /// <summary>
        /// Or operator
        /// </summary>
        /// <param name="left">left operand in this OR operation</param>
        /// <param name="right">right operand in this OR operation</param>
        /// <returns>New specification </returns>
        public static Specification<TEntity> operator |(Specification<TEntity> left, Specification<TEntity> right)
		{
			return left.Or(right);
		}

		/// <summary>
		/// Not specification
		/// </summary>
		/// <param name="specification">Specification to negate</param>
		/// <returns>New specification</returns>
		public static Specification<TEntity> operator !(Specification<TEntity> specification)
		{
			return specification.Not();
		}
    }
}