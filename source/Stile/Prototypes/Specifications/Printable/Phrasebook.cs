#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
	public interface IPhrasebook
	{
		ReadOnlyDictionary<Guid, IClause> Clauses { get; }
	}

	public class Phrasebook : IPhrasebook
	{
		private static readonly Lazy<Phrasebook> _core = new Lazy<Phrasebook>(ReadCore);

		protected Phrasebook(IEnumerable<IClause> clauses)
		{
			Clauses = new ReadOnlyDictionary<Guid, IClause>(clauses.ToDictionary(x => x.Id));
		}

		public ReadOnlyDictionary<Guid, IClause> Clauses { get; private set; }

		public static IPhrasebook Core
		{
			get { return _core.Value; }
		}

		public static IEnumerable<IClause> ReadClauses<THost>()
		{
			const BindingFlags flags = BindingFlags.Static | BindingFlags.Public;
			Type type = typeof(THost);
			Predicate<Type> isIClause = x => typeof(IClause).IsAssignableFrom(x);
			return
				type.GetProperties(flags)
					.Where(x => isIClause(x.PropertyType) && x.CanRead)
					.Select(x => x.GetValue(null) as IClause)
					.Concat(type.GetFields(flags).Where(x => isIClause(x.FieldType)).Select(x => x.GetValue(null) as IClause))
					.ToList();
		}

		private static Phrasebook ReadCore()
		{
			IEnumerable<IClause> properties = ReadClauses<Clause>();
			return new Phrasebook(properties);
		}
	}
}
