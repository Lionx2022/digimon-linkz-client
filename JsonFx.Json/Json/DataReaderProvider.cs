﻿using System;
using System.Collections.Generic;

namespace JsonFx.Json
{
	public class DataReaderProvider : IDataReaderProvider
	{
		private readonly IDictionary<string, IDataReader> ReadersByMime = new Dictionary<string, IDataReader>(StringComparer.OrdinalIgnoreCase);

		public DataReaderProvider(IEnumerable<IDataReader> readers)
		{
			if (readers != null)
			{
				foreach (IDataReader dataReader in readers)
				{
					if (!string.IsNullOrEmpty(dataReader.ContentType))
					{
						this.ReadersByMime[dataReader.ContentType] = dataReader;
					}
				}
			}
		}

		public IDataReader Find(string contentTypeHeader)
		{
			string key = DataWriterProvider.ParseMediaType(contentTypeHeader);
			if (this.ReadersByMime.ContainsKey(key))
			{
				return this.ReadersByMime[key];
			}
			return null;
		}
	}
}
