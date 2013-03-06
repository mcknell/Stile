#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IClause
	{
		Guid Id { get; }
	}

	public class Clause : IClause
	{
		public static readonly IClause AlwaysTrue = new Clause(new Guid("{34A27DC8-69DB-4459-AC5A-627C5869ED7B}"));
		public static readonly IClause HasHashCode = new Clause(new Guid("{1FFA684E-4150-418F-9002-CE57DF4E46B0}"));
		public static readonly IClause HasItemsSatisfying =
			new Clause(new Guid("{928110B2-72D6-4584-9501-9C22953F3D2F}"));
		public static readonly IClause HasAll = new Clause(new Guid("{1F7E5898-D056-45E0-B65E-E8E7B70322D3}"));
		public static readonly IClause IsComparablyEquivalentTo =
			new Clause(new Guid("{C85750B1-D2A3-4517-A0D7-4490F111E99E}"));
		public static readonly IClause IsGreaterThan = new Clause(new Guid("{AB05D2D4-0BFF-4FCB-B8A6-961254A7199B}"));
		public static readonly IClause IsEqualTo = new Clause(new Guid("{2283137A-E48C-4046-9CDE-08074F630721}"));

		public Clause(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; private set; }
	}

	public class IsEqualTo : Clause{
		public IsEqualTo()
			: base(new Guid("{2283137A-E48C-4046-9CDE-08074F630721}")) { }
	}
}
