﻿using OPIS.CMS.Core.PagedList.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OPIS.CMS.Core.PagedList.Implementations
{
	/// <summary>
	/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
	/// </summary>
	/// <remarks>
	/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
	/// </remarks>
	/// <typeparam name="T">The type of object the collection should contain.</typeparam>
	/// <seealso cref="IPagedList{T}"/>
	/// <seealso cref="BasePagedList{T}"/>
	/// <seealso cref="StaticPagedList{T}"/>
	/// <seealso cref="List{T}"/>
	public class PagedList<T> : BasePagedList<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only containes the objects contained in the subset specified by index.
		/// </summary>
		/// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
		/// <param name="index">The index of the subset of objects to be contained by this instance.</param>
		/// <param name="pageSize">The maximum size of any individual subset.</param>
		/// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
		public PagedList(IEnumerable<T> superset, int index, int pageSize)
			: this(superset == null ? new List<T>().AsQueryable() : superset.AsQueryable(), index, pageSize)
		{
		}

		private PagedList(IQueryable<T> superset, int index, int pageSize)
			: base(index, pageSize, superset.Count())
		{
			// add items to internal list
			if (TotalItemCount > 0)
				if (index == 0)
					AddRange(superset.Take(pageSize).ToList());
				else if (this.PageCount > this.PageIndex)
					AddRange(superset.Skip((index - 1) * pageSize).Take(pageSize).ToList());
				else
				{
					this.PageIndex = this.PageCount;
					AddRange(superset.Skip((this.PageIndex - 1) * pageSize).Take(pageSize).ToList());
				}
		}

		public PagedList(int index, int pageSize, int totalItemCount)
			: base(index, pageSize, totalItemCount)
		{

		}

	}
}