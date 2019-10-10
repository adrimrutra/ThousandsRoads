using System;
using System.Collections.Generic;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using Travel.Core;
using InterSystems.Data.CacheTypes;
using Travel.CacheMigration;
using Travel.Common.Enums;

namespace Travel.CasheService.Mapping
{
	public class EmitCacheObjectMapper : IObjectMapper
	{
		public TDest Map<TSource, TDest>(TSource Object)
		{
			IMappingConfigurator config = EmitMapperConfiguration.Config;
			ObjectsMapper<TSource, TDest> mapper = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDest>(config);
			return mapper.Map(Object);
		}

		public TDest Map<TSource, TDest>(TSource Object, Func<TSource, TDest> Mapper)
		{
			return Mapper(Object);
		}

		public Func<TSource, TDest> GetMapper<TSource, TDest>()
		{
			throw new NotImplementedException();
		}
	}
	public static class EmitMapperConfiguration
	{
		private static IMappingConfigurator config = GetConfig();
		public static IMappingConfigurator Config { get { return config; } }
		private static IMappingConfigurator GetConfig()
		{
			var config = new DefaultMapConfig();
			config.ConvertGeneric(typeof(CacheListOfObjects), typeof(ICollection<>),
				new DefaultCustomConverterProvider(typeof(CacheConvertCollectionExtends<>)));
			var manager = ObjectMapperManager.DefaultInstance;

			return config

			.ConvertUsing<ThousandsRoads.Customer, Data.User>(x =>
			{
				return x == null ? null : new Data.User
				{
					Id = Int32.Parse(x.Id()),
					DisplayName = x.DisplayName,
					Avatar = x.Avatar,
					Email = x.Email,
					Tokens = manager.GetMapper<CacheListOfObjects, ICollection<Data.Token>>(config)
										.Map(x.Tokens),
					Comments = manager.GetMapper<CacheListOfObjects, ICollection<Data.Comment>>(config)
										.Map(x.Comments)
				};
			})
			.ConvertUsing<ThousandsRoads.Token, Data.Token>(x =>
			{
				return x == null ? null : new Data.Token
				{
					Id = Int32.Parse(x.Id()),
					SocialId = x.SocialId,
					Tokentype = (TokenType)x.Tokentype,
					UserId = int.Parse(x.User.Id())
				};
			})

			.ConvertUsing<ThousandsRoads.Travel, Data.Travel>(x =>
			{
				//var date = x.Startdate.Value;
				//date.AddHours(x.Starttime.Value.Hour);

				//date.AddMinutes(x.Starttime.Value.Minute);
				return x == null ? null : new Data.Travel
				{
					Id = Int32.Parse(x.Id()),
					Capacity = (int?)x.Capacity,
					Startdate = x.Startdate,
					Enddate = x.Enddate,
					Startpoint = x.Startpoint,
					Endpoint = x.Endpoint,
					Mapsnapshot = x.Mapsnapshot,
					Luggage = (LuggageType)x.Luggage.Value,
					Travelers = manager.GetMapper<CacheListOfObjects, ICollection<Data.Traveler>>(config)
										.Map(x.Travelers),

					MapPoints = manager.GetMapper<CacheListOfObjects, ICollection<Data.MapPoint>>(config)
										.Map(x.MapPoints)
				};
			})
			.ConvertUsing<ThousandsRoads.MapPoint, Data.MapPoint>(x =>
			{
				return x == null ? null : new Data.MapPoint    //pomulka zavtra rozibratus
				{
					Id = Int32.Parse(x.Id()),
					Latitude = (double)x.Latitude,
					Longitude = (double)x.Longitude,
					Name = x.Name
				};
			})
			.ConvertUsing<ThousandsRoads.Comment, Data.Comment>(x =>
			{
				return x == null ? null : new Data.Comment
				{
					Message = x.Message,
					Data = x.Data.Value,
					Messenger = new Data.User
					{
						DisplayName = x.Messenger.DisplayName,
						Id = (int)x.MessengerId.Value
					},
					Type = (CommentType)x.Type.Value,
					MessengerId = (int)x.MessengerId.Value
				};
			})

			.ConvertUsing<ThousandsRoads.Traveler, Data.Traveler>(x =>
			{
				return x == null ? null : new Data.Traveler
				{
					Id = Int32.Parse(x.Id()),
					User = manager.GetMapper<ThousandsRoads.Customer, Data.User>(config)
							.Map(x.User),
					TravelId = Int32.Parse(x.Travel.Id()),
					UserId = int.Parse(x.User.Id()),
					Usertype = (UserType)x.CustomerType
				};
			})
			.ConvertUsing<ThousandsRoads.Message, Data.Message>(x =>
			{
				return x == null ? null : new Data.Message
				{
					Id = Int32.Parse(x.Id()),
					MessageText = x.MessageText,
					State = x.State.Value,
					Type = (MessageType)x.Type.Value,
					Theme = x.Theme,
					UserId = (int)(long)x.UserId,
					MessengerId = (int)(long)x.MessengerId,
					TravelId = (int)(long)x.TravelId,
					PersonCount = (int)(long)x.PersonCount,
					Direction = (DirectionType)x.Direction.Value,
					Luggage = (LuggageType)x.Luggage.Value
				};
			})

			;
		}

	}
}
