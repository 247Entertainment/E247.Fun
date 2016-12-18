using System;
using E247.Fun.Exceptions;

#pragma warning disable 1591

namespace E247.Fun
{
    /// <summary>
    /// A container for a value of one of the given types
    /// </summary>
    public struct Choice<T1 ,T2> : IEquatable<Choice<T1,T2>>
    {
        private readonly T1 _case1;
        private readonly T2 _case2;

        private readonly int _selectedCase;

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2}" /> with the given T1 as the available value
        /// </summary>
        public Choice(T1 value) : this()
        {
            _case1 = value;
            _selectedCase = 1;
        }

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2}" /> with the given T2 as the available value
        /// </summary>
        public Choice(T2 value) : this()
        {
            _case2 = value;
            _selectedCase = 2;
        }

        public static implicit operator Choice<T1,T2>(T1 input) => new Choice<T1, T2>(input);
        public static implicit operator Choice<T1, T2>(T2 input) => new Choice<T1, T2>(input);

        /// <summary>
        /// Given a function for each possible choice, will invoke the relevant function for the available value in this <see cref="Choice{T1,T2}"/> 
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2}"/> contain an instance of T2</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<T2, TR> case2)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 2)
                return case2(_case2);

            throw new FailedMatchException();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Choice<T1, T2> && Equals((Choice<T1, T2>)obj);
        }
        public override int GetHashCode()
        {
            return _case1.GetHashCode() ^ _case2.GetHashCode();
        }
        public bool Equals(Choice<T1, T2> other)
        {
            var selected = _selectedCase;
            var c1 = _case1;
            var c2 = _case2;

            return other.Match(
                x => selected == 1 && Equals(x, c1),
                y => selected == 2 && Equals(y, c2));
        }
        public static bool operator ==(Choice<T1, T2> lhs, Choice<T1, T2> rhs) => lhs.Equals(rhs);
        public static bool operator !=(Choice<T1, T2> lhs, Choice<T1, T2> rhs) => !lhs.Equals(rhs);
    }

    ///<see cref="Choice{T1,T2}"/>
    public struct Choice<T1, T2, T3> : IEquatable<Choice<T1, T2, T3>>
    {
        private readonly T1 _case1;
        private readonly T2 _case2;
        private readonly T3 _case3;

        private readonly int _selectedCase;

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3}" /> with the given T1 as the available value
        /// </summary>
        public Choice(T1 value) : this()
        {
            _case1 = value;
            _selectedCase = 1;
        }

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3}" /> with the given T2 as the available value
        /// </summary>
        public Choice(T2 value) : this()
        {
            _case2 = value;
            _selectedCase = 2;
        }

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3}" /> with the given T3 as the available value
        /// </summary>
        public Choice(T3 value) : this()
        {
            _case3 = value;
            _selectedCase = 3;
        }

        public static implicit operator Choice<T1, T2, T3>(T1 input) => new Choice<T1, T2, T3>(input);
        public static implicit operator Choice<T1, T2, T3>(T2 input) => new Choice<T1, T2, T3>(input);
        public static implicit operator Choice<T1, T2, T3>(T3 input) => new Choice<T1, T2, T3>(input);

        /// <summary>
        /// Given a function for each possible choice, will invoke the relevant function for the available value in this <see cref="Choice{T1,T2,T3}"/> 
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3}"/> contain an instance of T3</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<T2, TR> case2, Func<T3, TR> case3)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 3)
                return case3(_case3);

            throw new FailedMatchException();
        }
        /// <summary>
        /// Given a function to handle T1 and a function to be called should this <see cref="Choice{T1,T2,T3}"/> not contain a value of T1, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3}"/> contain an instance of T1</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3}"/> NOT contain an instance of T1</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T2 and a function to be called should this <see cref="Choice{T1,T2,T3}"/> not contain a value of T2, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3}"/> contain an instance of T2</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3}"/> NOT contain an instance of T2</param>
        public TR Match<TR>(Func<T2, TR> case2, Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T3 and a function to be called should this <see cref="Choice{T1,T2,T3}"/> not contain a value of T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3}"/> NOT contain an instance of T3</param>
        public TR Match<TR>(Func<T3, TR> case3, Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 3)
                return case3(_case3);

            return caseElse();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Choice<T1, T2, T3> && Equals((Choice<T1, T2, T3>)obj);
        }
        public override int GetHashCode()
        {
            return _case1.GetHashCode() ^ _case2.GetHashCode() ^ _case3.GetHashCode();
        }
        public bool Equals(Choice<T1, T2, T3> other)
        {
            var selected = _selectedCase;
            var c1 = _case1;
            var c2 = _case2;
            var c3 = _case3;

            return other.Match(
                x => selected == 1 && Equals(x, c1),
                y => selected == 2 && Equals(y, c2),
                z => selected == 3 && Equals(z, c3));
        }
        public static bool operator ==(Choice<T1, T2, T3> lhs, Choice<T1, T2, T3> rhs) => lhs.Equals(rhs);
        public static bool operator !=(Choice<T1, T2, T3> lhs, Choice<T1, T2, T3> rhs) => !lhs.Equals(rhs);
    }

    ///<see cref="Choice{T1,T2}"/>
    public struct Choice<T1, T2, T3, T4> : IEquatable<Choice<T1, T2, T3, T4>>
    {
        private readonly T1 _case1;
        private readonly T2 _case2;
        private readonly T3 _case3;
        private readonly T4 _case4;

        private readonly int _selectedCase;

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4}" /> with the given T1 as the available value
        /// </summary>
        public Choice(T1 value) : this()
        {
            _case1 = value;
            _selectedCase = 1;
        }

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4}" /> with the given T2 as the available value
        /// </summary>
        public Choice(T2 value) : this()
        {
            _case2 = value;
            _selectedCase = 2;
        }

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4}" /> with the given T3 as the available value
        /// </summary>
        public Choice(T3 value) : this()
        {
            _case3 = value;
            _selectedCase = 3;
        }

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4}" /> with the given T4 as the available value
        /// </summary>
        public Choice(T4 value) : this()
        {
            _case4 = value;
            _selectedCase = 4;
        } 

        public static implicit operator Choice<T1, T2, T3, T4>(T1 input) => new Choice<T1, T2, T3, T4>(input);
        public static implicit operator Choice<T1, T2, T3, T4>(T2 input) => new Choice<T1, T2, T3, T4>(input);
        public static implicit operator Choice<T1, T2, T3, T4>(T3 input) => new Choice<T1, T2, T3, T4>(input);
        public static implicit operator Choice<T1, T2, T3, T4>(T4 input) => new Choice<T1, T2, T3, T4>(input);

        /// <summary>
        /// Given a function for each possible choice, will invoke the relevant function for the available value in this <see cref="Choice{T1,T2,T3,T4}"/> 
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T4</param>
        public TR Match<TR>(
            Func<T1, TR> case1, 
            Func<T2, TR> case2, 
            Func<T3, TR> case3,
            Func<T4, TR> case4)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 3)
                return case3(_case3);
            if (_selectedCase == 4)
                return case4(_case4);

            throw new FailedMatchException();
        }
        /// <summary>
        /// Given a function to handle T1 and a function to be called should this <see cref="Choice{T1,T2,T3,T4}"/> not contain a value of T1, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T1</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> NOT contain an instance of T1</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1 and a function to be called should this <see cref="Choice{T1,T2,T3,T4}"/> not contain a value of T1, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T2</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> NOT contain an instance of T2</param>
        public TR Match<TR>(Func<T2, TR> case2, Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T3 and a function to be called should this <see cref="Choice{T1,T2,T3,T4}"/> not contain a value of T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> NOT contain an instance of T3</param>
        public TR Match<TR>(Func<T3, TR> case3, Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 3)
                return case3(_case3);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T4 and a function to be called should this <see cref="Choice{T1,T2,T3,T4}"/> not contain a value of T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> NOT contain an instance of T4</param>
        public TR Match<TR>(Func<T4, TR> case4, Func<TR> caseElse)
        {
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T2, and a function to be called should this <see cref="Choice{T1,T2,T3,T4}"/> 
        /// not contain a value of T1 or T2, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T2</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> NOT contain an instance of either T1 or T2</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<T2, TR> case2, Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 2)
                return case2(_case2);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T3, and a function to be called should this <see cref="Choice{T1,T2,T3,T4}"/> 
        /// not contain a value of T1 or T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> NOT contain an instance of either T1 or T3</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<T3, TR> case3, Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 3)
                return case3(_case3);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T4, and a function to be called should this <see cref="Choice{T1,T2,T3,T4}"/> 
        /// not contain a value of T1 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T1</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> NOT contain an instance of either T1 or T4</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<T4, TR> case4, Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T2, a function to handle T3, and a function to be called should this <see cref="Choice{T1,T2,T3,T4}"/> 
        /// not contain a value of T2 or T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> NOT contain an instance of either T2 or T3</param>
        public TR Match<TR>(Func<T2, TR> case2, Func<T3, TR> case3, Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 3)
                return case3(_case3);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T2, a function to handle T3, and a function to be called should this <see cref="Choice{T1,T2,T3,T4}"/> 
        /// not contain a value of T2 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> NOT contain an instance of either T2 or T4</param>
        public TR Match<TR>(Func<T2, TR> case2, Func<T4, TR> case4, Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T3, a function to handle T4, and a function to be called should this <see cref="Choice{T1,T2,T3,T4}"/> 
        /// not contain a value of T3 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4}"/> NOT contain an instance of either T3 or T4</param>
        public TR Match<TR>(Func<T3, TR> case3, Func<T4, TR> case4, Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 3)
                return case3(_case3);
            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Choice<T1, T2, T3, T4> && Equals((Choice<T1, T2, T3, T4>)obj);
        }
        public override int GetHashCode()
        {
            return _case1.GetHashCode() 
                ^ _case2.GetHashCode() 
                ^ _case3.GetHashCode()
                ^ _case4.GetHashCode();
        }
        public bool Equals(Choice<T1, T2, T3, T4> other)
        {
            var selected = _selectedCase;
            var c1 = _case1;
            var c2 = _case2;
            var c3 = _case3;
            var c4 = _case4;

            return other.Match(
                x => selected == 1 && Equals(x, c1),
                y => selected == 2 && Equals(y, c2),
                z => selected == 3 && Equals(z, c3),
                a => selected == 4 && Equals(a, c4));
        }
        public static bool operator ==(Choice<T1, T2, T3, T4> lhs, Choice<T1, T2, T3, T4> rhs) => lhs.Equals(rhs);
        public static bool operator !=(Choice<T1, T2, T3, T4> lhs, Choice<T1, T2, T3, T4> rhs) => !lhs.Equals(rhs);
    }

    ///<see cref="Choice{T1,T2}"/>
    public struct Choice<T1, T2, T3, T4, T5> : IEquatable<Choice<T1, T2, T3, T4, T5>>
    {
        private readonly T1 _case1;
        private readonly T2 _case2;
        private readonly T3 _case3;
        private readonly T4 _case4;
        private readonly T5 _case5;

        private readonly int _selectedCase;

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5}" /> with the given T1 as the available value
        /// </summary>
        public Choice(T1 value) : this()
        {
            _case1 = value;
            _selectedCase = 1;
        }
        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5}" /> with the given T2 as the available value
        /// </summary>
        public Choice(T2 value) : this()
        {
            _case2 = value;
            _selectedCase = 2;
        }
        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5}" /> with the given T3 as the available value
        /// </summary>
        public Choice(T3 value) : this()
        {
            _case3 = value;
            _selectedCase = 3;
        }
        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5}" /> with the given T4 as the available value
        /// </summary>
        public Choice(T4 value) : this()
        {
            _case4 = value;
            _selectedCase = 4;
        }
        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5}" /> with the given T5 as the available value
        /// </summary>
        public Choice(T5 value) : this()
        {
            _case5 = value;
            _selectedCase = 5;
        }

        public static implicit operator Choice<T1, T2, T3, T4, T5>(T1 input) => new Choice<T1, T2, T3, T4, T5>(input);
        public static implicit operator Choice<T1, T2, T3, T4, T5>(T2 input) => new Choice<T1, T2, T3, T4, T5>(input);
        public static implicit operator Choice<T1, T2, T3, T4, T5>(T3 input) => new Choice<T1, T2, T3, T4, T5>(input);
        public static implicit operator Choice<T1, T2, T3, T4, T5>(T4 input) => new Choice<T1, T2, T3, T4, T5>(input);
        public static implicit operator Choice<T1, T2, T3, T4, T5>(T5 input) => new Choice<T1, T2, T3, T4, T5>(input);


        /// <summary>
        /// Given a function for each possible choice, will invoke the relevant function for the available value in this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<T5, TR> case5)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 3)
                return case3(_case3);
            if (_selectedCase == 4)
                return case4(_case4);
            if (_selectedCase == 5)
                return case5(_case5);

            throw new FailedMatchException();
        }
        /// <summary>
        /// Given a function to handle T1 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> not contain a value of T1, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of T1</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T2 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> not contain a value of T2, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of T2</param>
        public TR Match<TR>(Func<T2, TR> case2, Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T3 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> not contain a value of T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of T3</param>
        public TR Match<TR>(Func<T3, TR> case3, Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 3)
                return case3(_case3);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T4 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> not contain a value of T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of T4</param>
        public TR Match<TR>(Func<T4, TR> case4, Func<TR> caseElse)
        {
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> not contain a value of T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of T5</param>
        public TR Match<TR>(Func<T5, TR> case5, Func<TR> caseElse)
        {
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T2, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T1 or T2, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T1 or T2</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<T2, TR> case2, Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 2)
                return case2(_case2);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T3, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T1 or T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T1 or T3</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<T3, TR> case3, Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 3)
                return case3(_case3);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T4, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T1 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T1 or T4</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<T4, TR> case4, Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T5, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T1 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T1 or T5</param>
        public TR Match<TR>(Func<T1, TR> case1, Func<T5, TR> case5, Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T2, a function to handle T3, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T2 or T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T2 or T3</param>
        public TR Match<TR>(Func<T2, TR> case2, Func<T3, TR> case3, Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 3)
                return case3(_case3);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T2, a function to handle T4, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T2 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T2 or T4</param>
        public TR Match<TR>(Func<T2, TR> case2, Func<T4, TR> case4, Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T2, a function to handle T5, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T2 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T2 or T5</param>
        public TR Match<TR>(Func<T2, TR> case2, Func<T5, TR> case5, Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T3, a function to handle T4, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T3 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T3 or T4</param>
        public TR Match<TR>(Func<T3, TR> case3, Func<T4, TR> case4, Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 3)
                return case3(_case3);
            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T3, a function to handle T5, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T3 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T3 or T5</param>
        public TR Match<TR>(Func<T3, TR> case3, Func<T5, TR> case5, Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 3)
                return case3(_case3);
            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T4, a function to handle T5, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T4 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T4 or T5</param>
        public TR Match<TR>(Func<T4, TR> case4, Func<T5, TR> case5, Func<TR> caseElse)
        {
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 4)
                return case4(_case4);
            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T2, a function to handle T3, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T1, T2, or T3 - will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T1, T2 or T3</param>
        public TR Match<TR>(
            Func<T1, TR> case1, 
            Func<T2, TR> case2, 
            Func<T3, TR> case3, 
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 3)
                return case3(_case3);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T2, a function to handle T4, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T1, T2, or T4 - will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T1, T2 or T4</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T4, TR> case4,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T2, a function to handle T5, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T1, T2, or T5 - will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T1, T2 or T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T3, a function to handle T4, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T1, T3, or T4 - will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T1, T3 or T4</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 3)
                return case3(_case3);
            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T3, a function to handle T5, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T1, T3, or T5 - will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T1, T3 or T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T3, TR> case3,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 3)
                return case3(_case3);
            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T1, a function to handle T4, a function to handle T5, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T1, T4, or T5 - will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T1</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T1, T4 or T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 1)
                return case1(_case1);
            if (_selectedCase == 4)
                return case4(_case4);
            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T2, a function to handle T3, a function to handle T4, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T2, T3, or T4 - will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T2, T3 or T4</param>
        public TR Match<TR>(
           Func<T2, TR> case2,
           Func<T3, TR> case3,
           Func<T4, TR> case4,
           Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 3)
                return case3(_case3);
            if (_selectedCase == 4)
                return case4(_case4);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T2, a function to handle T3, a function to handle T5, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T2, T3, or T5 - will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T2, T3 or T5</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 3)
                return case3(_case3);
            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T2, a function to handle T4, a function to handle T5, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T2, T4, or T5 - will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T2, T4 or T5</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 2)
                return case2(_case2);
            if (_selectedCase == 4)
                return case4(_case4);
            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }
        /// <summary>
        /// Given a function to handle T3, a function to handle T4, a function to handle T5, and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5}"/> 
        /// not contain a value of T3, T4, or T5 - will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5}"/> NOT contain an instance of either T3, T4 or T5</param>
        public TR Match<TR>(
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));

            if (_selectedCase == 3)
                return case3(_case3);
            if (_selectedCase == 4)
                return case4(_case4);
            if (_selectedCase == 5)
                return case5(_case5);

            return caseElse();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Choice<T1, T2, T3, T4, T5> && Equals((Choice<T1, T2, T3, T4, T5>)obj);
        }
        public override int GetHashCode()
        {
            return _case1.GetHashCode()
                ^ _case2.GetHashCode()
                ^ _case3.GetHashCode()
                ^ _case4.GetHashCode()
                ^ _case5.GetHashCode();
        }
        public bool Equals(Choice<T1, T2, T3, T4, T5> other)
        {
            var selected = _selectedCase;
            var c1 = _case1;
            var c2 = _case2;
            var c3 = _case3;
            var c4 = _case4;
            var c5 = _case5;

            return other.Match(
                x => selected == 1 && Equals(x, c1),
                y => selected == 2 && Equals(y, c2),
                z => selected == 3 && Equals(z, c3),
                a => selected == 4 && Equals(a, c4),
                b => selected == 5 && Equals(b, c5));
        }
        public static bool operator ==(Choice<T1, T2, T3, T4, T5> lhs, Choice<T1, T2, T3, T4, T5> rhs) => lhs.Equals(rhs);
        public static bool operator !=(Choice<T1, T2, T3, T4, T5> lhs, Choice<T1, T2, T3, T4, T5> rhs) => !lhs.Equals(rhs);
    }

    ///<see cref="Choice{T1,T2}"/>
    public struct Choice<T1, T2, T3, T4, T5, T6> : IEquatable<Choice<T1, T2, T3, T4, T5, T6>>
    {
        private readonly T1 _case1;
        private readonly T2 _case2;
        private readonly T3 _case3;
        private readonly T4 _case4;
        private readonly T5 _case5;
        private readonly T6 _case6;

        private readonly int _selectedCase;

        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5,T6}" /> with the given T1 as the available value
        /// </summary>
        public Choice(T1 value) : this()
        {
            _case1 = value;
            _selectedCase = 1;
        }


        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5,T6}" /> with the given T2 as the available value
        /// </summary>
        public Choice(T2 value) : this()
        {
            _case2 = value;
            _selectedCase = 2;
        }


        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5,T6}" /> with the given T3 as the available value
        /// </summary>
        public Choice(T3 value) : this()
        {
            _case3 = value;
            _selectedCase = 3;
        }


        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5,T6}" /> with the given T4 as the available value
        /// </summary>
        public Choice(T4 value) : this()
        {
            _case4 = value;
            _selectedCase = 4;
        }


        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5,T6}" /> with the given T5 as the available value
        /// </summary>
        public Choice(T5 value) : this()
        {
            _case5 = value;
            _selectedCase = 5;
        }


        /// <summary>
        /// Creates a <see cref="Choice{T1,T2,T3,T4,T5,T6}" /> with the given T6 as the available value
        /// </summary>
        public Choice(T6 value) : this()
        {
            _case6 = value;
            _selectedCase = 6;
        }

        public static implicit operator Choice<T1, T2, T3, T4, T5, T6>(T1 input)
            => new Choice<T1, T2, T3, T4, T5, T6>(input);

        public static implicit operator Choice<T1, T2, T3, T4, T5, T6>(T2 input)
            => new Choice<T1, T2, T3, T4, T5, T6>(input);

        public static implicit operator Choice<T1, T2, T3, T4, T5, T6>(T3 input)
            => new Choice<T1, T2, T3, T4, T5, T6>(input);

        public static implicit operator Choice<T1, T2, T3, T4, T5, T6>(T4 input)
            => new Choice<T1, T2, T3, T4, T5, T6>(input);

        public static implicit operator Choice<T1, T2, T3, T4, T5, T6>(T5 input)
            => new Choice<T1, T2, T3, T4, T5, T6>(input);

        public static implicit operator Choice<T1, T2, T3, T4, T5, T6>(T6 input)
            => new Choice<T1, T2, T3, T4, T5, T6>(input);

        /// <summary>
        /// Given a function for each possible choice, will invoke the relevant function for the available value in this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>

        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<T6, TR> case6)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);

            throw new FailedMatchException();
        }

        /// <summary>
        /// Given a function to handle T1 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T3 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T3</param>
        public TR Match<TR>(
            Func<T3, TR> case3,
            Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 3) return case3(_case3);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T4 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T4</param>
        public TR Match<TR>(
            Func<T4, TR> case4,
            Func<TR> caseElse)
        {
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 4) return case4(_case4);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T5</param>
        public TR Match<TR>(
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T6</param>
        public TR Match<TR>(
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T3 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T3</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T3, TR> case3,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 3) return case3(_case3);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T4 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T4</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T4, TR> case4,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 4) return case4(_case4);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T3 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T3</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T4 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T4</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T4, TR> case4,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 4) return case4(_case4);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T5</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T6</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T3, function to handle T4 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T3 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T3 or T4</param>
        public TR Match<TR>(
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T3, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T3 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T3 or T5</param>
        public TR Match<TR>(
            Func<T3, TR> case3,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T3, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T3 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T3 or T6</param>
        public TR Match<TR>(
            Func<T3, TR> case3,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T4, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T4 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T4 or T5</param>
        public TR Match<TR>(
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T4, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T4 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T4 or T6</param>
        public TR Match<TR>(
            Func<T4, TR> case4,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T5 or T6</param>
        public TR Match<TR>(
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2, function to handle T3 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2 or T3, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2 or T3</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2, function to handle T4 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2 or T4</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T4, TR> case4,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 4) return case4(_case4);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2 or T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T3, function to handle T4 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T3 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T3 or T4</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T3, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T3 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T3 or T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T3, TR> case3,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T3, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T3 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T3 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T3, TR> case3,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T4, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T4 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T4 or T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T4, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T4 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T4 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T4, TR> case4,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T5 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T3, function to handle T4 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T3 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T3 or T4</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T3, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T3 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T3 or T5</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T3, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T3 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T3 or T6</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T4, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T4 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T4 or T5</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T4, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T4 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T4 or T6</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T4, TR> case4,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T5 or T6</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T3, function to handle T4, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T3 or T4 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T3 or T4 or T5</param>
        public TR Match<TR>(
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T3, function to handle T4, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T3 or T4 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T3 or T4 or T6</param>
        public TR Match<TR>(
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T3, function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T3 or T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T3 or T5 or T6</param>
        public TR Match<TR>(
            Func<T3, TR> case3,
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T4, function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T4 or T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T4 or T5 or T6</param>
        public TR Match<TR>(
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2, function to handle T3, function to handle T4 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2 or T3 or T4, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2 or T3 or T4</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2, function to handle T3, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2 or T3 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2 or T3 or T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2, function to handle T3, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2 or T3 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2 or T3 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2, function to handle T4, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2 or T4 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2 or T4 or T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2, function to handle T4, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2 or T4 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2 or T4 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T4, TR> case4,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T2, function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T2 or T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T2 or T5 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T2, TR> case2,
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T3, function to handle T4, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T3 or T4 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T3 or T4 or T5</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T3, function to handle T4, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T3 or T4 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T3 or T4 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T3, function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T3 or T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T3 or T5 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T3, TR> case3,
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T1, function to handle T4, function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T1 or T4 or T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case1">The <see cref="System.Func{T1,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T1</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T1 or T4 or T5 or T6</param>
        public TR Match<TR>(
            Func<T1, TR> case1,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case1 == null) throw new ArgumentNullException(nameof(case1));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 1) return case1(_case1);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T3, function to handle T4, function to handle T5 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T3 or T4 or T5, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T3 or T4 or T5</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T3, function to handle T4, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T3 or T4 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T3 or T4 or T6</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T3, function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T3 or T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T3 or T5 or T6</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T3, TR> case3,
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T2, function to handle T4, function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T2 or T4 or T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case2">The <see cref="System.Func{T2,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T2</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T2 or T4 or T5 or T6</param>
        public TR Match<TR>(
            Func<T2, TR> case2,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case2 == null) throw new ArgumentNullException(nameof(case2));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 2) return case2(_case2);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        /// <summary>
        /// Given a function to handle T3, function to handle T4, function to handle T5, function to handle T6 and a function to be called should this <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> 
        /// not contain a value of T3 or T4 or T5 or T6, will invoke the relevant function
        /// </summary>
        /// <typeparam name="TR">The return type of all possible case functions</typeparam>
        /// <param name="case3">The <see cref="System.Func{T3,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T3</param>
        /// <param name="case4">The <see cref="System.Func{T4,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T4</param>
        /// <param name="case5">The <see cref="System.Func{T5,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T5</param>
        /// <param name="case6">The <see cref="System.Func{T6,TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> contain an instance of T6</param>
        /// <param name="caseElse">The <see cref="System.Func{TR}" /> to invoke should the <see cref="Choice{T1,T2,T3,T4,T5,T6}"/> NOT contain an instance of either T3 or T4 or T5 or T6</param>
        public TR Match<TR>(
            Func<T3, TR> case3,
            Func<T4, TR> case4,
            Func<T5, TR> case5,
            Func<T6, TR> case6,
            Func<TR> caseElse)
        {
            if (case3 == null) throw new ArgumentNullException(nameof(case3));
            if (case4 == null) throw new ArgumentNullException(nameof(case4));
            if (case5 == null) throw new ArgumentNullException(nameof(case5));
            if (case6 == null) throw new ArgumentNullException(nameof(case6));
            if (caseElse == null) throw new ArgumentNullException(nameof(caseElse));
            if (_selectedCase == 3) return case3(_case3);
            if (_selectedCase == 4) return case4(_case4);
            if (_selectedCase == 5) return case5(_case5);
            if (_selectedCase == 6) return case6(_case6);
            return caseElse();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Choice<T1, T2, T3, T4, T5, T6> && Equals((Choice<T1, T2, T3, T4, T5, T6>)obj);
        }

        public override int GetHashCode()
        {
            return _case1.GetHashCode()
                    ^ _case2.GetHashCode()
                    ^ _case3.GetHashCode()
                    ^ _case4.GetHashCode()
                    ^ _case5.GetHashCode()
                    ^ _case6.GetHashCode();
        }

        public bool Equals(Choice<T1, T2, T3, T4, T5, T6> other)
        {
            var selected = _selectedCase;
            var c1 = _case1;
            var c2 = _case2;
            var c3 = _case3;
            var c4 = _case4;
            var c5 = _case5;
            var c6 = _case6;
            return other.Match(
                x1 => selected == 1 && Equals(x1, c1),
                x2 => selected == 2 && Equals(x2, c2),
                x3 => selected == 3 && Equals(x3, c3),
                x4 => selected == 4 && Equals(x4, c4),
                x5 => selected == 5 && Equals(x5, c5),
                x6 => selected == 6 && Equals(x6, c6));
        }

        public static bool operator ==(
            Choice<T1, T2, T3, T4, T5, T6> lhs, Choice<T1, T2, T3, T4, T5, T6> rhs) => lhs.Equals(rhs);

        public static bool operator !=(
            Choice<T1, T2, T3, T4, T5, T6> lhs, Choice<T1, T2, T3, T4, T5, T6> rhs) => !lhs.Equals(rhs);
    }
}