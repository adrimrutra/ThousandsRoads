using System;
using System.Collections.Generic;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using Travel.Core;
using InterSystems.Data.CacheTypes;
using Travel.Common.Enums;
using System.Linq;

namespace Travel.Service.Mapping
{
	public class EmitObjectMapper : IObjectMapper
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
			var config = new DefaultMapConfig()
				.ConvertGeneric(typeof(ICollection<>), typeof(ICollection<>),
				new DefaultCustomConverterProvider(typeof(CollectionToCollection<,>)));

			var manager = ObjectMapperManager.DefaultInstance;

			return config
				.ConvertUsing<Entity.Comment, Data.Comment>(x =>
				{

					return x == null ? null : new Data.Comment
					{
						Message = x.Message,
						Data = x.Data.Value,
						Messenger = new Data.User
						{
							DisplayName = x.Messenger.DisplayName,
							Id = x.MessengerId
						},
						Type = x.Type,
						MessengerId = (int)x.MessengerId
					};
				})
				.ConvertUsing<Entity.Travel, Data.Travel>(x =>
				{
					var spoint = "";
					var epoint = "";
					if (x.MapPoints.FirstOrDefault() != null)
						spoint = x.MapPoints.FirstOrDefault().Name;
					if (x.MapPoints.LastOrDefault() != null)
						epoint = x.MapPoints.LastOrDefault().Name;
					var driverid = 0;
					var driver = x.Travelers.SingleOrDefault(u => u.Usertype == UserType.Driver);
					if (driver != null)
						driverid = driver.UserId;
					return x == null ? null : new Data.Travel
					{
						Id = x.Id,
						Capacity = x.Capacity,
						Enddate = x.Enddate,
						Startdate = x.Startdate,
						Startpoint = spoint,
						Endpoint = epoint,
						Mapsnapshot = x.Mapsnapshot,
						DriverId = driverid,
						DisplayName=driver.User.DisplayName,						
						Luggage = x.Luggage,
						Travelers = manager.GetMapper<ICollection<Entity.Traveler>, ICollection<Data.Traveler>>(config).Map(x.Travelers),
						MapPoints = manager.GetMapper<ICollection<Entity.MapPoint>, ICollection<Data.MapPoint>>(config).Map(x.MapPoints)
					};
				})
				.ConvertUsing<Data.Travel, Entity.Travel>(x =>
				{
					return x == null ? null : new Entity.Travel
					{
						Id = x.Id,
						Capacity = x.Capacity,
						Enddate = x.Enddate,
						Startdate = x.Startdate,
						Mapsnapshot = x.Mapsnapshot,
						//Luggages = manager.GetMapper<ICollection<Entity.Luggage>, ICollection<Data.Luggage>>(config).Map(x.Luggages),
						Travelers = manager.GetMapper<ICollection<Data.Traveler>, ICollection<Entity.Traveler>>(config).Map(x.Travelers),
						MapPoints = manager.GetMapper<ICollection<Data.MapPoint>, ICollection<Entity.MapPoint>>(config).Map(x.MapPoints)
					};
				})
				.ConvertUsing<Entity.MapPoint, Data.MapPoint>(x =>
				{
					return x == null ? null : new Data.MapPoint
					{
						Id = x.Id,
						Latitude = x.Latitude,
						Longitude = x.Longitude,
						Name = x.Name
					};
				})
				.ConvertUsing<Data.MapPoint, Entity.MapPoint>(x =>
				{
					return x == null ? null : new Entity.MapPoint
					{
						Id = x.Id,
						Latitude = x.Latitude,
						Longitude = x.Longitude,
						Name = x.Name
					};
				})
				.ConvertUsing<Entity.User, Data.User>(x =>
				{
					return x == null ? null : new Data.User
					{
						Id = x.Id,
						DisplayName = x.DisplayName,
						Avatar = x.Avatar,
						Email = x.Email,
						Tokens = manager.GetMapper<ICollection<Entity.Token>, ICollection<Data.Token>>(config)
											.Map(x.Tokens),
						Comments = manager.GetMapper<ICollection<Entity.Comment>, ICollection<Data.Comment>>(config)
											.Map(x.Comments)
					};
				})
				.ConvertUsing<Data.User, Entity.User>(x =>
				{
					return x == null ? null : new Entity.User
					{
						Id = x.Id,
						DisplayName = x.DisplayName,
						Avatar = x.Avatar,
						Email = x.Email,
						Tokens = manager.GetMapper<ICollection<Data.Token>, ICollection<Entity.Token>>(config)
											.Map(x.Tokens),
						Comments = manager.GetMapper<ICollection<Data.Comment>, ICollection<Entity.Comment>>(config)
											.Map(x.Comments)

					};
				})
				.ConvertUsing<Entity.Token, Data.Token>(x =>
				{
					return x == null ? null : new Data.Token
					{
						Id = x.Id,
						SocialId = x.SocialId,
						Tokentype = (TokenType)x.Tokentype,
						UserId = x.User.Id
					};
				})
				.ConvertUsing<Data.Token, Entity.Token>(x =>
				{
					return x == null ? null : new Entity.Token
					{
						Id = x.Id,
						SocialId = x.SocialId,
						Tokentype = (TokenType)x.Tokentype,
						UserId = x.User.Id
					};
				})
				.ConvertUsing<Entity.Traveler, Data.Traveler>(x =>
				{
					return x == null ? null : new Data.Traveler
					{
						Id = x.Id,
						User = manager.GetMapper<Entity.User, Data.User>(config)
								.Map(x.User),
						UserId = x.UserId,
						TravelId = x.Travel.Id,
						Usertype = x.Usertype
					};
				})
				.ConvertUsing<Data.Traveler, Entity.Traveler>(x =>
				{
					return x == null ? null : new Entity.Traveler
					{
						Id = x.Id,
						User = manager.GetMapper<Data.User, Entity.User>(config)
								.Map(x.User),
						TravelId = x.Travel.Id,
						Usertype = x.Usertype
					};
				})
			.ConvertUsing<Entity.Message, Data.Message>(x =>
			{
				return x == null ? null : new Data.Message
				{
					Id = x.Id,
					MessageText = x.MessageText,
					State = x.State,
					Theme = x.Theme,
					UserId = x.UserId.Value,
					MessengerId = x.MessengerId.Value,
					TravelId = x.TravelId.Value,
					PersonCount = x.PersonCount,
					Direction = x.Direction,
					Luggage = x.Luggage,
					Type = x.Type,
					MessengerDisplayName = x.Messenger == null ? "" : x.Messenger.DisplayName,
					MessengerAvatar = x.Messenger == null ? "" : x.Messenger.Avatar,
					MessengerEmail = x.Messenger == null ? "" : x.Messenger.Email
				};
			})
			;
		}

	}
}
